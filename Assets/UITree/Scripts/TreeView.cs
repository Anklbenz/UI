using System;
using UITree;
using UnityEngine;
namespace UITree {

	public class TreeView : NodeRoot {
		public event Action<Node> NodeSelectedEvent;

		private const string DEFAULT_NODE_NAME = "Node";
		private const string RESOURCES_FILE_NAME = "UITreeNode_template";
		/* 	  selected.FullPath; 	 */
		private Node _selected;


		public Node CreateNode(string nodeName) {
			var node = CreateNode();
			node.nodeName = nodeName;
			node.name = nodeName;
			return node;
		}

		public Node CreateNode() {
			var node = Instantiate(Resources.Load<Node>(RESOURCES_FILE_NAME));
			node.tree = this;
			node.Initialize();
			node.name = DEFAULT_NODE_NAME;
			return node;
		}
		public void OnNodeSelect(Node sender) {
			if (_selected == null) {
				Select(sender);
				return;
			}

			if (_selected == sender) {
				Deselect(sender);
				return;
			}
			_selected.isSelected = false;
			Select(sender);
		}
		private void Deselect(Node sender) {
			_selected = null;
			sender.isSelected = false;
		}
		private void Select(Node sender) {
			_selected = sender;
			_selected.isSelected = true;
		}
	}
}
