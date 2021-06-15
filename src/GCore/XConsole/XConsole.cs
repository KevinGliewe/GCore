using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCore.XConsole {
    /// <summary>
    /// Console helper class.
    /// </summary>
    public class XConsole {
        /// <summary>
        /// The color that is used to print out errors to the console.
        /// </summary>
        public static ConsoleColor ErrorColor = ConsoleColor.Red;

        /// <summary>
        /// The color that is used to print out warnings to the console.
        /// </summary>
        public static ConsoleColor WarningColor = ConsoleColor.Yellow;

        /// <summary>
        /// The color that is used to print out infos to the console.
        /// </summary>
        public static ConsoleColor SuccessColor = ConsoleColor.Green;

        /// <summary>
        /// The color that is used to print out infos to the console.
        /// If set to null, the current console color is used.
        /// </summary>
        public static ConsoleColor? InfoColor;



        public static void ErrorLine(string msg, params object[] args) {
            WriteLine(ErrorColor, msg, args);
        }


        public static void Error(string msg, params object[] args) {
            Write(ErrorColor, msg, args);
        }


        public static void WarnLine(string msg, params object[] args) {
            WriteLine(WarningColor, msg, args);
        }


        public static void Warn(string msg, params object[] args) {
            Write(WarningColor, msg, args);
        }

        public static void InfoLine(string msg, params object[] args) {
            WriteLine(InfoColor ?? Console.ForegroundColor, msg, args);
        }

        public static void Info(string msg, params object[] args) {
            Write(InfoColor ?? Console.ForegroundColor, msg, args);
        }

        public static void SuccessLine(string msg, params object[] args) {
            WriteLine(SuccessColor, msg, args);
        }


        public static void Success(string msg, params object[] args) {
            Write(SuccessColor, msg, args);
        }


        /// <summary>
        /// Clears the current line.
        /// </summary>
        public static void ClearLine() {
            var position = Console.CursorLeft;

            //overwrite with white space (backspace doesn't really clear the buffer,
            //would need a hacky combination of \b\b and single whitespace)
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("".PadRight(position));
            Console.SetCursorPosition(0, Console.CursorTop);
        }


        public static void Write(string msg, params object[] args) {
            Console.Write(msg, args);
        }


        public static void WriteLine(ConsoleColor color, string msg, params object[] args) {
            Write(color, msg, args);
            Console.Out.WriteLine();
        }


        public static void Write(ConsoleColor color, string msg, params object[] args) {
            try {
                Console.ForegroundColor = color;
                Console.Write(msg, args);
            } finally {
                Console.ResetColor();
            }
        }

        public static class ArrayPrinter {
            #region Usage
            /*
	    	 * 
	    	 *
	        int value = 997;
	        string[,] arrValues = new string[5, 5];
	        for (int i = 0; i < arrValues.GetLength(0); i++)
	        {
	            for (int j = 0; j < arrValues.GetLength(1); j++)
	            {
	                value++;
	                arrValues[i, j] = value.ToString();
	            }
	        }
	        ArrayPrinter.PrintToConsole(arrValues);
	        Console.ReadLine(); 
	    	 * 
	    	 * 
	    	 */
            #endregion


            #region Declarations

            static bool isLeftAligned = false;
            const string cellLeftTop = "┌";
            const string cellRightTop = "┐";
            const string cellLeftBottom = "└";
            const string cellRightBottom = "┘";
            const string cellHorizontalJointTop = "┬";
            const string cellHorizontalJointbottom = "┴";
            const string cellVerticalJointLeft = "├";
            const string cellTJoint = "┼";
            const string cellVerticalJointRight = "┤";
            const string cellHorizontalLine = "─";
            const string cellVerticalLine = "│";

            #endregion

            #region Private Methods

            private static int GetMaxCellWidth(string[,] arrValues) {
                int maxWidth = 1;

                for (int i = 0; i < arrValues.GetLength(0); i++) {
                    for (int j = 0; j < arrValues.GetLength(1); j++) {
                        int length = arrValues[i, j].Length;
                        if (length > maxWidth) {
                            maxWidth = length;
                        }
                    }
                }

                return maxWidth;
            }

            private static string GetDataInTableFormat(string[,] arrValues) {
                string formattedString = string.Empty;

                if (arrValues == null)
                    return formattedString;

                int dimension1Length = arrValues.GetLength(0);
                int dimension2Length = arrValues.GetLength(1);

                int maxCellWidth = GetMaxCellWidth(arrValues);
                //int indentLength = (dimension2Length * maxCellWidth) + (dimension2Length - 1);
                //printing top line;
                formattedString = string.Format("{0}{1}{2}{3}", cellLeftTop, Indent(dimension2Length, maxCellWidth, true), cellRightTop, System.Environment.NewLine);

                for (int i = 0; i < dimension1Length; i++) {
                    string lineWithValues = cellVerticalLine;
                    string line = cellVerticalJointLeft;
                    for (int j = 0; j < dimension2Length; j++) {
                        string value = (isLeftAligned) ? arrValues[i, j].PadRight(maxCellWidth, ' ') : arrValues[i, j].PadLeft(maxCellWidth, ' ');
                        lineWithValues += string.Format("{0}{1}", value, cellVerticalLine);
                        line += Indent(maxCellWidth);
                        if (j < (dimension2Length - 1)) {
                            line += cellTJoint;
                        }
                    }
                    line += cellVerticalJointRight;
                    formattedString += string.Format("{0}{1}", lineWithValues, System.Environment.NewLine);
                    if (i < (dimension1Length - 1)) {
                        formattedString += string.Format("{0}{1}", line, System.Environment.NewLine);
                    }
                }

                //printing bottom line
                formattedString += string.Format("{0}{1}{2}{3}", cellLeftBottom, Indent(dimension2Length, maxCellWidth, false), cellRightBottom, System.Environment.NewLine);
                return formattedString;
            }

            private static string Indent(int count) {
                return string.Empty.PadLeft(count, '─');
            }

            private static string Indent(int dimension2Length, int maxCellWidth, bool Top) {
                string joint = Top ? cellHorizontalJointTop : cellHorizontalJointbottom;
                //return string.Empty.PadLeft(count, '─');     
                string Indent = string.Empty.PadLeft(maxCellWidth, '─');
                for (int n = 1; n < dimension2Length; n++)
                    Indent += joint + string.Empty.PadLeft(maxCellWidth, '─');
                return Indent;
            }

            #endregion

            #region Public Methods

            public static void PrintToStream(string[,] arrValues, StreamWriter writer) {
                if (arrValues == null)
                    return;

                if (writer == null)
                    return;

                writer.Write(GetDataInTableFormat(arrValues));
            }

            public static void PrintToConsole(string[,] arrValues) {
                if (arrValues == null)
                    return;

                Console.WriteLine(GetDataInTableFormat(arrValues));
            }

            #endregion
        }

    }
}
