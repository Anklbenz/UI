using UnityEditor;

namespace AcceptSlider {
	[CustomEditor(typeof(UIAcceptSlider))]
	public class UINullableToggleEditor : UnityEditor.UI.SliderEditor {
		private const string RESET_IF_REACHED_FIELD_NAME = "Reset If Reached";
		private SerializedProperty _onAccept, _onReject, _resetIfReached;
		private UIAcceptSlider _uiAcceptSlider;

		protected override void OnEnable() {
			base.OnEnable();
			_onAccept = serializedObject.FindProperty("onAccept");
			_onReject = serializedObject.FindProperty("onReject");
			_resetIfReached = serializedObject.FindProperty("_resetIfReached");
			_uiAcceptSlider = serializedObject.targetObject as UIAcceptSlider;
		}

		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();
			var resetIfReachValue = EditorGUILayout.Toggle(RESET_IF_REACHED_FIELD_NAME, _resetIfReached.boolValue);
			_resetIfReached.boolValue = resetIfReachValue;
			serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(_uiAcceptSlider);

			base.OnInspectorGUI();
			EditorGUILayout.PropertyField(_onAccept, true);
			EditorGUILayout.PropertyField(_onReject, true);
		}
	}
}