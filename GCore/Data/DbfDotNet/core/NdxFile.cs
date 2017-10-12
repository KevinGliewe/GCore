using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace GCore.Data.DbfDotNet.Core
{
    internal abstract class NdxFile : ClusteredFile, IEnumerable, IHasEncoding
    {
        protected static QuickSerializer mNdxHeaderWriter;
        protected static QuickSerializer mNdxEntryWriter;
        
        internal ConstructorInfo mSortFieldsConstructor;
        internal byte[] mSortFieldsReadBuffer;
        internal QuickSerializer mSortFieldsWriter;
        internal QuickSerializer mSortFieldsDbfBufferReader;
        internal int mSortFieldsCount;
        internal NdxHeader mNdxHeader;
        protected Encoding mEncoding;

        static NdxFile()
        {
            mNdxHeaderWriter = new QuickSerializer(typeof(NdxHeader), DbfVersion.DbfDotNet, null, 0, false, true);
            mNdxEntryWriter = new QuickSerializer(typeof(NdxEntry), DbfVersion.DbfDotNet, null, 0, false, true);
        }

        public NdxFile()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return InternalGetEnumerator();
        }

        protected abstract IEnumerator InternalGetEnumerator();

        #region IHasEncoding Members

        Encoding IHasEncoding.Encoding
        {
            get { return mEncoding; }
        }

        #endregion

        internal override protected Record InternalGetRecord(UInt32 recordNo, bool returnNullIfNotInCache)
        {
            throw new Exception("Invalid call, InternalGetPage should be called on NdxFile rather than InternalGetRecord");
        }

        internal NdxPage InternalGetPage(NdxPage parentPage, UInt32 recordNo, bool returnNullIfNotInCache)
        {
            NdxPage record = (NdxPage)base.InternalGetRecord(recordNo, returnNullIfNotInCache);
            if (record != null)
            {
                record.ParentPage = parentPage;
            }
            return record;
        }

        public override object NewRecord()
        {
            throw new Exception("Invalid call, NewPage should be called on NdxFile rather than NewRecord");
        }

        public NdxPage NewPage(NdxPage parentPage)
        {
            NdxPage record = (NdxPage)base.NewRecord();
            record.ParentPage = parentPage;
            return record;
        }

        internal void Dump()
        {
            System.Diagnostics.Debug.WriteLine("-- Dump index:" + this.mOriginalFile + "------------------------");
            System.Diagnostics.Debug.WriteLine("Root: " + this.mNdxHeader.StartingPageRecordNo);

            System.Diagnostics.Debug.WriteLine(this.mRecordCount + " records");

            NdxPage page = InternalGetPage((NdxPage)null, (uint)this.mNdxHeader.StartingPageRecordNo, false);
            NdxEntry lastNode = null;
            page.Dump("", null, null, true, ref lastNode);
            System.Diagnostics.Debug.WriteLine("-----------------------------------------------------------------------------------");
        }

    }

    internal class NdxFile<TRecord> : NdxFile
        where TRecord : DbfRecord, new()
    {
        private Type mSortFieldType;
        internal DbfFile<TRecord> mDbfFile;
        private SortOrder<TRecord> mSortOrder;
        internal DbfIndex<TRecord> mDbfIndex;
        
        static NdxFile()
        {
        }

        public NdxFile()
        {
        }

        internal static NdxFile<TRecord> Get(string filePath, DbfFile<TRecord> dbfFile, SortOrder<TRecord> sortOrder)
        {
            var file = ClusteredFile.Get<NdxFile<TRecord>>(filePath, OpenFileMode.OpenOrCreate, 123);
            if (file.ReaderCount==1)
            {
                file.mDbfFile = dbfFile;
                file.mSortOrder = sortOrder;
                file.Initialize();                
            }
            else
            {
                // perhaps check encoding and version match 
            }
            return file;
        }

        public static object CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            System.Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }

        protected override Record OnCreateNewRecord(bool isNew, UInt32 recordNo)
        {
            return new NdxPage();
        }

        protected override void OnReadRecordBuffer(byte[] buffer, Record record)
        {
            NdxPage ndxPage = (NdxPage)record;
            ndxPage.OnReadRecord(buffer);
        }

        internal override bool OnFillWriteBuffer(Record record, Byte[] buffer)
        {
            NdxPage ndxPage = (NdxPage)record;
            if (ndxPage.mIsModified)
            {
                ndxPage.mIsModified = false;
                ndxPage.OnFillWriteBuffer(buffer);
                return true;
            }
            else return false;
        }

        #region IEnumerable Members

        private IEnumerable<TRecord> Enumerate(NdxPage parentPage, UInt32 pageNo)
        {
            NdxPage page = InternalGetPage(parentPage, pageNo, false);
            for (int i = 0; i < page.EntriesCount; i++)
            {
                NdxEntry entry = page.GetEntry(i);

                if (entry.DbfRecordNo == 0)
                {
                    foreach (TRecord record in Enumerate(page, entry.LowerPageRecordNo))
                    {
                        yield return record;
                    }
                }
                else
                {
                    TRecord result = (TRecord)mDbfFile.InternalGetRecord(entry.DbfRecordNo - 1, false);
                    yield return result;
                }
            }
           // page.ReleaseLock();
        }
     
        #endregion

        public new IEnumerator<TRecord> GetEnumerator()
        {
            foreach (TRecord r in Enumerate(null, this.mNdxHeader.StartingPageRecordNo))
            {
                yield return r;
            }
        }

        protected override IEnumerator InternalGetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        internal void OnDbfRecordModified(UInt32 recordNo, byte[] oldBuffer, byte[] newBuffer)
        {
            UpdateIndex(recordNo, oldBuffer, newBuffer);
        }

        private void UpdateIndex(UInt32 recordNo, byte[] oldBuffer, byte[] newBuffer)
        {
            SortFields oldNdxEntryFields =null;
            SortFields newNdxEntryFields=null;

            if (oldBuffer != null)
            {
                // remove old key
                oldNdxEntryFields = (SortFields)mSortFieldsConstructor.Invoke(null);
                mSortFieldsDbfBufferReader.Read(this, oldBuffer, oldNdxEntryFields);
            }

            if (newBuffer != null)
            {
                // remove old key
                newNdxEntryFields = (SortFields)mSortFieldsConstructor.Invoke(null);
                mSortFieldsDbfBufferReader.Read(this, newBuffer, newNdxEntryFields);
            }

            if (oldNdxEntryFields != null && newNdxEntryFields != null)
            {
                var result=new int[this.mSortFieldsCount];
                var isEqual = oldNdxEntryFields.ChainCompare(newNdxEntryFields, result);
                if (isEqual) return;
            }

            if (oldBuffer != null)
            {
                // we should delete the old index entry
                NdxPage page;
                page = InternalGetPage(null, this.mNdxHeader.StartingPageRecordNo, false);
                NdxEntry oldEntry = new NdxEntry() { DbfRecordNo = recordNo, mNdxNativeLowerPageNo = 0, Fields = oldNdxEntryFields };
#if DUMP_INSERTS
                System.Diagnostics.Trace.WriteLine("Removing entry " + oldEntry.Fields.ToString() + " #" + oldEntry.DbfRecordNo);
                System.Diagnostics.Trace.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif
                page.LocateAndDelete(oldEntry);
            }

            if (newBuffer != null)
            {
                // create new key
                var ndxEntryFields = (SortFields)mSortFieldsConstructor.Invoke(null);
                mSortFieldsDbfBufferReader.Read(this, newBuffer, ndxEntryFields);

                // now we can try to insert this tuple in the index
                NdxPage page;

                if (this.mRecordCount == 0)
                {
                    page = (NdxPage)NewPage(null);
                    this.mNdxHeader.StartingPageRecordNo = page.RecordNo;
                }
                else page = InternalGetPage(null, this.mNdxHeader.StartingPageRecordNo, false);
                System.Diagnostics.Debug.Assert(page.ParentPage == null, "page.mParentPage should be null.");

                NdxEntry newEntry = new NdxEntry() { DbfRecordNo = recordNo, mNdxNativeLowerPageNo = 0, Fields = ndxEntryFields };
                // System.Diagnostics.Trace.WriteLine("Inserting " + recordNo);
#if DUMP_INSERTS
                System.Diagnostics.Trace.WriteLine("Inserting entry " + newEntry.Fields.ToString() + " #" + newEntry.DbfRecordNo);
                System.Diagnostics.Trace.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif
                page.LocateAndInsert(newEntry);
            }
        }

        

        private void GetNewSortField(object dbfRecord)
        {
            throw new NotImplementedException();
        }

        private void GetOldSortField(object dbfRecord)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            if (mFileAccess != FileAccess.Read)
            {
                base.WriteHeader();
                base.Flush();
            }
        }

        protected override void OnWriteHeader(Stream mStream)
        {
            Byte[] headerBuff = new byte[mNdxHeaderWriter.RecordWidth];
            mNdxHeaderWriter.Write(this, headerBuff, mNdxHeader);
            mStream.Seek(0, 0);
            mStream.Write(headerBuff, 0, headerBuff.Length);
        }


        public int CountChildren()
        {
            NdxPage page = InternalGetPage((NdxPage)null, (uint)this.mNdxHeader.StartingPageRecordNo, false);
            return page.CountChildren(int.MaxValue);
        }

        protected override void OnInitialize(Stream stream)
        {
            mEncoding = mDbfFile.Encoding;
            mDbfFile.mIndexes.Add(this);

            var types = new List<Type>();
            var sortOrderColumns = new List<ColumnDefinition>();
            var dbfColumns = new List<ColumnDefinition>();

            if (mSortOrder == null)
                mSortOrder = new SortOrder<TRecord>(false);

            Byte[] headerBuff = new byte[mNdxHeaderWriter.RecordWidth];
            mNdxHeader = new NdxHeader();
            int read = stream.Read(headerBuff, 0, headerBuff.Length);

            if (read > 0)
            {
                mNdxHeaderWriter.Read(this, headerBuff, mNdxHeader);
                if (mNdxHeader.KeyString1.Length < 255)
                    mNdxHeader.KeyString2 = string.Empty;

                var keyString = (mNdxHeader.KeyString1 + mNdxHeader.KeyString2).Trim();
                if (keyString.Length > 0)
                {
                    mSortOrder.Fields.Clear();
                    mSortOrder.AddField(keyString);
                }
            }
            mRecordCount = (UInt32)(stream.Length / 512);
            mSortFieldsCount = mSortOrder.Fields.Count;

            if (mSortFieldsCount <= 0)
                throw new Exception("An Index require at least one column");
            else if (mSortFieldsCount > 5)
                throw new Exception("An Index cannot be defined on more than 5 columns.");

            for (int i = 0; i < mSortFieldsCount; i++)
            {
                var ic = mSortOrder.Fields[i];
                var cd = mDbfFile.GetColumnByName(ic.ColumnName);
                types.Add(cd.mFieldInfo.FieldType);

                var sortOrderColumn = new ColumnDefinition();
                sortOrderColumn.Initialize("f" + (i + 1).ToString(), cd, ic.Ascending, ic.Width);
                sortOrderColumns.Add(sortOrderColumn);

                var dbfColumn = new ColumnDefinition();
                dbfColumn.Initialize("f" + (i + 1).ToString(), cd, ic.Ascending, ic.Width);
                dbfColumn.mOffset = cd.mOffset;
                dbfColumn.mWidth = cd.mWidth;
                dbfColumn.mDecimals = cd.mDecimals;
                dbfColumns.Add(dbfColumn);

            }

            mSortFieldType = typeof(SortFields<>).MakeGenericType(types.ToArray());
            mSortFieldsConstructor = mSortFieldType.GetConstructor(new Type[] { });

            mSortFieldsWriter = new QuickSerializer(mSortFieldType, mDbfFile.Version, sortOrderColumns, 0, false, true);
            mSortFieldsReadBuffer = new byte[mSortFieldsWriter.RecordWidth];

            mSortFieldsDbfBufferReader = new QuickSerializer(mSortFieldType, mDbfFile.Version, dbfColumns, 0, true, /*setOffset*/ false);

            if (read == 0)
            {
                mRecordWidth = 512;
                mHeaderWidth = 512;
                mNdxHeader.TotalNoOfPages = 0;
                mNdxHeader.KeyString1 = mSortOrder.ToKeyString();
                mNdxHeader.KeyString2 = "";
                mNdxHeader.KeyType = 0;
                mNdxHeader.SizeOfKeyRecord = (uint)mSortFieldsWriter.RecordWidth;
                mNdxHeader.NoOfKeysPerPage = (ushort)((512 - 4) / (mSortFieldsWriter.RecordWidth + 8));
                mNdxHeader.StartingPageRecordNo = UInt32.MaxValue;
                mNdxHeader.TotalNoOfPages = 0;
                mNdxHeader.UniqueFlag = mSortOrder.IsUnique;
                mRecordCount = 0;
                if (mDbfFile != null && mDbfFile.RecordCount > 0)
                {
                    Byte[] newBuffer = null;
                    for (UInt32 i = 0; i < mDbfFile.RecordCount; i++)
                    {
                        TRecord r = mDbfFile.InternalGetRecord(i);
                        newBuffer = r.mHolder.GetCurrentBuffer(/*readIfNeeded*/true);
                        UpdateIndex((uint)i, null, newBuffer);
#if DUMP_INSERTS
                            Dump();
                            var count = CountChildren();
                            System.Diagnostics.Debug.Assert(count == i + 1, "CountChildren:" + count + " should be:" + (i + 1));
#endif
                    }
                }
            }
            else
            {
                // perhaps we should read from the header
                mRecordWidth = 512;
                mHeaderWidth = 512;
            }
        }


        internal TRecord GetRecordByRowIndex(UInt32 p)
        {

            if (p == mLastRecordByRowIndex)
            {
                return mLastRecordByRowRecord;
            }
            else
            {
                mLastRecordByRowIndex = p;
                NdxPage page = (NdxPage)InternalGetPage(null, this.mNdxHeader.StartingPageRecordNo, false);
                System.Diagnostics.Debug.Assert(page.ParentPage == null, "page.mParentPage should be null.");
                mLastRecordByRowRecord = GetRecordSlow(page, ref p);
                return mLastRecordByRowRecord;
            }
        }

        private UInt32 mLastRecordByRowIndex=UInt32.MaxValue;
        private TRecord mLastRecordByRowRecord;

        private TRecord GetRecordSlow(NdxPage page, ref UInt32 p)
        {
            TRecord result;
            for (int i = 0; i < page.EntriesCount; i++)
            {
                var entry = page.GetEntry(i);

                if (entry.LowerPageRecordNo == UInt32.MaxValue)
                {
                    if (p == 0) return mDbfFile.InternalGetRecord(entry.DbfRecordNo);
                    p--;
                }
                else
                {
                    var ndxPage2 = InternalGetPage(
                        page,
                        entry.LowerPageRecordNo, 
                        /* returnNullIfNotInCache */ false) as NdxPage;
                    result = GetRecordSlow(ndxPage2, ref p);
                    if (result!=null) return result;
                }
            }
            return null;
        }

    }
}
