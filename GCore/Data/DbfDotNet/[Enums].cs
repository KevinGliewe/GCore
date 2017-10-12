using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{
    public enum DbfVersion
    {
        dBaseIII,
        dBaseIV,
        DbfDotNet
    }

    [Flags()]
    public enum FieldMapping
    {
        ExplicitColumnsOnly = 0,
        PublicFields = 1,
        PrivateFields = 2
    }

    public enum OverflowBehaviour
    {
        ThrowError,
        Truncate
    }

    [Flags]
    internal enum RecordState : byte
    {
        New,
        Disposed
    }

    public enum ColumnType
    {
        UNKNOWN,
        CHARACTER,
        NUMERICAL,
        LOGICAL,
        DATE_YMD,
        DATE_YYYYMMDD,
        GRAPHICS,
        MEMO,
        TIME,
        BOOL,
        BYTE,
        SBYTE,
        CHAR,
        CHARW,
        INT16,
        UINT16,
        INT32,
        UINT32,
        INT64,
        UINT64,
        SINGLE,
        DOUBLE,
        DECIMAL,
        DATETIME,
        BYTES,
        DELAYED,
        DELETED_FLAG
    }

}
