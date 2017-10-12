using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;

namespace GCore.Sys.Process {
    class ProcessWrapper {
        public delegate void ProcessCallback(string msg);

        public ProcessCallback Callback;

        public ProcessStartInfo PInfo;
        public System.Diagnostics.Process Proc;

        string Output = "";

        private bool isRunning = false;
        public bool IsRunning {
            get { return isRunning; }
        }

        public ProcessWrapper(string command, string arguments) {
            PInfo = new ProcessStartInfo(command, arguments);
            // Redirect the standard output of the process. 
            PInfo.RedirectStandardOutput = true;
            PInfo.RedirectStandardError = true;
            PInfo.RedirectStandardInput = true;
            PInfo.CreateNoWindow = true;

            // Set UseShellExecute to false for redirection
            PInfo.UseShellExecute = false;

            Proc = new System.Diagnostics.Process();
            Proc.StartInfo = PInfo;
            Proc.EnableRaisingEvents = true;

            // Set our event handler to asynchronously read the sort output.
            //Proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
            Proc.Exited += new EventHandler(proc_Exited);

            //Proc.Start();
            // Start the asynchronous read of the sort output stream. Note this line!

        }

        public void Start() {
            Proc.Start();
            //Proc.BeginOutputReadLine();
            Proc.BeginErrorReadLine();
            isRunning = true;

            Thread t1 = new Thread(new ThreadStart(_worker));
            t1.Start();
            Thread t2 = new Thread(new ThreadStart(_reader));
            t2.Start();
        }

        public void Kill() {
            Proc.Kill();
        }

        public void Command(string msg) {
            //last += msg.Length;
            Proc.StandardInput.WriteLine(msg);
            if (msg.Equals("exit")) {
                Thread.Sleep(10);
                Proc.StandardInput.WriteLine("exit");
                Thread.Sleep(10);
                if (isRunning) this.Kill();
                return;
            }
        }

        void proc_Exited(object sender, EventArgs e) {
            isRunning = false;
        }



        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data == null) {
                isRunning = false;
                return;
            }
            if (Callback != null) Callback("ERROR : " + e.Data);
        }

        void _reader() {
            while (this.isRunning) {
                Output += (char)Proc.StandardOutput.Read();
            }
        }

        int last = 0;
        void _worker() {

            while (this.isRunning) {
                if (Output.Length > last && Callback != null) {
                    lock (Output) {
                        Callback(Output.Substring(last));
                        last = Output.Length;
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}
