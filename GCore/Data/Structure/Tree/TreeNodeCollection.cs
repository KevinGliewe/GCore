using GCore.Extensions.IEnumerableEx;
using GCore.Extensions.ObjectEx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GCore.Data.Structure.Tree
{
    public class TreeNodeCollection<T> : Collection<ITreeNode<T>>, ITreeNodeCollection<T>
    {
        #region Fields

        private ITreeNode<T> _parent;

        #endregion

        #region Ctor

        public TreeNodeCollection(ITreeNode<T> parent)
        {
            _parent = parent;
        }

        #endregion

        #region Implementation of ITreeNodeCollection<T>

        #region Properties

        public ITreeNode<T> Parent {
            get { return _parent; }
            set {
                if (_parent == value)
                    return;

                if (_parent != null)
                    _parent.Children = new TreeNodeCollection<T>(_parent);
                _parent = value;
                if (_parent != null) {
                    _parent.Children.DetachFromParent();
                    _parent.Children = this;
                }
                this.Foreach(x => x.SetParent(_parent, false));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the given value as new node to the list and sets it's parent to the parent of the list.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the new created node.</returns>
        public ITreeNode<T> Add(T value)
        {
            var node = new TreeNode<T>(value);
            Add(node);

            return node;
        }

        /// <summary>
        ///     Detaches the collection and all it's items form it's current parent.
        /// </summary>
        public void DetachFromParent()
        {
            _parent = null;
            this.Foreach(x => x.Parent = null);
        }

        /// <summary>
        ///     Adds the given item to the list and sets it's parent to the parent of the list.
        /// </summary>
        /// <exception cref="ArgumentNullException">item can not be null.</exception>
        /// <param name="item">The item to add.</param>
        /// <param name="setParent">
        ///     A value indicating weather the parent of the given item should be set to the parent of the
        ///     collection or not.
        /// </param>
        public void Add(ITreeNode<T> item, Boolean setParent)
        {
            if (item is null) return;

            if (Contains(item))
                return;

            base.Add(item);
            if (setParent)
                item.Parent = Parent;
        }

        public Boolean Remove(ITreeNode<T> item, Boolean setParent)
        {
            if (item is null) return false;

            var result = base.Remove(item);
            if (result && setParent)
                item.Parent = null;

            return result;
        }

        #endregion

        #endregion

        #region Implementation of ICollection<ITreeNode<T>>

        public new void Add(ITreeNode<T> item)
        {
            Add(item, true);
        }

        public new Boolean Remove(ITreeNode<T> item)
        {
            return Remove(item, true);
        }

        #endregion

        #region Overrides of Object

        public override String ToString()
        {
            return $"Count: {Count}, Parent: {Parent.ToStringSafe()}";
        }

        #endregion
    }
}
