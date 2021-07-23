using System;

using GCore.Extensions.IEnumerableEx;
using GCore.Extensions.DataTableEx;
using Xunit;

namespace GCore.Test.Extensions.DataTableEx
{
    public class DataTableExtensionsEx
    {
        public class TestClass
        {
            public string Name { get; set; }
            public int Age => (DateTime.Parse("1.1.2021") - this.Birth).Days / 365;
            public DateTime Birth { get; set; }
        }

        static string Normalize(string s)
        {
            return s.Replace("\r", "").Replace("\n", "").Trim();;
        }

        [Fact]
        public void Test_ToHTML()
        {
            var data = new TestClass[] {
                new TestClass { Name = "John", Birth = new DateTime(1980, 1, 1) },
                new TestClass { Name = "Jane", Birth = new DateTime(1978, 6, 2) },
                new TestClass { Name = "Bob", Birth = new DateTime(1989, 8, 3) },
                new TestClass { Name = "Sally", Birth = new DateTime(1999, 3, 4) },
                new TestClass { Name = "Tim", Birth = new DateTime(1985, 1, 5) },
            };

            var table = data.ToDataTable();

            Assert.Equal(Normalize(@"
<table id="""" class="""" style="""">
    <thead>
        <tr>
            <th>Name</th>
            <th>Age</th>
            <th>Birth</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>John</td>
            <td>41</td>
            <td>01.01.1980</td>
        </tr>
        <tr>
            <td>Jane</td>
            <td>42</td>
            <td>02.06.1978</td>
        </tr>
        <tr>
            <td>Bob</td>
            <td>31</td>
            <td>03.08.1989</td>
        </tr>
        <tr>
            <td>Sally</td>
            <td>21</td>
            <td>04.03.1999</td>
        </tr>
        <tr>
            <td>Tim</td>
            <td>36</td>
            <td>05.01.1985</td>
        </tr>
    </tbody>
</table>
            "), Normalize(table.ToHtmlTable(formatter: (r, c) => {
                var v = r[c];
                if (v is DateTime vd)
                    return vd.ToString("dd.MM.yyyy");
                return v.ToString();
            })));
        }
    }
}