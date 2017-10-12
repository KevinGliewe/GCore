using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{
 
    public class DbfTable<TRecord> : IEnumerable<TRecord>
        where TRecord : DbfRecord, new()
    {
        internal Core.DbfFile<TRecord> mDbfFile;

        public DbfTable(string filePath, System.Text.Encoding encoding, DbfVersion version)
        {
            mDbfFile = Core.DbfFile<TRecord>.Get(filePath, encoding, version);
        }

        public TRecord NewRecord()
        {
            return mDbfFile.NewRecord();
        }

        #region IEnumerable<TRecord> Members

        public IEnumerator<TRecord> GetEnumerator()
        {
            return mDbfFile.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        //public NdxFile<TRecord>[] OpenIndexes
        //{
        //    get {
        //        return this.mDbfFile.mIndexes.ToArray();
        //    }
        //}


        public int RecordCount
        {
            get { return (int)mDbfFile.RecordCount; }
        }

        public List<Core.ColumnDefinition> Columns
        {
            get { return mDbfFile.Columns; }
        }
        
        public TRecord GetRecord(UInt32 p)
        {
            return (TRecord)mDbfFile.InternalGetRecord((uint)p, false);
        }

        public void Close()
        {
            mDbfFile.StopReading();
            mDbfFile = null;
        }

        public void EmptyTable()
        {
            mDbfFile.EmptyTable();
        }

        public DbfIndex<TRecord> GetIndex(string filepath, SortOrder<TRecord> sortOrder)
        {
            var indexTable = GCore.Data.DbfDotNet.Core.NdxFile<TRecord>.Get(filepath, this.mDbfFile, sortOrder);
            if (indexTable.mDbfIndex==null)
            {
                indexTable.mDbfIndex = new DbfIndex<TRecord>();
                indexTable.mDbfFile = mDbfFile;
                indexTable.mDbfIndex.mIndexTable = indexTable;
            }
            return (DbfIndex<TRecord>)indexTable.mDbfIndex;
        }
    }
}
