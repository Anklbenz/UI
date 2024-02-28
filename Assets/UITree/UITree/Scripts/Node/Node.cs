using UnityEngine;

namespace UITree {
	public class Node : NodeRoot {
		[SerializeField] private StateControl expandControl, imageControl;
		[SerializeField] private StateImageSettings expandControlSettings, imageControlSettings;
		[SerializeField] private SelectionControl selectionControl;
		[SerializeField] private bool expanded, imageControlEnabled;

		public bool imageEnabled {
			get => imageControl.isActive;
			set {
				imageControl.isActive = value;
				imageControlEnabled = value;
			}
		}
		public StateImageSettings imageSettings {
			set => imageControl.settings = value;
		}

		public StateImageSettings expandSettings {
			set {
				expandControl.settings = value;
				expandControlSettings = value;
			}
		}
		public bool isExpanded {
			get => expanded;
			set {
				base.childrenParentTransform.gameObject.SetActive(hasChildren && value);
				expanded = value;
				Redraw();
			}
		}

		public bool isSelected {
			get => _isSelected;
			set {
				_isSelected = value;
				selectionControl.isSelected = value;
			}
		}

		public override string nodeName {
			get => base.nodeName;
			set {
				base.nodeName = value;
				selectionControl.text = text;
			}
		}

		private bool hasChildren => nodeCollection is {Count: > 0};
		private bool _isSelected;

		public override void ExpandAll() {
			isExpanded = true;
			base.ExpandAll();
		}

		public override void CollapseAll() {
			isExpanded = false;
			base.CollapseAll();
		}

		private void OnExpandControlClick() {
			if (hasChildren)
				isExpanded = !isExpanded;
		}

		public override void Redraw() {
			if (!hasChildren) {
				expandControl.state = CheckboxState.Empty;
				imageControl.state = CheckboxState.Empty;
				return;
			}

			expandControl.state = isExpanded ? CheckboxState.Expanded : CheckboxState.Collapsed;
			imageControl.state = isExpanded ? CheckboxState.Expanded : CheckboxState.Collapsed;
		}

		protected override void OnEnable() {
			base.OnEnable();
			nodes.ChangedEvent += Redraw;
			expandControl.ClickedEvent += OnExpandControlClick;
			selectionControl.ClickEvent += OnClick;

			Redraw();
		}
		private void OnClick() =>
				tree.OnNodeSelect(this);

		protected override void OnDisable() {
			nodes.ChangedEvent -= Redraw;
			expandControl.ClickedEvent -= OnExpandControlClick;
			selectionControl.ClickEvent -= OnClick;
		}

		public void Destroy() {
			parent.nodes.RemoveListRecord(this);
			Destroy(gameObject);
			parent.Redraw();
		}

		protected override void Awake() =>
				isExpanded = expanded;

#if UNITY_EDITOR
		protected override void OnValidate() {
			selectionControl.text = text;
			isExpanded = expanded;
			imageEnabled = imageControlEnabled;
			imageSettings = imageControlSettings;
			expandSettings = expandControlSettings;

			if (expandControl == null || imageControl == null)
				return;
			Redraw();
		}
  #endif
	}
}