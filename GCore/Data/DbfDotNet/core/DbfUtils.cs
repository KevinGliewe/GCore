using System;
using System.Data;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;

namespace GCore.Data.DbfDotNet.Core
{

    internal class DbfUtils
    {
// Earlier code not used anymore
//        private static int CodePage(int foxProCodePage)
//        {
//            switch (foxProCodePage)
//            {
//                case 0x1:
//                    // 01h DOS USA code page 437 
//                    return 437;
//                case 0x2:
//                    // DOS Multilingual 
//                    return 850;
//                case 0x3:
//                    // Windows ANSI 
//                    return 1252;
//                case 0x4:
//                    // Standard Macintosh 
//                    return 0;
//                case 0x64:
//                    // EE MS-DOS 
//                    return 852;
//                case 0x65:
//                    // Nordic MS-DOS 
//                    return 865;
//                case 0x66:
//                    // Russian MS-DOS 
//                    return 866;
//                case 0xc8:
//                    // Windows EE 

//                    return 1250;
//                case 0x67:
//                    // Icelandic MS-DOS 
//                    return 0;
//                case 0x68:
//                    // Kamenicky (Czech) MS-DOS 
//                    return 0;
//                case 0x69:
//                    // Mazovia (Polish) MS-DOS 
//                    return 0;
//                case 0x6a:
//                    // Greek MS-DOS (437G) 
//                    return 0;
//                case 0x6b:
//                    // Turkish MS-DOS 
//                    return 0;
//                case 0x96:
//                    // Russian Macintosh 
//                    return 0;
//                case 0x97:
//                    // Eastern European Macintosh 
//                    return 0;
//                case 0x98:
//                    // Greek Macintosh 
//                    return 0;
//                case 0xc9:
//                    // Russian Windows 
//                    return 0;
//                case 0xca:
//                    // Turkish Windows 
//                    return 0;
//                case 0xcb:
//                    // Greek Windows 
//                    return 0;
//                default:
//                    return 0;

//            }
//        }

//        private static Type ToSystemType(char type, int length)
//        {
//            switch (type)
//            {
//                case 'C':
//                    return typeof(string);
//                case 'N':
//                    return typeof(double);
//                case 'L':
//                    return typeof(bool);
//                case 'D':
//                    return typeof(System.DateTime);
//                case 'G':
//                    // memo or OLE object
//                    return typeof(byte[]);
//                case 'M':
//                    return typeof(string);
//                case 'I':
//                    return typeof(int);
//                case 'T':
//                    return typeof(DateTime);
//                default:
//                    throw new Exception("Invalid column datatype :" + type);
//            }
//        }

//        public static DataTable ToDataTable(string filepath)
//        {
//            FileInfo dbfFile = new FileInfo(filepath);
//            Stream dbfStream = dbfFile.OpenRead();
//            RecordStream headerStream = new RecordStream(dbfStream, 32, 32);
//            DbfHeader dbfHeader = (DbfHeader)MakeStruct(headerStream.ReadHeader(), typeof(DbfHeader));

//            int nbColumns = (dbfHeader.HeaderSize - 32) / 32;
//            int[] columnOffset = new int[nbColumns];
//            int[] columnSize = new int[nbColumns];
//            char[] columnType = new char[nbColumns];

//            DataTable dataTable = new DataTable();

//            dataTable.ExtendedProperties.Add("xBaseVerNumber", dbfHeader.VerNumber);
//            bool foundEndMarker = false;
//            for (int fieldno = 0; fieldno <= nbColumns - 1; fieldno++)
//            {
//                byte[] columnBuff = new byte[32];
//                headerStream.ReadRecord(columnBuff, fieldno);
//                DbfColumnHeader ColumnHeader = (DbfColumnHeader)MakeStruct(columnBuff, typeof(DbfColumnHeader));
//                if (fieldno == 0)
//                {
//                    columnOffset[fieldno] = 1;
//                }
//                else
//                {
//                    columnOffset[fieldno] = columnOffset[fieldno - 1] + columnSize[fieldno - 1];
//                }
//                columnType[fieldno] = ColumnHeader.DataType;
//                columnSize[fieldno] = ColumnHeader.FieldLength;
//                if (ColumnHeader.ColumnName[0] == 0xd)
//                {
//                    foundEndMarker = true;
//                    nbColumns = fieldno;
//                    break; // TODO: might not be correct. Was : Exit For
//                }
//                {
//                    dataTable.Columns.Add().ExtendedProperties.Add("xBaseDataType", ColumnHeader.DataType);
//                    dataTable.Columns.Add().ExtendedProperties.Add("xBaseFieldLength", ColumnHeader.FieldLength);
//                    dataTable.Columns.Add().ExtendedProperties.Add("xBaseDecimalCount", ColumnHeader.DecimalCount);
//                    dataTable.Columns.Add().ColumnName = ColumnHeader.ColumnName;
//                    dataTable.Columns.Add().DataType = ToSystemType(ColumnHeader.DataType, ColumnHeader.FieldLength);
//                    if (ColumnHeader.DataType == 'C') dataTable.Columns.Add().MaxLength = ColumnHeader.FieldLength;
//                }
//            }
//            if (!foundEndMarker && headerStream.GetNextByte() != 0xd)
//            {
//                throw new Exception("Invalid DBF header: Column header terminator was not found.");
//            }

//            MemoFile memoFile = new MemoFile(dbfFile.FullName);

//            RecordStream mainStream = new RecordStream(dbfStream, dbfHeader.HeaderSize, dbfHeader.RecordSize);
//            for (int i = 0; i <= dbfHeader.NbRecords - 1; i++)
//            {
//                byte[] recordBuff = new byte[dbfHeader.RecordSize];
//                mainStream.ReadRecord(recordBuff, i);
//                DataRow nr = dataTable.NewRow();
//                for (int c = 0; c <= nbColumns - 1; c++)
//                {
//                    switch (columnType[c])
//                    {
//                        case 'G':
//                        case 'M':
//                            //ignore bitmaps
//                            nr.ItemArray[c] = memoFile.GetValue(recordBuff, columnOffset[c], columnSize[c], columnType[c]);

//                            break;
//                        case 'N':
//                        case 'D':
//                        case 'C':
//                            string valueString = System.Text.UTF8Encoding.ASCII.GetString(recordBuff, columnOffset[c], columnSize[c]);
//                            valueString = valueString.TrimEnd();
//                            if (valueString.Length == 0)
//                            {
//                                nr.ItemArray[c] = DBNull.Value;
//                                continue;
//                            }


//                            switch (columnType[c])
//                            {
//                                case 'N':
//                                    nr.ItemArray[c] = double.Parse(valueString);
//                                    break;
//                                case 'D':
//                                    int year = int.Parse(valueString.Substring(0, 4));
//                                    int month = int.Parse(valueString.Substring(4, 2));
//                                    int day = int.Parse(valueString.Substring(6, 2));
//                                    nr.ItemArray[c] = new System.DateTime(year, month, day);
//                                    break;
//                                case 'C':
//                                    nr.ItemArray[c] = valueString;
//                                    break;
//                            }
//                            break;
//                        case 'I':
//                            //ignore
//                            Int32 value = BitConverter.ToInt32(recordBuff, columnOffset[c]);
//                            nr.ItemArray[c] = value;
//                            break;
//                        case 'T':
//                            //datetime
//                            Int64 days = BitConverter.ToInt32(recordBuff, columnOffset[c]);
//                            Int64 seconds = BitConverter.ToInt32(recordBuff, columnOffset[c] + 4);
//                            if (days != 0 | seconds != 0)
//                            {
//                                DateTime dt = new DateTime(1, 1, 1);
//                                dt = dt.AddDays(days);
//                                dt = dt.AddYears(-4713);
//                                dt = dt.AddSeconds(seconds);
//                                nr.ItemArray[c] = dt;
//                            }

//                            break;
//                        default:
//                            throw new Exception("Invalid column type " + columnType[c]);
//                    }
//                }
//                dataTable.Rows.Add(nr);
//            }
//            return dataTable;
//        }

//        public static void SaveToDbf(DataTable datatable, string filepath)
//        {
//            FileInfo dbfFile = new FileInfo(filepath);
//            Stream dbfStream = dbfFile.OpenWrite();
//            RecordStream headerStream = new RecordStream(dbfStream, 32, 32);
//            DbfHeader dbfHeader = new DbfHeader();

//            int nbColumns = datatable.Columns.Count;

//            string[] columnName = new string[nbColumns];
//            int[] columnOffset = new int[nbColumns];
//            int[] columnSize = new int[nbColumns];
//            char[] columnType = new char[nbColumns];

//            bool hasMemo = false;

//            dbfHeader.RecordSize = 1;
//            for (int fieldno = 0; fieldno <= nbColumns - 1; fieldno++)
//            {
//                string xBaseColumnName = null;
//                char xBaseDataType = '\0';
//                byte xBaseFieldLength = 0;
//                byte xBaseDecimalCount = 0;

//                DataColumn column = datatable.Columns[fieldno];

//                columnOffset[fieldno] = dbfHeader.RecordSize;
//                xBaseColumnName = column.ColumnName;

//                if (column.ExtendedProperties.Contains("xBaseDataType"))
//                {
//                    xBaseDataType = (char)column.ExtendedProperties["xBaseDataType"];
//                }
//                if (column.ExtendedProperties.Contains("xBaseFieldLength"))
//                {
//                    xBaseFieldLength = (byte)column.ExtendedProperties["xBaseFieldLength"];
//                }
//                if (column.ExtendedProperties.Contains("xBaseDecimalCount"))
//                {
//                    xBaseDecimalCount = (byte)column.ExtendedProperties["xBaseDecimalCount"];
//                }
//                DbfColumnHeader columnHeader = new DbfColumnHeader();
//                columnHeader.ColumnName = xBaseColumnName;
//                columnHeader.DataType = xBaseDataType;
//                columnHeader.DecimalCount = xBaseDecimalCount;
//                columnHeader.FieldLength = xBaseFieldLength;

//                columnName[fieldno] = xBaseColumnName;
//                columnOffset[fieldno] = dbfHeader.RecordSize;
//                columnSize[fieldno] = xBaseFieldLength;
//                columnType[fieldno] = xBaseDataType;

//                if (xBaseDataType == 'M' || xBaseDataType == 'G') hasMemo = true;
//                headerStream.Write(DbfUtils.MakeBuff(columnHeader), fieldno);
//                dbfHeader.RecordSize += xBaseFieldLength;
//            }

//            byte[] EndOfFields = new byte[] { 0xd };
//            headerStream.WriteRaw(EndOfFields);
//            if (hasMemo)
//            {
//                dbfHeader.VerNumber = 0x8b;
//            }
//            else
//            {
//                //? 
//                dbfHeader.VerNumber = 0;
//            }
//            dbfHeader.HeaderSize = (short)headerStream.Position;
//            headerStream.Position = 0;
//            DateTime now = DateTime.Now;
//            dbfHeader.LastUpdate.DD = (byte)now.Day;
//            dbfHeader.LastUpdate.MM = (byte)now.Month;
//            dbfHeader.LastUpdate.YY = (byte)(now.Year - 1900);
//            dbfHeader.NbRecords = datatable.Rows.Count;

//            headerStream.WriteRaw(DbfUtils.MakeBuff(dbfHeader));

//            MemoFile memoFile = new MemoFile(dbfFile.FullName);
//            if (hasMemo) memoFile.WriteHeader();

//            RecordStream mainStream = new RecordStream(dbfStream, dbfHeader.HeaderSize, dbfHeader.RecordSize);
//            for (int recordNo = 0; recordNo <= datatable.Rows.Count - 1; recordNo++)
//            {
//                DataRow row = datatable.Rows[recordNo];
//                byte[] recordBuff = new byte[dbfHeader.RecordSize];
//                DataRow nr = datatable.NewRow();
//                for (int c = 0; c <= nbColumns - 1; c++)
//                {
//                    recordBuff[0] = 32;
//                    //not deleted
//                    int written = 0;
//                    switch (columnType[c])
//                    {
//                        case 'G':
//                        case 'M':
//                            string data = null;
//                            if (row.IsNull(c))
//                            {
//                                data = null;
//                            }
//                            else
//                            {
//                                data = (string)row[c];
//                            }

//                            memoFile.Write(data, recordBuff, columnOffset[c], columnSize[c], columnType[c]);
//                            break;
//                        case 'C':
//                            string value = null;
//                            if (row.IsNull(c))
//                            {
//                                value = string.Empty;
//                            }
//                            else
//                            {
//                                value = (string)row[c];
//                            }

//                            int len = value.Length;
//                            if (len > columnSize[c]) len = columnSize[c];
//                            written = System.Text.UTF8Encoding.ASCII.GetBytes(value, 0, len, recordBuff, columnOffset[c]);
//                            //valuestring,valuestring.Length,.GetString(recordBuff, columnOffset[c], columnSize[c]).TrimEnd
//                            break;
//                        case 'D':
//                            value = null;
//                            if (row.IsNull(c))
//                            {
//                                value = string.Empty;
//                            }
//                            else
//                            {
//                                System.DateTime dt = (System.DateTime)row[c];
//                                value = dt.ToString("yyyyMMdd");
//                            }

//                            len = value.Length;
//                            if (len > columnSize[c]) len = columnSize[c];
//                            written = System.Text.UTF8Encoding.ASCII.GetBytes(value, 0, len, recordBuff, columnOffset[c]);
//                            //valuestring,valuestring.Length,.GetString(recordBuff, columnOffset[c], columnSize[c]).TrimEnd
//                            break;

//                        case 'N':
//                            value = null;
//                            if (row.IsNull(c))
//                            {
//                                value = string.Empty;
//                            }
//                            else
//                            {
//                                value = ((double)row[c]).ToString();
//                            }

//                            len = value.Length;
//                            if (len > columnSize[c]) len = columnSize[c];
//                            written = System.Text.UTF8Encoding.ASCII.GetBytes(value, 0, len, recordBuff, columnOffset[c]);
//                            //valuestring,valuestring.Length,.GetString(recordBuff, columnOffset[c], columnSize[c]).TrimEnd
//                            break;
//                        default:
//                            throw new Exception("Invalid column type " + columnType[c]);
//                    }
//                    while (written < columnSize[c])
//                    {
//                        recordBuff[columnOffset[c] + written] = 32;
//                        // SPACE
//                        written += 1;
//                    }
//                }
//                mainStream.Write(recordBuff, recordNo);
//            }
//            if (hasMemo)
//            {
//                memoFile.WriteHeader2();
//            }
//        }

//        public static object MakeStruct(byte[] Buff, System.Type MyType)
//        {
//            GCHandle MyGC = GCHandle.Alloc(Buff, GCHandleType.Pinned);
//            object Obj = Marshal.PtrToStructure(MyGC.AddrOfPinnedObject(), MyType);
//            MyGC.Free();
//            return Obj;
//        }

//        public static byte[] MakeBuff(object @struct)
//        {
//            // initialize Structure (Dummmy  Values)
//            int len = Marshal.SizeOf(@struct);
//            IntPtr Ptr = Marshal.AllocHGlobal(len);
//            byte[] result = new byte[len];
//            //now copy structure to Ptr pointer 
//            Marshal.StructureToPtr(@struct, Ptr, false);
//            Marshal.Copy(Ptr, result, 0, len);
//            //now use result ready for use 
//            Marshal.FreeHGlobal(Ptr);
//            return result;
//        }

    }

}
