using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace GCore.Data.DbfDotNet.Core
{
    public class ColumnDefinition
    {
        public FieldInfo mFieldInfo;
        
        internal int mOffset;
        internal TypeCode NativeTypeCode;
        internal bool IsNullable;
        internal int ColumnIndex;
        internal string mColumnName;
        internal int mWidth;
        internal int mDecimals;
        internal ColumnType mColumnType;
        internal ColumnDefinition mOriginalColumn;
        internal bool mAscending = true;
        internal DbfVersion mVersion;
        
        public ColumnDefinition()
        {
        }

        // called when created from existing DBF file
        public void Initialize(DbfVersion version, FieldInfo fieldinfo, ColumnAttribute columnAttribute)
        {
            this.mFieldInfo = fieldinfo;
            this.mVersion = version;
            if (columnAttribute == null) columnAttribute = new ColumnAttribute();

            Type fieldType = mFieldInfo.FieldType;

            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                IsNullable=true;
                NativeTypeCode= Type.GetTypeCode(fieldType.GetGenericArguments()[0]);
            }
            else this.NativeTypeCode = Type.GetTypeCode(fieldType);
            

            mColumnName = (columnAttribute.ColumnName == null || columnAttribute.ColumnName.Length == 0 ?
                fieldinfo.Name : columnAttribute.ColumnName);

            mColumnType = columnAttribute.Type;
            mWidth = columnAttribute.Width;
            mDecimals = columnAttribute.Decimals;
            int defaultWidth, defaultDecimals;

            if (mColumnType == ColumnType.UNKNOWN)
            {
                if (NativeTypeCode == TypeCode.Object)
                {
                    GetDefaultColumnType(fieldType, mVersion, out mColumnType, out defaultWidth, out defaultDecimals);
                }
                else
                {
                    GetDefaultColumnType(NativeTypeCode, IsNullable, mVersion, out mColumnType, out defaultWidth, out defaultDecimals);
                }
                if (mWidth == -1) mWidth = defaultWidth;
                if (mDecimals == -1) mDecimals = defaultDecimals;
            }
            CheckWidthAndDecimals();
        }

        // called when created from fieldinfo
        public void Initialize(string name, char type, int width, int decimals)
        {
            mColumnName = name;
            switch (type)
            {
                case 'C':
                    mColumnType = ColumnType.CHARACTER;
                    break;
                case 'N':
                    mColumnType = ColumnType.NUMERICAL;
                    break;
                case 'L':
                    mColumnType = ColumnType.LOGICAL;
                    break;
                case 'M':
                    mColumnType = ColumnType.MEMO;
                    break;
                case 'D':
                    mColumnType = ColumnType.DATE_YYYYMMDD;
                    break;
                default:
                    throw new Exception(string.Format("Invalid column datatype :'{0}'", type));
            }
            mWidth = width;
            mDecimals = decimals;
        }

        // called when created for indexes
        public void Initialize(string columnName, ColumnDefinition originalColumn, bool ascending, int width)
        {
            this.mColumnName = columnName;
            this.mOriginalColumn = originalColumn;
            this.mFieldInfo = originalColumn.mFieldInfo;
            mColumnType = originalColumn.mColumnType;
            mWidth = (width > 0 ? width : mOriginalColumn.mWidth);
            mAscending = ascending;
            CheckWidthAndDecimals();
        }


        public static void GetDefaultColumnType(TypeCode nativeTypeCode, bool nullable, DbfVersion version, out ColumnType type, out int defaultWidth, out int defaultDecimals)
        {
            defaultDecimals = 0; // default
            if (version == DbfVersion.DbfDotNet)
            {
                switch (nativeTypeCode)
                {
                    case TypeCode.Boolean:
                        type = ColumnType.LOGICAL;
                        defaultWidth = 1;
                        break;
                    case TypeCode.Byte:
                        type = ColumnType.BYTE;
                        defaultWidth = 1;
                        break;
                    case TypeCode.SByte:
                        type = ColumnType.SBYTE;
                        defaultWidth = 1;
                        break;
                    case TypeCode.Int16:
                        type = ColumnType.INT16;
                        defaultWidth = 2;
                        break;
                    case TypeCode.UInt16:
                        type = ColumnType.UINT16;
                        defaultWidth = 2;
                        break;
                    case TypeCode.Int32:
                        type = ColumnType.INT32;
                        defaultWidth = 4;
                        break;
                    case TypeCode.UInt32:
                        type = ColumnType.UINT32;
                        defaultWidth = 4;
                        break;
                    case TypeCode.Single:
                        type = ColumnType.SINGLE;
                        defaultWidth = 4;
                        break;
                    case TypeCode.Int64:
                        type = ColumnType.INT64;
                        defaultWidth = 8;
                        break;
                    case TypeCode.UInt64:
                        type = ColumnType.UINT64;
                        defaultWidth = 8;
                        break;
                    case TypeCode.Double:
                        type = ColumnType.DOUBLE;
                        defaultWidth = 8;
                        break;
                    case TypeCode.Decimal:
                        type = ColumnType.DECIMAL;
                        defaultWidth = 13;// 96 bits + 0..28 (4 bits exponent) + sign = 101 bits minimum
                        break;
                    case TypeCode.DateTime:
                        type = ColumnType.DATETIME;
                        defaultWidth = 8; // 64 bits
                        break;
                    case TypeCode.String:
                        type = ColumnType.CHARACTER;
                        defaultWidth = 20; // unknown
                        break;
                    case TypeCode.Char:
                        // TODO depend on the encoding
                        type = ColumnType.CHARW;
                        defaultWidth = 2;
                        break;
                    default:
                        throw new InvalidColumnException(nativeTypeCode.ToString(), null);
                }
            }
            else
            {
                switch (nativeTypeCode)
                {
                    case TypeCode.Boolean:
                        type = ColumnType.LOGICAL;
                        defaultWidth = 1; // T or F
                        break;
                    case TypeCode.Byte:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 3; // 255
                        break;
                    case TypeCode.SByte:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 4; // -128
                        break;
                    case TypeCode.Int16:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 6; // -32768
                        break;
                    case TypeCode.UInt16:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 5; // 65535
                        break;
                    case TypeCode.Int32:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 11; // -2147483648
                        break;
                    case TypeCode.UInt32:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 10; // 4294967295
                        break;
                    case TypeCode.Single:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 9 + 1; // 7..9 decimals digits of decimals + the DOT
                        defaultDecimals = 2;
                        break;
                    case TypeCode.Int64:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 20; // Int64.MinValue	"-9223372036854775808"	
                        break;
                    case TypeCode.UInt64:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 19; // UInt64.MaxValue	"18446744073709551615"	
                        break;
                    case TypeCode.Double:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 17 + 1; // 15..17 decimal digits of decimals + the DOT
                        defaultDecimals = 2;
                        break;
                    case TypeCode.Decimal:
                        type = ColumnType.NUMERICAL;
                        defaultWidth = 30 + 1;	// Decimal.MinValue "-79228162514264337593543950335" + the DOT
                        defaultDecimals = 4;
                        break;
                    case TypeCode.DateTime:
                        type = ColumnType.DATE_YYYYMMDD;
                        defaultWidth = 8;
                        break;
                    case TypeCode.String:
                        type = ColumnType.CHARACTER;
                        defaultWidth = 20; // unknown really
                        break;
                    case TypeCode.Char:
                        // TODO depend on the encoding
                        type = ColumnType.CHARACTER;
                        defaultWidth = 1;
                        break;
                    default:
                        throw new InvalidColumnException(nativeTypeCode.ToString(), null);
                }
            }
        }

        public static void GetDefaultColumnType(Type objectType, DbfVersion version, out ColumnType type, out int defaultWidth, out int defaultDecimals)
        {
            defaultDecimals = 0;
            if (objectType == typeof(byte[]))
            {
                type = ColumnType.BYTES;
                defaultWidth = 0;
            }
            else throw new InvalidColumnException(objectType, null);
        }

        private void CheckWidthAndDecimals()
        {
            int minWidth = 1;
            int maxWidth = 255;
            int minDecimals = 0;
            int maxDecimals = 0;
            int fixedwidth = -1;
            int defaultWidth = -1;
            int defaultDecimals = 0;

            switch (mColumnType)
            {
                case ColumnType.BOOL:
                case ColumnType.LOGICAL:
                    fixedwidth = 1; // T or F
                    break;
                //case ColumnType.INTEGER:
                //    maxWidth = 20; // Int64.MinValue	"-9223372036854775808"	
                //    break;
                case ColumnType.NUMERICAL:
                    defaultWidth = 30 + 1;	// Decimal.MinValue "-79228162514264337593543950335" + the DOT
                    maxDecimals = 255;
                    break;
                case ColumnType.DATE_YMD:
                    fixedwidth = 3;
                    break;
                case ColumnType.DELAYED:
                    maxDecimals = 255;
                    break;
                case ColumnType.DELETED_FLAG:
                    fixedwidth = 1;
                    break;
                case ColumnType.BYTES:
                    maxWidth = int.MaxValue; // we assume you know what you do on this type
                    break;
                case ColumnType.CHARACTER:
                    break;
                case ColumnType.BYTE:
                case ColumnType.SBYTE:
                case ColumnType.CHAR:
                    fixedwidth = 1;
                    break;
                case ColumnType.INT16:
                case ColumnType.UINT16:
                case ColumnType.CHARW:
                    fixedwidth = 2;
                    break;
                case ColumnType.INT32:
                case ColumnType.UINT32:
                case ColumnType.SINGLE:
                    fixedwidth = 4;
                    break;
                case ColumnType.INT64:
                case ColumnType.UINT64:
                case ColumnType.DOUBLE:
                case ColumnType.DATE_YYYYMMDD:
                    fixedwidth = 8;
                    break;
                case ColumnType.DECIMAL:
                    fixedwidth = 13;// 96 bits + 0..28 (4 bits exponent) + sign = 101 bits minimum
                    break;
                case ColumnType.DATETIME:
                    fixedwidth = 8; // 64 bits
                    break;
                default:
                    throw new InvalidColumnException(mColumnType.ToString(), null);
            }

            if (fixedwidth >= 0)
            {
                minWidth = fixedwidth;
                maxWidth = fixedwidth;
                defaultWidth = fixedwidth;
            }
            if (mWidth == -1) mWidth = defaultWidth;
            if (mDecimals == -1) mDecimals = defaultDecimals;

            if (mWidth < minWidth || mWidth > maxWidth)
            {
                throw new InvalidColumnException(mColumnName,
                    (minWidth == maxWidth ?
                    string.Format("Width must be {0}.", minWidth) :
                    string.Format("Width must be between {0} and {1}.", minWidth, maxWidth))
                    );
            }
            if (mDecimals < minDecimals || mDecimals > maxDecimals)
            {
                throw new InvalidColumnException(mColumnName,
                    (minWidth == maxWidth ?
                    string.Format("Decimals must be {0}.", minDecimals) :
                    string.Format("Decimals must be between {0} and {1}.", minDecimals, maxDecimals))
                    );
            }
        }

        internal char DbfColumnType
        {
            get { 
                switch(mColumnType)
                {
                    case ColumnType.CHARACTER:
                        return 'C';
                    case ColumnType.NUMERICAL:
                        return 'N';
                    case ColumnType.DATE_YYYYMMDD:
                        return 'D';
                    case ColumnType.LOGICAL:
                        return 'L';
                    default:
                        throw new Exception("Invalid column type");
                }
            }
        }
    }

}

