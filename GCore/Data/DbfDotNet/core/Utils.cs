using System.Text;

namespace GCore.Data.DbfDotNet.Core
{
    public class Utils
    {
        public static string HexDump(byte[] bytes)
        {
            if (bytes == null) return "<null>";
            int len = bytes.Length;
            //           1         2         3         4         5         6         7         8
            // 012345678901234567890123456789012345678901234567890123456789012345678901234567890
            // 12345678  00 11 22 33 44 55 66 77  88 99 AA BB CC DD EE FF  0123456789ABCDEF<<
            StringBuilder result = new StringBuilder(((len + 15) / 16) * 76);
            char[] chars = new char[78];
            // fill all with blanks
            for (int i = 0; i < 75; i++) chars[i] = ' ';
            chars[76] = '\r';
            chars[77] = '\n';

            for (int i1 = 0; i1 < len; i1 += 16)
            {
                chars[0] = HexChar(i1 >> 28);
                chars[1] = HexChar(i1 >> 24);
                chars[2] = HexChar(i1 >> 20);
                chars[3] = HexChar(i1 >> 16);
                chars[4] = HexChar(i1 >> 12);
                chars[5] = HexChar(i1 >> 8);
                chars[6] = HexChar(i1 >> 4);
                chars[7] = HexChar(i1 >> 0);

                int offset1 = 11;
                int offset2 = 60;

                for (int i2 = 0; i2 < 16; i2++)
                {
                    if (i1 + i2 >= len)
                    {
                        chars[offset1] = ' ';
                        chars[offset1 + 1] = ' ';
                        chars[offset2] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i1 + i2];
                        chars[offset1] = HexChar(b >> 8);
                        chars[offset1 + 1] = HexChar(b);
                        chars[offset2] = (b < 32 ? '·' : (char)b);
                    }
                    offset1 += (i2 == 8 ? 4 : 3);
                    offset2++;
                }
                result.Append(chars);
            }
            return result.ToString();
        }

        private static char HexChar(int value)
        {
            value &= 0xF;
            if (value >= 0 && value <= 9) return (char)('0' + value);
            else return (char)('A' + (value - 10));
        }

        public static void Nop()
        {
        }

    }  
}
