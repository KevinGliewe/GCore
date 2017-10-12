using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GCore.Extensions.ArrayEx;
using GCore.Extensions.MemoryStreamEx;
using GCore.Networking.Socket;


//Author : KEG
//Datum  : 25.09.2015 11:23:41
//Datei  : RPCBase.cs


namespace GCore.Networking.RPC.Protokoll
{
    public abstract class RPCBase
    {

        #region Members
        private int _msglen = 0;
        private MemoryStream _msgdata = new MemoryStream();
        private BaseSocket _socket = null;
        protected Dictionary<Type, Func<BasePacket, BaseResponse>> _handler = new Dictionary<Type, Func<BasePacket, BaseResponse>>();
        #endregion

        #region Events
        public event Action<RPCBase> Closed; 
        #endregion

        #region Initialization
        public RPCBase(BaseSocket socket)
        {
            _socket = socket;
            _socket.DataArrived += new BaseSocket.DataArrivedHandler(_socket_DataArrived);
            _socket.Closed += new BaseSocket.ClosedHandler(_socket_Closed);
        }
        #endregion

        #region Finalization
        ~RPCBase()
        {

        }
        #endregion

        #region Interface

        public void Send(BasePacket packet)
        {
            byte[] data = packet.Serialize();

            byte[] outBuffer = new byte[data.Length + 4];

            ByteWriter.WriteInt32_BE(data.Length, outBuffer, 0);

            Array.Copy(data, 0, outBuffer, 4, data.Length);

            this._socket.Send(outBuffer);
        }
        #endregion

        #region Eventhandler
        void _socket_DataArrived(BaseSocket sender, byte[] data)
        {
            _msgdata.Position = _msgdata.Length;
            _msgdata.Write(data, 0, data.Length);
            _msgdata.Position = 0;

            BasePacket message = null;

            do
            {
                message = null;

                if (_msgdata.Length >= 4 && _msglen == 0)
                {
                    _msglen = ByteWriter.ReadInt32_BE(_msgdata.GetBuffer(), 0);
                    _msgdata.Position = 4;
                    _msgdata = _msgdata.Cut();
                }

                if (_msgdata.Length >= _msglen && _msglen > 0)
                {

                    message = BasePacket.Deserialize(_msgdata.ToArray().Slice(0, _msglen));

                    if (_msgdata.Length == _msglen)
                        _msgdata = new MemoryStream();
                    else
                    {
                        _msgdata.Position = _msglen;
                        _msgdata = _msgdata.Cut();
                    }

                    _msglen = 0;
                }

#if (COMMTEST)
                Console.WriteLine(string.Format("{0} ~ ms[{1}]", IsServer ? "Server" : "Client", _msgdata.Length));
#endif

                if (message != null)
                {
                    

#if (COMMTEST)
                    Console.WriteLine(message);
#endif
                    try
                    {
                        Type msgType = message.GetType();
                        foreach (var handler in _handler)
                        {
                            if (handler.Key == msgType)
                            {
                                BaseResponse resp = handler.Value(message);
                                if (resp != null)
                                    this.Send(resp);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Send(new ExceptionResponse(ex.ToString(), message));
                    }

                }
            } while (message != null);
        }

        void _socket_Closed(BaseSocket sender)
        {
            if (this.Closed != null)
                this.Closed(this);
        }
        #endregion

        #region Tools
        #endregion

        #region Browsable Properties
        #endregion

        #region NonBrowsable Properties
        #endregion
    }
}