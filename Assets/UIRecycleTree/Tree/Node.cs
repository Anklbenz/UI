using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;


namespace UIRecycleTree {
	[Serializable]
	public class Node {
		protected NodeCollection nodeCollection;
		protected Node parent;
		protected TreeView treeView;
		public bool hasChildren => nodeCollection.Count > 0;
		public bool isExpanded { get; set; }
		public bool isSelected { get; set; }
		private string _name;

		public NodeCollection nodes => nodeCollection;

		public TreeView tree {
			get => treeView;
			set => treeView = value;
		}
		public Node parentNode {
			get => parent;
			set => parent = value;
		}

		public string name {
			get => _name;
			set => _name = value;
		}

		public Node(string name) : this() =>
				_name = name;

		public Node(TreeView treeView, Node[] children, string name = default(string)) : this() {
			_name = name;
			tree = treeView;
			nodeCollection.AddRange(children);
		}

		public Node(TreeView treeView, string name = default(string)) : this() {
			_name = name;
			tree = treeView;
		}

		public Node() =>
				nodeCollection = new NodeCollection(this);

		public void OnSelect() =>
				tree.OnNodeSelect(this);
		public virtual void ExpandAll() {
			isExpanded = true;

			foreach (var node in nodeCollection)
				node.ExpandAll();
		}

		public virtual void CollapseAll() {
			isExpanded = false;

			foreach (var node in nodeCollection)
				node.CollapseAll();
		}

		public void GetExpandedNodes(List<Node> visibleNodes) {
			foreach (var node in nodes) {
				visibleNodes.Add(node);
				if (node.isExpanded)
					node.GetExpandedNodes(visibleNodes);
			}
		}

		public int GetChildCount() {
			int count = 0;
			foreach (var node in nodes)
				count += node.GetChildCount() + 1;
			return count;
		}

		public string fullPath {
			get {
				if (treeView == null)
					throw new Exception("Tree Node Has No Parent");
				StringBuilder path = new StringBuilder();
				GetFullPath(path, treeView.separator);
				return path.ToString();
			}
		}
		private void GetFullPath(StringBuilder path, string pathSeparator) {
			if (parent == null)
				return;
			parent.GetFullPath(path, pathSeparator);
			if (parent.parent != null)
				path.Append(pathSeparator);
			path.Append(_name);
		}
	}
}