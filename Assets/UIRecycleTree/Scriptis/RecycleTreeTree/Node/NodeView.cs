using System;
using UnityEngine;

namespace UIRecycleTree {

	public class NodeView : MonoBehaviour, IRecycleItem, INodeView {
		public event Action ClickedEvent, ExpandClickEvent;
		[SerializeField] private IndentBox indentBox;
		[SerializeField] private StateControl expandControl, imageControl;
		[SerializeField] private SelectionControl selectionControl;

		public float indent {
			set => indentBox.indent = value;
		}
		
		public string text {
			get => selectionControl.text;
			set => selectionControl.text = value;
		}

		public CheckboxState state {
			set {
				expandControl.state = value;
				imageControl.state = value;
			}
		}

		public void UnregisterEvents() {
			ClickedEvent = null;
			ExpandClickEvent = null;
		}

		public bool isSelected {
			get => selectionControl.isSelected;
			set => selectionControl.isSelected = value;
		}

		private void ClickNotify() =>
				ClickedEvent?.Invoke();

		private void OnExpandClick() =>
				ExpandClickEvent?.Invoke();

		protected void OnEnable() {
			expandControl.ClickedEvent += OnExpandClick;
			selectionControl.ClickEvent += ClickNotify;
		}

		protected void OnDisable() {
			expandControl.ClickedEvent -= OnExpandClick;
			selectionControl.ClickEvent -= ClickNotify;
		}
	}
}