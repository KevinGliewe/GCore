using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{

    public class DbfIndex<TRecord> : IEnumerable<TRecord>
        where TRecord : DbfRecord, new()
    {
        internal Core.NdxFile<TRecord> mIndexTable;
        
        internal DbfIndex() { }

        #region IEnumerable<TRecord> Members

        public IEnumerator<TRecord> GetEnumerator()
        {
            return mIndexTable.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mIndexTable.GetEnumerator();
        }

        #endregion

        public void Dump()
        {
            this.mIndexTable.Dump();
        }

        public TRecord GetRecord(UInt32 p)
        {
            return mIndexTable.GetRecordByRowIndex(p);
        }

    }
}
