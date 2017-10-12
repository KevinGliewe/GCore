using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
#pragma warning disable 649

    [Record(FieldMapping = FieldMapping.ExplicitColumnsOnly, Width = 32)]
    internal class NdxHeader 
    {
        [Column(Type = ColumnType.UINT32)]
        private UInt32 mNativeNdxStartingPageRecordNo; //0..3

        public UInt32 StartingPageRecordNo
        {
            get { return mNativeNdxStartingPageRecordNo - 1; }
            set { mNativeNdxStartingPageRecordNo = value + 1; }
        }


        [Column(Type = ColumnType.UINT32)]
        public UInt32 TotalNoOfPages; //4..7

        [Column(Type = ColumnType.UINT32)]
        public UInt32 Zero; //8..11

        [Column(Type = ColumnType.UINT16)]
        public UInt16 KeyLength; //12..13


        [Column(Type = ColumnType.UINT16)]
        public UInt16 NoOfKeysPerPage; // 14..15

        [Column(Type = ColumnType.UINT16)]
        public UInt16 KeyType; // 16..17 - 0 = char; 1 = num

        [Column(Type = ColumnType.UINT32)]
        public UInt32 SizeOfKeyRecord; // 18..21

        [Column(Type = ColumnType.BYTE)]
        public Byte Reserved; // 22

        [Column(Type = ColumnType.BOOL)]
        public bool UniqueFlag; // 23

        [Column(Type = ColumnType.CHARACTER, Width = 255)] // 
        public string KeyString1; // 24..

        [Column(Type = ColumnType.CHARACTER, Width = 233)] // 512 - 255 - 24
        public string KeyString2;
    }

}
