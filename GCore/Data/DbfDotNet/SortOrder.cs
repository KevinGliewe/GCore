using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace GCore.Data.DbfDotNet
{
    using GCore.Data.DbfDotNet.Core;

    public class SortOrder<TRecord>
        where TRecord : new()
    {
        private List<IndexColumn> mColumns;
        private bool mUnique;
        private static Type mRecordType;

        static SortOrder()
        {
            mRecordType = typeof(TRecord);
        }

        public SortOrder(bool unique)
        {
            mUnique = unique;
            mColumns = new List<IndexColumn>();            
        }

        public void AddField(string columnName)
        {
            AddField(columnName,true);
        }

        public void AddField(string columnName, bool ascending)
        {
            AddField(columnName, ascending, 0);
        }

        public void AddField(string columnName, bool ascending, int width)
        {
            var field = mRecordType.GetField(columnName, 
                BindingFlags.Public 
                | BindingFlags.NonPublic 
                | BindingFlags.IgnoreCase 
                | BindingFlags.Instance);
            if (field == null) throw new InvalidColumnException(columnName, "Unknown sort order column");
            mColumns.Add(new IndexColumn() { ColumnName = columnName, Ascending = ascending, Width = width });
        }

        internal List<IndexColumn> Fields
        {
            get
            {
                return mColumns;
            }
        }

        internal bool IsUnique
        {
            get { return mUnique; }
        }

        internal string ToKeyString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < mColumns.Count; i++)
            {
                if (i > 0) sb.Append(' ');
                sb.Append(mColumns[i].ColumnName);
            }
            return sb.ToString();
        }
    }
}
