using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GCore.Data.MPack;
using GCore.Logging;
using GCore.Threading;

namespace GCore.Networking.Socket
{

    public class MPackSocket
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

        private TcpClient _tcpClient;

        public TcpClient TCP_Client
        {
            get { return _tcpClient; }
        }

        public delegate void DataArrivedHandler(MPackSocket sender, MPack Data);

        public event MPackSocket.DataArrivedHandler DataArrived;

        public delegate void ClosedHandler(MPackSocket sender);

        public event MPackSocket.ClosedHandler Closed;

        public int MaxPackSize;

        protected MPackSocket()
        {
        }

        public MPackSocket(TcpClient tcpclient)
        {
            if (!tcpclient.Connected) throw new Exception("Must be connected");
            _tcpClient = tcpclient;

            Init(tcpclient.GetStream(), tcpclient.GetStream());
        }

        public MPackSocket(Stream inStream, Stream outStream)
        {
            this.Init(inStream, outStream);
        }

        public void Init(Stream inStream, Stream outStream)
        {
            _inStream = inStream;
            _outStream = outStream;

            _isActive = true;

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

        public virtual void Sent(MPack data)
        {
            data.EncodeToStream(_outStream);
            _outStream.Flush();
        }

        public virtual async Task SendAsync(MPack data, CancellationToken cancellationToken)
        {
            await data.EncodeToStreamAsync(_outStream, cancellationToken);
            await _outStream.FlushAsync(cancellationToken);
        }

        public virtual async Task SendAsync(MPack data)
        {
            await data.EncodeToStreamAsync(_outStream);
            await _outStream.FlushAsync();
        }

        #endregion

        #region Loops

        protected virtual void _readerloop()
        {
            while (_isActive)
            {
                try
                {
                    MPack data = MPack.ParseFromStream(_inStream);

                    PushDataArrived(data);
                }
                catch (System.IO.IOException ex)
                {
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

        protected void PushDataArrived(MPack data)
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

    public class MPackSocketListener : GSocketListener<MPackSocket>
    {
        public MPackSocketListener(IPAddress address, int port, ClientArrivedHandler handler, bool setReuseAddress = true) : base(address, port, handler, setReuseAddress, GSocketActivator)
        {
        }

        public MPackSocketListener(int port, ClientArrivedHandler handler, bool setReuseAddress = true) : base(port, handler, setReuseAddress, GSocketActivator)
        {
        }

        protected static MPackSocket GSocketActivator(GSocketListener<MPackSocket> self, TcpClient tcpClient) => new MPackSocket(tcpClient);
    }
}