using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace GCore.Data.DbfDotNet.Core
{

    //internal class MemoFile
    //{
    //    private struct DbtHeader
    //    {
    //        public Int32 NextAvailable;
    //        public Int32 SizeOfBlocks;

    //        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
    //        public string DbfFileName;
    //        public Int16 BlockLength;
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 490)]
    //        public byte[] Reserved3;
    //    }

    //    private struct UsedDbtBlock
    //    {
    //        public Int32 Reserved;
    //        public Int32 LengthOfMemoField;
    //    }

    //    private struct UnusedDbtBlock
    //    {
    //        public Int32 NextFreeBlock;
    //        public Int32 NextUsedBlock;
    //        //      <MarshalAs(UnmanagedType.ByValArray, SizeConst:=512 - 8)> Public Data() As Byte
    //    }

    //    private struct FptHeader
    //    {
    //        public Int32 NextAvailable;
    //        public Int16 reserved1;
    //        public Int16 SizeOfBlocks;

    //        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
    //        public string DbfFileName;
    //        public Int16 BlockLength;
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 490)]
    //        public byte[] Reserved3;
    //    }

    //    private struct UsedFptBlock
    //    {
    //        public Int32 RecordType;
    //        public Int32 LengthOfMemoField;
    //    }

    //    private string mDbfPath;
    //    RecordStream dbtStream;
    //    RecordStream fptStream = null;
    //    const Int32 USED_BLOCK_MARKER = 0x8ffff;
    //    FptHeader mfptHeader;

    //    public MemoFile(string dbfpath)
    //    {
    //        mDbfPath = dbfpath;
    //        FileInfo dbtFile = new FileInfo(Path.ChangeExtension(dbfpath, ".dbt"));
    //        DbtHeader dbtHeader = default(DbtHeader);
    //        if (dbtFile.Exists)
    //        {
    //            dbtStream = new RecordStream(dbtFile.OpenRead(), 0, 512);
    //            byte[] memobuff = new byte[512];
    //            dbtStream.ReadRecord(memobuff, 0);
    //            dbtHeader = (DbtHeader)DbfUtils.MakeStruct(memobuff, typeof(DbtHeader));
    //        }

    //        FileInfo fptFile = new FileInfo(Path.ChangeExtension(dbfpath, ".fpt"));

    //        if (fptFile.Exists)
    //        {
    //            fptStream = new RecordStream(fptFile.OpenRead(), 0, 512);
    //            byte[] memobuff = new byte[512];
    //            fptStream.ReadRecord(memobuff, 0);
    //            mfptHeader = (FptHeader)DbfUtils.MakeStruct(memobuff, typeof(FptHeader));
    //            mfptHeader.SizeOfBlocks = LittleIndian(mfptHeader.SizeOfBlocks);
    //            if (mfptHeader.SizeOfBlocks != 512)
    //            {
    //                fptStream = new RecordStream(fptFile.OpenRead(), 0, mfptHeader.SizeOfBlocks);
    //            }

    //        }
    //    }

    //    public static Int32 LittleIndian(Int32 x)
    //    {
    //        int b0 = x & 0xff;
    //        int b1 = (x >> 8) & 0xff;
    //        int b2 = (x >> 16) & 0xff;
    //        int b3 = (x >> 24) & 0xff;

    //        return b3 | b2 << 8 | b1 << 16 | b0 << 24;
    //    }

    //    public static Int16 LittleIndian(Int16 x)
    //    {
    //        int b0 = x & 0xff;
    //        int b1 = (x >> 8) & 0xff;

    //        return (short)(b1 | b0 << 8);
    //    }

    //    public object GetValue(byte[] recordbuff, int columnOffset, int columnSize, char columntype)
    //    {
    //        if (dbtStream != null)
    //        {
    //            string valueString = System.Text.UTF8Encoding.ASCII.GetString(recordbuff, columnOffset, columnSize).TrimEnd();
    //            if (valueString.Length > 0 && valueString[0] != null)
    //            {
    //                int entry = int.Parse(valueString);

    //                byte[] memobuff = new byte[512];
    //                dbtStream.ReadRecord(memobuff, entry);
    //                UsedDbtBlock usedBlock = (UsedDbtBlock)DbfUtils.MakeStruct(memobuff, typeof(UsedDbtBlock));
    //                if (usedBlock.Reserved != USED_BLOCK_MARKER)
    //                {
    //                    throw new Exception("Invalid Block marker in MemoFile.");
    //                }
    //                byte[] data = new byte[usedBlock.LengthOfMemoField];
    //                dbtStream.ReadRaw(entry * 512 + 8, data);

    //                switch (columntype)
    //                {
    //                    case 'M':
    //                        string dataString = System.Text.UTF8Encoding.ASCII.GetString(data);
    //                        return dataString;
    //                    case 'G':
    //                        //Image bmp = Bitmap.FromStream(new IO.MemoryStream(data));
    //                        //return bmp;
    //                        return null;
    //                }
    //            }
    //        }
    //        else if (fptStream != null)
    //        {
    //            Int32 entry = BitConverter.ToInt32(recordbuff, columnOffset);
    //            if (entry > 0)
    //            {
    //                byte[] memobuff = new byte[mfptHeader.SizeOfBlocks];
    //                fptStream.ReadRecord(memobuff, entry);
    //                UsedFptBlock usedBlock = (UsedFptBlock)DbfUtils.MakeStruct(memobuff, typeof(UsedFptBlock));

    //                usedBlock.RecordType = LittleIndian(usedBlock.RecordType);
    //                usedBlock.LengthOfMemoField = LittleIndian(usedBlock.LengthOfMemoField);

    //                if (usedBlock.LengthOfMemoField < 10000000)
    //                {
    //                    byte[] data = new byte[usedBlock.LengthOfMemoField];
    //                    fptStream.ReadRaw(entry * mfptHeader.SizeOfBlocks + 8, data);

    //                    switch (columntype)
    //                    {
    //                        case 'M':
    //                            string dataString = System.Text.UTF8Encoding.ASCII.GetString(data);
    //                            return dataString;
    //                        case 'G':
    //                            //Image bmp = Bitmap.FromStream(new IO.MemoryStream(data));
    //                            //return bmp;
    //                            return null;
    //                    }
    //                }
    //                else
    //                {
    //                    int x = 0;
    //                }
    //            }
    //        }
    //        return DBNull.Value;
    //    }

    //    RecordStream memoStream = null;
    //    DbtHeader memoHeader; 
    //    int NextAvailableMemo = 1;

    //    public void WriteHeader()
    //    {
    //        FileInfo dbtFile = new FileInfo(Path.ChangeExtension(mDbfPath, ".dbt"));
    //        memoHeader = new DbtHeader();
    //        memoHeader.BlockLength = 512;
    //        memoHeader.DbfFileName = Path.GetFileNameWithoutExtension(mDbfPath);
    //        memoHeader.NextAvailable = 0;
    //        memoHeader.SizeOfBlocks = 0;
    //        memoStream = new RecordStream(dbtFile.OpenWrite(), 0, 512);
    //    }

    //    public void WriteHeader2()
    //    {
    //        memoHeader.NextAvailable = NextAvailableMemo;
    //        memoStream.Position = 0;
    //        memoStream.WriteRaw(DbfUtils.MakeBuff(memoHeader));
    //    }


    //    public int Write(string data, byte[] recordbuff, int columnOffset, int columnSize, char columntype)
    //    {
    //        int written = 0;
    //        //ignore bitmaps
    //        if (memoStream == null)
    //        {
    //            throw new Exception("MemoFile was not found.");
    //        }

    //        string value = null;
    //        if (data != null && data.Length > 0)
    //        {
    //            UsedDbtBlock usedDbtBlock = default(UsedDbtBlock);
    //            value = NextAvailableMemo.ToString();
    //            usedDbtBlock.Reserved = USED_BLOCK_MARKER;
    //            usedDbtBlock.LengthOfMemoField = data.Length;
    //            memoStream.Position = 512 * NextAvailableMemo;
    //            memoStream.WriteRaw(DbfUtils.MakeBuff(usedDbtBlock));
    //            memoStream.WriteRaw(System.Text.UTF8Encoding.ASCII.GetBytes(data));
    //            NextAvailableMemo = (int)(memoStream.Position + 511) / 512;
    //        }
    //        else
    //        {
    //            value = string.Empty;
    //        }
    //        int len = value.Length;
    //        written = System.Text.UTF8Encoding.ASCII.GetBytes(value, 0, len, recordbuff, columnOffset);
    //        return written;
    //    }
    //}
}
