using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:54:53
//Datei  : GetMemberResponse.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class GetMemberResponse : BaseResponse
    {

        #region Members
        public Object Result { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public GetMemberResponse(Object result, BasePacket resp) : base(resp)
        {
            this.Result = result;
        }
        #endregion

        #region Finalization
        ~GetMemberResponse()
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