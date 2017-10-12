using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCore.Extensions.StreamEx {
    public static class StreamExtensions {
        /// <summary>
        /// Schreibt den String in die übergebene Datei.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputFile">Pfad der Datei</param>
        /// <param name="fileMode"></param>
        public static void StreamToFile(this System.IO.Stream inputStream, string outputFile, FileMode fileMode) {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");

            if (System.String.IsNullOrEmpty(outputFile))
                throw new ArgumentException("Argument null or empty.", "outputFile");

            using (FileStream outputStream = new FileStream(outputFile, fileMode, FileAccess.Write)) {
                int cnt = 0;
                const int LEN = 4096;
                byte[] buffer = new byte[LEN];

                while ((cnt = inputStream.Read(buffer, 0, LEN)) != 0)
                    outputStream.Write(buffer, 0, cnt);
            }
        }
    }
}
