using TMPro;
using UIRecycleTreeNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour {
	[SerializeField] private UIRecycleTreeNamespace.UIRecycleTree treeView;
	[SerializeField] protected GameObject target;
	[SerializeField] protected Button button, expand, collapse, delete;
	[SerializeField] private TMP_Text text, nodesCountText;

	void Start() {
		Application.targetFrameRate = 400;

		button.onClick.AddListener(Fill);
		expand.onClick.AddListener(Expand);
		collapse.onClick.AddListener(Collapse);
		delete.onClick.AddListener(Delete);
		treeView.onNodeSelected.AddListener(GetPath);
		treeView.onNodeDblClick.AddListener(DoubleClicked);
		treeView.onSelectionChanged.AddListener(SelectionChanged);
		treeView.onExpandStateChanged.AddListener(OnExpand);
		treeView.onNodeCheckedChanged.AddListener(OnChecked);
		/*treeView.NodeSelectedEvent += GetPath;
		treeView.NodeDoubleClickEvent += DoubleClicked;
		treeView.ExpandedStateChangedEvent += OnExpand;*/
		//	Fill();

		nodesCountText.text = treeView.nodesCount.ToString();
		//	Start1();
	}
	private void OnChecked(Node arg0) {
		Debug.Log(arg0.name + "Checked");
	}
	private void SelectionChanged() {
		Debug.Log("Selection Changed");
	}
	private void OnExpand(Node obj) {
		Debug.Log(obj.name + "Expanded = " + obj.isExpanded);
	}
	private void DoubleClicked(Node obj) {
		obj.isFaded = !obj.isFaded;
	}
	private void GetPath(Node selected) {
		Debug.Log("Expanded " +  selected.isExpanded);
		text.text = selected.fullPath;
	}
	private void Delete() {
		if (treeView.selectedNode != null)
			treeView.selectedNode.RemoveYourself();

		nodesCountText.text = treeView.nodesCount.ToString();
	}
	private void Collapse() {
		treeView.CollapseAll();
	}
	private void Expand() {
		treeView.ExpandAll();
	}
	private void Fill() {
		FillTree(treeView.rootNode, target.transform);
		nodesCountText.text = treeView.nodesCount.ToString();
	}

	private void FillTree(Node node, Transform targetTransform) {
		foreach (Transform child in targetTransform) {
			var node2 = node.nodes.AddFluent(child.name);
			if (targetTransform.childCount > 0)
				FillTree(node2, child);
		}
	}

	void Start1() {
		var n0 = new Node("n0dfhhdfhdfhdfhdf");
		var n1 = new Node("n1");
		var n2 = new Node("n2");
		var n20 = new Node("n20");
		var n200 = new Node("n200");
		var n21 = new Node("n21gdfhfh");
		var n3 = new Node("n3");
		var n4 = new Node("n4 wet34hfhdbcvb");
		var n5 = new Node("n5  wtettewt");
		var n6 = new Node("n6 wettwet");
		var n7 = new Node("n7");
		var n8 = new Node("n8");
		var n9 = new Node("n9 ");
		var n10 = new Node("n10");
		var n11 = new Node("n11cfgggew wet etewt twte et");

		var array = new Node[] {n0, n1, n2, n3, n4, n5, n6, n7, n8};

		//	var n = new Node(treeView, array, "n");
		//	n3.nodes.Add(n31);

		treeView.nodes.AddRange(array); //.nodes.AddFluent("n1-0");
		n0.nodes.AddFluent(n10).nodes.AddFluent(n11);
		n0.nodes.Add(n9);

		n2.nodes.AddRange(new Node[] {n20, n21});
		n20.nodes.Add(n200);

		//	Debug.Log(n2.fullPath);
	}
}