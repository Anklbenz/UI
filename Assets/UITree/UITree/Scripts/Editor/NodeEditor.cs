using UnityEditor;
using UnityEngine;

namespace UITree {
	[CustomEditor(typeof(Node))]
	public class NodeEditor : RootNodeEditor {
		private const string DELETE_FOLDOUT_NAME = "Delete";
		private const string DELETE_LABEL = "Delete This Node With All Children";
		private const string NODE_VISUAL_NAME = "Node Visual";
		private SerializedProperty _expandControlSettings;
		private SerializedProperty _imageControlSettings;
		private SerializedProperty _nodeText;
		private SerializedProperty _expanded;
		private SerializedProperty _imageEnabled;

		private Node _node;
		private bool _deleteFoldoutStatus;

		protected override void OnEnable() {
			base.OnEnable();
			_node = (Node)target;

			_expanded = serializedObject.FindProperty("expanded");
			_nodeText = serializedObject.FindProperty("text");
			_imageEnabled = serializedObject.FindProperty("imageControlEnabled");
			_imageControlSettings = serializedObject.FindProperty("imageControlSettings");
			_expandControlSettings = serializedObject.FindProperty("expandControlSettings");
		}
		public override void OnInspectorGUI() {
			DrawProperties();
			GUILayout.Space(5);
			DrawVisual();
			GUILayout.Space(5);
			GUIUtils.DrawLine();

			//base root node Properties
			base.OnInspectorGUI();
			//

			GUIUtils.DrawLine(1);
			GUILayout.Space(5);

			var nodeDeleted = DeleteCurrentNode();
			if (!nodeDeleted)
				serializedObject.ApplyModifiedProperties();
		}

		private void DrawProperties() {
			EditorGUILayout.PropertyField(_nodeText);
			EditorGUILayout.PropertyField(_expanded);
		}
		private void DrawVisual() {
			GUILayout.Label(NODE_VISUAL_NAME, EditorStyles.boldLabel);
			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			GUILayout.Space(20);
			EditorGUILayout.PropertyField(_expandControlSettings);
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.Space(20);
			EditorGUILayout.PropertyField(_imageEnabled);
			EditorGUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(20);
			EditorGUILayout.PropertyField(_imageControlSettings);
			GUILayout.EndHorizontal();
		}

		private bool DeleteCurrentNode() {
			_deleteFoldoutStatus = EditorGUILayout.Foldout(_deleteFoldoutStatus, DELETE_FOLDOUT_NAME, true);
			if (!_deleteFoldoutStatus) return false;
			GUILayout.Space(5);
			GUILayout.Label(DELETE_LABEL, EditorStyles.boldLabel);

			if (!GUILayout.Button("Delete"))
				return false;

			Delete();
			return true;
		}
		private void Delete() {
			var parent = _node.parentNode;
			parent.nodes.RemoveListRecord(_node);
			DestroyImmediate(_node.gameObject);
			parent.Redraw();
		}
	}
}