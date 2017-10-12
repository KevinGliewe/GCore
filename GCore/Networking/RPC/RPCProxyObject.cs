using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GCore.Networking.RPC.Protokoll;
using GCore.Networking.Socket;


//Author : KEG
//Datum  : 25.09.2015 10:36:49
//Datei  : RPCProxyObject.cs


namespace GCore.Networking.RPC
{
    public class RPCProxyObject : DynamicObject
    {

        #region Members
        public RPCClient Client { get; private set; }
        public int ObjectID { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public RPCProxyObject(RPCClient client, int objectID)
        {
            this.Client = client;
            this.ObjectID = objectID;
        }
        #endregion

        #region Finalization
        ~RPCProxyObject()
        {

        }
        #endregion

        #region Interface

        #endregion

        #region Interface(DynamicObject)

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return base.GetDynamicMemberNames();
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = this;
            return true;
            //return base.TryConvert(binder, out result);
        }

        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            return base.TryCreateInstance(binder, args, out result);
        }

        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            return base.TryDeleteIndex(binder, indexes);
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return base.TryDeleteMember(binder);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return base.TryGetIndex(binder, indexes, out result);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            GetMemberRequest req = new GetMemberRequest(this.ObjectID, binder.Name, binder.ReturnType);
            this.Client.Send(req);
            BaseResponse resp = this.Client.WaitForResponse(req, 50000);
            if (resp is GetMemberResponse)
            {
                result = (resp as GetMemberResponse).Result;
            }
            else if (resp is ExceptionResponse)
            {
                throw new Exception((resp as ExceptionResponse).Message);
            }
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            InvokeRequest req = new InvokeRequest(this.ObjectID, binder.Name, args);
            this.Client.Send(req);
            BaseResponse resp = this.Client.WaitForResponse(req, 5000);
            if (resp is InvokeResponse)
            {
                result = (resp as InvokeResponse).Result;
            }
            else if (resp is ExceptionResponse)
            {
                throw new Exception((resp as ExceptionResponse).Message);
            }
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return base.TrySetIndex(binder, indexes, value);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetMemberRequest req = new SetMemberRequest(value, this.ObjectID, binder.Name);
            this.Client.Send(req);
            BaseResponse resp = this.Client.WaitForResponse(req, 50000000);
            if (resp is SetMemberResponse)
            {
                return (resp as SetMemberResponse).Success;
            }
            else if (resp is ExceptionResponse)
            {
                throw new Exception((resp as ExceptionResponse).Message);
            }
            else
            {
                return false;
            }
            return true;
        }

        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        #endregion

        #region Tools
        #endregion

        #region Browsable Properties
        #endregion

        #region NonBrowsable Properties
        #endregion
    }
}