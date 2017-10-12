using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:54:05
//Datei  : InvokeResponse.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class InvokeResponse : BaseResponse
    {

        #region Members
        public object Result { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public InvokeResponse(object result, BasePacket resp) : base(resp)
        {
            this.Result = result;
        }
        #endregion

        #region Finalization
        ~InvokeResponse()
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