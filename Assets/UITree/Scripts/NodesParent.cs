using System;
using UnityEngine;
//[ExecuteAlways]
public class NodesParent : MonoBehaviour {
	public event Action ChildrenCountChangedEvent;
	public Transform children => transform;

	public bool isActive {
		set => gameObject.SetActive(value);
	}
	private void OnTransformChildrenChanged() {
		ChildrenCountChangedEvent?.Invoke();
	}
}