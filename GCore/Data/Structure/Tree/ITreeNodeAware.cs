using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.Structure.Tree
{
    public interface ITreeNodeAware<T>
    {
        ITreeNode<T> Node { get; set; }
    }
}
