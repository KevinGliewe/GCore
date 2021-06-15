using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.WinForms.Controls {
    public class ComboboxItem {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString() {
            return Text;
        }

        public ComboboxItem() { }

        public ComboboxItem(string text, object value) {
            this.Text = text;
            this.Value = value;
        }
    }
}
