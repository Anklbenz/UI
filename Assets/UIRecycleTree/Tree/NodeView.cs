using System;
using UnityEngine;

namespace UIRecycleTree {
	public class NodeView : MonoBehaviour, IRecycleItem {
		public event Action ClickedEvent, ExpandClickEvent;

		[SerializeField] private StateControl expandControl, imageControl;
		[SerializeField] private SelectionControl selectionControl;

		public NodeData data {
			get => _data;
			set {
				_data = value;
				Refresh();
			}
		}

		private NodeData _data;

		private void Refresh() {
			selectionControl.text = _data.text;

			if (!_data.hasChildren) {
				expandControl.state = CheckboxState.Empty;
				imageControl.state = CheckboxState.Empty;
				return;
			}

			expandControl.state = _data.expanded ? CheckboxState.Expanded : CheckboxState.Collapsed;
			imageControl.state = _data.expanded ? CheckboxState.Expanded : CheckboxState.Collapsed;
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