using System;
using System.Collections;
using System.Collections.Generic;

namespace UIRecycleTree {

	public class NodeCollection : IList<Node> {
		public event Action ChangedEvent;
		public int Count => _childNodes.Count;

		private readonly List<Node> _childNodes = new();
		private readonly Node _owner;
		public NodeCollection(Node ownerNode) =>
				_owner = ownerNode;

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

		public void Add(Node item) {
			if (item == null) return;

			_childNodes.Add(item);
			item.parentNode = _owner;
			item.tree = _owner.tree;

			CollectionChangedNotify();
		}

		public void Clear() {
			_childNodes.Clear();
			CollectionChangedNotify();
		}

		public bool Remove(Node item) {
			if (item == null) return false;

			var removed = _childNodes.Remove(item);
			if (!removed) return false;

			CollectionChangedNotify();
			return true;
		}
		
		public void RemoveAt(int index) {
			_childNodes.RemoveAt(index);
			CollectionChangedNotify();
		}
		private void CollectionChangedNotify() =>
				_owner.tree.OnItemsCountChanged();
		//treenode register

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