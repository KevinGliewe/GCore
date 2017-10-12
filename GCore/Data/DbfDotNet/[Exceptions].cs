using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{
    public class InvalidColumnException : Exception
    {
        public InvalidColumnException(Type columnType, string msg)
            : base(String.Format("Unsupported column type {0} in DbfTable. {1}", columnType.Name, msg))
        {
        }
        public InvalidColumnException(string columnType, string msg)
            : base(String.Format("Unsupported column type {0} in DbfTable. {1}", columnType, msg))
        {
        }
    }

    public class RuntimeException : Exception
    {
        public RuntimeException(string msg) : base(msg) { }
    }

    public class FieldOverflowException : Exception
    {
        public FieldOverflowException(string msg) : base(msg) { }
    }

}
