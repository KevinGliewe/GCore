using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:54:33
//Datei  : GetMemberRequest.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class GetMemberRequest : BaseRequest
    {

        #region Members
        public int ObjectID { get; private set; }
        public string Member { get; private set; }
        public Type ReturnType { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public GetMemberRequest(int objectId, string member, Type returnType)
        {
            this.ObjectID = objectId;
            this.Member = member;
            this.ReturnType = returnType;
        }
        #endregion

        #region Finalization
        ~GetMemberRequest()
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