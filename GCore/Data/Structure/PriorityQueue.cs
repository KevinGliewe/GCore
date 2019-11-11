using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.Structure {
    public class PriorityQueue<T> where T : IComparable {
        protected List<T> LstHeap = new List<T>();

        public virtual int Count {
            get { return LstHeap.Count; }
        }

        public virtual void Add(T val) {
            LstHeap.Add(val);
            SetAt(LstHeap.Count - 1, val);
            UpHeap(LstHeap.Count - 1);
        }

        public virtual T Peek() {
            if (LstHeap.Count == 0) {
                throw new IndexOutOfRangeException("Peeking at an empty priority queue");
            }

            return LstHeap[0];
        }

        public virtual T Pop() {
            if (LstHeap.Count == 0) {
                throw new IndexOutOfRangeException("Popping an empty priority queue");
            }

            T valRet = LstHeap[0];

            SetAt(0, LstHeap[LstHeap.Count - 1]);
            LstHeap.RemoveAt(LstHeap.Count - 1);
            DownHeap(0);
            return valRet;
        }

        protected virtual void SetAt(int i, T val) {
            LstHeap[i] = val;
        }

        protected bool RightSonExists(int i) {
            return RightChildIndex(i) < LstHeap.Count;
        }

        protected bool LeftSonExists(int i) {
            return LeftChildIndex(i) < LstHeap.Count;
        }

        protected int ParentIndex(int i) {
            return (i - 1) / 2;
        }

        protected int LeftChildIndex(int i) {
            return 2 * i + 1;
        }

        protected int RightChildIndex(int i) {
            return 2 * (i + 1);
        }

        protected T ArrayVal(int i) {
            return LstHeap[i];
        }

        protected T Parent(int i) {
            return LstHeap[ParentIndex(i)];
        }

        protected T Left(int i) {
            return LstHeap[LeftChildIndex(i)];
        }

        protected T Right(int i) {
            return LstHeap[RightChildIndex(i)];
        }

        protected void Swap(int i, int j) {
            T valHold = ArrayVal(i);
            SetAt(i, LstHeap[j]);
            SetAt(j, valHold);
        }

        protected void UpHeap(int i) {
            while (i > 0 && ArrayVal(i).CompareTo(Parent(i)) > 0) {
                Swap(i, ParentIndex(i));
                i = ParentIndex(i);
            }
        }

        protected void DownHeap(int i) {
            while (i >= 0) {
                int iContinue = -1;

                if (RightSonExists(i) && Right(i).CompareTo(ArrayVal(i)) > 0) {
                    iContinue = Left(i).CompareTo(Right(i)) < 0 ? RightChildIndex(i) : LeftChildIndex(i);
                } else if (LeftSonExists(i) && Left(i).CompareTo(ArrayVal(i)) > 0) {
                    iContinue = LeftChildIndex(i);
                }

                if (iContinue >= 0 && iContinue < LstHeap.Count) {
                    Swap(i, iContinue);
                }

                i = iContinue;
            }
        }
    }
}
