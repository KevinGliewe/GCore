using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:53:50
//Datei  : InvokeRequest.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class InvokeRequest : BaseRequest
    {

        #region Members
        public int ObjectID { get; private set; }
        public string Name { get; private set; }
        public Object[] Arguments { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization

        public InvokeRequest(int objectID, string name, object[] arguments)
        {
            this.ObjectID = objectID;
            this.Name = name;
            this.Arguments = arguments;
        }
        #endregion

        #region Finalization
        ~InvokeRequest()
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