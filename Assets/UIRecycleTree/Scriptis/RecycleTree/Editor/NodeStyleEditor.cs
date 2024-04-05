using UnityEditor;

namespace UIRecycleTreeNamespace {



	[CustomEditor(typeof(NodeStyle))]
	public class NodeStyleEditor : Editor {
		private string[] _options = new string[2] {"Simple", "Sliced"};

		private SerializedProperty _nodeSprite, _nodeColor, _imageType, _pixelPerUnitMultiplier, _selectionColor, _fadedAlpha;
		private SerializedProperty _expandToggleStyle, _iconStyle, _checkboxStyle, _textStyle;

		protected void OnEnable() {
			_nodeSprite = serializedObject.FindProperty("_nodeSprite");
			_nodeColor = serializedObject.FindProperty("_nodeColor");
			_imageType = serializedObject.FindProperty("_imageType");
			_pixelPerUnitMultiplier = serializedObject.FindProperty("_pixelPerUnitMultiplier");
			_selectionColor = serializedObject.FindProperty("_selectionColor");
			_fadedAlpha = serializedObject.FindProperty("_fadedAlpha");

			_expandToggleStyle = serializedObject.FindProperty("_expandToggleStyle");
			_iconStyle = serializedObject.FindProperty("_iconStyle");
			_checkboxStyle = serializedObject.FindProperty("_checkboxStyle");
			_textStyle = serializedObject.FindProperty("_textStyle");
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.LabelField("Node main", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_nodeSprite);
			EditorGUILayout.PropertyField(_nodeColor);

			if (_nodeSprite.objectReferenceValue != null) {
				_imageType.enumValueIndex = EditorGUILayout.Popup("ImageType", _imageType.enumValueIndex, _options);

				if (_imageType.enumValueIndex == 1) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(_pixelPerUnitMultiplier);
					EditorGUI.indentLevel--;
				}
			}

			EditorGUILayout.Space(5);
			EditorGUILayout.PropertyField(_selectionColor);
			EditorGUILayout.PropertyField(_fadedAlpha);
			EditorGUILayout.Space(5);


			EditorGUILayout.LabelField("Node elements styles", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_expandToggleStyle, true);
			EditorGUILayout.PropertyField(_iconStyle, true);
			EditorGUILayout.PropertyField(_checkboxStyle, true);
			EditorGUILayout.PropertyField(_textStyle, true);

			serializedObject.ApplyModifiedProperties();
		}
	}
}