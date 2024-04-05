using System;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace UIRecycleTreeNamespace {
	[Serializable]
	public class Node {
		private const short ROOT_INDENT = -1;

		[SerializeReference] protected NodeCollection nodeCollection;
		[SerializeReference] protected UIRecycleTree treeView;
		[SerializeReference] protected Node parent;
		[SerializeReference] private string _name;
		[SerializeField] private int _id;
		[SerializeField] private bool _isExpanded;
		[SerializeField] private bool _isChecked;
		[SerializeField] private int _styleIndex;
		public bool hasChildren => nodeCollection.Count > 0;
		public int childCount => nodeCollection.Count;
		public int depth => parent == null ? ROOT_INDENT : parent.depth + 1;
		public bool isExpanded {
			get => _isExpanded;
			set {
				if (value == _isChecked) return;
				SetExpandedStateWithoutNotify(value);

				if (tree != null)
					tree.Rebuild();
			}
		}
		public bool isSelected {
			get => _isSelected;
			set {
				if (value == _isSelected) return;
			//	SetSelectedWithoutNotify(value);
				
				if(tree!=null)
					tree.SelectNode(this);
				
			}
		}
		public bool isChecked {
			get => _isChecked;
			set {
				if (value == _isChecked) return;

				SetCheckedWithoutNotify(value);

				if (tree != null) {
					if (tree.recursiveChecked)
						ChangeIsCheckedStateForAllChildren(_isChecked);
					tree.Repaint();
					tree.OnNodeCheckedChangedNotify(this);
				}
			}
		}

		public bool isFaded {
			get => _isFaded;
			set {
				_isFaded = value;
				if (tree != null)
					tree.Repaint();
			}
		}

		public string name {
			get => _name;
			set {
				_name = value;
				if (tree != null)
					tree.Repaint();
			}
		}
		public int styleIndex {
			get => _styleIndex;
			set {

				_styleIndex = value;
				if (tree != null)
					tree.Repaint();
			}
		}
		
		public object data { get; set; }
		public NodeCollection nodes => nodeCollection;
		public int indexInCollection => parent.nodes.IndexOf(this);
		public int nodeId => _id;
		public UIRecycleTree tree {
			get => treeView;
			set {
				treeView = value;
				_id = treeView.GetNextId();
			}
		}
		public Node parentNode {
			get => parent;
			set => parent = value;
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

		private bool _isSelected, _isFaded;


		public Node(string name) : this() =>
				_name = name;

		public Node(UIRecycleTree treeView, Node[] children, string name = default(string)) : this() {
			_name = name;
			tree = treeView;
			nodeCollection.AddRange(children);
		}

		public Node(UIRecycleTree treeView, string name = default(string)) : this() {
			_name = name;
			tree = treeView;
		}

		public Node() =>
				nodeCollection = new NodeCollection(this);
		
		public bool TryCastData<T>(out T castedData) where T :class {
			castedData = null;
			if (data == null)
				return false;
			
			try {
				castedData = (T)data;
				return true;
			}
			catch {
				castedData = null;
				return false;
			}
		}
		
		public bool CheckAllParentExpanded() {
			if (parent == null)
				return true;
			if (!parent.isExpanded)
				return false;

			return parent.CheckAllParentExpanded();
		}

		public virtual void ExpandAll() {
			foreach (var node in nodeCollection)
				node.ExpandAllWithoutNotify();

			isExpanded = true;
		}

		public virtual void CollapseAll() {
			foreach (var node in nodeCollection)
				node.CollapseAllWithoutNotify();

			isExpanded = false;
		}

		public void ExpandAllWithoutNotify() {
			foreach (var node in nodeCollection)
				node.ExpandAllWithoutNotify();

			_isExpanded = true;
		}
		public void CollapseAllWithoutNotify() {
			foreach (var node in nodeCollection)
				node.CollapseAllWithoutNotify();

			_isExpanded = false;
		}

		public void SetCheckedWithoutNotify(bool nodeIsChecked) =>
				_isChecked = nodeIsChecked;

		public void SetExpandedStateWithoutNotify(bool nodeIsExpanded) =>
				_isExpanded = nodeIsExpanded;

		public void SetSelectedWithoutNotify(bool nodeIsSelected) =>
				_isSelected= nodeIsSelected;

		public Node[] GetAllChildrenRecursive() {
			List<Node> childList = new() {this};
			GetAllChildrenRecursive(childList);
			return childList.ToArray();
		}

		public void GetAllChildrenRecursive(List<Node> childList) {
			foreach (var node in nodes) {
				childList.Add(node);
				node.GetAllChildrenRecursive(childList);
			}
		}
		
		public Node FindNodeByDataRecursive(object searchedData) {
			foreach (var node in nodes) {
				if (ReferenceEquals(node.data, searchedData))
					return node;
				if (!node.hasChildren)
					continue;
				var foundNode = node.FindNodeByDataRecursive(searchedData);
				if (foundNode != null)
					return foundNode;
			}
			return null;
		}
		
		public Node FindNodeByNameRecursive(string searchedName) {
			foreach (var node in nodes) {
				if (node.name == searchedName)
					return node;
				if (!node.hasChildren)
					continue;
				var foundNode = node.FindNodeByDataRecursive(searchedName);
				if (foundNode != null)
					return foundNode;
			}
			return null;
		}
		
		public void FindNodesByNameRecursive(string searchName, List<Node> foundedItems) {
			foreach (var node in nodes) {
				if (node.name == searchName) 
					foundedItems.Add(node);
				
				if (node.hasChildren)
					node.FindNodesByNameRecursive(searchName, foundedItems);
			}
		}
		public void GetAllExpandedChildrenRecursive(List<Node> childList) {
			foreach (var node in nodes) {
				childList.Add(node);
				if (node.isExpanded)
					node.GetAllExpandedChildrenRecursive(childList);
			}
		}

		public int GetAllChildrenCountRecursive() {
			int count = 0;
			foreach (var node in nodes)
				count += node.GetAllChildrenCountRecursive() + 1;
			return count;
		}

		public void RemoveYourself() {
			if (parent != null)
				parent.nodes.Remove(this);
		}

		public Node FindNodeByIdRecursive(int id, Node item) {
			if (item == null)
				return null;
			if (item.nodeId == id)
				return item;
			if (!item.hasChildren)
				return null;
			foreach (var child in item.nodes) {
				var itemRecursive = FindNodeByIdRecursive(id, child);
				if (itemRecursive != null)
					return itemRecursive;
			}
			return null;
		}

		public void FindNodeByDataRecursive(object searchObject, List<Node> foundedItems) {
			foreach (var node in nodes) {
				if (ReferenceEquals(node.data, searchObject)) 
					foundedItems.Add(node);
				
				if (node.hasChildren)
					node.FindNodeByDataRecursive(searchObject, foundedItems);
			}
		}
		
		public void FindAllIsCheckedNodesRecursive(List<Node> foundedItems) {
			foreach (var node in nodes) {
				if (node._isChecked)
					foundedItems.Add(node);
				if (node.hasChildren)
					node.FindAllIsCheckedNodesRecursive(foundedItems);
			}
		}

		private void ChangeIsCheckedStateForAllChildren(bool isCheck) {
			foreach (var node in nodes) {
				node.SetCheckedWithoutNotify(isCheck);
				if (tree != null)
					tree.OnNodeCheckedChangedNotify(node);
				if (node.hasChildren)
					node.ChangeIsCheckedStateForAllChildren(isChecked);
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