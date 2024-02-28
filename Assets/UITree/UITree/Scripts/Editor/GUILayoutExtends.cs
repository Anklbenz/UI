using UnityEditor;
using UnityEngine;

namespace UITree {
	public static class GUIUtils {
		public static void DrawLine(int height = 1) {
			Rect rect = EditorGUILayout.GetControlRect(false, height);
			rect.height = height;
			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
		}
	}
}
