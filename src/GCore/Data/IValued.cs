using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data
{
    public interface IValued<T>
    {
        T Value { get; }
    }
}
