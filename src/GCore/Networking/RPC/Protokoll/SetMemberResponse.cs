using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 14:43:03
//Datei  : SetMemberResponse.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class SetMemberResponse : BaseResponse
    {

        #region Members
        public bool Success { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public SetMemberResponse(bool success, BasePacket resp) : base(resp)
        {
            this.Success = success;
        }
        #endregion

        #region Finalization
        ~SetMemberResponse()
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