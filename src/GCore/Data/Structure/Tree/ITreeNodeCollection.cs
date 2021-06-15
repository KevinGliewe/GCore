using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.Structure.Tree
{
    public interface ITreeNodeCollection<T> : ICollection<ITreeNode<T>>
    {
        #region Properties

        ITreeNode<T> Parent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the given value as new node to the list and sets it's parent to the parent of the list.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the new created node.</returns>
        ITreeNode<T> Add(T value);

        /// <summary>
        ///     Detaches the collection and all it's items form it's current parent.
        /// </summary>
        void DetachFromParent();

        /// <summary>
        ///     Adds the given item to the list and sets it's parent to the parent of the list.
        /// </summary>
        /// <exception cref="ArgumentNullException">item can not be null.</exception>
        /// <param name="item">The item to add.</param>
        /// <param name="setParent">
        ///     A value indicating weather the parent of the given item should be set to the parent of the
        ///     collection or not.
        /// </param>
        void Add(ITreeNode<T> item, Boolean setParent);

        Boolean Remove(ITreeNode<T> item, Boolean setParent);
        #endregion
    }
}
