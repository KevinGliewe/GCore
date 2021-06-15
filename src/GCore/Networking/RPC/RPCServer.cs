using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCore.Networking.RPC.Protokoll;
using GCore.Networking.Socket;


//Author : KEG
//Datum  : 25.09.2015 10:36:09
//Datei  : RPCServer.cs


namespace GCore.Networking.RPC
{
    public class RPCServer : RPCBase
    {

        #region Members
        private List<ProxyHolder> _proxys = new List<ProxyHolder>();
        #endregion

        #region Events
        #endregion

        #region Initialization
        public RPCServer(BaseSocket socket) : base(socket)
        {
            _handler.Add(typeof(GetMemberRequest), _handler_GetMember);
            _handler.Add(typeof(SetMemberRequest), _handler_SetMember);
            _handler.Add(typeof(InvokeRequest), _handler_Invoke);
            _handler.Add(typeof(ProxyRequest), _handler_Proxy);
        }
        #endregion

        #region Packet Handler
        private BaseResponse _handler_GetMember(BasePacket packet)
        {
            GetMemberRequest p = packet as GetMemberRequest;

            ProxyHolder proxy = this.GetProxyByID(p.ObjectID);

            if(proxy == null)
                throw new Exception("Proxy Nr. not found: " + p.ObjectID);

            return new GetMemberResponse(proxy.GetMember(p.Member, p.ReturnType), p);
        }

        private BaseResponse _handler_SetMember(BasePacket packet)
        {
            SetMemberRequest p = packet as SetMemberRequest;

            ProxyHolder proxy = this.GetProxyByID(p.ObjectID);

            if (proxy == null)
                throw new Exception("Proxy Nr. not found: " + p.ObjectID);

            return new SetMemberResponse(proxy.SetMember(p.Name, p.Value), p);
        }

        private BaseResponse _handler_Invoke(BasePacket packet)
        {
            InvokeRequest p = packet as InvokeRequest;

            ProxyHolder proxy = this.GetProxyByID(p.ObjectID);

            if (proxy == null)
                throw new Exception("Proxy Nr. not found: " + p.ObjectID);

            return new InvokeResponse(proxy.Invoke(p.Name, p.Arguments), p);
        }

        private BaseResponse _handler_Proxy(BasePacket packet)
        {
            ProxyRequest p = packet as ProxyRequest;

            ProxyHolder proxy = this.GetProxyByName(p.Name);

            if (proxy == null)
                throw new Exception("Proxy not found: " + p.Name);

            return new ProxyResponse(proxy.ObjectID, p);
        }
        #endregion

        #region Finalization
        ~RPCServer()
        {

        }
        #endregion

        #region Interface

        public int AddProxyedObject(object o, string name)
        {
            foreach (ProxyHolder proxyHolder in _proxys)
            {
                if (proxyHolder.ProxyedObject == o && proxyHolder.Name == name)
                    return proxyHolder.ObjectID;
            }
            ProxyHolder p = new ProxyHolder(name, o);
            this._proxys.Add(p);
            return p.ObjectID;
        }

        public ProxyHolder GetProxyByName(string name)
        {
            foreach (ProxyHolder proxyHolder in _proxys)
            {
                if (proxyHolder.Name == name)
                    return proxyHolder;
            }
            return null;
        }

        public ProxyHolder GetProxyByID(int id)
        {
            foreach (ProxyHolder proxyHolder in _proxys)
            {
                if (proxyHolder.ObjectID == id)
                    return proxyHolder;
            }
            return null;
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