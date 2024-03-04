using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIRecycleTree {
	public class TreeView : UIBehaviour, IRecycleDataSource {
		private const string DEFAULT_NODE_NAME = "Node";
		private const string RESOURCES_FILE_NAME = "UITreeNode_template";
		private const float INDENT = 10;

		public event Action<Node> NodeSelectedEvent;

		[SerializeField] private string pathSeparator = "/";
		[SerializeField] private bool isExpandedAsDefault;
		[SerializeField] private RecycleView recycleView;
		[SerializeField] private Node root;

		[SerializeField] private List<Node> expandedNodes;

		public NodeCollection nodes => root.nodes;

		public string separator {
			get => pathSeparator;
			set => pathSeparator = value;
		}

		private Node _selected;

		public void OnNodeSelect(Node sender) {
			if (_selected == null) {
				SelectAndNotify(sender);
			}
			else if (_selected == sender) {
				Deselect(sender);
			}
			else {
				_selected.isSelected = false;
				SelectAndNotify(sender);
			}
		}
		private void SelectAndNotify(Node sender) {
			_selected = sender;
			_selected.isSelected = true;
			NodeSelectedEvent?.Invoke(sender);
		}
		private void Deselect(Node sender) {
			_selected = null;
			sender.isSelected = false;
		}

		public void OnItemsCountChanged() {
			expandedNodes = new List<Node>();
			root.GetChildrenInDepthIfExpanded(expandedNodes);
			StartCoroutine(recycleView.Reload());
		}

		// public void Register(Node )
				//hashSet nodes

		protected override void Awake() {
			// will not work with serialize
			root.tree = this;
			root.isExpanded = true;
			recycleView.recycleDataSource = this;
		}

		public int count => expandedNodes.Count;
		public void SetDataToItem(IRecycleItem recycleItem, int index) {
			var node = expandedNodes[index];
			var nodeView = (NodeView)recycleItem;
			node.view = nodeView;
		}
	}
}