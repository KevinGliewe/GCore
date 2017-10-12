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

        #region Helper
        public static byte[] GetBytes(string str) {
            byte[] bytes = new byte[str.Length];
            char[] chars = str.ToCharArray();
            for (int n = 0; n < str.Length; n++)
                bytes[n] = (byte)chars[n];
            return bytes;
        }

        public static string GetString(byte[] bytes) {
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
        #endregion
    }
}