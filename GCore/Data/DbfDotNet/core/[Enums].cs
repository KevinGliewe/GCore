using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    internal enum OpenFileMode
    {
        OpenReadOnly,
        OpenReadWrite,
        OpenOrCreate
    }

    public enum BufferType
    {
        ReadBuffer,
        WriteBuffer
    }

}
