using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionControl : Selectable, IPointerClickHandler {
	public event Action ClickEvent;

	[SerializeField] private TMP_Text textField;
	[SerializeField] private Image selectionImage;
	[SerializeField] private Color _color = Color.clear;
	[SerializeField] private Color _selectedColor = Color.gray;

	[SerializeField] private Color _textColor = Color.white;

	private bool _isSelected;

	public bool isSelected {
		get => _isSelected;
		set {
			_isSelected = value;
			color = value ? _selectedColor : _color;
		}
	}

	public string text {
		get => textField.text;
		set => textField.text = value;
	}

	public Sprite selectionSprite {
		set => selectionImage.sprite = value;
	}

	public Color color {
		get => selectionImage.color;
		set => selectionImage.color = value;
	}

	public Color textColor {
		get => _textColor;
		set {
			textField.color = value;
			_textColor = value;
		}
	}


	public FontSettings settings {
		set {
			/*expandedSprite = value.expand;
			collapsedSprite = value.collapse;
			emptySprite = value.empty;

			expandedColor = value.expandColor;
			collapsedColor = value.collapseColor;
			emptyColor = value.emptyColor;

			targetImage.rectTransform.sizeDelta = value.imageSizeDelta;*/
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		ClickEvent?.Invoke();
	}
}
