using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GCore.Threading;

namespace GCore.Networking.Socket {
    public class GSocketListener<T> where T: class {
        private int _port;
        private bool _active = true;
        private Func<GSocketListener<T>, TcpClient, T> _activator = null;

        public int Port {
            get { return _port; }
        }

        private TcpListener _tcpListener;

        public TcpListener TCP_Listener {
            get { return _tcpListener; }
        }

        private GThread _acceptorThread;

        public delegate void ClientArrivedHandler(GSocketListener<T> sender, T Client);
        public event ClientArrivedHandler ClientArrived;

        public GSocketListener(IPAddress address, int port, GSocketListener<T>.ClientArrivedHandler handler, bool setReuseAddress = true, Func<GSocketListener<T>, TcpClient, T> activator = null) {
            _port = port;
            _activator = activator;

            ClientArrived += handler;

            _tcpListener = new TcpListener(address, _port);
            if(setReuseAddress)
                _tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            _tcpListener.Start();

            _acceptorThread = new GThread(new ThreadStart(_acceptorloop), "Acceptor Loop", true);
            _acceptorThread.Start();
        }

        public GSocketListener(int port, GSocketListener<T>.ClientArrivedHandler handler, bool setReuseAddress = true, Func<GSocketListener<T>, TcpClient, T> activator = null) {
            _port = port;
            _activator = activator;

            ClientArrived += handler;

            _tcpListener = new TcpListener(_port);
            if (setReuseAddress)
                _tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            _tcpListener.Start();

            _acceptorThread = new GThread(new ThreadStart(_acceptorloop), "Acceptor Loop", true);
            _acceptorThread.Start();
        }

        public void Kill() {
            _active = false;
            _acceptorThread.Abort();
            _tcpListener.Stop();
        }

        private void _acceptorloop() {
            while (this._active) {
                if (!_tcpListener.Pending()) {
                    Thread.Sleep(100); // choose a number (in milliseconds) that makes sense
                    continue; // skip to next iteration of loop
                }
                TcpClient clientSocket = _tcpListener.AcceptTcpClient();

                T tclient = null;

                if (_activator is null)
                {
                    tclient = (T)Activator.CreateInstance(typeof(T) ,this, clientSocket);
                }
                else
                {
                    tclient = _activator(this, clientSocket);
                }

                if (ClientArrived != null) ClientArrived(this, tclient);
            }
        }
    }

    public class GSocketListener : GSocketListener<GSocket>
    {
        public GSocketListener(IPAddress address, int port, ClientArrivedHandler handler, bool setReuseAddress = true) : base(address, port, handler, setReuseAddress, GSocketActivator)
        {
        }

        public GSocketListener(int port, ClientArrivedHandler handler, bool setReuseAddress = true) : base(port, handler, setReuseAddress, GSocketActivator)
        {
        }

        protected static GSocket GSocketActivator(GSocketListener<GSocket> self, TcpClient tcpClient) => new GSocket(tcpClient);
    }

    public class TcpSocketListener : GSocketListener<TcpClient>
    {
        public TcpSocketListener(IPAddress address, int port, ClientArrivedHandler handler, bool setReuseAddress = true) : base(address, port, handler, setReuseAddress, GSocketActivator)
        {
        }

        public TcpSocketListener(int port, ClientArrivedHandler handler, bool setReuseAddress = true) : base(port, handler, setReuseAddress, GSocketActivator)
        {
        }

        protected static TcpClient GSocketActivator(GSocketListener<TcpClient> self, TcpClient tcpClient) => tcpClient;
    }

}
