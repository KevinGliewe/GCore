using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace GCore.Data.DbfDotNet.Core
{
    internal abstract class ClusteredFile
    {
        protected UInt32 mRecordCount;
        internal UInt32 mCurrentRecordNoPosition;
        private Dictionary<UInt32, LinkedListNode<RecordHolder>> mRecordsByRecordNo;
        private LinkedList<RecordHolder> mRecordsByLastUsed;
        private int mConstructorThreadId;
        private bool mIsDisposed = false;    // To detect redundant calls
        internal int mRecordWidth = 0;
        internal int mHeaderWidth = 0;
        private Stream mStream;
        protected FileMode mFileMode;
        protected FileAccess mFileAccess;
        protected FileShare mFileShare;
        internal List<ColumnDefinition> mColumns;       
        protected static Dictionary<string, ClusteredFile> mOpenFiles;
        private int mReaderCount;
        internal string mOriginalFile;

        const double PERCENT_CACHED_RECORDS = 2.5 / 100; // 2.5% normally
        const int MIN_CACHED_RECORDS = 100;
        const int MAX_CACHED_RECORDS = 10000; 
        
        static ClusteredFile()
        {
            // the filesystem ignore case
            mOpenFiles = new Dictionary<string, ClusteredFile>(StringComparer.OrdinalIgnoreCase);
        }

        protected ClusteredFile()
        {
            mConstructorThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
        
        private void Seek(UInt32 newRecordNo)
        {
            Debug.Assert(newRecordNo<UInt32.MaxValue,"Invalid RecordNo");
            if (newRecordNo == mCurrentRecordNoPosition) return;
            mStream.Seek(newRecordNo * mRecordWidth + mHeaderWidth, SeekOrigin.Begin);
            mCurrentRecordNoPosition = newRecordNo;
        }
        
        #region IDisposable Members

        protected virtual void OnDisposing()
        {
            Flush();
        }

        private void Dispose(bool disposing)
        {
            if (mIsDisposed) return;
            if (disposing)
            {
                OnDisposing();
                mStream.Close();
                GC.SuppressFinalize(this);
            }
            mIsDisposed = true;
            mStream = null;
        }

        public virtual void Flush()
        {
            if (mFileAccess != FileAccess.Read)
            {
                var values = mRecordsByRecordNo.Values;

                foreach (var r in values)
                {
                    Flush(r);
                }
                WriteHeader();
            }
        }

        private void Flush(LinkedListNode<RecordHolder> node)
        {
            var recordHolder = node.Value;
            var recordNo = recordHolder.RecordNo;
            var record = recordHolder.Record;
            if (record != null)
                InternalFillWriteBuffer(recordHolder, record);
            if (recordHolder.IsModified())
                recordHolder.Save();
        }



        ~ClusteredFile()
        {
            Dispose(false);
        }
        
        #endregion

        internal virtual protected Record InternalGetRecord(UInt32 recordNo, bool returnNullIfNotInCache)
        {
            Debug.Assert(recordNo < UInt32.MaxValue, "Invalid RecordNo");
            LinkedListNode<RecordHolder> linkListNode = null;
            RecordHolder holder = null;
            Record record = null;

            if (mRecordsByRecordNo.TryGetValue(recordNo, out linkListNode))
            {
                holder = linkListNode.Value;
                record = holder.Record;
                if (record != null)
                {
                    SetLastUsed(linkListNode);
                    return record;
                }
                else
                {
                    // we're going to add a new one in one sec
                    RemoveLastUsed(linkListNode);
                }
            }

            if (returnNullIfNotInCache) return null;
            // we're here because the record was disposed or never created

            var newRecord = OnCreateNewRecord(/*isnew*/false, recordNo);


            if (holder == null)
            {
                holder = new RecordHolder(this, recordNo, newRecord);
                linkListNode = new LinkedListNode<RecordHolder>(holder);
                SetLastUsed(linkListNode);
                mRecordsByRecordNo[recordNo] = linkListNode;
#if DUMP_FINALIZE
                    System.Diagnostics.Trace.WriteLine(this.mOriginalFile + " Save Record #" + recordNo);
#endif
            }
            else
            {
                // holder exist but object doesn't we expect that a finalized happened
                if (holder.RecordFinalized.WaitOne(10000))
                {
                    // we (re)connect the recordHolder to the new Record
                    holder.RecordFinalized.Reset();
                    holder.Record = newRecord;
                }
                else
                {
                    throw new Exception("Something very wrong happened.");
                }
            }
            newRecord.SetHolderForNewRecord(holder);

            Byte[] sourceBuffer = holder.GetCurrentBuffer(/*readIfNeeded*/true);
            OnReadRecordBuffer(sourceBuffer, newRecord);

            return newRecord;
        }

        private void RemoveLastUsed(LinkedListNode<RecordHolder> linkListNode)
        {
            if (linkListNode.List == mRecordsByLastUsed)
                mRecordsByLastUsed.Remove(linkListNode);
        }

        private void SetLastUsed(LinkedListNode<RecordHolder> linkListNode)
        {
            if (linkListNode.List == mRecordsByLastUsed)
            {
                if (linkListNode != mRecordsByLastUsed.Last)
                {
                    mRecordsByLastUsed.Remove(linkListNode);
                    mRecordsByLastUsed.AddLast(linkListNode);
                }
            }
            else
            {
                mRecordsByLastUsed.AddLast(linkListNode);
            }

            // for every table we use PERCENT_CACHED_RECORDS
            int limit = (int)(PERCENT_CACHED_RECORDS * mRecordCount);
            if (limit < MIN_CACHED_RECORDS) limit = MIN_CACHED_RECORDS;
            else if (limit > MAX_CACHED_RECORDS) limit = MAX_CACHED_RECORDS;

            var node = mRecordsByLastUsed.First;
            while (node != null && mRecordsByLastUsed.Count > limit)
            {
                RecordHolder recordHolder = node.Value;
                var nextNode = node.Next;

                node.Value.Hold = false;
                mRecordsByLastUsed.Remove(node);
#if DUMP_FINALIZE
                System.Diagnostics.Trace.WriteLine(this.mOriginalFile + " Remove last used Record #" + recordHolder.mRecordNo);
#endif
                node = nextNode;
            }
        }



        /// <summary>
        /// Note that this method is called from flush and from the record destructor in the garbage collector thread.
        /// </summary>
        /// <param name="recordHolder"></param>
        internal void InternalFillWriteBuffer(RecordHolder recordHolder, Record record)
        {
            Debug.Assert(record != null, "InternalFillWriteBuffer : record must be not null.");
            var newBuffer = new Byte[mRecordWidth];

            if (OnFillWriteBuffer(record, newBuffer))
            {
                recordHolder.SetNewBuffer(newBuffer);
            }
        }

        protected abstract Record OnCreateNewRecord(bool isNew, UInt32 recordNo);
        protected abstract void OnReadRecordBuffer(byte[] buffer, Record record);
        internal abstract bool OnFillWriteBuffer(Record record, Byte[] buffer);
        protected abstract void OnInitialize(Stream mStream);
        protected abstract void OnWriteHeader(Stream stream);

        internal virtual void OnWriteBufferModified(UInt32 recordNo, byte[] oldBuffer, byte[] newBuffer)
        {
        }

        public virtual object NewRecord()
        {
            var recordNo = mRecordCount;
            mRecordCount += 1;
            var result = OnCreateNewRecord(/*isnew*/true, recordNo);

            var holder = new RecordHolder(this, recordNo, result);
            var linkedListNode = new LinkedListNode<RecordHolder>(holder);

            result.SetHolderForNewRecord(holder);

            mRecordsByRecordNo[recordNo] = linkedListNode;
            SetLastUsed(linkedListNode);
#if DUMP_FINALIZE
                System.Diagnostics.Trace.WriteLine(this.mOriginalFile + " Add Record #" + recordNo);
#endif


            return result;
        }

        internal ColumnDefinition GetColumnByName(string columnName)
        {
            for (int i = 0; i < mColumns.Count; i++)
            {
                if (mColumns[i].mColumnName == columnName)
                {
                    return mColumns[i];
                }
            }
            // not found try again with case off
            for (int i = 0; i < mColumns.Count; i++)
            {
                if (string.Compare(mColumns[i].mColumnName, columnName, true) == 0)
                {
                    return mColumns[i];
                }
            }
            return null;
        }

        static public T Get<T>(string filepath, OpenFileMode mode, int bufferSize)
            where T : ClusteredFile, new()
        {
            var fi = new System.IO.FileInfo(filepath);
            // in case the file is readonly we assume users expect us to force the OpenReadOnly mode.
            if (fi.Exists && (fi.Attributes & FileAttributes.ReadOnly) != 0) mode = OpenFileMode.OpenReadOnly;
            ClusteredFile clusteredFile = null;
            if (!mOpenFiles.TryGetValue(fi.FullName, out clusteredFile))
            {
                clusteredFile = new T();
                mOpenFiles.Add(fi.FullName, clusteredFile);
            }
            FileMode fm;
            FileAccess fa;
            FileShare fs;
            switch (mode)
            {
                case OpenFileMode.OpenOrCreate:
                    fm = FileMode.OpenOrCreate;
                    fa = FileAccess.ReadWrite;
                    fs = FileShare.Read;
                    break;
                case OpenFileMode.OpenReadOnly:
                    fm = FileMode.Open;
                    fa = FileAccess.Read;
                    fs = FileShare.ReadWrite;
                    break;
                default: // OpenFileMode.OpenReadWrite:
                    fm = FileMode.Open;
                    fa = FileAccess.ReadWrite;
                    fs = FileShare.Read;
                    break;
            }
            clusteredFile.InternalAddReader(fi, fm, fa, fs, bufferSize);
            return (T)clusteredFile;
        }

        private void InternalAddReader(FileInfo fi, FileMode fm, FileAccess fa, FileShare fs, int bufferSize)
        {
            if (mStream == null)
            {
                mStream = new FileStream(fi.FullName, fm, fa, fs, bufferSize);
                mFileMode = fm;
                mFileAccess = fa;
                mFileShare = fs;

                //if (encoding == null) encoding = System.Text.Encoding.ASCII;
                //mEncoding = encoding;
                mOriginalFile = fi.FullName;
                mCurrentRecordNoPosition = UInt32.MaxValue;
                mRecordsByRecordNo = new Dictionary<UInt32, LinkedListNode<RecordHolder>>();
#if DUMP_FINALIZE
                System.Diagnostics.Trace.WriteLine(this.mOriginalFile + "Clear Records");
#endif
                mRecordsByLastUsed = new LinkedList<RecordHolder>();
            }
            else
            {
                // todo check that the access make sense.
            }
            mReaderCount++;
        }

        public void StopReading()
        {
            mReaderCount--;
            if (mReaderCount == 0)
            {
                Dispose(true);
            }
        }

        public int ReaderCount
        {
            get
            {
                return mReaderCount;
            }
        }

        protected void WriteHeader()
        {
            mStream.Position = 0;
            OnWriteHeader(mStream);
            mCurrentRecordNoPosition = UInt32.MaxValue;
        }

        protected void Initialize()
        {
            OnInitialize(mStream);
        }

        internal void EmptyTable()
        {
            var node = mRecordsByLastUsed.First;

            while (node != null && mRecordsByLastUsed.Count > 0)
            {
                RecordHolder recordHolder = node.Value;
                var nextNode = node.Next;
                recordHolder.Save();
                node = nextNode;
            }
#if DUMP_FINALIZE
            System.Diagnostics.Trace.WriteLine(this.mOriginalFile + " Clear Records");
#endif

            mRecordsByRecordNo.Clear();
            mRecordsByLastUsed.Clear();
            mRecordCount = 0;
            mStream.SetLength(mHeaderWidth);
        }


        internal void InternalReadRecordFromDisk(UInt32 recordNo, Byte[] buffer)
        {
            Seek(recordNo);
            mStream.Read(buffer, 0, mRecordWidth);
            mCurrentRecordNoPosition = recordNo + 1;
#if DUMP_DISK_ACTIVITY
            System.Diagnostics.Debug.WriteLine(mOriginalFile + " ReadFromDisk Record #" + recordNo);
            System.Diagnostics.Debug.WriteLine(Utils.HexDump(buffer));
#endif
        }

        internal void InternalSaveRecordToDisk(UInt32 recordNo, Byte[] buffer)
        {
            Seek(recordNo);
            mStream.Write(buffer, 0, mRecordWidth);
            mCurrentRecordNoPosition = recordNo + 1;
#if DUMP_DISK_ACTIVITY
            System.Diagnostics.Debug.WriteLine(mOriginalFile + " SaveToDisk Record #" + recordNo);
            System.Diagnostics.Debug.WriteLine(Utils.HexDump(buffer));
#endif
        }

    }
}

