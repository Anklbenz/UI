using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIRecycleTree {
	public class TreeView : UIBehaviour {
		private const string DEFAULT_NODE_NAME = "Node";
		private const string RESOURCES_FILE_NAME = "UITreeNode_template";

		public event Action<Node> NodeSelectedEvent;

		[SerializeField] private string pathSeparator = "/";
		[SerializeField] private bool isExpandedAsDefault;
		[SerializeField] private Node root;

		[SerializeField] private List<Node> visibleNodes;

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
		protected override void Awake() {
			// will not work with serialize
			root.tree = this;
		}
	}
}