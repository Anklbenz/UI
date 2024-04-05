using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIRecycleTreeNamespace {
	public class CheckboxControl : Selectable, IPointerClickHandler {
		public event Action ClickedEvent;

		[SerializeField] private Image targetImage;
		private Sprite _checkedSprite, _uncheckedSprite;
		private Color _checkedColor, _uncheckedColor;
		private RectTransform _rectTransform;

		public bool isChecked {
			set {
				targetImage.sprite = value ? _checkedSprite : _uncheckedSprite;
				targetImage.color = value ? _checkedColor : _uncheckedColor;
			}
		}

		public CheckboxStyle style {
			set {
				_uncheckedSprite = value.uncheckedSprite;
				_checkedSprite = value.checkedSprite;

				_uncheckedColor = value.uncheckedColor;
				_checkedColor = value.checkedColor;
			}
		}

		public float width {
			set => _rectTransform.sizeDelta = new Vector2(value, _rectTransform.sizeDelta.y);
		}

		public Vector2 iconSize {
			get => targetImage.rectTransform.sizeDelta;
			set => targetImage.rectTransform.sizeDelta = value;
		}

		public bool isActive {
			get => gameObject.activeInHierarchy;
			set {
				if (gameObject.activeInHierarchy == value) return;
				gameObject.SetActive(value);
			}
		}
		
		public void OnPointerClick(PointerEventData eventData) =>
				ClickedEvent?.Invoke();

		protected override void Awake() {
			base.Awake();
			_rectTransform = (RectTransform)transform;
		}
	}
}