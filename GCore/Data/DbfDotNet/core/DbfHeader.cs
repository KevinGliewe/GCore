using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    [Record(FieldMapping = FieldMapping.ExplicitColumnsOnly, Width = 32)]
    internal class DbfHeader
    {
        [Column(Type = ColumnType.BYTE)]
        public byte VerNumber;

        [Column(Type = ColumnType.DATE_YMD)]
        public DateTime LastUpdate;

        [Column(Type = ColumnType.UINT32)]
        public UInt32 NbRecords;

        [Column(Type = ColumnType.UINT16)]
        public UInt16 HeaderWidth;

        [Column(Type = ColumnType.UINT16)]
        public UInt16 RecordWidth;

        [Column(Type = ColumnType.INT16)]
        public Int16 Zero;

        [Column(Type = ColumnType.BYTE)]
        public byte IncompleteTransaction;

        [Column(Type = ColumnType.BYTE)]
        public byte EncryptionFlag;

        [Column(Type = ColumnType.DELAYED, Width = 12)]
        public byte[] LanOnly;

        [Column(Type = ColumnType.BYTE)]
        public byte Indexed;
        // MdxFlag

        [Column(Type = ColumnType.BYTE)]
        public byte Language;

        [Column(Type = ColumnType.INT16)]
        public Int16 Zero2;

        public void Clear()
        {
            VerNumber = 0;
            LastUpdate = DateTime.MinValue;
            NbRecords = 0;
            HeaderWidth = 0;
            RecordWidth = 0;
            Zero = 0;
            IncompleteTransaction = 0;
            EncryptionFlag = 0;
            LanOnly = new byte[] { };
            Indexed = 0;
            Language = 0;
            Zero2 = 0;
        }
    }

}
