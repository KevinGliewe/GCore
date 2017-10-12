using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:50:34
//Datei  : ExceptionResponse.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class ExceptionResponse : BaseResponse
    {

        #region Members
        public string Message { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public ExceptionResponse(string message, BasePacket resp) : base(resp)
        {
            this.Message = message;
        }
        #endregion

        #region Finalization
        ~ExceptionResponse()
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