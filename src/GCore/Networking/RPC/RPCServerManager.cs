using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using GCore.Networking.RPC.Protokoll;
using GCore.Networking.Socket;


//Author : KEG
//Datum  : 25.09.2015 13:54:23
//Datei  : RPCServerManager.cs


namespace GCore.Networking.RPC
{
    public class RPCServerManager
    {

        #region Members
        private List<RPCBase> _clients = new List<RPCBase>();
        public BaseListener Listener { get; private set; }

        private List<ProxyHolder> _proxys = new List<ProxyHolder>();
        #endregion

        #region Events

        public Func<TcpClient, BaseSocket> GenSocket = client => new GSocket(client);
        #endregion

        #region Initialization
        public RPCServerManager(BaseListener listener)
        {
            this.Listener = listener;
            this.Listener.ClientArrived += new BaseListener.ClientArrivedHandler(Listener_ClientArrived);
        }
        #endregion

        #region Finalization
        ~RPCServerManager()
        {

        }
        #endregion

        #region Eventhanler
        void Listener_ClientArrived(BaseListener sender, System.Net.Sockets.TcpClient Client)
        {
            RPCServer client = new RPCServer(this.GenSocket(Client));
            _clients.Add(client);
            client.Closed += new Action<Protokoll.RPCBase>(client_Closed);

            foreach (ProxyHolder proxyHolder in _proxys)
            {
                client.AddProxyedObject(proxyHolder.ProxyedObject, proxyHolder.Name);
            }
        }

        void client_Closed(RPCBase obj)
        {
            _clients.Remove(obj);
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

            foreach (RPCBase rpcBase in _clients)
            {
                (rpcBase as RPCServer).AddProxyedObject( p.ProxyedObject, p.Name);
            }

            return p.ObjectID;
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