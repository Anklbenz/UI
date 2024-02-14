using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings", fileName = "NodeSettings", order = 0)]
public class NodeSettings : ScriptableObject {
	/*[Header("TreeView")]
	[SerializeField] private bool isBackgroundTransparent;
	[SerializeField] private Color backgroundColor = Color.clear;
	*/

	[Header("Content")]
	[SerializeField] private float nodeHeight = 60;
	[SerializeField] private float expandParentWidth = 60;
	[SerializeField] private float imageParentWidth = 60;
	[SerializeField] private float spacing = 0;
	[SerializeField] private float childrenIndent = 65;
	[SerializeField] private Sprite sprite;
	[SerializeField] private Color contentColor = Color.clear;
	[SerializeField] private Color selectionColor = Color.blue;

	[Header("Images")]
	[SerializeField] private StateImageSettings expandImageSettings;
	[SerializeField] private StateImageSettings contentImageSettings;

	[Header("Text Setting")]
	public TMP_FontAsset font;
	[SerializeField] private FontSettings selected;
	[SerializeField] private FontSettings expanded;
	[SerializeField] private FontSettings collapsed;
	[SerializeField] private FontSettings empty;
}