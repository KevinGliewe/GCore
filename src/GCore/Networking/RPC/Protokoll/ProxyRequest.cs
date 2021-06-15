using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:56:05
//Datei  : ProxyRequest.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class ProxyRequest : BaseRequest
    {

        #region Members
        public string Name { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public ProxyRequest(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Finalization
        ~ProxyRequest()
        {

        }
        #endregion

        #region Interface
        #endregion

        #region Tools
        #endregion

        #region Browsable Properties
        #endregion

        #region NonBrowsable Properties
        #endregion
    }
}