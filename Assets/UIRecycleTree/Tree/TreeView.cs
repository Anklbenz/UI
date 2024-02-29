using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIRecycleTree {
	public class TreeView : UIBehaviour {
		private const string DEFAULT_NODE_NAME = "Node";
		private const string RESOURCES_FILE_NAME = "UITreeNode_template";

		public event Action<Node> NodeSelectedEvent;

		[SerializeField] private string pathSeparator = "/";
		[SerializeField] private bool isExpandedAsDefault;

		public string separator {
			get => pathSeparator;
			set => pathSeparator = value;
		}
		private Node _selected;

		public Node CreateNode(string nodeText) {
			var node = CreateNode();
			node.nodeName = nodeText;
			node.name = nodeText;
			return node;
		}

		public Node CreateNode() {
			var node = Instantiate(Resources.Load<Node>(RESOURCES_FILE_NAME));
			//	node.tree = this;
			node.Initialize();
			node.isExpanded = isExpandedAsDefault;
			node.name = DEFAULT_NODE_NAME;
			return node;
		}
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
	}
}