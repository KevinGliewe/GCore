using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.SortedListEx {
    public static class SortedListExtensions {

        /// <summary>
        /// Fügt das Element an der nächsten freien Stelle in der Liste ein.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedList"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int AddNext<T>(this SortedList<int, T> sortedList, T item) {
            int key = 1; // Make it 0 to start from Zero based index
            int count = sortedList.Count;

            int counter = 0;
            do {
                if (count == 0) break;
                int nextKeyInList = sortedList.Keys[counter++];

                if (key != nextKeyInList) break;

                key = nextKeyInList + 1;

                if (count == 1 || counter == count) break;


                if (key != sortedList.Keys[counter])
                    break;

            } while (true);

            sortedList.Add(key, item);
            return key;
        }

    }
}
