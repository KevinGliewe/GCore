using System;
using System.CodeDom.Compiler;
using System.Data;
using System.IO;
using GCore.Extensions.IndentedTextWriterEx;

namespace GCore.Extensions.DataTableEx
{
    public static class DataTableExtensions
    {
        public static void ToHtmlTable(this DataTable dt, IndentedTextWriter writer, string table_id = "", string table_class = "", string table_style = "", Func<DataRow, DataColumn, string> formatter = null)
        {
            writer.WriteLine("<table id=\"{0}\" class=\"{1}\" style=\"{2}\">", table_id, table_class, table_style);

            using(writer.IndentR(1))
            {
                writer.WriteLine("<thead>");
                using(writer.IndentR(1))
                {
                    writer.WriteLine("<tr>");
                    foreach(DataColumn column in dt.Columns)
                    {
                        writer.WriteLine("    <th>{0}</th>", column.ColumnName);
                    }
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</thead>");

                writer.WriteLine("<tbody>");
                using(writer.IndentR(1))
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        writer.WriteLine("<tr>");
                        foreach(DataColumn column in dt.Columns)
                        {
                            writer.WriteLine("    <td>{0}</td>", formatter is null ? row[column]: formatter(row, column));
                        }
                        writer.WriteLine("</tr>");
                    }
                }
                writer.WriteLine("</tbody>");
            }
            writer.WriteLine("</table>");
        }

        public static string ToHtmlTable(this DataTable dt, string table_id = "", string table_class = "", string table_style = "", Func<DataRow, DataColumn, string> formatter = null)
        {
            using (var output = new StringWriter())
            using (var writer = new IndentedTextWriter(output))
            {
                ToHtmlTable(dt, writer, table_id, table_class, table_style, formatter);
                return output.ToString();
            }
        }

    }
}