
using UnityEngine;

namespace UITree {
	public class Node : NodeRoot {
		[SerializeField] private StateControl expandControl, imageControl;
		[SerializeField] private StateImageSettings expandControlSettings, imageControlSettings;
		[SerializeField] private string text;
		[SerializeField] private bool expanded, imageControlEnabled;

		public SelectionControl selectionControl;

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
				base.childrenParentTransform.gameObject.SetActive(value);
				expanded = value;
			}
		}

		public bool isSelected {
			get => _isSelected;
			set {
				_isSelected = value;
				selectionControl.isSelected = value;
			}
		}

		public string nodeName {
			get => text;
			set {
				text = value;
				selectionControl.text = text;
			}
		}
		private bool hasChildren => nodeCollection is {Count: > 0};

		private bool _isSelected;

		public void ExpandAll() {
			foreach (var node in nodeCollection)
				node.ExpandAll();
			isExpanded = true;
		}

		private void OnExpandControlClick() {
			if (hasChildren)
				isExpanded = !isExpanded;
			Redraw();
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

		public void Destroy() =>
				Destroy(gameObject);

		protected override void Awake() {
			isExpanded = expanded;
		}

		//private void GetFullPath(StringBuilder path, string pathSeparator)

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