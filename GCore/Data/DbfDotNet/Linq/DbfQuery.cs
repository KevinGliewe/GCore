using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Data.DbfDotNet.Linq
{
    using System.Linq;
    using System.Linq.Expressions;

    class DbfQuery<TRecord> : IQueryable<TRecord>
        where TRecord : DbfRecord, new()
    {
        Expression mExpression;
        DbfTable<TRecord> mTable;

        public DbfQuery(DbfTable<TRecord> table, Expression expression)
        {
            mTable = table;
            mExpression = expression;
        }


        #region IEnumerable<TRecord> Members

        IEnumerator<TRecord> IEnumerable<TRecord>.GetEnumerator()
        {
            return null;
            // this is where the fun starts
            //var mIndices = mTable.o.OpenIndexes;
            //if (mIndices.Length == 0)
            //{
            //    return mTable.GetEnumerator();
            //}
            //else
            //{
            //    DbfIndex<TRecord> index = mTable.OpenIndexes[0];
            //    return index.GetEnumerator();
            //}
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
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
            get { return mTable; }
        }

        #endregion
    }
}
