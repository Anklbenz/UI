using TMPro;
using UnityEngine;

namespace UIRecycleTreeNamespace {
	[CreateAssetMenu(menuName = "Node/NodePrefs", fileName = "NodePrefs", order = 0)]
	public class NodePrefs : ScriptableObject {
		[SerializeField] private bool _fullRectSelect;
		
		[Header("Spacing preferences")]
		[Space(5)]
		[SerializeField] private float _childIndent = 55;
		[SerializeField] private float _spacing = 1;
		[SerializeField] private float _contentLeftOffset = 10;
		[SerializeField] private float _contentRightOffset = 10;

		[Header("Expand Toggle preferences")]
		[Space(5)]
		[SerializeField] private float _toggleWidth = 60;
		[SerializeField] private Vector2 _toggleIconSize = new(40, 40);

		[Header("Icon preferences")]
		[Space(5)]
		[SerializeField] private bool _iconEnabled = true;
		[SerializeField] private float _iconWidth = 60;
		[SerializeField] private Vector2 _iconSize = new(40, 40);

		[Header("Checkbox preferences")]
		[Space(5)]
		[SerializeField] private bool _checkboxEnabled;
		[SerializeField] private bool _recursiveChecked;
		[SerializeField] private float _checkedWidth = 60;
		[SerializeField] private Vector2 _checkedIconSize = new(40, 40);

		[Header("Font preferences")]
		[Space(5)]
		[SerializeField] private TMP_FontAsset _font;
		[SerializeField] private float _fontSize = 35;

		public bool fullRectSelect => _fullRectSelect;
		public float childIndent => _childIndent;
		public float spacing => _spacing;
		public float contentRightOffset => _contentRightOffset;
		public float contentLeftOffset => _contentLeftOffset;
		public float toggleWidth => _toggleWidth;
		public Vector2 toggleIconSize => _toggleIconSize;
		public bool iconEnabled => _iconEnabled;
		public float iconWidth => _iconWidth;
		public Vector2 iconSize => _iconSize;
		public bool checkboxEnabled => _checkboxEnabled;
		public bool recursiveChecked => _recursiveChecked;
		public float checkedWidth => _checkedWidth;
		public Vector2 checkedIconSize => _checkedIconSize;
		public TMP_FontAsset font => _font;
		public float fontSize => _fontSize;
	}
}