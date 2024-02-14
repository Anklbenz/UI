using System;
using UITree;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeView))]
public class TreeViewEditor : RootNodeEditor {
	private const string DEFAULT_ROOT_NAME = "root";
	private TreeView _treeView;
	private string _rootNodeName;
//	private void OnEnable() {
//		_treeView = (TreeView)target;
//		_rootNodeName = DEFAULT_ROOT_NAME;
//	}

	public override void OnInspectorGUI() {
		/*GUILayout.Label("Root node name");
		_rootNodeName = GUILayout.TextField(_rootNodeName);

		if (GUILayout.Button("Create root node"))
			_treeView.AddFromEditor(_rootNodeName);*/
		base.OnInspectorGUI();
	}
}