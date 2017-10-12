using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GCore.Extensions.ObjectEx.ReflectionEx {
    /// <summary>
    /// Description of ObjectExtensions.
    /// </summary>
    public static class ObjectExtensions {
        public static System.Object InvokeMethod(this System.Object val, string MehodName, object[] Parameters) {
            MethodInfo method = val.GetType().GetMethod(MehodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(val, Parameters);
        }
        public static System.Object InvokeGenericMethod(this System.Object val, System.Type[] GenericTypes, string MethodName, object[] Parameters) {
            MethodInfo method = val.GetType().GetMethod(MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            method = method.MakeGenericMethod(GenericTypes);
            return method.Invoke(val, Parameters);
        }

        public static System.Object InvokeMethod(this System.Object val, string MehodName, System.Type[] methodTypes, object[] Parameters) {
            MethodInfo method = val.GetType().GetMethod(MehodName, methodTypes);
            return method.Invoke(val, Parameters);
        }

        public static System.Object InvokeGenericMethod(this System.Object val, System.Type[] GenericTypes, string MethodName, System.Type[] methodTypes, object[] Parameters) {
            MethodInfo method = val.GetType().GetMethod(MethodName, methodTypes);
            method = method.MakeGenericMethod(GenericTypes);
            return method.Invoke(val, Parameters);
        }

        public static System.Object InvokeGenericMethod(this System.Object val, string[] GenericTypes, string MethodName, object[] Parameters) {
            List<System.Type> types = new List<System.Type>();
            foreach (string type in GenericTypes)
                types.Add(System.Type.GetType(type));
            return val.InvokeGenericMethod(types.ToArray(), MethodName, Parameters);
        }

        public static System.Object InvokeGenericMethod(this System.Object val, string[] GenericTypes, string MethodName, System.Type[] methodTypes, object[] Parameters) {
            List<System.Type> types = new List<System.Type>();
            foreach (string type in GenericTypes)
                types.Add(System.Type.GetType(type));
            return val.InvokeGenericMethod(types.ToArray(), MethodName, methodTypes, Parameters);
        }

        public static object GetPrivateField(this System.Object val, string Name) {
            FieldInfo fi = val.GetType().GetField(Name, BindingFlags.NonPublic | BindingFlags.Instance);
            return fi.GetValue(val);
        }

        public static bool SetPrivateField(this System.Object _this_, string Name, Object val) {
            FieldInfo fi = val.GetType().GetField(Name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null)
                return false;

            try {
                fi.SetValue(_this_, val);
            } catch (Exception ex) { return false; }

            return true;
        }


        public static object GetField(this System.Object val, string Name) {
            FieldInfo fi = val.GetType().GetField(Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            return fi.GetValue(val);
        }

        public static bool SetField(this System.Object _this_, string Name, Object val) {
            FieldInfo fi = val.GetType().GetField(Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (fi == null)
                return false;

            try {
                fi.SetValue(_this_, val);
            } catch (Exception ex) { return false; }

            return true;
        }

        public static string MakeGenericTypeString(string type, string[] genericTypes) {
            string returnString = type + "`" + genericTypes.Length.ToString() + "[";

            for (int n = 0; n < genericTypes.Length; n++)
                returnString += genericTypes[n] + (n < genericTypes.Length - 1 ? "," : "");

            returnString += "]";
            return returnString;
        }

        /// <summary>
        /// Gibt den Wehrt einer Property anhand ihres Namens zurück.
        /// </summary>
        /// <typeparam name="T">Typ der Property</typeparam>
        /// <param name="this_"></param>
        /// <param name="name">Name der Property</param>
        /// <param name="success">Gibt zurueck ob ein Fehler unterlaufen ist</param>
        /// <returns>Wehrt der Property</returns>
        public static T GetProperty<T>(this System.Object @this_, string name, out bool success) {
            success = true;
            try {
                PropertyInfo propinfo = @this_.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                //Nur lesen wenn es diese Property gibt und auch lesbar ist.
                if (propinfo != null && propinfo.CanRead) {
                    Type destType = typeof(T);
                    Type srcType = propinfo.PropertyType;
                    //Nur lesen wenn dar Typ der Property zum Rueckgabetyp gecastet werden kann
                    if (srcType.IsSubclassOf(destType) || srcType.Equals(destType)) {
                        return (T)propinfo.GetValue(@this_, null);
                    }
                }
            } catch (Exception) { }
            success = false;
            return default(T);
        }

        public static T GetProperty<T>(this System.Object @this_, string name) {
            bool dummy;
            return @this_.GetProperty<T>(name, out dummy);
        }

        public static T GetPropertyOrDefault<T>(this System.Object @this_, string name, T defaultT) {
            bool success;
            T ret = @this_.GetProperty<T>(name, out success);
            if (success)
                return ret;
            else
                return defaultT;
        }

        /// <summary>
        /// Setzt die Property anhand ihres Namens
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="name">Name der Property</param>
        /// <param name="val">Wehrt der der Property zugewiesen werden soll</param>
        /// <returns>Gibt zurueck ob des Setzten erfolgreich war</returns>
        public static bool SetProperty<T>(this System.Object @this_, string name, T val) {
            try {
                PropertyInfo propinfo = @this_.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                //Nur setzten wenn die Property gefunden wurde und schreibbar ist
                if (propinfo != null && propinfo.CanWrite) {
                    Type destType = propinfo.PropertyType;
                    Type srcType = typeof(T);
                    //Nur setzten wenn der uebergebene Typ zu dem Propertytyp gecastet werden kann.
                    if (srcType.IsSubclassOf(destType) || srcType.Equals(destType)) {
                        propinfo.SetValue(@this_, val, null);
                        return true;
                    }
                }
            } catch (Exception) { }
            return false;
        }

        /// <summary>
        /// Setzt die Property anhand ihres Namens
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="name">Name der Property</param>
        /// <param name="val">Wehrt der der Property zugewiesen werden soll</param>
        /// <returns>Gibt zurueck ob des Setzten erfolgreich war</returns>
        public static bool SetProperty(this System.Object @this_, string name, object val)
        {
            try
            {
                PropertyInfo propinfo = @this_.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                //Nur setzten wenn die Property gefunden wurde und schreibbar ist
                if (propinfo != null && propinfo.CanWrite)
                {
                    Type destType = propinfo.PropertyType;
                    Type srcType = val.GetType();
                    //Nur setzten wenn der uebergebene Typ zu dem Propertytyp gecastet werden kann.
                    if (srcType.IsSubclassOf(destType) || srcType.Equals(destType))
                    {
                        propinfo.SetValue(@this_, val, null);
                        return true;
                    }
                }
            }
            catch (Exception) { }
            return false;
        }
    }
}
