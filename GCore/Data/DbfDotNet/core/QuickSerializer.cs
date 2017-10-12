using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace GCore.Data.DbfDotNet.Core
{
    internal delegate void QuickReadDelegate(IHasEncoding encoding, Byte[] bytes, object record);
    internal delegate void QuickWriteDelegate(IHasEncoding encoding, Byte[] bytes, object record);
    internal delegate int QuickCompareDelegate(IHasEncoding encoding, object record1, object record2);

    internal class QuickSerializer
    {

        internal List<ColumnDefinition> mColumns;
        internal int mRecordWidth;
        internal DbfVersion mVersion;
        internal bool mIsFixedRecordWidth;
        internal QuickReadDelegate mQuickReadMethod;
        internal QuickWriteDelegate mQuickWriteMethod;
        internal QuickCompareDelegate mQuickCompareMethod;
        internal bool mCreateColumns;

        private ILGenerator mIlRead;
        private ILGenerator mIlWrite;
        private ILGenerator mIlCompare;
        internal bool mIgnoreMissingFields;
        
        public QuickSerializer(
            Type recordType, 
            DbfVersion version, 
            List<ColumnDefinition> columns, 
            int recordWidth,
            bool ignoreMissingFields, 
            bool setOffset)
        {
            mVersion = version;
            if (columns == null)
            {
                columns = new List<ColumnDefinition>();
                mCreateColumns = true;
            }
            mColumns = columns;
            mIgnoreMissingFields = ignoreMissingFields;

            mRecordWidth = 0; // dbf use a character to specify deleted record
            mIsFixedRecordWidth = true;

            var readMethod = new DynamicMethod("__readRecord",
                        null,
                        new Type[] { typeof(IHasEncoding), typeof(Byte[]), typeof(object) },
                        recordType,
                        true);

            mIlRead = readMethod.GetILGenerator();

            var writeMethod = new DynamicMethod("__writeRecord",
                        null,
                        new Type[] { typeof(IHasEncoding), typeof(Byte[]), typeof(object) },
                        recordType,
                        true);

            mIlWrite = writeMethod.GetILGenerator();

            var compareMethod = new DynamicMethod("__compareRecord",
                        typeof(int),
                        new Type[] { typeof(IHasEncoding), typeof(object), typeof(object) },
                        recordType,
                        true);

            mIlCompare = compareMethod.GetILGenerator();

            EnumerateFields(recordType);

            int currentOffset = 0;
            int columnIndex = 0;
            foreach(var cd in mColumns)
            {
                if (setOffset)
                {
                    cd.mOffset = currentOffset;
                    currentOffset += cd.mWidth;
                }
                cd.ColumnIndex = columnIndex++;
                EmitColumnCode(cd);
            }
            mRecordWidth = currentOffset;
            
            mIlRead.Emit(OpCodes.Ret);
            mIlWrite.Emit(OpCodes.Ret);

            mIlCompare.Emit(OpCodes.Ldc_I4_0); // if not return yet, we can say that the records are equal
            mIlCompare.Emit(OpCodes.Ret);

            mQuickReadMethod = (QuickReadDelegate)readMethod.CreateDelegate(typeof(QuickReadDelegate));
            mQuickWriteMethod = (QuickWriteDelegate)writeMethod.CreateDelegate(typeof(QuickWriteDelegate));
            mQuickCompareMethod = (QuickCompareDelegate)compareMethod.CreateDelegate(typeof(QuickCompareDelegate));

            if (recordWidth > 0) mRecordWidth = recordWidth;
        }

        private void EnumerateFields(Type recordType)
        {
            if (recordType == null) return;
            // we first enumerate the inherited classes
            EnumerateFields(recordType.BaseType);

            var atts = recordType.GetCustomAttributes(typeof(RecordAttribute), false);
            var recordAttribute = (atts.Length == 1 ? (RecordAttribute)atts[0] : new RecordAttribute());

            // We enumerate All Public and NonPublic fields to get the 
            // columns that explicitely have the [Column(...)] attribute.
            foreach (var fieldInfo in recordType.GetFields(
                   BindingFlags.Instance
                   | BindingFlags.Public
                   | BindingFlags.NonPublic
                   | BindingFlags.DeclaredOnly))
            {
                AddColumn(fieldInfo, recordAttribute);
            }
        }

        private void AddColumn(FieldInfo fieldInfo, RecordAttribute recordAttribute)
        {
            // not sure whether it is more convenient for the user to inherit or not
            var atts = fieldInfo.GetCustomAttributes(typeof(ColumnAttribute), true);

            ColumnAttribute column = null;
            
            if (atts.Length == 0)
            {
                var fm = recordAttribute.FieldMapping;
                if (fieldInfo.IsPublic)
                {
                    if ((fm & FieldMapping.PublicFields) == 0) return;
                }
                else
                {
                    if ((fm & FieldMapping.PrivateFields) == 0) return;
                }
            }
            else column = (ColumnAttribute)atts[0];
            ColumnDefinition cd = null;
            if (mCreateColumns)
            {
                cd = new ColumnDefinition();
                cd.Initialize(mVersion, fieldInfo, column);
                mColumns.Add(cd);
            }
            else if (column != null && column.Type == ColumnType.DELETED_FLAG)
            {
                cd = new ColumnDefinition();
                cd.Initialize(mVersion, fieldInfo, column);
                mColumns.Insert(0, cd);
            }
            else
            {
                bool found = false;
                for (int i = 0; i < mColumns.Count; i++)
                {
                    cd = mColumns[i];
                    if (fieldInfo.Name == cd.mColumnName)
                    {
                        cd.mFieldInfo = fieldInfo;
                        found = true;
                        break;
                    }
                }
                if (found == false && mIgnoreMissingFields == false)
                {
                    throw new Exception(string.Format("Column {0} was not found in existing DBF file", fieldInfo.Name));
                }
            }

            if (cd.mWidth == 0) mIsFixedRecordWidth = false;
        }

        void EmitColumnCode(ColumnDefinition cd)
        {
            //  Ldarg_0 : DbfTableDefinitions
            //  Ldarg_1 : Byte[]
            //  Ldarg_2 : DbfRecord
            if (cd.mColumnType == ColumnType.DELAYED) return;

            string readMethodName;
            string writeMethodName;

            string dbfType = cd.mColumnType.ToString();
            string nativeType;

            Type fieldType = cd.mFieldInfo.FieldType;
            nativeType = fieldType.Name;

            if (fieldType.IsGenericType)
            {
                if (fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    nativeType = "Nullable" + fieldType.GetGenericArguments()[0].Name;
                }
            }
            else if (fieldType.IsArray)
            {
                nativeType = fieldType.GetElementType().Name + "Array";
            }
            else if (fieldType.IsEnum)
            {
                nativeType = Type.GetTypeCode(fieldType).ToString();
            }


            readMethodName = "Read_" + dbfType
                + "_" + nativeType;
            writeMethodName = "Write_" + nativeType
                + "_" + dbfType;



            var writeXXX = this.GetType().GetMethod(
                writeMethodName,
                BindingFlags.NonPublic | BindingFlags.Static);

            if (writeXXX == null)
            {
                throw new Exception(string.Format("Unknown Method : {0}.", writeMethodName));
            }
            //void WriteXXX(
            mIlWrite.Emit(OpCodes.Ldarg_1);                 // << 1:Byte[] bytes
            mIlWrite.Emit(OpCodes.Ldarg_0);                 // << 2:dbfTable
            mIlWrite.Emit(OpCodes.Ldc_I4, cd.ColumnIndex);  // << 3: ColumIndex
            mIlWrite.Emit(OpCodes.Ldc_I4, cd.mOffset);      // << 4:Offset // for speed
            mIlWrite.Emit(OpCodes.Ldc_I4, cd.mWidth);       // << 5:Length // for speed
            mIlWrite.Emit(OpCodes.Ldarg_2);                 // <  Record
            mIlWrite.Emit(OpCodes.Ldfld, cd.mFieldInfo);    // >< 5:DateTime/... value
            mIlWrite.Emit(OpCodes.Call, writeXXX);

            var readXXX = this.GetType().GetMethod(
                readMethodName,
                BindingFlags.NonPublic | BindingFlags.Static);

            if (readXXX == null)
            {
                throw new Exception(string.Format("Unknown Method : {0}.", readMethodName));
            }

            //void ReadXXX(
            mIlRead.Emit(OpCodes.Ldarg_2);                       // <  Record
            mIlRead.Emit(OpCodes.Ldarg_0);                      // << 2:dbfTable
            mIlRead.Emit(OpCodes.Ldarg_1);                       // << 0:Byte[]
            mIlRead.Emit(OpCodes.Ldc_I4, cd.mOffset);          // << 1:Offset // for speed
            mIlRead.Emit(OpCodes.Ldc_I4, cd.mWidth); // << 2:Length // for speed
            mIlRead.Emit(OpCodes.Call, readXXX);
            mIlRead.Emit(OpCodes.Stfld, cd.mFieldInfo);              // >< 5:DateTime/... value
        }

        public int RecordWidth
        {
            get { return mRecordWidth; }
        }

        public void Read(IHasEncoding encoding, byte[] bytes, object record)
        {
            mQuickReadMethod(encoding, bytes, record);
        }

        public void Write(IHasEncoding encoding, byte[] bytes, object record)
        {
            mQuickWriteMethod(encoding, bytes, record);
        }

        public int Compare(IHasEncoding encoding, object record1, object record2)
        {
            return mQuickCompareMethod(encoding, record1, record2);
        }

        public List<ColumnDefinition> Columns
        {
            get { return mColumns; }
        }

        private static void WriteIntInBytes(byte[] dstBytes, int dst, int width, int value, bool recordModified)
        {
            var str = value.ToString();
            if (str.Length > width) throw new Exception(string.Format("Cannot write {0} in {1} digits.", value, width));
            while (str.Length < width) str = "0" + str;
            var srcBytes = System.Text.Encoding.ASCII.GetBytes(str);
            WriteInBytes(dstBytes, dst, srcBytes);
        }

        private static void WriteSingleByte(Byte[] dstBytes, int offset, Byte srcByte)
        {
            if (dstBytes[offset] != srcByte)
            {
                dstBytes[offset] = srcByte;
            }
        }

        private static void WriteInBytes(Byte[] dstBytes, int offset, Byte[] srcBytes)
        {
            int src = 0;
            int srcLength = srcBytes.Length;
            int dst = offset;

            while (src < srcLength)
            {
                if (dstBytes[dst] != srcBytes[src])
                {
                    while (src < srcLength)
                    {
                        dstBytes[dst++] = srcBytes[src++];
                    }
                    break;
                }
                dst++;
                src++;
            }
        }

        #region Functions Used in Emitted Code

        protected static string Read_CHARACTER_String(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            int realLength;
            for (realLength = 0; realLength < length && srcBytes[offset + realLength] != 0; realLength++) ;
            var result = encoding.Encoding.GetString(srcBytes, offset, realLength).TrimEnd();
            return result;
        }

        protected static void Write_String_CHARACTER(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            string stringValue)
        {
            if (stringValue == null) stringValue = String.Empty;
            var srcBytes = encoding.Encoding.GetBytes(stringValue);
            int srcLength = srcBytes.Length;

            if (srcLength > dstLength)
            {
                throw new FieldOverflowException(string.Format("Column #{0} cannot store more than {1} characters.", columnIndex + 1, dstLength));
            }

            int src = 0;

            while (src < srcLength)
            {
                dstBytes[dst++] = srcBytes[src++];
            }
            while (src < dstLength)
            {
                dstBytes[dst++] = 0;
                src++;
            }
        }

        protected static Byte[] Read_BYTES_ByteArray(
           IHasEncoding encoding,
           Byte[] srcBytes,
           int offset,
           int length
           )
        {
            Byte[] result = new Byte[length];
            Array.Copy(srcBytes, offset, result, 0, length);
            return result;
        }

        protected static void Write_ByteArray_BYTES(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Byte[] value)
        {
            Array.Copy(value, 0, dstBytes, dst, dstLength);
        }

        protected static char Read_CHAR_Char(
           IHasEncoding encoding,
           Byte[] srcBytes,
           int offset,
           int length
           )
        {
            var resultString = encoding.Encoding.GetString(srcBytes, offset, length);
            if (resultString.Length == 0) return '\0';
            return resultString[0];
        }

        protected static void Write_Char_CHAR(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            char charValue)
        {
            var stringValue = charValue.ToString();
            var srcBytes = encoding.Encoding.GetBytes(stringValue);
            WriteInBytes(dstBytes, dst, srcBytes);
        }

        protected static DateTime Read_DATE_YMD_DateTime(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            //ref string stringValue
            )
        {
            int y, m, d;

            y = 1900 + srcBytes[offset];
            m = srcBytes[offset + 1];
            d = srcBytes[offset + 2];

            if (d >= 1 && d <= 31 && m >= 1 && m <= 12)
            {
                try
                {
                    return new DateTime(y, m, d);
                }
                catch (Exception)
                {
                    // ignore
                }
            }
            return new DateTime();
        }

        protected static void Write_DateTime_DATE_YMD(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            DateTime dateValue)
        {
            int y, m, d;
            y = dateValue.Year - 1900;
            m = dateValue.Month;
            d = dateValue.Day;

            WriteSingleByte(dstBytes, dst, (Byte)y);
            WriteSingleByte(dstBytes, dst + 1, (Byte)m);
            WriteSingleByte(dstBytes, dst + 2, (Byte)d);

        }

        protected static DateTime? Read_DATE_YYYYMMDD_NullableDateTime(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            //ref string stringValue
            )
        {
            int? year = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset, 4);
            int? month = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset + 4, 2);
            int? day = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset + 6, 2);
            //
            if (year.HasValue && month.HasValue && day.HasValue)
            {
                return new DateTime(year.Value, month.Value, day.Value);
            }
            else return null;
        }

        protected static void Write_NullableDateTime_DATE_YYYYMMDD(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            DateTime? dateValue)
        {
            string strValue;
            if (dateValue.HasValue)
                strValue = dateValue.Value.ToString("yyyyMMdd");
            else
                strValue = null;
            Write_String_CHARACTER(dstBytes, encoding, columnIndex, dst, dstLength, strValue);
        }

        protected static DateTime Read_DATE_YYYYMMDD_DateTime(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            //ref string stringValue
            )
        {
            int? year = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset, 4);
            int? month = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset + 4, 2);
            int? day = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset + 6, 2);
            //
            if (year.HasValue && month.HasValue && day.HasValue)
            {
                try
                {
                    return new DateTime(year.Value, month.Value, day.Value);
                }
                catch (Exception)
                {

                    return new DateTime();
                }
            }
            else return DateTime.MinValue;
        }

        protected static void Write_DateTime_DATE_YYYYMMDD(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            DateTime dateValue)
        {
            var strValue = dateValue.ToString("yyyyMMdd");
            Write_String_CHARACTER(dstBytes, encoding, columnIndex, dst, dstLength, strValue);
        }

        protected static DateTime Read_DATETIME_DateTime(
          IHasEncoding encoding,
          Byte[] srcBytes,
          int offset,
          int length
            //ref string stringValue
          )
        {
            Int64 ticks = BitConverter.ToInt64(srcBytes, offset);
            return new DateTime(ticks);
        }

        protected static void Write_DateTime_DATETIME(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            DateTime dateValue)
        {
            Int64 ticks = dateValue.Ticks;

            Byte[] srcBytes = BitConverter.GetBytes(ticks);
            WriteInBytes(dstBytes, dst, srcBytes);
        }


        protected static byte Read_BYTE_Byte(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            return srcBytes[offset];
        }

        protected static void Write_Byte_BYTE(Byte[] dstBytes,
            IHasEncoding encoding,            
            int columnIndex,
            int dst,
            int dstLength,
            Byte byteValue)
        {
            WriteSingleByte(dstBytes, dst, byteValue);
        }

        protected static Boolean? Read_LOGICAL_NullableBoolean(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            switch ((char)srcBytes[offset])
            {
                case 'T':
                case 't':
                case 'Y':
                case 'y':
                    return true;
                case 'F':
                case 'f':
                case 'N':
                case 'n':
                    return false;
                default:
                    return null;
                //throw new Exception("Invalid Logical Value");
            }
        }

        protected static void Write_NullableBoolean_LOGICAL(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Boolean? value)
        {
            char val;
            if (value.HasValue) val = (value.Value ? 'T' : 'F');
            else val = ' ';
            WriteSingleByte(dstBytes, dst, (Byte)val);
        }

        protected static Boolean Read_BOOL_Boolean(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            char v=(char)srcBytes[offset];
            switch (v)
            {
                case '\x01':
                    return true;
                case '\x00':
                    return false;
                default:
                    throw new Exception("Invalid Logical Value");
            }
        }

        protected static void Write_Boolean_BOOL(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Boolean value)
        {
            char val = (value ? 'T' : 'F');
            WriteSingleByte(dstBytes, dst, (Byte)val);               
        }

        protected static Boolean Read_DELETED_FLAG_Boolean(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            switch ((char)srcBytes[offset])
            {
                case '\x01':
                    return true;
                case '\x00':
                    return false;
                default:
                    throw new Exception("Invalid Logical Value");
            }
        }

        protected static void Write_Boolean_DELETED_FLAG(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Boolean value)
        {
            char val = (value ? '\x01' : '\x00');
            WriteSingleByte(dstBytes, dst, (Byte)val);
        }


        protected static Int32 Read_INT32_Int32(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            var result = srcBytes[offset]
                | (srcBytes[offset + 1] << 8)
                | (srcBytes[offset + 2] << 16)
                | (srcBytes[offset + 3] << 24);

            return result;
        }

        protected static void Write_Int32_INT32(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Int32 value)
        {
            WriteSingleByte(dstBytes, dst, (byte)(value & 0xFF));
            WriteSingleByte(dstBytes, dst + 1, (byte)((value >> 8) & 0xFF));
            WriteSingleByte(dstBytes, dst + 2, (byte)((value >> 16) & 0xFF));
            WriteSingleByte(dstBytes, dst + 3, (byte)((value >> 24) & 0xFF));
        }

        protected static UInt32 Read_UINT32_UInt32(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            var result = (UInt32)(srcBytes[offset]
                | (srcBytes[offset + 1] << 8)
                | (srcBytes[offset + 2] << 16)
                | (srcBytes[offset + 3] << 24));

            return result;
        }

        protected static void Write_UInt32_UINT32(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            UInt32 value)
        {
            WriteSingleByte(dstBytes, dst, (byte)(value & 0xFF));
            WriteSingleByte(dstBytes, dst + 1, (byte)((value >> 8) & 0xFF));
            WriteSingleByte(dstBytes, dst + 2, (byte)((value >> 16) & 0xFF));
            WriteSingleByte(dstBytes, dst + 3, (byte)((value >> 24) & 0xFF));
        }
        protected static Int16 Read_INT16_Int16(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            return (Int16)(srcBytes[offset]
                | (srcBytes[offset + 1] << 8));
        }

        protected static void Write_Int16_INT16(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Int16 value)
        {
            WriteSingleByte(dstBytes, dst, (byte)(value & 0xFF));
            WriteSingleByte(dstBytes, dst + 1, (byte)((value >> 8) & 0xFF));
        }

        protected static UInt16 Read_UINT16_UInt16(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            return (UInt16)(srcBytes[offset]
                | (srcBytes[offset + 1] << 8));
        }

        protected static void Write_UInt16_UINT16(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            UInt16 value)
        {
            WriteSingleByte(dstBytes, dst, (byte)(value & 0xFF));
            WriteSingleByte(dstBytes, dst + 1, (byte)((value >> 8) & 0xFF));
        }

        protected static double? Read_NUMERICAL_NullableDouble(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            string valueStr = Read_CHARACTER_String(encoding, srcBytes, offset, length).TrimEnd();
            if (valueStr.Length == 0) return null;
            return double.Parse(valueStr);
        }

        protected static void Write_NullableDouble_NUMERICAL(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            double? value)
        {
            string valueStr;
            if (value.HasValue)
                valueStr = value.ToString();
            else valueStr = null;
            Write_String_CHARACTER(dstBytes, encoding, columnIndex, dst, dstLength, valueStr);
        }

        protected static Int32? Read_NUMERICAL_NullableInt32(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            string valueStr = Read_CHARACTER_String(encoding, srcBytes, offset, length);
            if (valueStr.Length == 0)
            {
                return null;
            }
            else return Int32.Parse(valueStr);
        }

        protected static void Write_NullableInt32_NUMERICAL(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            Int32? value)
        {
            string valueStr = value.ToString();
            Write_String_CHARACTER(dstBytes, encoding, columnIndex, dst, dstLength, valueStr);
        }

        protected static string Read_MEMO_String(
            IHasEncoding encoding,
            Byte[] srcBytes,
            int offset,
            int length
            )
        {
            int? val = Read_NUMERICAL_NullableInt32(encoding, srcBytes, offset, length);
            // todo read the memo file...
            if (val.HasValue)
            {
                return val.ToString(); // BitConverter.ToDouble(srcBytes, offset);
            }
            else return null;
        }

        protected static void Write_String_MEMO(Byte[] dstBytes,
            IHasEncoding encoding,
            int columnIndex,
            int dst,
            int dstLength,
            string value)
        {
            // Byte[] srcBytes = BitConverter.GetBytes(value);
            // WriteInBytes(dstBytes, dst, srcBytes);
        }

        #endregion

    }


}
