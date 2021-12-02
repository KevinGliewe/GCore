using GCore.Data.Filter;
using Xunit;

namespace GCore.Test.Filter
{
    public class BlackWhiteRegexFilterTests
    {
        [Fact]
        public void Test(){
            var filter = new BlackWhiteRegexFilter(
                new string[] {
                    @"^t1\.t2\.t3\(x1,x2,x3\)$",
                    @"^.*x$"
                },
                new string[] {
                    @"^t1\..*$"
                }
            );

            Assert.True(filter.Passes("t1.t2.t3.t4(x1,x2,x3)"));
            Assert.True(filter.Passes("t1.t2"));
            Assert.False(filter.Passes("t1.t2.t3(x1,x2,x3)"));
            Assert.False(filter.Passes("t1.t2.t3.t4(x1,x2,x3)x"));
            Assert.False(filter.Passes("t5.t2.t3.t4(x1,x2,x3)"));
        }
    }
}