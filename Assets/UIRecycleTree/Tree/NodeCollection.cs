using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIRecycleTree {
	/*[Serializable]*/
	public class NodeCollection : IList<Node> {
		/*[SerializeField]*/
		private readonly List<Node> _childNodes = new();
		/*[SerializeField]*/
		private readonly Node _owner;
		public event Action ChangedEvent;
		public int Count => _childNodes.Count;
		public NodeCollection(Node ownerNode) =>
				_owner = ownerNode;

		public Node AddFluent(Node item) {
			Add(item);
			return item;
		}

		public void Add(Node item) {
			if (item == null) return;

			_childNodes.Add(item);
			item.parentNode = _owner;
			//
			item.tree = _owner.tree;

			ChangedNotify();
		}

		public void Clear() {
			//	for (var i = childNodes.Count - 1; i > 0; i--)
			//		childNodes[i].Destroy();

			_childNodes.Clear();
			ChangedNotify();
		}

		public bool Remove(Node item) {
			if (item == null) return false;

			var removed = _childNodes.Remove(item);
			;
			if (!removed) return false;

			//	item.Destroy();

			ChangedNotify();
			return true;
		}

		public bool RemoveListRecord(Node item) {
			var result = _childNodes.Remove(item);

			ChangedNotify();
			return result;
		}

		public void RemoveAt(int index) {
			//		childNodes[index].Destroy();

			_childNodes.RemoveAt(index);
			ChangedNotify();
		}
		private void ChangedNotify() =>
				ChangedEvent?.Invoke();

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