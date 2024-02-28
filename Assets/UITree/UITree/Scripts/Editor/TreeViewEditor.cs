using UnityEditor;

namespace UITree {
	[CustomEditor(typeof(TreeView))]
	public class TreeViewEditor : RootNodeEditor {
		private const string DEFAULT_SEPARATOR = "/";
		private SerializedProperty _pathSeparator, _isExpandedAsDefault;

		protected override void OnEnable() {
			base.OnEnable();
			_pathSeparator = serializedObject.FindProperty("pathSeparator");
			_isExpandedAsDefault = serializedObject.FindProperty("isExpandedAsDefault");
			
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.PropertyField(_pathSeparator);
			EditorGUILayout.PropertyField(_isExpandedAsDefault);
			base.OnInspectorGUI();
		}
	}
}