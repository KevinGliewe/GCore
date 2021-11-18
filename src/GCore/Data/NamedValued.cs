using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data
{
    public class NamedValued<TVal> : INamedValued<TVal>
    {
        public string Name { get; set; }
        public TVal Value { get; set; }
    }
}
