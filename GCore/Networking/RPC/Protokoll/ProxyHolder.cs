using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GCore.Extensions.IEnumerableEx;
using GCore.Extensions.ObjectEx.ReflectionEx;


//Author : KEG
//Datum  : 25.09.2015 11:46:48
//Datei  : ProxyHolder.cs


namespace GCore.Networking.RPC.Protokoll
{
    public class ProxyHolder
    {
        private static int NextID = 0;

        #region Members
        public int ObjectID { get; private set; }
        public string Name { get; private set; }
        public object ProxyedObject { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public ProxyHolder(string name, object proxyedObject)
        {
            this.ObjectID = NextID++;
            this.Name = name;
            this.ProxyedObject = proxyedObject;
        }
        #endregion

        #region Finalization
        ~ProxyHolder()
        {

        }
        #endregion

        #region Interface

        public object GetMember(string name, Type type)
        {
            return this.ProxyedObject.GetProperty<object>(name);
        }

        public bool SetMember(string name, object value)
        {
            return this.ProxyedObject.SetProperty(name, value);
        }

        public object Invoke(string name, object[] arguments)
        {
            return this.ProxyedObject.InvokeMethod(name, arguments.Foreach(o => o.GetType()), arguments);
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