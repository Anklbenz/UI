using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine.EventSystems;

namespace UIRecycleTree {
	public class RecycleView : ExtendedScrollRect {
		private const int EXTRA_ITEMS_COUNT = 6;
		private const int RECYCLE_BOUNDS_THRESHOLD_IN_ITEMS = 4;
		private const int DEFAULT_ITEM_HEIGHT = 30;

		[SerializeField] private RecycleItem template;
		[SerializeField] private VerticalLayoutGroup contentLayoutGroup;
		[SerializeField] private float itemHeight = DEFAULT_ITEM_HEIGHT;

		public float spacing {
			get => contentLayoutGroup == null ? 0 : contentLayoutGroup.spacing;
			set {
				if (contentLayoutGroup == null) return;
				if (Math.Abs(contentLayoutGroup.spacing - value) <= 0.01f) return;
				contentLayoutGroup.spacing = value;
				StartCoroutine(Reload());
			}
		}
		public RectOffset contentPadding {
			get => contentLayoutGroup.padding;
			set {
				if (contentLayoutGroup.padding == value) return;
				contentLayoutGroup.padding = value;
				StartCoroutine(Reload());
			}
		}
		public IRecycleDataSource recycleDataSource { get; set; }

		private Vector2 contentPosition {
			get => content.anchoredPosition;
			set => content.anchoredPosition = value;
		}
		private RectTransform firstRectTransform => _recycleItemsPool[0].rectTransform; 
		private RectTransform lastRectTransform => _recycleItemsPool[^1].rectTransform; 
		private int lowestRecyclingIndex => _topmostRecyclingIndex + _visibleItemsPoolSize;
		
		//for CanvasScaler "Scale with Screen Size mode"
		private float scaledItemHeight => (itemHeight + spacing) * transform.lossyScale.x;
		private float recycleVerticalThreshold => scaledItemHeight * RECYCLE_BOUNDS_THRESHOLD_IN_ITEMS;
		private float extraContentSize => (itemHeight + spacing) * EXTRA_ITEMS_COUNT;
		private float verticalPaddingValue => contentPadding.top + contentPadding.bottom;
		private Vector2 contentOffset => new(0, itemHeight + spacing);

		private readonly Vector3[] _corners = new Vector3[4];
		private ObservableCollection<RecycleItem> _recycleItemsPool = new();
	    private Bounds _recyclableViewBounds;
		private int _topmostRecyclingIndex, _visibleItemsPoolSize, _requiredItemsCountOnScreen;
		private bool _isReloading;

		public IEnumerator Reload() {
		
			if (recycleDataSource == null) yield break;
			_isReloading = true;

			BeforeReload();

			yield return null;
			template.rectTransform.sizeDelta = new Vector2(template.rectTransform.sizeDelta.x, itemHeight);

			_requiredItemsCountOnScreen = GetRequiredItemsCountOnScreen();

			var newVisibleItemsCount = Mathf.Min(_requiredItemsCountOnScreen, recycleDataSource.expandedCount);

			var scrollContentSizeY = newVisibleItemsCount * (itemHeight + spacing) - spacing + verticalPaddingValue;
			content.sizeDelta = new Vector2(content.sizeDelta.x, scrollContentSizeY);

			if (newVisibleItemsCount > _visibleItemsPoolSize)
				IncreasePool(newVisibleItemsCount);
			else if (newVisibleItemsCount < _visibleItemsPoolSize)
				DecreasePool(newVisibleItemsCount);

			_visibleItemsPoolSize = newVisibleItemsCount;

			OnPoolReady();
			StopMoving();
			Repaint();

			_isReloading = false;
		}

		public void Repaint() {
			var isVisibleContentSmallerThanPool = recycleDataSource.expandedCount - _topmostRecyclingIndex < _visibleItemsPoolSize;
			int currentIndex = isVisibleContentSmallerThanPool ? recycleDataSource.expandedCount - _visibleItemsPoolSize : _topmostRecyclingIndex;

			foreach (RecycleItem child in _recycleItemsPool) {
				GetDataFromSource(child, currentIndex);
				currentIndex++;
			}
		}

		private void GetDataFromSource(RecycleItem item, int index) =>
				recycleDataSource.MergeDataWithItem(item, index);

		private void IncreasePool(int newPoolSize) {
			for (int i = _visibleItemsPoolSize; i < newPoolSize; i++) {
				var item = CreateItem();
				_recycleItemsPool.Add(item);
				OnPoolIncrease(item);
			}
		}
		protected virtual RecycleItem CreateItem() =>
				Instantiate(template, content, true);

		private void DecreasePool(int newPoolSize) {
			for (int i = _visibleItemsPoolSize - 1; i >= newPoolSize; i--) {
				var item = _recycleItemsPool[i];
				BeforePoolDecrease(item);
				Destroy(item.gameObject);
				_recycleItemsPool.RemoveAt(i);
			}
		}
		protected virtual void OnPoolIncrease(RecycleItem item) {}
		protected virtual void BeforePoolDecrease(RecycleItem item) {}
		protected virtual void BeforeReload() {}
		protected virtual void OnPoolReady() {
			var itemsCountLessThanRequiredOnScreen = _visibleItemsPoolSize < _requiredItemsCountOnScreen;

			if (itemsCountLessThanRequiredOnScreen)
				_topmostRecyclingIndex = 0;
		}

