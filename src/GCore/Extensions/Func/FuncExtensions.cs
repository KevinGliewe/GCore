using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.FuncEx {
    public static class FuncExtensions {

        public static TResult Raise<TResult>(this Func<TResult> @thisX) {
            if (@thisX != null)
                return @thisX();
            return default(TResult);
        }


        //{{for n in range(17):}}
        //{{t1 = ["T"+str(i) for i in range(1,n +1)]}}
        //{{t2 = ["T"+str(i)+" t"+str(i) for i in range(1,n +1)]}}
        //{{t3 = ["t"+str(i) for i in range(1,n +1)]}}
        //        public static TResult Raise<{{=",".join(t1)}},TResult>(this Func<{{=",".join(t1)}},TResult> @thisX, {{=",".join(t2)}}) {
        //            if (@thisX != null)
        //                return @thisX({{=",".join(t3)}});
        //            return default(TResult);
        //        }
        //{{pass}}

        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="t1"></param>
        /// <returns></returns>
        public static TResult Raise<T1, TResult>(this Func<T1, TResult> @thisX, T1 t1) {
            if (@thisX != null)
                return @thisX(t1);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, TResult>(this Func<T1, T2, TResult> @thisX, T1 t1, T2 t2) {
            if (@thisX != null)
                return @thisX(t1, t2);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @thisX, T1 t1, T2 t2, T3 t3) {
            if (@thisX != null)
                return @thisX(t1, t2, t3);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            return default(TResult);
        }



        /// <summary>
        /// Füht die Funktion aus wenn sie nicht null ist.
        /// </summary>
        public static TResult Raise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @thisX, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) {
            if (@thisX != null)
                return @thisX(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            return default(TResult);
        }


    }
}
