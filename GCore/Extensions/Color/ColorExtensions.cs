using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GCore.Extensions.IComparableEx;

namespace GCore.Extensions.ColorEx {
    public static class ColorExtensions {
        /// <summary>
        /// Erhöht den Farbwehrt um die angegebene Wehrte und
        /// passt sie in den vorgegebenen Wehrtebereich ein.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Color Pitch(this Color color, int r = 0, int g = 0, int b = 0, int a = 0) {
            int R = (color.R + r).FitInto(0, 255);
            int G = (color.G + g).FitInto(0, 255);
            int B = (color.B + b).FitInto(0, 255);
            int A = (color.A + a).FitInto(0, 255);

            return Color.FromArgb(A, R, G, B);
        }
    }
}
