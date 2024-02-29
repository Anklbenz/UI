using System;
using System.Text;
using UnityEngine;

namespace UIRecycleTree {
	[Serializable]
	public class Node {
		[SerializeField] protected NodeCollection nodeCollection;
		[SerializeField] protected Node parent;
		//	[SerializeField] protected TreeView treeView;
		[SerializeField] protected NodeData data;

		public NodeCollection nodes => nodeCollection;

		/*public TreeView tree {
			get => treeView;
			set => treeView = value;
		}*/

		public Node parentNode {
			get => parent;
			set {
				parent = value;
//				if (parent != null)
//					transform.SetParent(parent.childrenParentTransform.transform, false);
			}
		}


		public void Initialize() =>
				nodeCollection.SetOwner(this);

		public virtual void ExpandAll() {
			foreach (var node in nodeCollection)
				node.ExpandAll();
		}

		public virtual void CollapseAll() {
			foreach (var node in nodeCollection)
				node.CollapseAll();
		}

		public int GetChildCount() {
			int count = 0;
			foreach (var node in nodes)
				count += node.GetChildCount() + 1;
			return count;
		}

		public string fullPath {
			get {
				//		if (treeView == null)
				//			throw new Exception("TreeNodeNoParent");
				StringBuilder path = new StringBuilder();
				//		GetFullPath(path, treeView.separator);
				return path.ToString();
			}
		}
		private void GetFullPath(StringBuilder path, string pathSeparator) {
			if (parent == null)
				return;
			parent.GetFullPath(path, pathSeparator);
			if (parent.parent != null)
				path.Append(pathSeparator);
			path.Append(data.text);
		}
	}
}