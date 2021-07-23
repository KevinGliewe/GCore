using System;
using Xunit;

using GCore.Extensions.ObjectEx;
using GCore.Extensions.DataTableEx;


namespace GCore.Test.Extensions.ObjectEx
{
    public class ObjectExTests
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
            var testObj = new TestClass { Name = "John", Birth = new DateTime(1980, 1, 1) };

            var dataTable = testObj.ToDataTableO();

                        Assert.Equal(Normalize(@"
<table id="""" class="""" style="""">
    <thead>
        <tr>
            <th>Name</th>
            <th>Value</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Name</td>
            <td>John</td>
        </tr>
        <tr>
            <td>Age</td>
            <td>41</td>
        </tr>
        <tr>
            <td>Birth</td>
            <td>01.01.1980</td>
        </tr>
    </tbody>
</table>
            "), Normalize(dataTable.ToHtmlTable(formatter: (r, c) => {
                var v = r[c];
                if (v is DateTime vd)
                    return vd.ToString("dd.MM.yyyy");
                return v.ToString();
            })));
        }
    }
}