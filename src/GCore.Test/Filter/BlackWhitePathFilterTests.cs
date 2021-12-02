using GCore.Data.Filter;
using Xunit;

namespace GCore.Test.Filter
{
    public class BlackWhitePathFilterTests
    {
        #region Regex Generator
        [Fact]
        public void TestRegexStar() {
            Assert.Equal(
                "^n1/[^/]*/n2$", 
                BlackWhitePathFilter.PatternToRegex("n1\\*/n2"));
        }

        [Fact]
        public void TestRegexStarStar() {
            Assert.Equal(
                "^n1/.*/n2$", 
                BlackWhitePathFilter.PatternToRegex("n1\\**/n2"));
        }
        #endregion

        #region Blacklist
        [Fact]
        public void TestBlacklistStarEnd() {
            var filter = new BlackWhitePathFilter(
                new string[] {
                    "n1/n2/n3/*",
                },
                new string[] {
                    "**",
                }
            );
            Assert.True (filter.Passes("n1/n2/n3"));
            Assert.True (filter.Passes("n1/n2/n3/n4/n5"));

            Assert.False(filter.Passes("n1/n2/n3/n4"));
            Assert.False(filter.Passes("n1\\n2\\n3\\n4"));
        }

        [Fact]
        public void TestBlacklistStarMid() {
            var filter = new BlackWhitePathFilter(
                new string[] {
                    "n1/n2/*/n4",
                },
                new string[] {
                    "**",
                }
            );
            Assert.True (filter.Passes("n1/n2/n3"));
            Assert.True (filter.Passes("n1/n2/n3/nx"));

            Assert.False(filter.Passes("n1/n2/n3/n4"));
            Assert.False(filter.Passes("n1/n2/nx/n4"));
            Assert.False(filter.Passes("n1\\n2\\nx\\n4"));
        }

        [Fact]
        public void TestBlacklistStarStarEnd() {
            var filter = new BlackWhitePathFilter(
                new string[] {
                    "n1/n2/n3/**",
                },
                new string[] {
                    "**",
                }
            );

            Assert.True (filter.Passes("n1/n2/n3"));

            Assert.False(filter.Passes("n1/n2/n3/n4/n5"));
            Assert.False(filter.Passes("n1/n2/n3/n4"));
            Assert.False(filter.Passes("n1\\n2\\n3\\n4"));
        }

        [Fact]
        public void TestBlacklistStarStarMid() {
            var filter = new BlackWhitePathFilter(
                new string[] {
                    "n1/**/nx",
                },
                new string[] {
                    "**",
                }
            );

            Assert.True (filter.Passes("n1/n2/n3/n4"));
            Assert.True (filter.Passes("nx/n2/n3/nx"));

            Assert.False(filter.Passes("n1/n2/n3/nx"));
            Assert.False(filter.Passes("n1\\n2\\n3\\nx"));
        }

        [Fact]
        public void TestBlacklistQuestionmark() {
            var filter = new BlackWhitePathFilter(
                new string[] {
                    "n1/?2/n3",
                },
                new string[] {
                    "**",
                }
            );

            Assert.True (filter.Passes("n1/nx/n3"));
            Assert.True (filter.Passes("n1/2/n3"));

            Assert.False(filter.Passes("n1/n2/n3"));
            Assert.False(filter.Passes("n1/x2/n3"));
            Assert.False(filter.Passes("n1\\x2\\n3"));
        }
        #endregion

        #region Whitelist
        [Fact]
        public void TestWhitelistlistStarEnd() {
            var filter = new BlackWhitePathFilter(
                new string[] { },
                new string[] {
                    "n1/*",
                }
            );

            Assert.True (filter.Passes("n1/n2"));
            Assert.True (filter.Passes("n1\\n2"));

            Assert.False(filter.Passes("n2/n3/n4"));
            Assert.False(filter.Passes("n1"));
            Assert.False(filter.Passes("n1/n2/n3"));
        }

        [Fact]
        public void TestWhitelistlistStarMid() {
            var filter = new BlackWhitePathFilter(
                new string[] { },
                new string[] {
                    "n1/*/nx",
                }
            );

            Assert.True (filter.Passes("n1/n2/nx"));
            Assert.True (filter.Passes("n1/xx/nx"));
            Assert.True (filter.Passes("n1\\xx\\nx"));

            Assert.False(filter.Passes("n1/n2/n3"));
            Assert.False(filter.Passes("n1/n2"));
            Assert.False(filter.Passes("n1/n2/n3/n4"));
        }

        [Fact]
        public void TestWhitelistlistStarStarEnd() {
            var filter = new BlackWhitePathFilter(
                new string[] { },
                new string[] {
                    "n1/**",
                }
            );

            Assert.True (filter.Passes("n1/n2/n3"));
            Assert.True (filter.Passes("n1\\n2\\n3"));

            Assert.False(filter.Passes("n2/n3/n4"));
            Assert.False(filter.Passes("n1"));
        }

        [Fact]
        public void TestWhitelistlistStarStarMid() {
            var filter = new BlackWhitePathFilter(
                new string[] { },
                new string[] {
                    "n1/**/nx",
                }
            );

            Assert.True (filter.Passes("n1/n2/nx"));
            Assert.True (filter.Passes("n1/n2/n3/nx"));
            Assert.True (filter.Passes("n1\\n2\\n3\\nx"));

            Assert.False(filter.Passes("n2/n3/n4"));
            Assert.False(filter.Passes("n1"));
            Assert.False(filter.Passes("n1/n2/n3"));
        }

        [Fact]
        public void TestWhitelistlistQuestionmark() {
            var filter = new BlackWhitePathFilter(
                new string[] { },
                new string[] {
                    "n1/?2/n3",
                }
            );

            Assert.True (filter.Passes("n1/n2/n3"));
            Assert.True (filter.Passes("n1/x2/n3"));
            Assert.True (filter.Passes("n1\\x2\\n3"));

            Assert.False(filter.Passes("n1/nx/n3"));
            Assert.False(filter.Passes("n1/2/n3"));
        }
        #endregion
    }
}