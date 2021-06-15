using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GCore.Extensions.MemoryStreamEx
{
    public static class MemoryStreamEx
    {
        /// <summary>
        /// Cuts the MemoryStream bevore the position.
        /// </summary>
        /// <param key="this_">The Stream.</param>
        /// <returns>The result</returns>
        public static MemoryStream Cut(this MemoryStream this_)
        {
            byte[] buffer = this_.ToArray().Skip((int)this_.Position).ToArray();
            MemoryStream ret = new MemoryStream();
            ret.Write(buffer, 0, buffer.Length);
            ret.Position = 0;
            return ret;
        }
    }
}
