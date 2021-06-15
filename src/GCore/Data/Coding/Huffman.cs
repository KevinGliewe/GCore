using GCore.Data.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.Coding {
    // https://rosettacode.org/wiki/Huffman_coding#C.23
    internal class HuffmanNode<T> : IComparable {
        internal HuffmanNode(double probability, T value) {
            Probability = probability;
            LeftSon = RightSon = Parent = null;
            Value = value;
            IsLeaf = true;
        }

        internal HuffmanNode(HuffmanNode<T> leftSon, HuffmanNode<T> rightSon) {
            LeftSon = leftSon;
            RightSon = rightSon;
            Probability = leftSon.Probability + rightSon.Probability;
            leftSon.IsZero = true;
            rightSon.IsZero = false;
            leftSon.Parent = rightSon.Parent = this;
            IsLeaf = false;
        }

        internal HuffmanNode<T> LeftSon { get; set; }
        internal HuffmanNode<T> RightSon { get; set; }
        internal HuffmanNode<T> Parent { get; set; }
        internal T Value { get; set; }
        internal bool IsLeaf { get; set; }

        internal bool IsZero { get; set; }

        internal int Bit {
            get { return IsZero ? 0 : 1; }
        }

        internal bool IsRoot {
            get { return Parent == null; }
        }

        internal double Probability { get; set; }

        public int CompareTo(object obj) {
            return -Probability.CompareTo(((HuffmanNode<T>)obj).Probability);
        }
    }

    public class Huffman<T> where T : IComparable {
        private readonly Dictionary<T, HuffmanNode<T>> _leafDictionary = new Dictionary<T, HuffmanNode<T>>();
        private readonly HuffmanNode<T> _root;

        public Huffman(IEnumerable<T> values) {
            var counts = new Dictionary<T, int>();
            var priorityQueue = new PriorityQueue<HuffmanNode<T>>();
            int valueCount = 0;

            foreach (T value in values) {
                if (!counts.ContainsKey(value)) {
                    counts[value] = 0;
                }
                counts[value]++;
                valueCount++;
            }

            foreach (T value in counts.Keys) {
                var node = new HuffmanNode<T>((double)counts[value] / valueCount, value);
                priorityQueue.Add(node);
                _leafDictionary[value] = node;
            }

            while (priorityQueue.Count > 1) {
                HuffmanNode<T> leftSon = priorityQueue.Pop();
                HuffmanNode<T> rightSon = priorityQueue.Pop();
                var parent = new HuffmanNode<T>(leftSon, rightSon);
                priorityQueue.Add(parent);
            }

            _root = priorityQueue.Pop();
            _root.IsZero = false;
        }

        public List<int> Encode(T value) {
            var returnValue = new List<int>();
            Encode(value, returnValue);
            return returnValue;
        }

        public void Encode(T value, List<int> encoding) {
            if (!_leafDictionary.ContainsKey(value)) {
                throw new ArgumentException("Invalid value in Encode");
            }
            HuffmanNode<T> nodeCur = _leafDictionary[value];
            var reverseEncoding = new List<int>();
            while (!nodeCur.IsRoot) {
                reverseEncoding.Add(nodeCur.Bit);
                nodeCur = nodeCur.Parent;
            }

            reverseEncoding.Reverse();
            encoding.AddRange(reverseEncoding);
        }

        public List<int> Encode(IEnumerable<T> values) {
            var returnValue = new List<int>();

            foreach (T value in values) {
                Encode(value, returnValue);
            }
            return returnValue;
        }

        public T Decode(List<int> bitString, ref int position) {
            HuffmanNode<T> nodeCur = _root;
            while (!nodeCur.IsLeaf) {
                if (position > bitString.Count) {
                    throw new ArgumentException("Invalid bitstring in Decode");
                }
                nodeCur = bitString[position++] == 0 ? nodeCur.LeftSon : nodeCur.RightSon;
            }
            return nodeCur.Value;
        }

        public List<T> Decode(List<int> bitString) {
            int position = 0;
            var returnValue = new List<T>();

            while (position != bitString.Count) {
                returnValue.Add(Decode(bitString, ref position));
            }
            return returnValue;
        }
    }
}
