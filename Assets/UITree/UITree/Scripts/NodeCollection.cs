using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITree {
	[Serializable]
	public class NodeCollection : IList<Node> {

		[SerializeField] private List<Node> childNodes;
		[SerializeField] private NodeRoot owner;
		public event Action ChangedEvent;
		public int Count => childNodes.Count;
		public void SetOwner(NodeRoot ownerNode) =>
				owner = ownerNode;
		public Node Add1(Node item) {
			Add(item);
			return item;
		}

		public void Add(Node item) {
			if (item == null) return;

			childNodes.Add(item);
			item.parentNode = owner;

			ChangedNotify();
		}

		public void Clear() {
			for (var i = childNodes.Count - 1; i > 0; i--)
				childNodes[i].Destroy();

			childNodes.Clear();
			ChangedNotify();
		}

		public bool Remove(Node item) {
			if (item == null) return false;

			var removed = childNodes.Remove(item);
			;
			if (!removed) return false;

			item.Destroy();

			ChangedNotify();
			return true;
		}

		public bool RemoveListRecord(Node item) {
			var result = childNodes.Remove(item);

			ChangedNotify();
			return result;
		}

		public void RemoveAt(int index) {
			childNodes[index].Destroy();

			childNodes.RemoveAt(index);
			ChangedNotify();
		}
		private void ChangedNotify() =>
				ChangedEvent?.Invoke();

		// IList standard properties implement

		public int IndexOf(Node item) =>
				childNodes.IndexOf(item);

		public void Insert(int index, Node item) =>
				childNodes.Insert(index, item);

		public void CopyTo(Node[] array, int arrayIndex) =>
				childNodes.CopyTo(array, arrayIndex);
		public IEnumerator<Node> GetEnumerator() =>
				childNodes.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
				GetEnumerator();

		public Node this[int index] {
			get => childNodes[index];
			set => childNodes[index] = value;
		}

		public bool IsReadOnly => false;

		public bool Contains(Node item) =>
				childNodes.Contains(item);
	}
}