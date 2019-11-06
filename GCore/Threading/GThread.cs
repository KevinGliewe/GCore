using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCore.Threading {
    /// <summary>
    /// Description of XThread.
    /// </summary>
    public class GThread {
        public delegate void OnNewGThreadHandler(GThread gThread);
        public static event OnNewGThreadHandler OnNewGThread;

        public delegate void OnEndGThreadHandler(GThread gThread);
        public static event OnEndGThreadHandler OnEndGThread;

        public static List<GThread> AllGThreads = new List<GThread>();

        public static GThread GetCurrentGThread() {
            Thread currentThread = Thread.CurrentThread;
            foreach (GThread gThread in AllGThreads)
                if (gThread.Thread == currentThread)
                    return gThread;

            GThread gthread = new GThread();
            gthread._thread = currentThread;
            gthread._processThread = GThread.GetCurrentProcessThread();
            gthread.Name = GCore.Sys.Process.Utils.GetLastMethodName();
            AllGThreads.Add(gthread);
            return gthread;
        }
        

        public delegate void OnErrorHandler(GThread sender, Exception ex);
        public event OnErrorHandler OnError;


        internal Thread _thread;
        internal ProcessThread _processThread;
        private ThreadStart _threadStart;

        public Thread Thread { get { return _thread; } }
        public ProcessThread ProcessThread { get { return _processThread; } }

        private long _oldThreadCPUTime = 0;
        private long _oldProcessCPUTime = 0;

        public string Name { get; set; }

        internal GThread() { }

        public GThread(ThreadStart threadStart, string name = null, bool isBackround = false) {
            if (name == null) this.Name = GCore.Sys.Process.Utils.GetLastMethodName();
            else this.Name = name;
            _threadStart = threadStart;
            _thread = new Thread(_thread_start);
            _thread.IsBackground = isBackround;
            _thread.Name = this.Name;
        }
      

        public void Start() {
            _thread.Start();
        }

        private void _thread_start() {
            _processThread = GetCurrentProcessThread();
            AllGThreads.Add(this);
            if (OnNewGThread != null) OnNewGThread(this);
            try {
                _threadStart();
            } catch (Exception ex) {
                Logging.Log.Exception("Unhandled GThread Excaption", ex, this);
                if (OnError != null)
                    OnError(this, ex);
            } finally {
                if(AllGThreads.Contains(this))
                    AllGThreads.Remove(this);
                if (OnEndGThread != null) OnEndGThread(this);
            }
        }

        public double GetCPUUsageRelative() {
            if (_processThread is null)
                return double.NaN;
            Process p = Process.GetCurrentProcess();
            long threadCpuTime = _processThread.TotalProcessorTime.Ticks;
            long processCpuTime = p.TotalProcessorTime.Ticks;

            double usage = ((double)(threadCpuTime - _oldThreadCPUTime)) / ((double)(processCpuTime - _oldProcessCPUTime));
            //Console.WriteLine(string.Format("threadCpuTime={0} : threadCpuTime={1}\nprocessCpuTime={2} : _oldProcessCPUTime{3}",
            //                                threadCpuTime,_oldThreadCPUTime,
            //                                processCpuTime,_oldProcessCPUTime));

            _oldThreadCPUTime = threadCpuTime;
            _oldProcessCPUTime = processCpuTime;

            return usage;
        }

        public void Join() {
            _thread.Join();
        }

        public void Resume() { _thread.Resume(); }

        public void Abort() { _thread.Abort(); }

        public static ProcessThread GetCurrentProcessThread() {
            int threadID = AppDomain.GetCurrentThreadId();
            foreach (ProcessThread pt in Process.GetCurrentProcess().Threads) {
                if (pt.Id == threadID) {
                    return pt;
                }
            }
            return null;
            //ProcessThread pthread = thread;
        }

        public class GThreadManager {
            #region Static
            private static GThreadManager _manager;
            public static GThreadManager Manager {
                get {
                    if (_manager == null)
                        _manager = new GThreadManager();
                    return _manager;
                }
            }
            #endregion


            private GThreadManager() {
                GThread.OnNewGThread += new OnNewGThreadHandler(GThread_OnNewGThread);
                GThread.OnEndGThread += new OnEndGThreadHandler(GThread_OnEndGThread);
                new Task(_timer_Tick).Start();
            }

            async void _timer_Tick() {
                while (true)
                {
                    await Task.Delay(200);
                    foreach (GThread gThread in GThread.AllGThreads)
                        if (this.CPUUsage.ContainsKey(gThread))
                            this.CPUUsage[gThread] = gThread.GetCPUUsageRelative();
                        else
                            this.CPUUsage.Add(gThread, gThread.GetCPUUsageRelative());
                }
            }

            void GThread_OnEndGThread(GThread gThread) {
                this.CPUUsage.Remove(gThread);
            }

            void GThread_OnNewGThread(GThread gThread) {
                this.CPUUsage.Add(gThread, gThread.GetCPUUsageRelative());
            }

            public Dictionary<GThread, double> CPUUsage = new Dictionary<GThread, double>();


        }

        public class CycleSleeper {
            DateTime _lastSleep = DateTime.Now;
            TimeSpan _cycleTime = TimeSpan.FromMilliseconds(40);
            public CycleSleeper(TimeSpan cycleTime) {
                this._cycleTime = cycleTime;
            }

            public void Sleep() {
                
                TimeSpan sleep = _cycleTime - (DateTime.Now - _lastSleep);
                if(sleep.Ticks<0)
                    sleep = TimeSpan.FromTicks(0);
                Thread.Sleep(sleep);
                _lastSleep = DateTime.Now;
            }
        }
    }
}
