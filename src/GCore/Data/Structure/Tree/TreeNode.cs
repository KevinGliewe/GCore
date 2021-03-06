using GCore.Extensions.IEnumerableEx;
using GCore.Extensions.ObjectEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Data.Structure.Tree
{
    /// <summary>
    ///     Class representing a node of a tree.
    /// </summary>
    /// <typeparam name="T">The type of the value of the node.</typeparam>
    public class TreeNode<T> : ITreeNode<T>
    {
        #region Fields

        /// <summary>
        ///     The ancestors traversal direction.
        /// </summary>
        private TreeTraversalDirection _ancestorsTraversalDirection;

        /// <summary>
        ///     The children of the node.
        /// </summary>
        private ITreeNodeCollection<T> _children;

        /// <summary>
        ///     The descendants traversal direction.
        /// </summary>
        private TreeTraversalDirection _descendantsTraversalDirection;

        /// <summary>
        ///     The disposable traversal direction.
        /// </summary>
        private TreeTraversalDirection _disposeTraversalDirection;

        /// <summary>
        ///     The parent of the node.
        /// </summary>
        private ITreeNode<T> _parent;

        /// <summary>
        ///     The search traversal direction.
        /// </summary>
        private TreeTraversalDirection _searchTraversalDirection;

        /// <summary>
        ///     The traversal direction.
        /// </summary>
        private TreeTraversalDirection _traversalDirection;

        /// <summary>
        ///     The value of the node.
        /// </summary>
        private T _value;

        #endregion

        #region Ctor

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        public TreeNode()
        {
            Initialize(default(T));
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        public TreeNode(T value)
        {
            Initialize(value);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="parent">The parent of the node.</param>
        public TreeNode(ITreeNode<T> parent)
        {
            Initialize(default(T), parent);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="children">The children of the node.</param>
        public TreeNode(ITreeNodeCollection<T> children)
        {
            Initialize(default(T), children: children);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        /// <param name="children">The children of the node.</param>
        public TreeNode(T value, ITreeNodeCollection<T> children)
        {
            Initialize(value, children: children);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        /// <param name="parent">The parent of the node.</param>
        public TreeNode(T value, ITreeNode<T> parent)
        {
            Initialize(value, parent);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        /// <param name="parent">The parent of the node.</param>
        /// <param name="children">The children of the node.</param>
        public TreeNode(T value, ITreeNode<T> parent, ITreeNodeCollection<T> children)
        {
            Initialize(value, parent, children);
        }

        #endregion

        #region Implementation of ITreeNode

        #region Properties

        /// <summary>
        ///     Gets an enumeration of all tree nodes which are above the current node in the tree.
        /// </summary>
        /// <value>An enumeration of all tree nodes which are above the current node in the tree.</value>
        public IEnumerable<ITreeNode<T>> Ancestors {
            get { return GetAncestors(); }
        }

        /// <summary>
        ///     Gets or sets the value of the node.
        /// </summary>
        /// <value>The value of the node.</value>
        public T Value {
            get { return _value; }
            set {
                var oldValue = _value;
                _value = value;

                //Notify the value about it's node, if the value implements ITreeNodeAware
                var treeNodeAware = _value as ITreeNodeAware<T>;
                if (treeNodeAware != null)
                    treeNodeAware.Node = this;

                //Notify the old value about the change of it's node (new node is null)
                treeNodeAware = oldValue as ITreeNodeAware<T>;
                if (treeNodeAware != null)
                    treeNodeAware.Node = null;
            }
        }

        /// <summary>
        ///     Gets or sets the parent of the node.
        /// </summary>
        /// <remarks>
        ///     Detaches the node from it's old parent and attaches it to it's new parent.
        /// </remarks>
        /// <value>The parent of the node.</value>
        public ITreeNode<T> Parent {
            get { return _parent; }
            set { SetParent(value); }
        }

        /// <summary>
        ///     Gets the root of the tree.
        /// </summary>
        /// <value>The root of the tree.</value>
        public ITreeNode<T> Root {
            get { return (Parent == null) ? this : Parent.Root; }
        }

        /// <summary>
        ///     Gets or sets the children of the node.
        /// </summary>
        /// <value>The children of the node.</value>
        public ITreeNodeCollection<T> Children {
            get { return _children; }
            set {
                if (value == _children)
                    return;

                if (_children != null)
                    _children.Foreach(x => x.SetParent(null, false, false));

                _children = value;
                _children.Foreach(x => x.SetParent(this, false));
            }
        }

        /// <summary>
        ///     Gets or sets the search traversal direction.
        /// </summary>
        /// <value>The search traversal direction.</value>
        public TreeTraversalDirection SearchTraversalDirection {
            get { return _searchTraversalDirection; }
            set {
                _searchTraversalDirection = value;
                Children.Foreach(x => x.SearchTraversalDirection = value);
            }
        }

        /// <summary>
        ///     Gets or sets the dispose traversal direction.
        /// </summary>
        /// <value>The dispose traversal direction.</value>
        public TreeTraversalDirection DisposeTraversalDirection {
            get { return _disposeTraversalDirection; }
            set {
                _disposeTraversalDirection = value;
                Children.Foreach(x => x.DisposeTraversalDirection = value);
            }
        }

        /// <summary>
        ///     Gets or sets the ancestors traversal direction.
        /// </summary>
        /// <value>The ancestors traversal direction.</value>
        public TreeTraversalDirection AncestorsTraversalDirection {
            get { return _ancestorsTraversalDirection; }
            set {
                _ancestorsTraversalDirection = value;
                Children.Foreach(x => x.AncestorsTraversalDirection = value);
            }
        }

        /// <summary>
        ///     Gets or sets the descendants traversal direction.
        /// </summary>
        /// <value>The descendants traversal direction.</value>
        public TreeTraversalDirection DescendantsTraversalDirection {
            get { return _descendantsTraversalDirection; }
            set {
                _descendantsTraversalDirection = value;
                Children.Foreach(x => x.DescendantsTraversalDirection = value);
            }
        }

        /// <summary>
        ///     Gets or sets the traversal direction used to enumerate the nodes.
        /// </summary>
        /// <value>The traversal direction used to enumerate the nodes.</value>
        public TreeTraversalDirection TraversalDirection {
            get { return _traversalDirection; }
            set {
                _traversalDirection = value;
                Children.Foreach(x => x.TraversalDirection = value);
            }
        }

        /// <summary>
        ///     Gets the depth of the node.
        /// </summary>
        /// <value>The depth of the node.</value>
        public Int32 Depth {
            get { return (Parent == null ? -1 : Parent.Depth) + 1; }
        }

        /// <summary>
        ///     Gets a value indicating whether the node has any children or not.
        /// </summary>
        /// <value>A value indicating whether the node has any children or not.</value>
        public Boolean HasChildren {
            get { return Children != null && Children.Any(); }
        }

        /// <summary>
        ///     Gets a value indicating whether the node has a parent or not.
        /// </summary>
        /// <value>A value indicating whether the node has a parent or not.</value>
        public Boolean HasParent {
            get { return Parent != null; }
        }

        /// <summary>
        ///     Gets an enumeration of all tree nodes which are below the current node in the tree.
        /// </summary>
        /// <value>An enumeration of all tree nodes which are below the current node in the tree.</value>
        public IEnumerable<ITreeNode<T>> Descendants {
            get { return GetDescendants(); }
        }

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
        public virtual IEnumerable<T> FindValue(Func<ITreeNode<T>, Boolean> predicate)
        {
            var result = new List<T>();

            //Search from top to bottom
            switch (SearchTraversalDirection) {
                case TreeTraversalDirection.TopDown:
                    //From top to bottom
                    if (predicate(this))
                        result.Add(Value);
                    Children.Foreach(x => result.AddRange(x.FindValue(predicate)));
                    break;
                case TreeTraversalDirection.BottomUp:
                    //From bottom to top
                    Children.ForeachReverse(x => result.AddRange(x.FindValue(predicate)));
                    if (predicate(this))
                        result.Add(Value);
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Gets the nodes which matches the given predicate.
        /// </summary>
        /// <remarks>
        ///     Starts the search at the current tree node and traverses down the tree (Direction based on
        ///     <see cref="SearchTraversalDirection" />).
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns the nodes which matches the given predicate.</returns>
        public virtual IEnumerable<ITreeNode<T>> FindNode(Func<ITreeNode<T>, Boolean> predicate)
        {
            var result = new List<ITreeNode<T>>();

            //Search from top to bottom
            switch (SearchTraversalDirection) {
                case TreeTraversalDirection.TopDown:
                    //From top to bottom
                    if (predicate(this))
                        result.Add(this);
                    Children.Foreach(x => result.AddRange(x.FindNode(predicate)));
                    break;
                case TreeTraversalDirection.BottomUp:
                    //From bottom to top
                    Children.ForeachReverse(x => result.AddRange(x.FindNode(predicate)));
                    if (predicate(this))
                        result.Add(this);
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Gets the nodes with the given value.
        /// </summary>
        /// <param name="value">The value to search.</param>
        /// <returns>Returns the nodes with the given value.</returns>
        public IEnumerable<ITreeNode<T>> FindNode(T value)
        {
            var result = new List<ITreeNode<T>>();

            //Search from top to bottom
            switch (SearchTraversalDirection) {
                case TreeTraversalDirection.TopDown:
                    //From top to bottom
                    if (Value.Equals(value))
                        result.Add(this);
                    Children.Foreach(x => result.AddRange(x.FindNode(value)));
                    break;
                case TreeTraversalDirection.BottomUp:
                    //From bottom to top
                    Children.ForeachReverse(x => result.AddRange(x.FindNode(value)));
                    if (Value.Equals(value))
                        result.Add(this);
                    break;
            }

            return result;
        }

        public ITreeNode<T> FindNode(string name)
        {
            foreach (var node in Children)
                if (node.GetName() == name)
                    return node;

            return null;
        }

        public ITreeNode<T> FindNode(params string[] path)
        {
            ITreeNode<T> current = this;

            foreach(var name in path) {
                if (current is null) return null;
                current = current.FindNode(name);
            }

            return current;
        }

        /// <summary>
        ///     Adds the given value as child to the node.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns the added node.</returns>
        public ITreeNode<T> Add(T value)
        {
            var node = new TreeNode<T>(value, this) {
                DisposeTraversalDirection = DisposeTraversalDirection,
                SearchTraversalDirection = SearchTraversalDirection
            };
            Children.Add(node);
            return node;
        }

        /// <summary>
        ///     Adds the given node as child to the node, if it is not already a child of the node.
        /// </summary>
        /// <param name="node">The node to add.</param>
        /// <returns>Returns the added node.</returns>
        public ITreeNode<T> Add(ITreeNode<T> node)
        {
            if (!Children.Contains(node))
                Children.Add(node);

            return node;
        }

        /// <summary>
        ///     Sets the parent of the tree node.
        /// </summary>
        /// <param name="parent">The new parent.</param>
        /// <param name="attacheToNewParent">
        ///     A value determining whether the node should add it self to the children of the new parent
        ///     or not.
        /// </param>
        /// <param name="detachFromOldParent">A value indicating whether the node should detach itself from it's old parent or not.</param>
        public void SetParent(ITreeNode<T> parent,
                               Boolean attacheToNewParent = true,
                               Boolean detachFromOldParent = true)
        {
            if (_parent == parent)
                return;

            //Switch parent
            var oldParent = _parent;
            _parent = parent;

            //Remove node from old parent
            if (oldParent != null && detachFromOldParent)
                oldParent.Children.Remove(this, false);

            if (attacheToNewParent && Parent != null)
                Parent.Children.Add(this, false);
        }

        /// <summary>
        ///     Sets all directions (<see cref="DisposeTraversalDirection" />, <see cref="SearchTraversalDirection" />,
        ///     <see cref="AncestorsTraversalDirection" />, <see cref="DescendantsTraversalDirection" />).
        /// </summary>
        /// <param name="direction">The new direction.</param>
        public void SetAllDirections(TreeTraversalDirection direction)
        {
            SearchTraversalDirection = direction;
            DisposeTraversalDirection = direction;
            AncestorsTraversalDirection = direction;
            DescendantsTraversalDirection = direction;
            TraversalDirection = direction;
        }

        #endregion

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Collections.Generic.IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ITreeNode<T>> GetEnumerator()
        {
            return GetEnumeratorInternal()
                .GetEnumerator();
        }

        /*
        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Collections.IEnumerator" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }*/

        #endregion

        #region Overrides of Object

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override String ToString()
        {
            return $"Depth: {Depth} - Value: {Value}, Children: {Children.Count}, Parent: ({Parent})";

        }

        #endregion

        #region Private Members

        /// <summary>
        ///     Internal implementation to get a enumeration in the specified order.
        /// </summary>
        /// <returns>Returns a  enumeration of all nodes.</returns>
        private IEnumerable<ITreeNode<T>> GetEnumeratorInternal()
        {
            switch (TraversalDirection) {
                case TreeTraversalDirection.TopDown:
                    yield return this;
                    foreach (var child in Children) {
                        if (child is TreeNode<T> == false)
                            throw new NotSupportedException($"Child '{child}' is not of type TreeNode<T>.");

                        var enumeration = (child as TreeNode<T>).GetEnumeratorInternal();
                        foreach (var e in enumeration)
                            yield return e;
                    }

                    break;
                case TreeTraversalDirection.BottomUp:

                    foreach (var child in Children.Reverse()) {
                        if (child is TreeNode<T> == false)
                            throw new NotSupportedException($"Child '{child}' is not of type TreeNode<T>.");

                        var enumeration = (child as TreeNode<T>).GetEnumeratorInternal();
                        foreach (var e in enumeration)
                            yield return e;
                    }
                    yield return this;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Join("/", this.GetPath()),
                                                           $"The value '{TraversalDirection}' is a unknown tree traversal direction");
            }
        }

        private IEnumerable<ITreeNode<T>> GetDescendants(List<ITreeNode<T>> descendants = null)
        {
            descendants = descendants ?? new List<ITreeNode<T>>();
            if (Children == null || !Children.Any())
                return descendants;

            Children.Foreach(x => {
                if (x is TreeNode<T> == false)
                    throw new NotSupportedException($"Child '{x}' is not of type TreeNode<<T>>.");

                descendants.Add(x);
                (x as TreeNode<T>).GetDescendants(descendants);
            });

            return descendants;
        }

        private IEnumerable<ITreeNode<T>> GetAncestors(List<ITreeNode<T>> ancestors = null)
        {
            ancestors = ancestors ?? new List<ITreeNode<T>>();
            if (Parent == null)
                return ancestors;

            if (Parent is TreeNode<T> == false)
                throw new NotSupportedException("Parent is not of type TreeNode{T}.");

            ancestors.Add(Parent);
            return (Parent as TreeNode<T>).GetAncestors(ancestors);
        }

        private void Initialize(T value,
                                 ITreeNode<T> parent = null,
                                 ITreeNodeCollection<T> children = null)
        {
            Value = value;
            Parent = parent;
            Children = children ?? new TreeNodeCollection<T>(this);
            if (Parent != null) {
                if (!Parent.Children.Contains(this))
                    Parent.Children.Add(this);

                DisposeTraversalDirection = Parent.DisposeTraversalDirection;
                SearchTraversalDirection = Parent.SearchTraversalDirection;
                AncestorsTraversalDirection = Parent.AncestorsTraversalDirection;
                DescendantsTraversalDirection = Parent.DescendantsTraversalDirection;
            } else {
                DisposeTraversalDirection = TreeTraversalDirection.BottomUp;
                SearchTraversalDirection = TreeTraversalDirection.BottomUp;
                AncestorsTraversalDirection = TreeTraversalDirection.BottomUp;
                DescendantsTraversalDirection = TreeTraversalDirection.BottomUp;
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///     Release all resources hold by the node.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Destructs the tree node..
        /// </summary>
        ~TreeNode()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Releases the managed and unmanaged resource hold by the node.
        /// </summary>
        /// <param name="disposing">A value of true to release managed resources, false to release unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!disposing)
                return;

            //Release from bottom up (start with children).
            if (DisposeTraversalDirection == TreeTraversalDirection.BottomUp)
                foreach (var node in Children.Reverse())
                    node.Dispose();

            //Release the current node.
            var dispose = Value as IDisposable;
            if (dispose != null)
                (Value as IDisposable).Dispose();

            //Check if children are released or not.
            if (DisposeTraversalDirection != TreeTraversalDirection.TopDown)
                return;

            //Release from top down (start with current node).
            foreach (var node in Children)
                node.Dispose();
        }

        public string GetName()
        {
            var tnamedObject = Value as INamedObject;
            if (tnamedObject is null)
                return Value.ToStringSafe();
            else
                return tnamedObject.Name;
        }

        public string[] GetPath()
        {
            var names = new List<string>();

            foreach (var node in this.Ancestors) {
                names.Add(node.GetName());
            }
            names.Add(GetName());

            return names.ToArray();
        }

        #endregion
    }


}
