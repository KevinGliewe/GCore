using System;
using System.CodeDom.Compiler;

namespace GCore.Extensions.IndentedTextWriterEx
{
    public class IndentedTextWriterIndention : IDisposable
    {
        private readonly IndentedTextWriter _writer;
        private readonly int _indention;

        public IndentedTextWriterIndention(IndentedTextWriter writer, int indention)
        {
            _writer = writer;
            _indention = indention;
            _writer.Indent += _indention;
        }

        public void Dispose()
        {
            _writer.Indent -= _indention;
        }
    }

    public static class IndentedTextWriterExtensions
    {
        public static IndentedTextWriterIndention IndentR(this IndentedTextWriter writer, int indention)
        {
            return new IndentedTextWriterIndention(writer, indention);
        }
    }
}