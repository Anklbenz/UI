using System;
using TMPro;
using UIRecycleTree;
using UnityEngine;

public class TextControl : MonoBehaviour {
	[SerializeField] private TMP_Text textField;

	private Color _noChildColor, _collapsedColor, _expandedColor;
	private FontStyles _noChildStyle, _collapsedStyle, _expandedStyle;
	private ExpandedState _state;

	public FontStyle style {
		set {
			_noChildStyle = value.noChildStyle;
			_collapsedStyle = value.collapsedStyle;
			_expandedStyle = value.expandedStyle;

			_noChildColor = value.noChildColor;
			_collapsedColor = value.collapsedColor;
			_expandedColor = value.expandedColor;
		}
	}

	public ExpandedState state {
		get => _state;
		set {
			_state = value;
			Refresh();
		}
	}

	public string text {
		get => textField.text;
		set => textField.text = value;
	}
	public TMP_FontAsset font {
		set => textField.font = value;
	}
	public float fontSize {
		get => textField.fontSize;
		set => textField.fontSize = value;
	}

	private void Refresh() {
		switch (state) {
			case ExpandedState.NoChild:
				textField.fontStyle = _noChildStyle;
				textField.color = _noChildColor;
				break;
			case ExpandedState.Expanded:
				textField.fontStyle = _expandedStyle;
				textField.color = _expandedColor;
				break;
			case ExpandedState.Collapsed:
				textField.fontStyle = _collapsedStyle;
				textField.color = _collapsedColor;
				break;
			default:
				throw new Exception($"State {state} not implemented");
		}
	}
}