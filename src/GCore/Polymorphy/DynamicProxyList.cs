using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Reflection;
using GCore.Extensions.IEnumerableEx;

namespace GCore.Polymorphy {
    /// <summary>
    /// Eine Liste die alle Aufrufe an die enthaltetene Elemente weiter leitet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicProxyList<T> : DynamicObject {
        private List<T> _wrapped = new List<T>();

        public DynamicProxyList() { }
        public DynamicProxyList(T[] elements) {
            elements.Foreach((element) => { _wrapped.Add(element); });
        }

        public void AddToList(T element) {
            _wrapped.Add(element);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
            List<object> resultTmp = new List<object>();
            result = null;
            try {
                foreach (T element in _wrapped) {
                    resultTmp.Add(element.GetType().GetMethod(binder.Name).Invoke(element, args));
                }
                result = resultTmp.ToArray();
                return true;
            } catch {
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {

            try {
                foreach (T element in _wrapped) {
                    GetPropertyInfo(binder.Name, element).SetValue(element, value, null);
                }
                return true;
            } catch {
                return false;
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames() {
            return typeof(T).GetMembers().Foreach((e) => { return e.Name; });
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            List<object> resultTmp = new List<object>();
            result = null;
            try {
                foreach (T element in _wrapped) {
                    resultTmp.Add(GetPropertyInfo(binder.Name, element).GetValue(element, null));
                }
                result = resultTmp.ToArray();
                return true;
            } catch {
                return false;
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result) {
            if (binder.Type == typeof(T)) {
                result = this;
                return true;
            }
            result = null;
            return false;
        }

        public override bool TryBinaryOperation(
                BinaryOperationBinder binder,
                object arg,
                out object result) {
            result = null;
            return false;
        }

        public override bool TryCreateInstance(
                CreateInstanceBinder binder,
                object[] args,
                out object result) {
            result = null;
            return false;
        }

        public override bool TryDeleteIndex(
                DeleteIndexBinder binder,
                Object[] indexes) {
            return false;
        }


        public override bool TryGetIndex(
                GetIndexBinder binder,
                Object[] indexes,
                out Object result) {
            result = null;
            return false;
        }


        public override bool TryDeleteMember(
                DeleteMemberBinder binder) {
            return false;
        }


        public override bool TryInvoke(
                InvokeBinder binder,
                Object[] args,
                out Object result) {
            result = null;
            return false;
        }

        public override bool TrySetIndex(
                SetIndexBinder binder,
                Object[] indexes,
                Object value) {
            return false;
        }


        protected PropertyInfo GetPropertyInfo(string propertyName, T ProxiedObject) {
            return ProxiedObject.GetType().GetProperties().First
            (propertyInfo => propertyInfo.Name == propertyName);
        }

        public void Foreach(Action<T> callback) {
            foreach (T element in this._wrapped) {
                callback(element);
            }
        }

        public TResult[] Foreach<TResult>(Func<T, TResult> callback) {
            List<TResult> returns = new List<TResult>();
            foreach (T element in this._wrapped) {
                returns.Add(callback(element));
            }
            return returns.ToArray();
        }

        public T[] GetElements() {
            return this._wrapped.ToArray();
        }
    }
}
