using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;

namespace UIRecycleTreeNamespace {
	[Serializable]
	public class NodeEvent : UnityEvent<Node> {
	}

	public class UIRecycleTree : RecycleView, IRecycleDataSource {
		private const string ITEM_RESOURCE_NAME = "UINodeView_template";
		public NodeEvent onNodeSelected = new();
		public NodeEvent onNodeDeselected = new();
		public NodeEvent onNodeCheckedChanged = new();
		public NodeEvent onNodeDblClick = new();
		public NodeEvent onNodeExpandStateChanged = new();
		public UnityEvent onSelectionChanged = new();

		[SerializeField] private bool fullRowNodes;
		[SerializeField] private bool highlightSubSelected;
		
		[SerializeField] private float childIndent = 55;

		[SerializeField] private float leftPadding = 1;
		[SerializeField] private float rightPadding = 1;
		[SerializeField] private float contentSpacing = 1;

		[SerializeField] private float toggleWidth = 40;
		[SerializeField] private Vector2 toggleIconSize = new(30, 30);

		[SerializeField] private bool imageEnabled = true;
		[SerializeField] private float imageWidth = 40;
		[SerializeField] private Vector2 imageIconSize = new(35, 35);

		[SerializeField] private bool checkboxEnabled;
		[SerializeField] private bool recursiveChecked;
		[SerializeField] private float checkboxWidth = 40;
		[SerializeField] private Vector2 checkboxIconSize = new(35, 35);
		
		[SerializeReference] private Node root;
		[SerializeField] private string pathSeparator = "/";
		[SerializeField] private List<Node> expandedNodes;

		[SerializeField] private NodeStyle[] nodeStylesArray;
		[SerializeField] private int lastNodeId;
		public NodeCollection nodes => root.nodes;
		public Node rootNode => root;
		public int nodesCount => root.GetAllChildrenCountRecursive() - 1; //subtract root node
		public bool isCheckboxesEnabled => checkboxEnabled;
		public string separator {
			get => pathSeparator;
			set => pathSeparator = value;
		}
		public NodeStyle[] nodeStyles => nodeStylesArray;
		public int expandedCount => expandedNodes.Count;
		public Node selectedNode => _selectedNode;
		public bool hasSelected => _selectedNode != null;
		public bool isRecursiveChecked {
			get => recursiveChecked;
			set => recursiveChecked = value;
		}

		private Node _selectedNode;
		private float _maxItemWidth;

		public void ExpandAll() {
			foreach (var node in nodes)
				node.ExpandAllWithoutNotify();
			Rebuild();
		}
		public void CollapseAll() {
			foreach (var node in nodes)
				node.CollapseAllWithoutNotify();
			Rebuild();
		}

		public void Clear() {
			nodes.Clear();
			
			lastNodeId = root.nodeId;
		}
		
		public void OnNodeClicked(Node node) {
			if (_selectedNode == null) {
				SelectAndNotify(node);
			}
			else if (_selectedNode == node) {
				DeselectAndNotify(node);
			}
			else {
				DeselectAndNotify(_selectedNode);
				SelectAndNotify(node);
			}
			
			
			base.Repaint();

			onSelectionChanged?.Invoke();
		}
		public void UpdateNodeCheckedState(Node node) {
			NodeCheckedStateChangedNotify(node);
			Repaint();
		}

		public void NodeCheckedStateChangedNotify(Node node) =>
				onNodeCheckedChanged?.Invoke(node);

		public void Rebuild() {
			if (!Application.isPlaying || !isActiveAndEnabled) return;

			expandedNodes = new List<Node>();
			root.GetAllExpandedChildrenRecursive(expandedNodes);
			StartCoroutine(Reload());
		}
		public void MergeDataWithView(RecycleItem recycleItem, int index) {
			var node = expandedNodes[index];
			var view = (NodeView)recycleItem;

			view.ClearPreviousSubscribes();
			var nodeStyleIndex = node.styleIndex;
			if (nodeStylesArray.Length == 0 || nodeStyleIndex >= nodeStylesArray.Length)
				throw new Exception($"NodeStylesArray is empty or The Node {node.name} has an styleIndex {nodeStyleIndex} that is not in the UIRecycleTree stylesArray.");

			var style = nodeStylesArray[nodeStyleIndex];
			if (style == null)
				throw new Exception($"Tree not contain nodeStyle with index {nodeStyleIndex}. Please add Node Style or change styleIndex in node named {node.name} id {node.nodeId}");
	
			SetNodeStyle(style, node, view);

			view.text = node.name;
			view.indent = node.depth;
			view.isChecked = node.isChecked;
			view.isFaded = node.isFaded;

			if (!node.hasChildren)
				view.state = ExpandedState.NoChild;
			else
				view.state = node.isExpanded ? ExpandedState.Expanded : ExpandedState.Collapsed;

			view.ClickedEvent += delegate { OnNodeClicked(node); };
			view.ExpandClickEvent += delegate { OnNodeExpandClicked(node); };
			view.CheckboxClickedEvent += delegate { NodeCheckedClicked(node); };
			view.DoubleClickedEvent += delegate { OnNodeDoubleClick(node); };
			view.Refresh();
		}
		private void SetNodeStyle(NodeStyle style, Node node, NodeView view) {
	
			view.fadedAlpha = style.fadeAlpha;
			view.toggleIcons = style.toggleIcons;
			view.imageIcons = style.imageIcons;
			view.checkboxIcons = style.checkboxIcons;

			if (node.isSelected) {
				var selectedStyle = style.selectedState;
				view.backgroundStyle = selectedStyle.background;
				view.textStyle = selectedStyle.overrideFont ? selectedStyle.textStyle : style.textStyle;
			}
			else if (node.isSubSelected) {
				var subSelectedStyle = style.subSelectedState;
				view.backgroundStyle = subSelectedStyle.background;
				view.textStyle = subSelectedStyle.overrideFont ? subSelectedStyle.textStyle : style.textStyle;
			}
			else {
				view.backgroundStyle = style.background;
				view.textStyle = style.textStyle;
			}
		}

