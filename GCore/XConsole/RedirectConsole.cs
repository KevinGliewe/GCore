using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCore.XConsole {
    /// <summary>
    /// Description of RedirectConsole.
    /// </summary>
    /// <summary>
    /// OutToFile is an easy way to redirect console output to a file.
    ///
    /// Usage:
    ///    Console.WriteLine("This text goes to the console by default");
    ///    using (OutToFile redir = new OutToFile("out.txt"))
    ///    {
    /// 	   Console.WriteLine("Contents of out.txt");
    ///    }
    ///    Console.WriteLine("This text goes to console again");
    ///
    /// </summary>
    public class OutToFile : IDisposable {
        private StreamWriter fileOutput;
        private TextWriter oldOutput;

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        /// <param name="outFileName">
        /// The name of the file to capture console output
        /// </param>
        public OutToFile(string outFileName) {
            oldOutput = Console.Out;
            fileOutput = new StreamWriter(
                new FileStream(outFileName, FileMode.Create)
                );
            fileOutput.AutoFlush = true;
            Console.SetOut(fileOutput);
        }

        // Dispose() is called automatically when the object
        // goes out of scope
        public void Dispose() {
            Console.SetOut(oldOutput);  // Restore the console output
            fileOutput.Close(); 	   // Done with the file
        }
    }


    public class OutToStream : IDisposable {
        private StreamWriter streamOutput;
        private TextWriter oldOutput;

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        /// <param name="outStream">
        /// The Stream to capture console output
        /// </param>
        public OutToStream(Stream outStream) {
            oldOutput = Console.Out;
            streamOutput = new StreamWriter(
                outStream
                );
            streamOutput.AutoFlush = true;
            Console.SetOut(streamOutput);
        }

        // Dispose() is called automatically when the object
        // goes out of scope
        public void Dispose() {
            Console.SetOut(oldOutput);  // Restore the console output
            streamOutput.Close(); 	   // Done with the file
        }
    }


    public class OutToList : IDisposable {
        private StreamWriter streamOutput;
        private TextWriter oldOutput;
        private MemoryStream mem;
        private List<string> list;

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        /// <param name="outStream">
        /// The Stream to capture console output
        /// </param>
        public OutToList(List<string> outList) {
            list = outList;
            mem = new MemoryStream();
            oldOutput = Console.Out;
            streamOutput = new StreamWriter(
               mem
               );
            streamOutput.AutoFlush = true;
            Console.SetOut(streamOutput);
        }

        // Dispose() is called automatically when the object
        // goes out of scope
        public void Dispose() {
            Console.SetOut(oldOutput);  // Restore the console output
            mem.Position = 0;
            StreamReader reader = new StreamReader(mem);
            while (!reader.EndOfStream)
                list.Add(reader.ReadLine());
            streamOutput.Close(); 	   // Done with the file
        }
    }
}
