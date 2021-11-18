using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data
{
    public interface INamedValued<TVal> : INamed, IValued<TVal>
    {

    }
}
