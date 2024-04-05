using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace {
	public class NodeView : RecycleItem, IPointerClickHandler {
		private const float DOUBLE_TAP_MAX_DELAY = 0.4f;
		private const float DOUBLE_TAP_MIN_DELAY = 0.07f;

		public event Action ClickedEvent, DoubleClickedEvent, CheckboxClickedEvent, ExpandClickEvent;
		public event Action<float> WidthChangedEvent;

		[SerializeField] private IndentBox indentBox;
		[SerializeField] private StateControl expandToggleControl, iconControl;
		[SerializeField] private TextControl textControl;
		[SerializeField] private CheckboxControl checkboxControl;
		[SerializeField] private RectTransform content;
		[SerializeField] private Image fullRectSelectionImage, contentSelectionImage;
		[SerializeField] private CanvasGroup itemCanvasGroup;
		[SerializeField] private CanvasGroup contentCanvasGroup;
		public override RectTransform rectTransform => (RectTransform)transform;
		public NodePrefs nodePrefs {
			set {
				_fullRectSelect = value.fullRectSelect;

				_spacing = value.spacing;
				_contentRightOffset = value.contentRightOffset;
				_contentLeftOffset = value.contentLeftOffset;
				_childIndentPixels = value.childIndent;

				expandToggleControl.width = value.toggleWidth;
				expandToggleControl.iconSize = value.toggleIconSize;

				checkboxControl.isActive = value.checkboxEnabled;
				checkboxControl.width = value.checkedWidth;
				checkboxControl.iconSize = value.checkedIconSize;

				iconControl.isActive = value.iconEnabled;
				iconControl.width = value.iconWidth;
				iconControl.iconSize = value.iconSize;

				textControl.font = value.font;
				textControl.fontSize = value.fontSize;
			}
		}

		public NodeStyle style {
			set {
				var nodeSprite = value.nodeSprite;
				if (nodeSprite != null) {
					imageForSelect.sprite = value.nodeSprite;
					imageForSelect.type = value.imageType;
					imageForSelect.pixelsPerUnitMultiplier = value.pixelPerUnitMultiplier;
				}
				color = value.nodeColor;

				expandToggleControl.style = value.expandToggleStyle;
				iconControl.style = value.iconStyle;
				checkboxControl.style = value.checkboxStyle;
				textControl.style = value.textStyle;

				_selectionColor = value.selectionColor;
				_fadedAlpha = value.fadeStateAlpha;
			}
		}

		public float indent {
			set => indentBox.indent = value * _childIndentPixels;
		}
		public string text {
			set => textControl.text = value;
		}

		public ExpandedState state {
			set {
				expandToggleControl.state = value;
				iconControl.state = value;
				textControl.state = value;
			}
		}
		public bool isChecked {
			set => checkboxControl.isChecked = value;
		}
		public bool isFaded {
			set => contentCanvasGroup.alpha = value ? _fadedAlpha : 1;
		}
		public bool isSelected {
			set {
				_isSelected = value;
				imageForSelect.color = value ? _selectionColor : _nodeColor;
			}
		}
		private bool isDoubleClick {
			get {
				var currentTapTime = Time.realtimeSinceStartup;
				var tapTimeDelta = currentTapTime - _lastClickTime;
				if (tapTimeDelta is < DOUBLE_TAP_MAX_DELAY and > DOUBLE_TAP_MIN_DELAY)
					return true;
				_lastClickTime = currentTapTime;
				return false;
			}
		}

		private Color color {
			set {
				_nodeColor = value;
				imageForSelect.color = value;
			}
		}

		private Image imageForSelect => _fullRectSelect ? fullRectSelectionImage : contentSelectionImage;
		private Color _nodeColor, _selectionColor, _noChildTextColor, _collapsedTextColor, _expandedTextColor;
		private FontStyles _noChildStyle, _collapsedStyle, _expandedStyle;
		private float _childIndentPixels, _spacing, _contentLeftOffset, _contentRightOffset, _lastClickTime, _fadedAlpha;
		private bool _isSelected, _fullRectSelect, _initialized;

		//We need arrange content manually, without standard components, for better performance
		private IEnumerator ArrangeContent() {
			yield return null;
			//ArrangeContent
			float trackPosition = _contentLeftOffset;

			foreach (RectTransform child in content) {
				if (!child.gameObject.activeInHierarchy)
					continue;
				child.anchoredPosition = new Vector2(trackPosition, child.anchoredPosition.y);
				trackPosition += child.sizeDelta.x + _spacing;
			}
			trackPosition += _contentRightOffset;
			content.sizeDelta = new Vector2(trackPosition, content.sizeDelta.y);

			//Arrange Content and indent  

			trackPosition = 0;
			foreach (RectTransform child in rectTransform) {
				child.anchoredPosition = new Vector2(trackPosition, child.anchoredPosition.y);
				trackPosition += child.sizeDelta.x + _spacing;
			}
			rectTransform.sizeDelta = new Vector2(trackPosition, rectTransform.sizeDelta.y);
			WidthChangedEvent?.Invoke(trackPosition);

			//When first created, the size of the content elements is unknown, so an flicker appears, 
			if (!_initialized)
				SetVisible(true);
			_initialized = true;
		}

		public void ClearPreviousSubscribes() {
			ClickedEvent = null;
			DoubleClickedEvent = null;
			ExpandClickEvent = null;
			CheckboxClickedEvent = null;
		}

		private void ClickNotify() =>
				ClickedEvent?.Invoke();

		private void DoubleClickNotify() =>
				DoubleClickedEvent?.Invoke();

		private void OnExpandToggleClick() =>
				ExpandClickEvent?.Invoke();

		protected void OnEnable() {
			expandToggleControl.ClickedEvent += OnExpandToggleClick;
			checkboxControl.ClickedEvent += OnCheckedClick;
		}

		protected void OnDisable() {
			expandToggleControl.ClickedEvent -= OnExpandToggleClick;
			checkboxControl.ClickedEvent -= OnCheckedClick;
		}
		private void OnCheckedClick() =>
				CheckboxClickedEvent?.Invoke();

		private void OnDestroy() =>
				ClearPreviousSubscribes();

		public void Refresh() =>
				StartCoroutine(ArrangeContent());

		public void OnPointerClick(PointerEventData eventData) {
			ClickNotify();
			if (isDoubleClick)
				DoubleClickNotify();
		}

		private void Awake() =>
				SetVisible(false);

		private void SetVisible(bool isVisible) =>
				itemCanvasGroup.alpha = isVisible ? 1 : 0;
	}
}