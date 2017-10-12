using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:56:18
//Datei  : ProxyResponse.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class ProxyResponse : BaseResponse
    {

        #region Members
        public int ObjectID { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public ProxyResponse(int objectID, BasePacket resp) : base(resp)
        {
            this.ObjectID = objectID;
        }
        #endregion

        #region Finalization
        ~ProxyResponse()
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