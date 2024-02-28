using UnityEditor;
using UnityEditor.UI;

namespace UIRecycleTree {
	[CustomEditor(typeof(RecycleView))]
	public class RecycleViewEditor : ExtendedScrollRectEditor {
		private SerializedProperty _template;
		
		protected override void OnEnable() {
			base.OnEnable();
			_template = serializedObject.FindProperty("template");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			EditorGUILayout.PropertyField(_template);
			serializedObject.ApplyModifiedProperties();
		}
	}
}