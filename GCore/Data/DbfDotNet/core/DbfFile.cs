using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GCore.Data.DbfDotNet.Core
{
    internal class DbfFile<TRecord> : ClusteredFile, IHasEncoding, IEnumerable<TRecord>
        where TRecord : DbfRecord, new() 
    {
        private static QuickSerializer mDbfHeaderWriter;
        private static QuickSerializer mDbfColumnWriter;
        private QuickSerializer mRecordWriter;
        internal List<NdxFile<TRecord>> mIndexes;
        private DbfHeader mDbfHeader;
        private byte END_OF_COLUMN_DEFINITIONS = 13;
        protected System.Text.Encoding mEncoding;
        protected DbfVersion mVersion;

        static DbfFile()
        {
            mDbfHeaderWriter = new QuickSerializer(typeof(DbfHeader), DbfVersion.DbfDotNet, null, 0, false, true);
            mDbfColumnWriter = new QuickSerializer(typeof(DbfColumnHeader), DbfVersion.DbfDotNet, null, 0, false, true);
        }

        public DbfFile()
        {
            mIndexes = new List<NdxFile<TRecord>>();
        }
        
        public DbfVersion Version
        {
            get { return mVersion; }
        }

        public System.Text.Encoding Encoding
        {
            get { return mEncoding; }
        }

      
        protected override void OnReadRecordBuffer(byte[] mBuffer, Record result)
        {
            mRecordWriter.Read(this, mBuffer, (TRecord)result);
        }


        public new TRecord NewRecord()
        {
            return (TRecord)base.NewRecord();
        }

        public UInt32 RecordCount { get { return mRecordCount; } }

        internal override bool OnFillWriteBuffer(Record record, Byte[] buffer)
        {
            mRecordWriter.Write(this, buffer, (TRecord)record);
            return true;
        }

        internal override void OnWriteBufferModified(UInt32 recordNo, byte[] oldBuffer, byte[] newBuffer)
        {
            for (int i = 0; i < mIndexes.Count; i++)
            {
                NdxFile<TRecord> index = mIndexes[i];
                index.OnDbfRecordModified(recordNo, oldBuffer, newBuffer);
            }
        }

        protected override Record OnCreateNewRecord(bool isNew, UInt32 recordNo)
        {
            return new TRecord();
        }


        #region IEnumerable<TRecord> Members

        public IEnumerator<TRecord> GetEnumerator() // IEnumerable<TRecord>.
        {
            for (UInt32 i = 0; i < mRecordCount; i++)
            {
                TRecord r = InternalGetRecord(i);
                yield return r;
            }
        }


        public TRecord InternalGetRecord(UInt32 recordNo)
        {
            TRecord r = (TRecord)InternalGetRecord(recordNo, false);
            return r;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<TRecord> Members

        IEnumerator<TRecord> IEnumerable<TRecord>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void WriteHeader(Stream stream, bool withColumns)
        {
            mDbfHeader.NbRecords = (UInt32)mRecordCount;

            UInt16 newHeaderWith = (UInt16)(mDbfHeaderWriter.RecordWidth + (mColumns.Count-1) * mDbfColumnWriter.RecordWidth + 1);
            mDbfHeader.HeaderWidth = newHeaderWith;
            mHeaderWidth = newHeaderWith;

            Byte[] headerBuff = new byte[mDbfHeaderWriter.RecordWidth];
            mDbfHeaderWriter.Write(this, headerBuff, mDbfHeader);
            
            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(headerBuff, 0, headerBuff.Length);
#if DUMP_DISK_ACTIVITY
            System.Diagnostics.Debug.WriteLine(mOriginalFile + " WriteHeader");
            System.Diagnostics.Debug.WriteLine(Utils.HexDump(headerBuff));
#endif

            if (withColumns)
            {
                Byte[] columnHeaderBuff = new byte[mDbfColumnWriter.RecordWidth];
                DbfColumnHeader dbfColumnHeader = new DbfColumnHeader();
                // we ignore column 0
                for (int i = 1; i < mColumns.Count; i++)
                {
                    ColumnDefinition c = mColumns[i];
                    dbfColumnHeader.ColumnName = c.mColumnName;
                    dbfColumnHeader.ColumnType = c.DbfColumnType;
                    dbfColumnHeader.ColumnWidth = (byte)c.mWidth;
                    dbfColumnHeader.Decimals = (byte)c.mDecimals;

                    mDbfColumnWriter.Write(this, columnHeaderBuff, dbfColumnHeader);
                    stream.Write(columnHeaderBuff, 0, columnHeaderBuff.Length);
#if DUMP_DISK_ACTIVITY
                    System.Diagnostics.Debug.WriteLine(mOriginalFile + " WriteColumn #" + i);
                    System.Diagnostics.Debug.WriteLine(Utils.HexDump(columnHeaderBuff));
#endif

                }
                stream.WriteByte(END_OF_COLUMN_DEFINITIONS);
            }
            base.mCurrentRecordNoPosition = UInt32.MaxValue;
        }

        protected override void OnDisposing()
        {
            base.OnDisposing();
            for (int i = 0; i < mIndexes.Count; i++)
            {
                mIndexes[i].Flush();
            }
            mIndexes.Clear();
        }

        internal static DbfFile<TRecord> Get(string filePath, System.Text.Encoding encoding, DbfVersion version)
        {
            var file = ClusteredFile.Get<DbfFile<TRecord>>(filePath, OpenFileMode.OpenOrCreate, 4096);
            if (file.ReaderCount==1)
            {
                file.mEncoding = encoding;
                file.mVersion = version;
                file.Initialize();
            }
            else
            {
                // perhaps check encoding and version match 
            }
            return file;
        }

        private new void Initialize()
        {
            base.Initialize();
        }

        internal List<ColumnDefinition> Columns
        {
            get { return mRecordWriter.Columns; }
        }

        protected override void OnWriteHeader(Stream stream)
        {
            WriteHeader(stream, false);
        }

        protected override void OnInitialize(Stream stream)
        {

            Byte[] headerBuff = new byte[mDbfHeaderWriter.RecordWidth];
            mDbfHeader = new DbfHeader();
            int read = stream.Read(headerBuff, 0, headerBuff.Length);

            if (read > 0)
            {
                mDbfHeaderWriter.Read(this, headerBuff, mDbfHeader);
                //File size reported by the operating system must match the logical file size. Logical file size = ( Length of header + ( Number of records * Length of each record ) ) 
                //mHeaderSerializer.mQuickWriteMethod(this, headerBuff, header);
                var nbColumns = ((mDbfHeader.HeaderWidth - mDbfHeaderWriter.RecordWidth) / mDbfColumnWriter.RecordWidth) + 1;
                var totalLength = stream.Length;
                mRecordWidth = mDbfHeader.RecordWidth;
                var recordsLength = totalLength - mDbfHeader.HeaderWidth;
                if (mRecordWidth > 0)
                {
                    var columns = new List<ColumnDefinition>(nbColumns);
                    mRecordCount = (UInt32)(recordsLength / mRecordWidth);

                    Byte[] columnHeaderBuff = new byte[mDbfColumnWriter.RecordWidth];
                    DbfColumnHeader dbfColumnHeader = new DbfColumnHeader();
                    ColumnDefinition columnDefinition = null;
                    //new ColumnDefinition();
                    //columnDefinition.Initialize(DbfVersion.dBaseIII, 0, typeof(DbfRecord).GetField("DeletedFlag",
                    //    BindingFlags.Instance
                    //   | BindingFlags.Public
                    //   | BindingFlags.NonPublic
                    //   | BindingFlags.DeclaredOnly), 0, null);
                    //columns.Add(columnDefinition);

                    for (int i = 1; i < nbColumns; i++)
                    {
                        read = stream.Read(columnHeaderBuff, 0, columnHeaderBuff.Length);
                        mDbfColumnWriter.Read(this, columnHeaderBuff, dbfColumnHeader);
                        System.Diagnostics.Trace.WriteLine(string.Format("{0},{1},{2},{3}",
                            dbfColumnHeader.ColumnName,
                            dbfColumnHeader.ColumnType,
                            dbfColumnHeader.ColumnWidth,
                            dbfColumnHeader.Decimals));

                        columnDefinition = new ColumnDefinition();
                        columnDefinition.Initialize(dbfColumnHeader.ColumnName, dbfColumnHeader.ColumnType, dbfColumnHeader.ColumnWidth, dbfColumnHeader.Decimals);
                        columns.Add(columnDefinition);
                    }
                    int lastByte = stream.ReadByte();
                    if (lastByte == END_OF_COLUMN_DEFINITIONS)
                    {
                        mColumns = columns;
                        mHeaderWidth = mDbfHeader.HeaderWidth;
                    }
                }
            }
            mRecordWriter = new QuickSerializer(
                typeof(TRecord),
                mVersion,
                mColumns,
                mRecordWidth,
                false,
                true);

            if (mColumns == null)
            {
                mColumns = mRecordWriter.Columns;
                mRecordWidth = mRecordWriter.RecordWidth;

                mDbfHeader.VerNumber = 131;
                mDbfHeader.LastUpdate = DateTime.Now;
                mDbfHeader.NbRecords = 0;
                var nbColumns = mColumns.Count;
                mDbfHeader.HeaderWidth = (UInt16)(mDbfHeaderWriter.RecordWidth + mDbfColumnWriter.RecordWidth * nbColumns + 1);
                mDbfHeader.RecordWidth = (UInt16)mRecordWriter.RecordWidth;
                mDbfHeader.Zero = 0;
                mDbfHeader.IncompleteTransaction = 0;
                mDbfHeader.EncryptionFlag = 0;
                //mDbfHeader.LanOnly = [];
                mDbfHeader.Indexed = 0;
                mDbfHeader.Language = 0;
                mDbfHeader.Zero2 = 0;

                WriteHeader(stream, /*withColumns*/true);
            }
        }

    }
}
