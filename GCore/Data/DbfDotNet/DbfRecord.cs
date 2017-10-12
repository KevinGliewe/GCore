using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{

    public class DbfRecord : Record
    {
        [Column(Type = ColumnType.DELETED_FLAG, Width = 1)]
        internal bool DeletedFlag = false;

    }

}
