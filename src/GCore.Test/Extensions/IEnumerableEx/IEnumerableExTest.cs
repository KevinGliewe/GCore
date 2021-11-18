using GCore.Extensions.IEnumerableEx;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GCore.Test.Extensions.IEnumerableEx
{
    public class IEnumerableExTest
    {
        int[] data0 = new int[] { };
        int[] data1 = new int[] { 0 };
        int[] data2 = new int[] { 0, 1 };
        int[] data5 = new int[] { 0, 1, 2, 3, 4 };

        [Fact]
        public void Test_Slice() {
            Assert.Equal(0, data0.Slice(0, -1).ToArr().Length);
            Assert.Equal(1, data1.Slice(0, -1).ToArr().Length);
            Assert.Equal(2, data2.Slice(0, -1).ToArr().Length);
            Assert.Equal(5, data5.Slice(0, -1).ToArr().Length);
            Assert.Equal(5, data5.Slice(0, 4).ToArr().Length);

            Assert.Equal(1, data2.Slice(1, -1).ToArr()[0]);
            Assert.Equal(4, data5.Slice(1, -1).ToArr()[3]);
        }

        [Fact]
        public void Test_IterIndexLast()
        {
            int[] data = new int[] { 0, 1, 2, 3, 4 };
            int i = 0;
            foreach (var (value, index, last) in data.IterIndexLast())
            {
                Assert.Equal(data[i], value);
                Assert.Equal(i, index);
                Assert.Equal(data.Length - 1 == i, last);
                i++;
            }
        }
    }
}
