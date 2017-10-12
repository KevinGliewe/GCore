using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GCore.Extensions.TypeEx {
    /// <summary>
    /// Description of TypeExtensions.
    /// </summary>
    public static class TypeExtensions {
        /// <summary>
        /// Prüft ob der Typ wirklich serialisierbar ist.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsRealySerializable(this System.Type type) {
            // base case
            if (type.IsValueType || type == typeof(string)) return true;

            //Assert.IsTrue(type.IsSerializable, type + " must be marked [Serializable]");
            if (!type.IsSerializable) return false;

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (propertyInfo.PropertyType.IsGenericType) {
                    foreach (var genericArgument in propertyInfo.PropertyType.GetGenericArguments()) {
                        if (genericArgument == type) continue; // base case for circularly referenced properties
                        if (!genericArgument.IsRealySerializable()) return false;
                    }
                } else if (propertyInfo.GetType() != type) // base case for circularly referenced properties
                    if (!propertyInfo.PropertyType.IsRealySerializable()) return false;
            }

            return true;
        }

        /// <summary>
        /// Sample:
        /// string key = typeof(MyClass)
        ///    .GetAttributeValue((DomainNameAttribute dna) => dna.Name);
        /// </summary>
        /// <typeparam key="TAttribute"></typeparam>
        /// <typeparam key="TValue"></typeparam>
        /// <param key="type"></param>
        /// <param key="valueSelector"></param>
        /// <returns></returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(
                this Type type,
                Func<TAttribute, TValue> valueSelector)
                where TAttribute : Attribute {

            var att = type.GetCustomAttributes(
                    typeof(TAttribute), true
                ).FirstOrDefault() as TAttribute;
            if (att != null) {
                return valueSelector(att);
            }
            return default(TValue);
        }

        public static bool IsCastableTo(this Type _this_, Type t) {
            return t.Equals(_this_) || _this_.IsSubclassOf(t);
        }

        public static bool IsCastableTo2(this Type from, Type to) {
            if (to.IsAssignableFrom(from)) {
                return true;
            }
            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                              .Where(
                                  m => m.ReturnType == to &&
                                       (m.Name == "op_Implicit" ||
                                        m.Name == "op_Explicit")
                              );
            return methods.Count() > 0;
        }
    }
}