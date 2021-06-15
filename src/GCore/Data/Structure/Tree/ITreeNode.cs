﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.Structure.Tree
{
    public interface ITreeNode<T> : IDisposable
    {
        #region Properties
        /// <summary>
        ///     Gets an enumeration of all tree nodes which are above the current node in the tree.
        /// </summary>
        /// <value>An enumeration of all tree nodes which are above the current node in the tree.</value>
        IEnumerable<ITreeNode<T>> Ancestors { get; }

        /// <summary>
        ///     Gets or sets the value of the node.
        /// </summary>
        /// <value>The value of the node.</value>
        T Value { get; set; }

        /// <summary>
        ///     Gets or sets the parent of the node.
        /// </summary>
        /// <remarks>
        ///     Detaches the node from it's old parent and attaches it to it's new parent.
        /// </remarks>
        /// <value>The parent of the node.</value>
        ITreeNode<T> Parent { get; set; }

        /// <summary>
        ///     Gets the root of the tree.
        /// </summary>
        /// <value>The root of the tree.</value>
        ITreeNode<T> Root { get; }
        /// <summary>
        ///     Gets or sets the children of the node.
        /// </summary>
        /// <value>The children of the node.</value>
        ITreeNodeCollection<T> Children { get; set; }

        /// <summary>
        ///     Gets or sets the search traversal direction.
        /// </summary>
        /// <value>The search traversal direction.</value>
        TreeTraversalDirection SearchTraversalDirection { get; set; }

        /// <summary>
        ///     Gets or sets the dispose traversal direction.
        /// </summary>
        /// <value>The dispose traversal direction.</value>
        TreeTraversalDirection DisposeTraversalDirection { get; set; }

        /// <summary>
        ///     Gets or sets the ancestors traversal direction.
        /// </summary>
        /// <value>The ancestors traversal direction.</value>
        TreeTraversalDirection AncestorsTraversalDirection { get; set; }

        /// <summary>
        ///     Gets or sets the descendants traversal direction.
        /// </summary>
        /// <value>The descendants traversal direction.</value>
        TreeTraversalDirection DescendantsTraversalDirection { get; set; }

        /// <summary>
        ///     Gets or sets the traversal direction used to enumerate the nodes.
        /// </summary>
        /// <value>The traversal direction used to enumerate the nodes.</value>
        TreeTraversalDirection TraversalDirection { get; set; }

        /// <summary>
        ///     Gets the depth of the node.
        /// </summary>
        /// <value>The depth of the node.</value>
        Int32 Depth { get; }

        /// <summary>
        ///     Gets a value indicating whether the node has any children or not.
        /// </summary>
        /// <value>A value indicating whether the node has any children or not.</value>
        Boolean HasChildren { get; }

        /// <summary>
        ///     Gets a value indicating whether the node has a parent or not.
        /// </summary>
        /// <value>A value indicating whether the node has a parent or not.</value>
        Boolean HasParent { get; }

        /// <summary>
        ///     Gets an enumeration of all tree nodes which are below the current node in the tree.
        /// </summary>
        /// <value>An enumeration of all tree nodes which are below the current node in the tree.</value>
        IEnumerable<ITreeNode<T>> Descendants { get; }

        #endregion

        #region Methods
        /// <summary>
        ///     Gets the values which matches the given predicate.
        /// </summary>
        /// <remarks>
        ///     Starts the search at the current tree node and traverses down the tree (Direction based on
        ///     <see cref="SearchTraversalDirection" />).
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns the values which matches the given predicate.</returns>
        IEnumerable<T> FindValue(Func<ITreeNode<T>, Boolean> predicate);

        /// <summary>
        ///     Gets the nodes which matches the given predicate.
        /// </summary>
        /// <remarks>
        ///     Starts the search at the current tree node and traverses down the tree (Direction based on
        ///     <see cref="SearchTraversalDirection" />).
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns the nodes which matches the given predicate.</returns>
        IEnumerable<ITreeNode<T>> FindNode(Func<ITreeNode<T>, Boolean> predicate);

        /// <summary>
        ///     Gets the nodes with the given value.
        /// </summary>
        /// <param name="value">The value to search.</param>
        /// <returns>Returns the nodes with the given value.</returns>
        IEnumerable<ITreeNode<T>> FindNode(T value);

        ITreeNode<T> FindNode(string name);

        ITreeNode<T> FindNode(params string[] path);

        /// <summary>
        ///     Adds the given value as child to the node.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the added node.</returns>
        ITreeNode<T> Add(T value);

        /// <summary>
        ///     Adds the given node as child to the node, if it is not already a child of the node.
        /// </summary>
        /// <param name="node">The node to add.</param>
        /// <returns>Returns the added node.</returns>
        ITreeNode<T> Add(ITreeNode<T> node);

        /// <summary>
        ///     Sets the parent of the tree node.
        /// </summary>
        /// <param name="parent">The new parent.</param>
        /// <param name="attacheToNewParent">
        ///     A value determining whether the node should add it self to the children of the new parent
        ///     or not.
        /// </param>
        /// <param name="detachFromOldParent">A value indicating whether the node should detach itself from it's old parent or not.</param>
        void SetParent(ITreeNode<T> parent,
                               Boolean attacheToNewParent = true,
                               Boolean detachFromOldParent = true);

        /// <summary>
        ///     Sets all directions (<see cref="DisposeTraversalDirection" />, <see cref="SearchTraversalDirection" />,
        ///     <see cref="AncestorsTraversalDirection" />, <see cref="DescendantsTraversalDirection" />).
        /// </summary>
        /// <param name="direction">The new direction.</param>
        void SetAllDirections(TreeTraversalDirection direction);

        string GetName();

        string[] GetPath();
        #endregion
    }
}
