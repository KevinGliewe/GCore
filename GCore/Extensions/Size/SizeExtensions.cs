using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ErhardtAbt.Extensions.SizeEx {
    public static class SizeExtensions {
        /// <summary>
        /// Addiert zu der Goße den übergebenen Wehrt hinzu.
        /// </summary>
        /// <param name="_this_"></param>
        /// <param name="wh">Width und Height</param>
        /// <returns></returns>
        public static Size Add(this Size _this_, int wh) {
            return _this_.Add(wh, wh);
        }

        /// <summary>
        /// Addiert zu der Goße die übergebenen Wehrte hinzu.
        /// </summary>
        /// <param name="_this_"></param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <returns></returns>
        public static Size Add(this Size _this_, int w, int h) {
            return new Size(_this_.Width + w, _this_.Height + h);
        }

        /// <summary>
        /// Addiet die Größe hinzu.
        /// </summary>
        /// <param name="_this_"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Size Add(this Size _this_, Size other) {
            return _this_.Add(other.Width, other.Height);
        }
    }
}