		public int GetNextId() =>
				++lastNodeId;

		public Node FindNodeByIdRecursive(int id) =>
				root.FindNodeByIdRecursive(id, root);

		public Node[] FindNodesByNameRecursive(string searchName) {
			var foundedItems = new List<Node>();
			root.FindNodesByNameRecursive(searchName, foundedItems);
			return foundedItems.ToArray();
		}

		public Node FindFirstNodeByDataRecursive(object searchedData) =>
				rootNode.FindNodeByDataRecursive(searchedData);
		
		private void SelectAndNotify(Node node) {
			_selectedNode = node;
			_selectedNode.SetSelectedWithoutNotify(true);

			if(highlightSubSelected)
				node.ChangeIsSubSelectedStateForAllChildren(true);
			
			onNodeSelected?.Invoke(node);
		}
		
		private void DeselectAndNotify(Node node) {
			_selectedNode = null;
			node.SetSelectedWithoutNotify(false);

			if(highlightSubSelected)
				node.ChangeIsSubSelectedStateForAllChildren(false);
			
			onNodeDeselected?.Invoke(node);
		}

		private void OnNodeExpandClicked(Node node) {
			if (!node.hasChildren) return;
			node.SetExpandedStateWithoutNotify(!node.isExpanded);
			Rebuild();

			onNodeExpandStateChanged?.Invoke(node);
		}

		private void NodeCheckedClicked(Node node) {
			var newCheckedState = !node.isChecked;
			node.SetCheckedWithoutNotify(newCheckedState);
			if (isRecursiveChecked)
				node.ChangeIsCheckedStateForAllChildren(newCheckedState);
			UpdateNodeCheckedState(node);
		}

		private void OnNodeDoubleClick(Node node) =>
				onNodeDblClick?.Invoke(node);

		protected override RecycleItem CreateItem() {
			var item = Instantiate(Resources.Load<NodeView>(ITEM_RESOURCE_NAME), content, false);
			item.treePrefs = GetTreePrefs();
			return item;
		}

		protected override void OnPoolIncrease(RecycleItem item) =>
				((NodeView)item).NodeWidthReadyEvent += UpdateContentWidth;

		protected override void BeforePoolDecrease(RecycleItem item) =>
				((NodeView)item).NodeWidthReadyEvent -= UpdateContentWidth;

		protected override void BeforeReload() =>
			_maxItemWidth = 0;

		protected override void AfterReload() {
			if(!fullRowNodes) return;
			var viewportWidth = viewport.rect.width;
			_maxItemWidth = viewportWidth;
			SetContentWidth(_maxItemWidth /*+ contentPadding.left + contentPadding.right*/);
		}

		private void UpdateContentWidth(float itemWidth) {
			if (itemWidth < _maxItemWidth)
				return;
			_maxItemWidth = itemWidth;
			SetContentWidth(_maxItemWidth + contentPadding.left + contentPadding.right);
		}

		private void SetContentWidth(float width) =>
				content.sizeDelta = new Vector2(width, content.sizeDelta.y);

		protected override void Awake() {
			if (root != null) return;
			lastNodeId = -1;
			root = new Node {
					tree = this,
					isExpanded = true
			};
			
		}

		protected override void OnEnable() {
			base.OnEnable();
			recycleDataSource = this;
			Rebuild();
		}

		private TreePrefs GetTreePrefs() => new() {
				fullRectSelect = fullRowNodes,
				childIndent = childIndent,
				toggleWidth = toggleWidth,
				toggleIconSize = toggleIconSize,
				iconEnabled = imageEnabled,
				iconWidth = imageWidth,
				iconSize = imageIconSize,
				checkboxEnabled = checkboxEnabled,
				checkedWidth = checkboxWidth,
				checkedIconSize = checkboxIconSize,
				leftPadding = leftPadding,
				rightPadding = rightPadding,
				spacing = contentSpacing,
		};
	}
}