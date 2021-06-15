using System;
using System.IO;

namespace GCore.Extensions.Array.BytaArrayEx
{
    public static class BytaArrayExtension
    {
        /// <summary>
        /// Schreibt den Array in eine Datei
        /// </summary>
        /// <param name="data">Die daten die geschrieben werden</param>
        /// <param name="filename">Die Datei die geschrieben werden soll</param>
        /// <param name="mode"></param>
        public static void SaveToFile(this Byte[] data, string filename, FileMode mode = FileMode.Create)
        {
            FileStream fs = new FileStream(filename, mode);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
    }
}
