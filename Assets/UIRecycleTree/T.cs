using System.Collections.Generic;
using UIRecycleTree;
using UnityEngine;

public class T : MonoBehaviour {
	[SerializeField] private TreeView treeView;
	void Start() {
		var n1 = new Node("1");
		var n2 = new Node("2");
		var n3 = new Node("3");
		var n31 = new Node("n31");
		var n4 = new Node("4");
		n3.isExpanded = true;
		var array = new Node[] {n1, n2, n3, n4};

		var n = new Node(treeView, array, "n");
		n3.nodes.Add(n31);
		
		treeView.nodes.Add(n);
		
		List<Node> list = new();

		n.GetExpandedNodes(list);
		Debug.Log(n31.fullPath);
	}
}