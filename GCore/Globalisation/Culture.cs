using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace GCore.Globalisation {
    public class Culture : IDisposable {
        public CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture("en-US");

        private CultureInfo _tmpCulture;

        public Culture() {
            _hook(DefaultCulture);
        }
        public Culture(CultureInfo culture) {
            _hook(culture);
        }

        private void _hook(CultureInfo culture) {
            _tmpCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public void Dispose() {
            Thread.CurrentThread.CurrentCulture = _tmpCulture;
        }
    }
}
