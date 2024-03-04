using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;


namespace UIRecycleTree {
	[Serializable]
	public class Node {
		private const short ROOT_INDENT = -1;
		
		protected NodeCollection nodeCollection;
		protected Node parent;
		protected TreeView treeView;
		public bool hasChildren => nodeCollection.Count > 0;
		public int depth => parent == null ? ROOT_INDENT : parent.depth + 1;
		public bool isExpanded {
			get=> _isExpanded;
			set => _isExpanded = value;
		}
		public bool isSelected {
			get => _isSelected;
			set {
				_isSelected = value;
				if (_view != null)
					_view.isSelected = value;
			}
		}

		public INodeView view {
			set {
				_view = value;
				_view.UnregisterEvents();
				_view.ClickedEvent += OnSelect;
				_view.ExpandClickEvent += OnExpand;
				_view.text = _name;
				_view.isSelected = _isSelected;
				_view.indent = depth * 55; // magic +++++

				if (!hasChildren)
					_view.state = CheckboxState.Empty;
				else
					_view.state = isExpanded ? CheckboxState.Expanded : CheckboxState.Collapsed;
			}
		}

		private string _name;
		private bool _isSelected, _isExpanded;
		private INodeView _view;

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

		public string fullPath {
			get {
				if (treeView == null)
					throw new Exception("Tree Node Has No Parent");
				StringBuilder path = new StringBuilder();
				GetFullPath(path, treeView.separator);
				return path.ToString();
			}
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

		public void OnExpand() {
			if(!hasChildren) return;
			_isExpanded = !isExpanded;
			tree.OnItemsCountChanged();
		}
		
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

		public void GetChildrenInDepth(List<Node> childList) {
			foreach (var node in nodes) {
				childList.Add(node);
				node.GetChildrenInDepth(childList);
			}
		}

		public void GetChildrenInDepthIfExpanded(List<Node> childList) {
			foreach (var node in nodes) {
				childList.Add(node);
				if (node.isExpanded)
					node.GetChildrenInDepthIfExpanded(childList);
			}
		}

		public int GetChildCountInDepth() {
			int count = 0;
			foreach (var node in nodes)
				count += node.GetChildCountInDepth() + 1;
			return count;
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
	
	
