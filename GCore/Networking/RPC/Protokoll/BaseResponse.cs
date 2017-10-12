using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:49:00
//Datei  : BaseResponse.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class BaseResponse : BasePacket
    {

        #region Members
        public int ResponseFor { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public BaseResponse(BasePacket resp)
        {
            this.ResponseFor = resp.ID;
        }
        #endregion

        #region Finalization
        ~BaseResponse()
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