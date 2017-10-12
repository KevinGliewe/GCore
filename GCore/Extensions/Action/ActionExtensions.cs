using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCore.Extensions.ActionEx {
    public static class ActionExtensions {

        public static Task DoLater(this Action @thisX, TimeSpan delay)
        {
            var task = new Task(() =>
            {
                Task.Delay(delay);
                @thisX();
            });
            task.Start();

            return task;
        }

        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise(this Action @thisX) {
            if (@thisX != null)
                @thisX();
        }


        //{{for n in range(17):}}
        //{{t1 = ["T"+str(i) for i in range(1,n +1)]}}
        //{{t2 = ["T"+str(i)+" t"+str(i) for i in range(1,n +1)]}}
        //{{t3 = ["t"+str(i) for i in range(1,n +1)]}}

        //        public static void Raise<{{=",".join(t1)}}>(this Action<{{=",".join(t1)}}> @thisX, {{=",".join(t2)}}) {
        //            if (@thisX != null)
        //                @thisX({{=",".join(t3)}});
        //        }
        //{{pass}}

        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1>(this Action<T1> @thisX, T1 t1) {
            if (@thisX != null)
                @thisX(t1);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2>(this Action<T1,T2> @thisX, T1 t1,T2 t2) {
            if (@thisX != null)
                @thisX(t1,t2);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3>(this Action<T1,T2,T3> @thisX, T1 t1,T2 t2,T3 t3) {
            if (@thisX != null)
                @thisX(t1,t2,t3);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4>(this Action<T1,T2,T3,T4> @thisX, T1 t1,T2 t2,T3 t3,T4 t4) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5>(this Action<T1,T2,T3,T4,T5> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6>(this Action<T1,T2,T3,T4,T5,T6> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7>(this Action<T1,T2,T3,T4,T5,T6,T7> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8>(this Action<T1,T2,T3,T4,T5,T6,T7,T8> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8);
        }





        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10,T11 t11) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10,T11 t11,T12 t12) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10,T11 t11,T12 t12,T13 t13) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13);
        }





        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10,T11 t11,T12 t12,T13 t13,T14 t14) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,t14);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10,T11 t11,T12 t12,T13 t13,T14 t14,T15 t15) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,t14,t15);
        }




        /// <summary>
        /// Führt die Action aus wenn sie nicht null ist
        /// </summary>
        /// <param name="thisX"></param>
        public static void Raise<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> @thisX, T1 t1,T2 t2,T3 t3,T4 t4,T5 t5,T6 t6,T7 t7,T8 t8,T9 t9,T10 t10,T11 t11,T12 t12,T13 t13,T14 t14,T15 t15,T16 t16) {
            if (@thisX != null)
                @thisX(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,t14,t15,t16);
        }


    }
}
