using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
namespace UITree {
	public abstract class NodeRoot : UIBehaviour {
		[SerializeField] protected NodeCollection nodeCollection;
		[SerializeField] protected Transform childrenTransform;
		[SerializeField] protected TreeView treeView;
		[SerializeField] protected NodeRoot parent;
		[SerializeField] protected string text;
		
		public NodeCollection nodes => nodeCollection;
		protected Transform childrenParentTransform => childrenTransform;

		public TreeView tree {
			get => treeView;
			set => treeView = value;
		}

		public NodeRoot parentNode {
			get => parent;
			set {
				parent = value;
				if (parent != null)
					transform.SetParent(parent.childrenParentTransform.transform, false);
			}
		}
		
		public virtual string nodeName {
			get => text;
			set {
				text = value;
			}
		}
		
		public void Initialize() =>
				nodeCollection.SetOwner(this);

		public virtual void Redraw() {}

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
				count +=  node.GetChildCount() + 1;
			return count;
		}
		
		public string fullPath {
			get {
				if (treeView == null)
					throw new Exception("TreeNodeNoParent");
				StringBuilder path = new StringBuilder();
				GetFullPath(path, treeView.separator);
				return path.ToString();
			}
		}
		private void GetFullPath(StringBuilder path, string pathSeparator)
		{
			if (parent == null)
				return;
			parent.GetFullPath(path, pathSeparator);
			if (parent.parent != null)
				path.Append(pathSeparator);
			path.Append(text);
		}
	}
}
