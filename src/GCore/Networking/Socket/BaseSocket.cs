using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using GCore.Logging;
using GCore.Threading;

namespace GCore.Networking.Socket
{
    public class BaseSocket : IDisposable
    {
        public string Name = "";

        private bool _isActive = false;

        public bool isActive
        {
            get { return _isActive; }
        }

        private Stream _inStream = null;
        private Stream _outStream = null;

        private GThread _readerThread;

        public delegate void DataArrivedHandler(BaseSocket sender, byte[] Data);

        public event BaseSocket.DataArrivedHandler DataArrived;

        public delegate void ClosedHandler(BaseSocket sender);

        public event BaseSocket.ClosedHandler Closed;

        public int MaxPackSize;

        protected BaseSocket()
        {
        }

        public BaseSocket(Stream inStream, Stream outStream, int maxPackSize = 2048)
        {
            this.Init(inStream, outStream, maxPackSize);
        }

        public void Init(Stream inStream, Stream outStream, int maxPackSize = 2048)
        {
            _inStream = inStream;
            _outStream = outStream;

            _isActive = true;
            MaxPackSize = maxPackSize;

            _readerThread = new GThread(new System.Threading.ThreadStart(_readerloop),
                "Reader-Tread for " + this.ToString(), true);
            _readerThread.Thread.IsBackground = true;
            _readerThread.Start();
        }

        public void Kill()
        {
            if (_isActive == false) return;
            _isActive = false;
            try
            {
                _inStream.Close();
            }
            catch (Exception)
            {
            }
            try
            {
                _outStream.Close();
            }
            catch (Exception)
            {
            }
            try
            {
                if (_readerThread.Thread.IsAlive) _readerThread.Abort();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (Closed != null) Closed(this);
            }
        }

        #region Send

        public int Send(string data)
        {
            return this.Send(GSocket.GetBytes(data));
            //return this.Send(System.Text.Encoding.UTF8.GetBytes(data));
            //return this.Send(new System.Text.ASCIIEncoding().GetBytes(data));
        }

        public virtual int Send(byte[] data)
        {

            _outStream.Write(data, 0, data.Length);
            _outStream.Flush();
            return data.Length;
        }

        public int Send(params int[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params uint[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params short[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params float[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params double[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params long[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params bool[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params char[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params ulong[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        public int Send(params ushort[] data)
        {
            List<byte> datalist = new List<byte>();
            foreach (var d in data)
                datalist.AddRange(BitConverter.GetBytes(d));
            return this.Send(datalist.ToArray());
        }

        #endregion

        #region Loops

        protected virtual void _readerloop()
        {
            byte[] data = new byte[MaxPackSize];
            while (_isActive)
            {
                try
                {
                    int size = _inStream.Read(data, 0, MaxPackSize);
                    if (size == 0)
                    {
                        this.Kill();
                        //Thread.Sleep(10);
                        //continue;
                    }
                    byte[] data1 = new byte[size];

                    Array.Copy(data, data1, size);


                    if (DataArrived != null) DataArrived(this, data1);
                }
                catch (System.IO.IOException ex)
                {
                    //Console.WriteLine("ex0: " + ex.Message);
                    this.Kill();
                }
                catch (Exception ex)
                {
                    Log.Exception("Exception while Data arrival", ex);
                }
            }
        }

        #endregion

        #region Helper
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            char[] chars = str.ToCharArray();
            for (int n = 0; n < str.Length; n++)
                bytes[n] = (byte)chars[n];
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];
            for (int n = 0; n < bytes.Length; n++)
                chars[n] = (char)bytes[n];
            return new string(chars);
        }

        //public static byte[] GetBytes(string str)
        //{
        //    byte[] bytes = new byte[str.Length * sizeof(char)];
        //    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    return bytes;
        //}

        //public static string GetString(byte[] bytes)
        //{
        //    char[] chars = new char[bytes.Length / sizeof(char)];
        //    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //    return new string(chars);
        //}

        protected void PushDataArrived(byte[] data)
        {
            if (this.DataArrived != null)
                this.DataArrived(this, data);
        }

        protected void PushClosed()
        {
            if (this.Closed != null)
                this.Closed(this);
        }
        #endregion

        public Stream InStream
        {
            get { return _inStream; }
        }

        public Stream OutStream
        {
            get { return _outStream; }
        }

        public void Dispose()
        {
            this.Kill();
        }
    }

}