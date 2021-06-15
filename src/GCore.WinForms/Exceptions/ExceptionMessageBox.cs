using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCore.WinForms.Exceptions {
    public partial class ExceptionMessageBox : Form
    {
        private Exception _exception;

        private ExceptionMessageBox(Exception ex, string caption = null) {
            InitializeComponent();

            this.Text = caption ?? ex.ToString();

            xPropertyGrid1.SelectedObject = ex;
        }

        public static void ShowExceptionMessageBox(Exception ex, string caption = null)
        {
            new ExceptionMessageBox(ex, caption).ShowDialog();
        }
    }
}
