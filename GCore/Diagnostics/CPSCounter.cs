using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Diagnostics {
    public class CPSCounter {
        private DateTime _lastCall;
        private double _cps = 0.0;
        private bool _fistCall = true;
        private double _damping = 1.0;

        public CPSCounter(double damping = 1.0) {
            _lastCall = DateTime.Now;
            _damping = damping;
        }

        public void Call() {
            double dt = (DateTime.Now - _lastCall).TotalSeconds;
            _lastCall = DateTime.Now;
            double currentcps = 1.0 / dt;
            if (_fistCall) {
                _fistCall = false;
                _cps = currentcps;
            } else
                _cps = (_cps * _damping + currentcps) / (_damping + 1.0);
        }

        public double CPS { get { return _cps; } }
    }
}
