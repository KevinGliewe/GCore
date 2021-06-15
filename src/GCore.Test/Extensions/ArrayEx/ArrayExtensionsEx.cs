using GCore.Extensions.ArrayEx;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GCore.Test.Extensions.ArrayEx
{
    public class ArrayExtensionsEx
    {
        int[] data0 = new int[] {  };
        int[] data1 = new int[] { 0 };
        int[] data2 = new int[] { 0 , 1 };
        int[] data5 = new int[] { 0 , 1 , 2 , 3 , 4 };

        [Fact]
        public void Test_Slice() {
            Assert.Equal(0, data0.Slice(0, -1).Length);
            Assert.Equal(1, data1.Slice(0, -1).Length);
            Assert.Equal(2, data2.Slice(0, -1).Length);
            Assert.Equal(5, data5.Slice(0, -1).Length);
            Assert.Equal(5, data5.Slice(0, 4).Length);

            Assert.Equal(1, data2.Slice(1, -1)[0]);
            Assert.Equal(4, data5.Slice(1, -1)[3]);
        }
    }
}
