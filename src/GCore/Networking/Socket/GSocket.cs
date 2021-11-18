using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using GCore.Threading;

namespace GCore.Networking.Socket {
    public class GSocket : BaseSocket {


        private TcpClient _tcpClient;

        public TcpClient TCP_Client
        {
            get { return _tcpClient; }
        }

        protected GSocket() {}

        public GSocket(TcpClient tcpclient, int maxPackSize = 2048) {
            this.Init(tcpclient, maxPackSize);
        }

        public void Init(TcpClient tcpclient, int maxPackSize = 2048)
        {
            if (!tcpclient.Connected) throw new Exception("Must be connected");
            _tcpClient = tcpclient;

            base.Init(tcpclient.GetStream(), tcpclient.GetStream(), maxPackSize);
        }

    }
}