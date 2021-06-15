using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.EventEx {
    public static class EventExtensions {

        //public event EventHandler SomethingHappened = delegate {};
        /// <summary>
        /// Führt den Event aus wenn er nicht null ist
        /// </summary>
        /// <param name="eventX"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static public void RaiseEvent(this EventHandler @eventX, object sender, EventArgs e) {
            if (@eventX != null)
                @eventX(sender, e);
        }

        /// <summary>
        /// Führt den Event aus wenn er nicht null ist
        /// </summary>
        static public void RaiseEvent<T>(this EventHandler<T> @eventX, object sender, T e)
            where T : EventArgs {
            if (@eventX != null)
                @eventX(sender, e);
        }
    }
}
