using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIRecycleTree {
	[Serializable]
	public class NodeEvent : UnityEvent<Node> {
	}

	public class UIRecycleTree : RecycleView, IRecycleDataSource {
		private const string ITEM_RESOURCE_NAME = "UINodeView_template";
		public NodeEvent onNodeSelected = new();
		public NodeEvent onNodeDeselected = new();
		public NodeEvent onNodeCheckedChanged = new();
		public NodeEvent onNodeDblClick = new();
		public NodeEvent onExpandStateChanged = new();
		public UnityEvent onSelectionChanged = new();

		[SerializeReference] private Node root;
		[SerializeField] private string pathSeparator = "/";
		[SerializeField] private List<Node> expandedNodes;

		[SerializeField] private NodePrefs nodePrefs;
		[SerializeField] private NodeStyle[] nodeStylesArray;
		[SerializeField] private int lastNodeId;
		public NodeCollection nodes => root.nodes;
		public Node rootNode => root; // убрать, чтобы индейцы не удалили рут из кода
		public int nodesCount => root.GetAllChildrenCountRecursive();
		public bool isCheckboxesEnabled => nodePrefs != null && nodePrefs.checkboxEnabled;
		public bool recursiveChecked => nodePrefs != null && nodePrefs.recursiveChecked;
		public string separator {
			get => pathSeparator;
			set => pathSeparator = value;
		}
		public NodeStyle[] nodeStyles => nodeStylesArray;
		public int expandedCount => expandedNodes.Count;
		public Node selectedNode => _selectedNode;

		private Node _selectedNode;
		private float _maxItemWidth;

		public void ExpandAll() {
			foreach (var node in nodes)
				node.ExpandAll();
			Rebuild();
		}
		public void CollapseAll() {
			foreach (var node in nodes)
				node.CollapseAll();
			Rebuild();
		}

		private void OnNodeSelect(Node node) {
			if (_selectedNode == null) {
				SelectAndNotify(node);
			}
			else if (_selectedNode == node) {
				Deselect(node);
			}
			else {
				_selectedNode.isSelected = false;
				SelectAndNotify(node);
			}
			base.Repaint();

			onSelectionChanged?.Invoke();
		}
		private void OnNodeExpandClicked(Node node) {
			if (!node.hasChildren) return;
			node.SetExpandedStateWithoutNotify(!node.isExpanded);
			Rebuild();

			onExpandStateChanged?.Invoke(node);
		}

		private void OnNodeCheckedClicked(Node node) {
			node.isChecked = !node.isChecked;
		}

		public void OnNodeCheckedChangedNotify(Node node) =>
				onNodeCheckedChanged?.Invoke(node);
		
		private void OnNodeDoubleClick(Node node) =>
				onNodeDblClick?.Invoke(node);

		private void SelectAndNotify(Node node) {
			_selectedNode = node;
			_selectedNode.isSelected = true;

			onNodeSelected?.Invoke(node);
		}
		private void Deselect(Node node) {
			_selectedNode = null;
			node.isSelected = false;

			onNodeDeselected?.Invoke(node);
		}
		public void Rebuild() {
			if (!Application.isPlaying) return;
			expandedNodes = new List<Node>();
			root.GetAllExpandedChildrenRecursive(expandedNodes);
			StartCoroutine(Reload());
		}

		protected override RecycleItem CreateItem() {
			var item = Instantiate(Resources.Load<NodeView>(ITEM_RESOURCE_NAME), content, false);
			item.nodePrefs = nodePrefs;
			return item;
		}

		public void MergeDataWithItem(RecycleItem recycleItem, int index) {
			var node = expandedNodes[index];
			var nodeView = (NodeView)recycleItem;

			nodeView.ClearPreviousSubscribes();
			var nodeStyleIndex = node.styleIndex;
			if (nodeStylesArray.Length == 0 || nodeStyleIndex >= nodeStylesArray.Length)
				throw new Exception($"NodeStylesArray is empty or The Node {node.name} has an styleIndex {nodeStyleIndex} that is not in the UIRecycleTree stylesArray.");
			nodeView.style = nodeStylesArray[nodeStyleIndex];

			nodeView.text = node.name;
			nodeView.indent = node.depth;
			nodeView.isSelected = node.isSelected;
			nodeView.isChecked = node.isChecked;
			nodeView.isFaded = node.isFaded;

			if (!node.hasChildren)
				nodeView.state = ExpandedState.NoChild;
			else
				nodeView.state = node.isExpanded ? ExpandedState.Expanded : ExpandedState.Collapsed;

			nodeView.ClickedEvent += delegate { OnNodeSelect(node); };
			nodeView.ExpandClickEvent += delegate { OnNodeExpandClicked(node); };
			nodeView.CheckboxClickedEvent += delegate { OnNodeCheckedClicked(node); };
			nodeView.DoubleClickedEvent += delegate { OnNodeDoubleClick(node); };
			nodeView.Refresh();
		}

		public int GetNextId() {
			if (nodes.Count != 1)
				return ++lastNodeId;
			lastNodeId = 0;
			return lastNodeId;
		}

		public Node FindNodeRecursive(int id) =>
				root.FindNodeByIdRecursive(id, root);

		public Node[] FindNodesByNameRecursive(string searchName) {
			var foundedItems = new List<Node>();
			root.FindChildByNameRecursive(searchName, foundedItems);
			return foundedItems.ToArray();
		}

		protected override void OnPoolIncrease(RecycleItem item) =>
				((NodeView)item).WidthChangedEvent += UpdateContentWidth;

		protected override void BeforePoolDecrease(RecycleItem item) =>
				((NodeView)item).WidthChangedEvent -= UpdateContentWidth;

		protected override void BeforeReload() =>
				_maxItemWidth = 0;

		private void UpdateContentWidth(float itemWidth) {
			if (itemWidth < _maxItemWidth)
				return;
			_maxItemWidth = itemWidth;
			content.sizeDelta = new Vector2(_maxItemWidth, content.sizeDelta.y);
		}
		protected override void Awake() {
			root ??= new Node();
			root.tree = this;
			root.isExpanded = true;
		}

		protected override void OnEnable() {
			base.OnEnable();
			recycleDataSource = this;
			Rebuild();
		}
	}
}