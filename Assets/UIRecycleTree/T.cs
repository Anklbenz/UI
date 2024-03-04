using UIRecycleTree;
using UnityEngine;

public class T : MonoBehaviour {
	[SerializeField] private TreeView treeView;
	/*void Start() {
		for (var i = 0; i < 10000; i++) {
			treeView.nodes.AddFluent(i.ToString());
		}
	}*/
	void Start() {
		var n0 = new Node("n0");
		var n1 = new Node("n1");
		var n2 = new Node("n2");
		var n20 = new Node("n20");
		var n200 = new Node("n200");
		var n21 = new Node("n21");
		var n3 = new Node("n3");
		var n4 = new Node("n4");
		var n5 = new Node("n5");
		var n6 = new Node("n6");
		var n7 = new Node("n7");
		var n8 = new Node("n8");
		var n9 = new Node("n9");
		var n10 = new Node("n10");
		var n11 = new Node("n11");

		var array = new Node[] {n0, n1, n2, n3, n4, n5, n6, n7, n8};

		//	var n = new Node(treeView, array, "n");
		//	n3.nodes.Add(n31);

		treeView.nodes.Add(array); //.nodes.AddFluent("n1-0");
		n0.nodes.AddFluent(n10).nodes.AddFluent(n11);
		n0.nodes.Add(n9);

		n2.nodes.Add(new Node[] {n20, n21});
		n20.nodes.Add(n200);

		//	Debug.Log(n2.fullPath);
	}
}