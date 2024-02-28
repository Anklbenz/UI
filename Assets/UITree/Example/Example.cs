using UnityEngine;
using UnityEngine.UI;
namespace UITree {
	public class Example : MonoBehaviour {
		[SerializeField] private TreeView treeView;
		[SerializeField] private Button expandButton, collapseButton, hide;
		[SerializeField] private GameObject target;
		private void Start() {
			Application.targetFrameRate = 400;
			treeView.Initialize();
			
			treeView.NodeSelectedEvent += OnSelect;
			expandButton.onClick.AddListener(Expand);
			collapseButton.onClick.AddListener(Collapse);
			hide.onClick.AddListener(OnHideClick);

			Example4();
		}

		private void OnHideClick() {
			for (var i = 0; i < 1000; i++) {
				var node = treeView.nodes[i].gameObject;
				if (node.activeInHierarchy)
					node.SetActive(false);
				else
					node.SetActive(true);
			}
		}
		private void Expand() {
			treeView.ExpandAll();
		}

		private void Collapse() {
			treeView.CollapseAll();
		}

		private void Example4() {
			var targetTransform = target.transform;
			foreach (Transform child in target.transform) {
			      var n = treeView.nodes.Add1(treeView.CreateNode(child.name));
			      if(child.childCount>0)
				      AddNode(n, child);
			}
			Debug.Log(treeView.GetChildCount());
		}

		private void AddNode(Node node, Transform targetTransform) {
			foreach (Transform child in targetTransform) {
				var node2 = node.nodes.Add1(treeView.CreateNode(child .name));
				if (targetTransform.childCount > 0)
					AddNode(node2, child);
			}
		}

		public void Example2() {
			for (var i = 0; i < 50; i++) {
				var node = treeView.nodes.Add1(treeView.CreateNode($"node{i}"));
				for (var j = 0; j < 10; j++) {
					var child = node.nodes.Add1(treeView.CreateNode($"child{j}"));
					for (var z = 0; z < 5; z++) {
						child.nodes.Add1(treeView.CreateNode($"preChild{z}"));
					}
				}
			}

			Debug.Log(treeView.GetChildCount());
		}
		
		public void Example3() {
			for (var i = 0; i < 1000; i++) {
				var node = treeView.nodes.Add1(treeView.CreateNode($"node{i}"));
			}

			Debug.Log(treeView.GetChildCount());
		}


		private void Example1() {
			var node = treeView.CreateNode("Cars");
			treeView.nodes.Add(node);

			var nodeTrucks = treeView.CreateNode("Trucks");
			treeView.nodes.Add(nodeTrucks);

			var nodeCorolla = treeView.CreateNode("Toyota corolla");
			var nodeCorollaHatchback = treeView.CreateNode("Toyota corolla hatchback");
			treeView.nodes[0].nodes.Add1(nodeCorolla).nodes.Add1(nodeCorollaHatchback);

			var nodeMan = treeView.CreateNode("Man");
			var nodeScania = treeView.CreateNode("Scania");
			treeView.nodes[1].nodes.Add(nodeMan);
			treeView.nodes[1].nodes.Add(nodeScania);

			Debug.Log(treeView.GetChildCount());
		}
		private void OnSelect(Node obj) {
			Debug.Log(obj.fullPath);
		}
	}
}