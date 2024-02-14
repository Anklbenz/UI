
using UnityEngine;
namespace UITree {
	public class NewBehaviourScript : MonoBehaviour {
		[SerializeField] private TreeView treeView;
		private void Start() {
			Application.targetFrameRate = 400;
			treeView.Initialize();
			var node = treeView.CreateNode("Cars");
			treeView.nodes.Add(node);

			var nodeTrucks = treeView.CreateNode("Trucks");
			treeView.nodes.Add(nodeTrucks);

			var nodeCorolla = treeView.CreateNode("Toyota corolla");
			var nodeCorollaHatchback = treeView.CreateNode("Toyota corolla hatchback");
			treeView.nodes[0].nodes.Add1(nodeCorolla).nodes.Add1(nodeCorollaHatchback);

			var nodeMan = treeView.CreateNode("Man");
			treeView.nodes[1].nodes.Add(nodeMan);

		}
	}
}
