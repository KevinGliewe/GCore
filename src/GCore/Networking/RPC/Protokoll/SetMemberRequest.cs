using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 14:40:11
//Datei  : SetMemberRequest.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class SetMemberRequest : BaseRequest
    {

        #region Members
        public object Value { get; private set; }
        public int ObjectID { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public SetMemberRequest(object value, int objectID, string name)
        {
            this.Value = value;
            this.Name = name;
            this.ObjectID = objectID;
        }
        #endregion

        #region Finalization
        ~SetMemberRequest()
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