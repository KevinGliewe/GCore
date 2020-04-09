using System;
using System.Collections.Generic;
using System.Text;
using GCore.Extensions.StringEx;
using Xunit;

namespace GCore.Test.Extensions.StringEx {
    public class StringExtensionsTest {
        [Fact]
        public void GuidCreate()
        {
            Assert.Equal(Guid.Parse("93c4ced8-70cc-56d6-a558-99ee2dcfcbc0"), "Hello World".GuidCreate());
        }
    }
}
