using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    [Record(FieldMapping = FieldMapping.ExplicitColumnsOnly)]
    internal class NdxEntry 
    {
        [Column(Type = ColumnType.UINT32)]
        internal UInt32 mNdxNativeLowerPageNo;

        [Column(Type = ColumnType.UINT32)]
        public UInt32 DbfRecordNo;

        private SortFields mFields;
        
        public SortFields Fields
        {
            get { return mFields; }
            set { 
                if (mFields==value) return;
                mFields = value;
            }
        }

        public UInt32 LowerPageRecordNo
        {
            get { return mNdxNativeLowerPageNo - 1; }
            set { mNdxNativeLowerPageNo = value + 1; }
        }

        public int ChainCompare(int[] cmp, NdxFile index, NdxEntry other)
        {
            this.Fields.ChainCompare(other.Fields, cmp);
            // to do check for ascending descending
            for (int i = 0; i < cmp.Length; i++)
            {
                if (cmp[i] != 0) return cmp[i];
            }
            // if we're here the fields are identical
            // we compare RecordNo when the index is NOT unique
            if (index.mNdxHeader.UniqueFlag) return 0;
            else return DbfRecordNo.CompareTo(other.DbfRecordNo);
            
        }

        public override string ToString()
        {
            return Fields.ToString() + " " + DbfRecordNo.ToString();
        }
    }
}
