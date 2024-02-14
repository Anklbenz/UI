using UITree;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeRoot))]
public class RootNodeEditor : Editor {
	private const string CHILD_NODES_NAME = "Child nodes";
	private const string NEW_CHILD_NAME = "New Child Name:";
	private const string DEFAULT_NODE_NAME = "Node";
	private const string CREATE_NODE_BUTTON_LABEL = "Create new child";
	private SerializedProperty _nodes;
	private NodeRoot _rootNode;
	private string _status, _nodeName;
	virtual protected void OnEnable() {
		_rootNode = (NodeRoot)target;
		_nodeName = DEFAULT_NODE_NAME;
		var nodeCollection = serializedObject.FindProperty("nodeCollection");
		_nodes = nodeCollection.FindPropertyRelative("childNodes");
	}
	public override void OnInspectorGUI() {
//base.OnInspectorGUI();
		
		GUILayout.Space(5);
		GUILayout.Label(CHILD_NODES_NAME, EditorStyles.boldLabel);
		GUILayout.Space(5);
		DrawChildNodes();
		AddChild();
		GUILayout.Space(5);
		serializedObject.ApplyModifiedProperties();

		if (EditorGUI.EndChangeCheck())
			EditorUtility.SetDirty(this);
	}
	private void DrawChildNodes() {
	    GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		EditorGUILayout.PropertyField(_nodes, new GUIContent(CHILD_NODES_NAME), true);
		GUILayout.EndHorizontal();

		GUILayout.Space(5);
	}
	private void AddChild() {
		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		GUILayout.Label(NEW_CHILD_NAME, GUILayout.Width(120));
		_nodeName = GUILayout.TextField(_nodeName);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		if (GUILayout.Button(CREATE_NODE_BUTTON_LABEL))
			AddFromEditor(_nodeName);

		GUILayout.EndHorizontal();
	}
	private void AddFromEditor(string nodeName) {
		if (_rootNode.tree == null) return;
		var instantiatedNode = _rootNode.tree.CreateNode(nodeName);
		_rootNode.nodes.Add(instantiatedNode);
		instantiatedNode.parentNode = _rootNode;
		_rootNode.Redraw();
	}
}