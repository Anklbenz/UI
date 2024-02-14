/*using UITree;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


[Serializable]
public class NodeCollection : List<Node>, ISerializable  {
	public event Action ChangedEvent;
	private readonly Node _owner;

	[SerializeField] private List<Node> list => this;

	public NodeCollection() {
		_owner = null;
	}
	public NodeCollection(Node owner) {
		_owner = owner;
	}
	//AddRange RemoveRange not implemented

	public new Node Add(Node item) {
		base.Add(item);
		item.parentNode = _owner;
		
		ChangedNotify();
		return item;
	}

	public new bool Remove(Node item) {
		var removed = base.Remove(item);

		if (removed)
			item.Destroy();

		ChangedNotify();
		return removed;
	}
	public new void RemoveAt(int index) {
		base[index].Destroy();

		base.RemoveAt(index);
		ChangedNotify();
	}

	public new void Clear() {
		for (var i = base.Count - 1; i > 0; i--)
			base[i].Destroy();

		base.Clear();
		ChangedNotify();
	}

	private void ChangedNotify() =>
			ChangedEvent?.Invoke();


	public void GetObjectData(SerializationInfo info, StreamingContext context) {
	//	base.GetO
	}
}*/