		private int GetRequiredItemsCountOnScreen() {
			float requiredContentHeightOnScreen = extraContentSize + viewport.rect.height;
			var requiredItemsCount = (int)Mathf.Ceil((requiredContentHeightOnScreen) / (itemHeight + spacing));
			return requiredItemsCount;
		}

		protected override void OnValueChanged(Vector2 position) {
			if(!Application.isPlaying) return;
			if (_isReloading) return;

			SetRecyclingBounds();
			var isPossibleRecycleFromTopToBottom = velocity.y > 0 && lastRectTransform.MaxY() > _recyclableViewBounds.min.y;
			var isPossibleRecycleFromBottomToTop = velocity.y < 0 && firstRectTransform.MinY() < _recyclableViewBounds.max.y;

			if (isPossibleRecycleFromTopToBottom)
				RecycleFromTopToBottom();

			if (isPossibleRecycleFromBottomToTop)
				RecycleFromBottomToTop();
		}

		public override void OnScroll(PointerEventData data) {
			if (_isReloading) return;
			StopMoving();

			SetRecyclingBounds();
			contentPosition -= data.scrollDelta * scrollSensitivity;
			var isPossibleRecycleFromTopToBottom = lastRectTransform.MaxY() > _recyclableViewBounds.min.y;
			var isPossibleRecycleFromBottomToTop = firstRectTransform.MinY() < _recyclableViewBounds.max.y;

			if (isPossibleRecycleFromTopToBottom)
				RecycleFromTopToBottom();

			if (isPossibleRecycleFromBottomToTop)
				RecycleFromBottomToTop();
		}

		private void RecycleFromTopToBottom() {
			while (firstRectTransform.MinY() > _recyclableViewBounds.max.y && lowestRecyclingIndex < recycleDataSource.expandedCount) {
				contentPosition -= contentOffset;
				m_ContentStartPosition -= contentOffset;

				var itemTransform = firstRectTransform;

				itemTransform.SetAsLastSibling();
				_recycleItemsPool.Move(0, _recycleItemsPool.Count - 1);

				GetDataFromSource(_recycleItemsPool[^1], lowestRecyclingIndex);
				_topmostRecyclingIndex++;
			}
		}

		private void RecycleFromBottomToTop() {
			while (lastRectTransform.MaxY() < _recyclableViewBounds.min.y && _topmostRecyclingIndex > 0) {
				contentPosition += contentOffset;
				m_ContentStartPosition += contentOffset;

				var itemTransform = lastRectTransform;
				itemTransform.SetAsFirstSibling();
				_recycleItemsPool.Move(_recycleItemsPool.Count - 1, 0);

				_topmostRecyclingIndex--;
				GetDataFromSource(_recycleItemsPool[0], _topmostRecyclingIndex);
			}
		}
		protected override void UpdateVerticalScrollbar(Vector2 offset) {
			if (recycleDataSource == null) return;
			if (verticalScrollbar) {
				verticalScrollbar.size = _visibleItemsPoolSize > 0 ? Mathf.Clamp01((float)(_visibleItemsPoolSize) / recycleDataSource.expandedCount) : 1;

				var invisibleItemsCount = recycleDataSource.expandedCount - _visibleItemsPoolSize;

				var visiblePartNormalizedPosition = invisibleItemsCount != 0 ? 1 - ((float)lowestRecyclingIndex - _visibleItemsPoolSize) / invisibleItemsCount : 0;

				verticalScrollbar.SetValueWithoutNotify(visiblePartNormalizedPosition);
			}
		}

		protected override void SetVerticalNormalizedPosition(float value) {
			if (recycleDataSource == null) return;

			value = Mathf.Clamp01(1 - value);
			var invisibleItemsCount = recycleDataSource.expandedCount - _visibleItemsPoolSize;
			var newLowestItemIndex = (int)(value * invisibleItemsCount) + _visibleItemsPoolSize - 1;
			var newTopmostItemIndex = newLowestItemIndex - _visibleItemsPoolSize + 1;

			if (_topmostRecyclingIndex != newTopmostItemIndex)
				normalizedPosition = new Vector2(normalizedPosition.x, _topmostRecyclingIndex > newTopmostItemIndex ? 1 : 0);

			_topmostRecyclingIndex = newTopmostItemIndex;
			Repaint();
		}

		private void SetRecyclingBounds() {
			viewport.GetWorldCorners(_corners);
			_recyclableViewBounds.min = new Vector3(_corners[0].x, _corners[0].y - recycleVerticalThreshold);
			_recyclableViewBounds.max = new Vector3(_corners[2].x, _corners[2].y + recycleVerticalThreshold);
		}

		protected override void OnRectTransformDimensionsChange() {
			if (gameObject.activeInHierarchy)
				StartCoroutine(Reload());
		}
		private void StopMoving() =>
				velocity = Vector2.zero;
		public void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(_recyclableViewBounds.min - new Vector3(2000, 0), _recyclableViewBounds.min + new Vector3(2000, 0));
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_recyclableViewBounds.max - new Vector3(2000, 0), _recyclableViewBounds.max + new Vector3(2000, 0));
		}
	}
}