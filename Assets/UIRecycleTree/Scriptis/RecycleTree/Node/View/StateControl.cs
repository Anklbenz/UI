using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIRecycleTree {
	public class StateControl : Selectable, IPointerClickHandler {
		public event Action ClickedEvent;

		[SerializeField] private Image targetImage;
		private Sprite _expandedSprite, _collapsedSprite, _hasNoChildSprite;
		private Color _expandedColor, _collapsedColor, _hasNoChildColor;
		private RectTransform _rectTransform;

		public ExpandStyle style {
			set {
				_hasNoChildSprite = value.noChildSprite;
				_collapsedSprite = value.collapseSprite;
				_expandedSprite = value.expandSprite;

				_hasNoChildColor = value.noChildColor;
				_collapsedColor = value.collapseColor;
				_expandedColor = value.expandColor;
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
		public ExpandedState state {
			get => _currentState;
			set {
				_currentState = value;
				Refresh();
			}
		}

		private ExpandedState _currentState;

		public void OnPointerClick(PointerEventData eventData) =>
				ClickedEvent?.Invoke();

		private void Refresh() {
			switch (state) {
				case ExpandedState.NoChild:
					targetImage.sprite = _hasNoChildSprite;
					targetImage.color = _hasNoChildColor;
					break;
				case ExpandedState.Expanded:
					targetImage.sprite = _expandedSprite;
					targetImage.color = _expandedColor;
					break;
				case ExpandedState.Collapsed:
					targetImage.sprite = _collapsedSprite;
					targetImage.color = _collapsedColor;
					break;
				default:
					throw new Exception($"State {state} not implemented");
			}
		}

		protected override void Awake() {
			base.Awake();
			_rectTransform = (RectTransform)transform;
		}
	}
}