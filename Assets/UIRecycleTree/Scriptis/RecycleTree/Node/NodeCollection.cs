using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIRecycleTree {
	[Serializable]
	public class NodeCollection : IList<Node> {
		public int Count => _childNodes.Count;

		[SerializeReference] private List<Node> _childNodes;
		[SerializeReference] private Node _owner;
		public NodeCollection(Node ownerNode) {
			_owner = ownerNode;
			_childNodes = new();
		}

		public void Add(Node[] array) {
			foreach (var node in array)
				Add(node);
		}

		public Node AddFluent(string name) {
			var node = new Node(name);
			return AddFluent(node);
		}

		public Node AddFluent(Node item) {
			Add(item);
			return item;
		}

		public void Add(Node node) {
			if (node == null) return;

			_childNodes.Add(node);
			node.parentNode = _owner;
			node.tree = _owner.tree;

			if (node.CheckAllParentExpanded())
				CollectionChangedNotify();
		}

		public void Clear() {
			_childNodes.Clear();
			CollectionChangedNotify();
		}

		public bool Remove(Node node) {
			if (node == null) return false;

			bool treeNotifyNeeded = node.CheckAllParentExpanded();

			var removed = _childNodes.Remove(node);
			if (!removed) return false;

			if (treeNotifyNeeded)
				CollectionChangedNotify();
			return true;
		}

		public void RemoveAt(int index) {
			bool treeNotifyNeeded = _childNodes[index].CheckAllParentExpanded();
			_childNodes.RemoveAt(index);

			if (treeNotifyNeeded)
				CollectionChangedNotify();
		}

		private void CollectionChangedNotify() =>
				_owner.tree.Rebuild();

		// IList standard properties implement

		public int IndexOf(Node item) =>
				_childNodes.IndexOf(item);

		public void Insert(int index, Node item) =>
				_childNodes.Insert(index, item);

		public void CopyTo(Node[] array, int arrayIndex) =>
				_childNodes.CopyTo(array, arrayIndex);

		public IEnumerator<Node> GetEnumerator() =>
				_childNodes.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
				GetEnumerator();

		public Node this[int index] {
			get => _childNodes[index];
			set => _childNodes[index] = value;
		}

		public bool IsReadOnly => false;

		public bool Contains(Node item) =>
				_childNodes.Contains(item);
	}
}