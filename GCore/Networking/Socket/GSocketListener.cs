using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GCore.Threading;

namespace GCore.Networking.Socket {
    public class GSocketListener {
        private int _port;
        private bool _active = true;

        public int Port {
            get { return _port; }
        }

        private TcpListener _tcpListener;

        public TcpListener TCP_Listener {
            get { return _tcpListener; }
        }

        private GThread _acceptorThread;

        public delegate void ClientArrivedHandler(GSocketListener sender, GSocket Client);
        public event ClientArrivedHandler ClientArrived;

        public GSocketListener(IPAddress address, int port, GSocketListener.ClientArrivedHandler handler, bool setReuseAddress = true) {
            _port = port;

            ClientArrived += handler;

            _tcpListener = new TcpListener(address, _port);
            if(setReuseAddress)
                _tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            _tcpListener.Start();

            _acceptorThread = new GThread(new ThreadStart(_acceptorloop), "Acceptor Loop", true);
            _acceptorThread.Start();
        }

        public GSocketListener(int port, GSocketListener.ClientArrivedHandler handler, bool setReuseAddress = true) {
            _port = port;

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
                TcpClient clientSocket = default(TcpClient);
                clientSocket = _tcpListener.AcceptTcpClient();
                GSocket gs = new GSocket(clientSocket);
                if (ClientArrived != null) ClientArrived(this, gs);
            }
        }
    }
}
