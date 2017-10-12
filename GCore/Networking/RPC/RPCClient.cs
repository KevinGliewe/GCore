using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using GCore.Extensions.IEnumerableEx;
using GCore.Logging;
using GCore.Networking.RPC.Protokoll;
using GCore.Networking.Socket;


//Author : KEG
//Datum  : 25.09.2015 10:36:25
//Datei  : RPCClient.cs


namespace GCore.Networking.RPC
{
    public class RPCClient : RPCBase
    {

        #region Members
        private List<BaseResponse> _responses = new List<BaseResponse>();
        private Dictionary<int,Action<BaseResponse>> _responsCallbacks = new Dictionary<int, Action<BaseResponse>>(); 
        #endregion

        #region Events
        #endregion

        #region Initialization
        public RPCClient(BaseSocket socket) : base(socket)
        {
            _handler.Add(typeof(ExceptionResponse), _handler_Exception);
            _handler.Add(typeof(GetMemberResponse), _handler_GetMember);
            _handler.Add(typeof(SetMemberResponse), _handler_GetMember);
            _handler.Add(typeof(InvokeResponse), _handler_Invoke);
            _handler.Add(typeof(ProxyResponse), _handler_Proxy);
        }
        #endregion

        #region Finalization
        ~RPCClient()
        {

        }
        #endregion

        #region PacketHandler

        private BaseResponse _handler_Exception(BasePacket packet)
        {
            _addresponse((BaseResponse)packet);
            return null;
        }

        private BaseResponse _handler_GetMember(BasePacket packet)
        {
            _addresponse((BaseResponse)packet);
            return null;
        }

        private BaseResponse _handler_SetMember(BasePacket packet)
        {
            _addresponse((BaseResponse)packet);
            return null;
        }

        private BaseResponse _handler_Invoke(BasePacket packet)
        {
            _addresponse((BaseResponse)packet);
            return null;
        }

        private BaseResponse _handler_Proxy(BasePacket packet)
        {
            _addresponse((BaseResponse)packet);
            return null;
        }

        private void _addresponse(BaseResponse resp)
        {
            if (_responsCallbacks.ContainsKey(resp.ResponseFor))
            {
                try
                {
                    _responsCallbacks[resp.ResponseFor](resp);
                }
                catch (Exception ex)
                {
                    Log.Exception("Exception during Response-Callback", ex);
                }
                _responsCallbacks.Remove(resp.ResponseFor);
                return;
            }

            _responses.Add(resp);
            if (_responses.Count > 100)
            {
                _responses.RemoveAt(0);
            }
        }

        #endregion

        #region Interface

        public BaseResponse GetResponse(BasePacket id)
        {
            BaseResponse resp = _responses.ReturnFirstTrue(response => response.ResponseFor == id.ID);
            if (resp != null) _responses.Remove(resp);
            return resp;
        }

        public BaseResponse WaitForResponse(BasePacket id, int timeout)
        {
            long end = DateTime.Now.Ticks + timeout * TimeSpan.TicksPerMillisecond;
            BaseResponse resp = null;
            while (end > DateTime.Now.Ticks)
            {
                resp = this.GetResponse(id);
                if (resp != null)
                    break;
                Thread.Sleep(50);
            }
            return resp;
        }

        public void AddResponseCallback(Action<BaseResponse> callback, BasePacket forPacket)
        {
            _responsCallbacks.Add(forPacket.ID, callback);
        }

        public RPCProxyObject GetProxyObject(string name)
        {
            BaseRequest req = new ProxyRequest(name);
            this.Send(req);
            BaseResponse resp = this.WaitForResponse(req, 5000000);
            return new RPCProxyObject(this, (resp as ProxyResponse).ObjectID);
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