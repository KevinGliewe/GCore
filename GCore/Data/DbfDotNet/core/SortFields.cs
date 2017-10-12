using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    [GCore.Data.DbfDotNet.Record(FieldMapping= FieldMapping.ExplicitColumnsOnly)]
    internal abstract class SortFields
    {
        internal void Read(NdxFile ndxFile, object dbfRecord)
        {
            throw new NotImplementedException();
        }

        internal void Read(NdxFile ndxFile, Byte[] recordBuffer)
        {
            throw new NotImplementedException();
        }

        protected abstract bool ChainCompareIsEqual(object other, int[] result);
        
        #region IChainCompare Members

        public bool ChainCompare(SortFields other, int[] result)
        {
            return ChainCompareIsEqual(other, result);
        }

        #endregion

        internal SortFields Clone()
        {
            SortFields result = (SortFields)base.MemberwiseClone();
            return result;
        }
    }

    // in .Net 3.5 you can't create types dynamically.
    // Using those SortField below however we can create dynamic class with fixed field count but arbitrary types.
    // This is useful for indexes

#pragma warning disable 649

    internal class SortFields<T1> : SortFields
        where T1 : IComparable<T1>
    {
        public T1 f1;

        protected override bool ChainCompareIsEqual(object other, int[] result)
        {
            int cmp = f1.CompareTo(((SortFields<T1>)other).f1);
            result[0] = cmp;
            return cmp == 0;
        }

        public override string ToString()
        {
            return f1.ToString();
        }
    }

    internal class SortFields<T1, T2> : SortFields<T1>
        where T1 : IComparable<T1>
        where T2 : IComparable<T2>
    {
        public T2 f2;

        protected override bool ChainCompareIsEqual(object other, int[] result)
        {
            if (base.ChainCompareIsEqual(other, result))
            {
                int cmp = f2.CompareTo(((SortFields<T1, T2>)other).f2);
                result[1] = cmp;
                return cmp == 0;
            }
            else return false;
        }
    }

    // that's enough for now (but we should extend to at least 4 or 5)
}
