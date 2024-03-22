using UnityEditor;
using UnityEngine;

namespace UIRecycleTree {
	[CustomEditor(typeof(UIRecycleTree))]
	public class UIRecycleTreeEditor : ExtendedScrollRectEditor {
		private SerializedProperty _nodePrefs;
		private SerializedProperty _itemHeight;
		private SerializedProperty _layoutGroup;
		private SerializedProperty _nodeStyle;
		private SerializedProperty _onNodeSelected, _onNodeDeselected, _onNodeChecked, _onNodeDblClick, _onExpandStateChanged, _onSelectionChanged;

		private UIRecycleTree _tree;
		private int _left, _right, _bottom, _top;
		private bool _paddingFoldout;

		protected override void OnEnable() {
			base.OnEnable();
			_tree = (UIRecycleTree)target;
			_nodePrefs = serializedObject.FindProperty("nodePrefs");
			_itemHeight = serializedObject.FindProperty("itemHeight");
			_layoutGroup = serializedObject.FindProperty("contentLayoutGroup");
			_nodeStyle = serializedObject.FindProperty("nodeStylesArray");

			_onNodeSelected = serializedObject.FindProperty("onNodeSelected");
			_onNodeDeselected = serializedObject.FindProperty("onNodeDeselected");
			_onNodeChecked = serializedObject.FindProperty("onNodeCheckedChanged");
			_onNodeDblClick = serializedObject.FindProperty("onNodeDblClick");
			_onExpandStateChanged = serializedObject.FindProperty("onExpandStateChanged");
			_onSelectionChanged = serializedObject.FindProperty("onSelectionChanged");
		}

		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.LabelField("Node preferences", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_nodePrefs);
			EditorGUILayout.PropertyField(_nodeStyle);

			EditorGUILayout.Space(5);

			if (GUILayout.Button("Open Tree Constructor"))
				TreeEditorWindow.Open(_tree);

			EditorGUILayout.Space(5);

			EditorGUILayout.PropertyField(_itemHeight);

			EditorGUILayout.Space(10);

			SetPadding();
			SetSpacing();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Scroll rect", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(_layoutGroup);
			serializedObject.ApplyModifiedProperties();

			base.OnInspectorGUI();

			EditorGUILayout.PropertyField(_onNodeSelected);
			EditorGUILayout.PropertyField(_onNodeDeselected);
			EditorGUILayout.PropertyField(_onNodeChecked);
			EditorGUILayout.PropertyField(_onNodeDblClick);
			EditorGUILayout.PropertyField(_onExpandStateChanged);
			EditorGUILayout.PropertyField(_onSelectionChanged);
			serializedObject.ApplyModifiedProperties();
			if(EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(_tree);
		}

		private void SetSpacing() =>
				_tree.spacing = EditorGUILayout.FloatField("Spacing", _tree.spacing);

		private void SetPadding() {
			_paddingFoldout = EditorGUILayout.Foldout(_paddingFoldout, "Padding", true);
			if (!_paddingFoldout) return;

			EditorGUI.indentLevel++;
			_left = EditorGUILayout.IntField("Left", _tree.contentPadding.left);
			_right = EditorGUILayout.IntField("Right", _tree.contentPadding.right);
			_top = EditorGUILayout.IntField("Top", _tree.contentPadding.top);
			_bottom = EditorGUILayout.IntField("Bottom", _tree.contentPadding.bottom);
			EditorGUI.indentLevel--;

			_tree.contentPadding = new RectOffset(_left, _right, _top, _bottom);
		}
	}
}