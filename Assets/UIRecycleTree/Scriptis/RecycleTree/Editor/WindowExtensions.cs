using UnityEditor;
using UnityEngine;

namespace UIRecycleTreeNamespace {
	public static class WindowExtensions {
		public static void CenterOnMainWin(this EditorWindow window) {
			Rect main = EditorGUIUtility.GetMainWindowPosition();
			Rect pos = window.position;
			float centerWidth = (main.width - pos.width) * 0.5f;
			float centerHeight = (main.height - pos.height) * 0.5f;
			pos.x = main.x + centerWidth;
			pos.y = main.y + centerHeight;
			window.position = pos;
		}
	}
}
