using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GCore.Threading;
using GCore.WinForms.Controls.GracialList;
using System.Diagnostics;

namespace GCore.WinForms.Controls {
    public partial class ThreadManager : UserControl {

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        PerformanceCounter ramCounter1;
        PerformanceCounter ramCounter2;
        PerformanceCounter pageCounter;

        public ThreadManager() {
            InitializeComponent();


            ramCounter = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
            cpuCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);//"_Total");
            ramCounter1 = new PerformanceCounter("Memory", "Available MBytes", String.Empty);
            ramCounter2 = new PerformanceCounter("Memory", "% Committed Bytes In Use", String.Empty);
            pageCounter = new PerformanceCounter("Paging File", "% Usage", Process.GetCurrentProcess().ProcessName);//, "_Total");

            GThread.OnNewGThread += new GThread.OnNewGThreadHandler(_AddGThread);
            GThread.OnEndGThread += new GThread.OnEndGThreadHandler(_EndGThread);

            foreach (GThread g in GThread.AllGThreads)
                this._AddGThread(g);
        }

        private void _AddGThread(GThread gThread) {
            GLItem item = new GLItem();
            item.Tag = gThread;

            GLSubItem itemName = new GLSubItem();
            itemName.Text = gThread.Name;

            ProgressBar pb = new ProgressBar();
            GLSubItem itemUsage = new GLSubItem();

            GLSubItem itemPercent = new GLSubItem();

            itemUsage.Control = pb;
            pb.Minimum = 0;
            pb.Maximum = 100;

            item.SubItems.Add(itemName);
            item.SubItems.Add(itemUsage);
            item.SubItems.Add(itemPercent);

            glGTreadList.Items.Add(item);
        }

        private void _EndGThread(GThread gThread) {
            GLItem itemToDelete = null;

            foreach(GLItem item in glGTreadList.Items)
                if (item.Tag == gThread) {
                    itemToDelete = item;
                    break;
                }
            if (itemToDelete == null) {
                Logging.Log.Warn("GThread not found", gThread);
                return;
            }

            glGTreadList.Items.Remove(itemToDelete);
        }

        private void timerUpdate_Tick(object sender, EventArgs e) {
            foreach (GLItem item in glGTreadList.Items) {
                GThread gThread = (GThread)item.Tag;
                ProgressBar pb = (ProgressBar)item.SubItems[1].Control;
                double usage = gThread.GetCPUUsageRelative() * 100.0;
                if(usage > 100.0) usage = 100.0;
                if(usage < 0.0) usage = 0.0;
                if (Double.IsNaN(usage)) usage = 100.0;
                usage = Math.Truncate(usage * 100) / 100;
                pb.Value = (int)usage;
                item.SubItems[2].Text = usage.ToString() + "%";
            }
            pbCPU.Value = (int)cpuCounter.NextValue();
            lblPRAM.Text = "Process RAM: " + ramCounter.NextValue().ToString() + "MB";
            lblSRAM.Text = "System RAM: " + ramCounter1.NextValue().ToString() + "MB";
        }
    }
}
