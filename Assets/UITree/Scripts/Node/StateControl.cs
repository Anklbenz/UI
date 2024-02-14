using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CheckboxState {
	Expanded,
	Collapsed,
	Empty
}
public class StateControl : Selectable, IPointerClickHandler {
	public event Action ClickedEvent;

	[SerializeField] private Image targetImage;
	[SerializeField] private Sprite expandedSprite, collapsedSprite, emptySprite;
	[SerializeField] private Color expandedColor = Color.white, collapsedColor = Color.white, emptyColor = Color.white;

	public StateImageSettings settings {
		set {
			expandedSprite = value.expand;
			collapsedSprite = value.collapse;
			emptySprite = value.empty;

			expandedColor = value.expandColor;
			collapsedColor = value.collapseColor;
			emptyColor = value.emptyColor;

			targetImage.rectTransform.sizeDelta = value.imageSizeDelta;
		}
	}

	public bool isActive {
		get => gameObject.activeInHierarchy;
		set => gameObject.SetActive(value);
	}
	
	public CheckboxState state {
		get => _currentState;
		set {
			_currentState = value;
			UpdateSprite();
		}
	}

	private CheckboxState _currentState;

	public void OnPointerClick(PointerEventData eventData) =>
			ClickedEvent?.Invoke();

	private void UpdateSprite() {
		switch (state) {
			case CheckboxState.Empty:
				targetImage.sprite = emptySprite;
				targetImage.color = emptyColor;
				break;
			case CheckboxState.Expanded:
				targetImage.sprite = expandedSprite;
				targetImage.color = expandedColor;
				break;
			case CheckboxState.Collapsed:
				targetImage.sprite = collapsedSprite;
				targetImage.color = collapsedColor;
				break;
			default:
				throw new Exception($"State {state} not implemented");
		}
	}
}