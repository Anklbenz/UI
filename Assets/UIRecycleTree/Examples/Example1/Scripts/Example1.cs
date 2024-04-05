using UnityEngine;
namespace UIRecycleTreeNamespace {
	public class Example1 : MonoBehaviour {
		[SerializeField] private UIRecycleTree treeView;
		[SerializeField] private GameObject targetGameObject;

		public void OnEnable() {
		//	FillTreeRecursive(treeView.rootNode, targetGameObject.transform);
			treeView.onNodeSelected.AddListener(OnNodeSelected);
			treeView.onNodeCheckedChanged.AddListener(OnNodeCheckedStateChanged);
		}
		//Fill treeView recursive
		private void FillTreeRecursive(Node node, Transform targetTransform) {
			foreach (Transform child in targetTransform) {
				var node2 = node.nodes.AddFluent(child.name);

				// Set gameObject transform to node data field
				node2.data = child.transform;
				if (targetTransform.childCount > 0)
					FillTreeRecursive(node2, child);
			}
		}
		private void OnNodeSelected(Node arg0) {
			
		}

		private void OnNodeCheckedStateChanged(Node arg0) {
			Debug.Log(arg0.name);
		//	var boundTransform = (Transform)arg0.data;
		//	if (boundTransform.TryGetComponent<Renderer>(out var rend)) {
		//		rend.material.color = Color.green;
		//	}
		}
	}
}