using UnityEngine;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace {
	[CreateAssetMenu(menuName = "Node/NodeStyle", fileName = "NodeStyle", order = 0)]
	public class NodeStyle : ScriptableObject {
		[SerializeField] private ExpandStyle _expandToggleStyle;
		[SerializeField] private ExpandStyle _iconStyle;
		[SerializeField] private CheckboxStyle _checkboxStyle;
		[SerializeField] private FontStyle _textStyle;
		[SerializeField] private Sprite _nodeSprite;
		[SerializeField] private Image.Type _imageType;
		[SerializeField] private Color _nodeColor = Color.clear;
		[SerializeField] private Color _selectionColor = Color.gray;
		[SerializeField] private float _fadedAlpha = 0.3f;
		[SerializeField] private float _pixelPerUnitMultiplier = 1;
		public ExpandStyle expandToggleStyle => _expandToggleStyle;
		public ExpandStyle iconStyle => _iconStyle;
		public CheckboxStyle checkboxStyle => _checkboxStyle;
		public FontStyle textStyle => _textStyle;
		public Color selectionColor => _selectionColor;
		public Color nodeColor => _nodeColor;
		public Sprite nodeSprite => _nodeSprite;
		public float fadeStateAlpha => _fadedAlpha;
		public Image.Type imageType => _imageType;
		public float pixelPerUnitMultiplier => _pixelPerUnitMultiplier;
	}
}