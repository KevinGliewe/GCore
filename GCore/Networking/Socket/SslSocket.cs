using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace GCore.Networking.Socket
{
    /*
        set /p passwrt=Enter The Passwort:%=%
        set /p srv=Enter The Server:%=%
        makecert.exe -r -pe -n "CN=%srv%" -sky exchange -sv server.pvk server.cer
        pvk2pfx -pvk server.pvk -spc server.cer -pfx server.pfx -pi %passwrt%
     */
    public class SslSocket : BaseSocket
    {
        private TcpClient _tcpClient = null;
        private X509Certificate _certificate = null;

        private SslStream _sslStream = null;

        private bool _isServer = false;

        protected SslSocket()
        {}

        public SslSocket(X509Certificate certificate, TcpClient tcpclient, bool isServer, int maxPackSize = 2048)
        {
            this.Init(certificate, tcpclient, isServer, maxPackSize);
        }

        public void Init(X509Certificate certificate, TcpClient tcpclient, bool isServer, int maxPackSize = 2048)
        {
            _isServer = isServer;
            _tcpClient = tcpclient;
            _certificate = certificate;

            if (_isServer)
            {
                _sslStream = new SslStream(_tcpClient.GetStream());
                _sslStream.AuthenticateAsServer(_certificate);
            }
            else
            {
                X509CertificateCollection certs = new X509CertificateCollection();
                certs.Add(_certificate);
                _sslStream = new SslStream(_tcpClient.GetStream(),
                    false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                _sslStream.AuthenticateAsClient("localhost", certs, SslProtocols.Tls, false);
            }

            base.Init(_sslStream, _sslStream, maxPackSize);
        }

        public virtual bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(sslPolicyErrors);
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0 ||
                (sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                return true;
            }

            return false;
        }
    }
}