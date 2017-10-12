using System;

namespace GCore.Data.DbfDotNet
{
    public class ColumnAttribute : Attribute
    {
        string mColumnName;
        int mWidth;
        int mDecimals;
        
        OverflowBehaviour mOverflowBehaviour;
        ColumnType mColumnType;

        public ColumnAttribute()
        {
            mWidth = -1;
            mDecimals = -1;
            mColumnType = ColumnType.UNKNOWN;
        }
        
        public string ColumnName
        {
            get { return mColumnName; }
            set { mColumnName = value; }
        }
        
        /// <summary>
        /// Full width of the column (including dot and decimals).
        /// </summary>
        [System.ComponentModel.Description("Full width of the column (including dot and decimals)")] 
        public int Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        public int Decimals
        {
            get { return mDecimals; }
            set { mDecimals = value; }
        }

        public OverflowBehaviour OverflowBehaviour
        {
            get { return mOverflowBehaviour; }
            set { mOverflowBehaviour = value; }
        }

        public ColumnType Type
        {
            get { return mColumnType; }
            set { mColumnType = value; }
        }
   }

    public class RecordAttribute : Attribute
    {
        DbfVersion mVersion;
        FieldMapping mColumnMapping;
        OverflowBehaviour mOverflowBehaviour;
        int mWidth;

        public RecordAttribute()
        {
            mColumnMapping = FieldMapping.PrivateFields | FieldMapping.PublicFields;
            mOverflowBehaviour = OverflowBehaviour.ThrowError;
        }

        public DbfVersion Version
        {
            get { return mVersion; }
            set { mVersion = value; }
        }

        public FieldMapping FieldMapping
        {
            get { return mColumnMapping; }
            set { mColumnMapping = value; }
        }

        public OverflowBehaviour OverflowBehaviour
        {
            get { return mOverflowBehaviour; }
            set { mOverflowBehaviour = value; }
        }

        public int Width
        {
            get { return mWidth; }
            set { mWidth = Width; }
        }


    }
}
