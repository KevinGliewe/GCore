using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Data.DbfDotNet.Linq
{
    using System.Linq;
    using System.Linq.Expressions;

    public class DbfTable<TRecord> : GCore.Data.DbfDotNet.DbfTable<TRecord>, IQueryProvider, IQueryable<TRecord>
        where TRecord : DbfRecord, new()
    {
        private System.Linq.Expressions.Expression mExpression;

        public DbfTable(string filepath, System.Text.Encoding encoding, DbfVersion version)
            : base(filepath, encoding, version)
        {
            mExpression = Expression.Constant(this);
        }

        #region IQueryProvider Members

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {
            if (typeof(TRecord) == typeof(TElement))
            {
                return (IQueryable<TElement>)new DbfQuery<TRecord>(this, expression);
            }
            throw new Exception("TElement should be TRecord");
        }

        IQueryable IQueryProvider.CreateQuery(System.Linq.Expressions.Expression expression)
        {
            return new DbfQuery<TRecord>(this, expression);
        }

        TResult IQueryProvider.Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        object IQueryProvider.Execute(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<TRecord> Members

        IEnumerator<TRecord> IEnumerable<TRecord>.GetEnumerator()
        {
            for (UInt32 i = 0; i < RecordCount; i++)
            {
                TRecord r = this.GetRecord(i);
                yield return r;
            }
        }

        //private new TRecord GetRecord(int i)
        //{
        //    throw new NotImplementedException();
        //}

        //private new int RecordCount
        //{
        //    get{ return 0; } 
        //}

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (UInt32 i = 0; i < RecordCount; i++)
            {
                TRecord r = this.GetRecord(i);
                yield return r;
            }
        }

        #endregion

        #region IQueryable Members

        Type IQueryable.ElementType
        {
            get { throw new NotImplementedException(); }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return mExpression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return this; }
        }

        #endregion

    }
}
