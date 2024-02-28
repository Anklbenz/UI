using UnityEngine;
using System.Collections;

namespace UIRecycleTree {

	public class RecycleView : ExtendedScrollRect {
		private const int EXTRA_ITEMS_COUNT = 3;
		private const int RECYCLE_BOUNDS_THRESHOLD_IN_ITEMS = 1;

		[SerializeField] private RectTransform template;
		//Set item height 
		[SerializeField] private float itemHeight;
		public IRecycleDataSource recycleDataSource { get; set; }
		private Vector2 contentPosition {
			get => content.anchoredPosition;
			set => content.anchoredPosition = value;
		}
		private RectTransform firstRectTransform => content.GetChild(0) as RectTransform;
		private RectTransform lastRectTransform => content.GetChild(content.childCount - 1) as RectTransform;
		private int lowestRecyclingIndex => _topmostRecyclingIndex + _visibleItemsPoolSize;
		private float recycleBoundsThreshold => _itemHeight * RECYCLE_BOUNDS_THRESHOLD_IN_ITEMS;
		private float extraContentSize => _itemHeight * EXTRA_ITEMS_COUNT;

		private readonly Vector3[] _corners = new Vector3[4];
		private Bounds _recyclableViewBounds;
		private int _topmostRecyclingIndex, _visibleItemsPoolSize;
		private float _itemHeight;
		private bool _isReloading;

		public IEnumerator Reload() {
			if (recycleDataSource == null) yield break;
			_isReloading = true;
			StopMoving();

			yield return null;
			_itemHeight = template.sizeDelta.y;

			var newVisibleItemsPoolSize = GetRequiredPoolSize();
			var scrollContentSizeY = newVisibleItemsPoolSize * _itemHeight;
			content.sizeDelta = new Vector2(content.sizeDelta.x, scrollContentSizeY);

			if (newVisibleItemsPoolSize > _visibleItemsPoolSize)
				IncreasePool(newVisibleItemsPoolSize);
			else
				DecreasePool(newVisibleItemsPoolSize);

			_visibleItemsPoolSize = newVisibleItemsPoolSize;

			yield return null;
			RedrawData();
			UpdateScrollbars(Vector2.zero);

			_isReloading = false;
		}
		
		private void StopMoving() =>
				velocity = Vector2.zero;

		private void RedrawData() {
			var isVisibleContentSmallerThanPool = recycleDataSource.count - _topmostRecyclingIndex < _visibleItemsPoolSize;
			int currentIndex = isVisibleContentSmallerThanPool ? recycleDataSource.count - _visibleItemsPoolSize : _topmostRecyclingIndex;

			foreach (Transform child in content) {
				GetDataFromSource(child, currentIndex);
				currentIndex++;
			}
		}
		private void GetDataFromSource(Transform item, int index) {
			var item1 = item.GetComponent<IItem>();

			recycleDataSource.GetDataByIndex(item1, index);
		}

		private void IncreasePool(int newPoolSize) {
			for (int i = _visibleItemsPoolSize; i < newPoolSize; i++)
				Instantiate(template, content, true);
		}

		private void DecreasePool(int newPoolSize) {
			for (int i = _visibleItemsPoolSize - 1; i >= newPoolSize; i--)
				Destroy(content.GetChild(i).gameObject);
		}

		private int GetRequiredPoolSize() {
			float requiredContentHeight = extraContentSize + viewport.rect.height;
			var requiredItemsCount = (int)Mathf.Ceil(requiredContentHeight / _itemHeight);
			return Mathf.Min(requiredItemsCount, recycleDataSource.count);
		}

		protected override void OnValueChanged(Vector2 position) {
			if (_isReloading) return;

			SetRecyclingBounds();
			var isPossibleRecycleFromTopToBottom = velocity.y > 0 && lastRectTransform.MaxY() > _recyclableViewBounds.min.y;
			var isPossibleRecycleFromBottomToTop = velocity.y < 0 && firstRectTransform.MinY() < _recyclableViewBounds.max.y;

			if (isPossibleRecycleFromTopToBottom)
				RecycleFromTopToBottom();

			if (isPossibleRecycleFromBottomToTop)
				RecycleFromBottomToTop();
		}

		protected override void UpdateScrollbars(Vector2 offset) {
			if (recycleDataSource == null) return;
			if (verticalScrollbar) {
				verticalScrollbar.size = _visibleItemsPoolSize > 0 ? Mathf.Clamp01((float)_visibleItemsPoolSize / recycleDataSource.count) : 1;

				var invisibleItemsCount = recycleDataSource.count - _visibleItemsPoolSize;
				var visiblePartNormalizedPosition = 1 - ((float)lowestRecyclingIndex /*+ 1*/ - _visibleItemsPoolSize) / invisibleItemsCount;

				verticalScrollbar.SetValueWithoutNotify(visiblePartNormalizedPosition);
			}

			// implement horizontal scroll bar 
		}

		protected override void SetVerticalNormalizedPosition(float value) {
			if (recycleDataSource == null) return;

			value = Mathf.Clamp01(1 - value);
			var invisibleItemsCount = recycleDataSource.count - _visibleItemsPoolSize;
			var newLowestItemIndex = (int)(value * invisibleItemsCount) + _visibleItemsPoolSize - 1;
			var newTopmostItemIndex = newLowestItemIndex - _visibleItemsPoolSize + 1;

			if (_topmostRecyclingIndex != newTopmostItemIndex)
				normalizedPosition = new Vector2(normalizedPosition.x, _topmostRecyclingIndex > newTopmostItemIndex ? 1 : 0);

			_topmostRecyclingIndex = newTopmostItemIndex;
			RedrawData();
		}

		private void RecycleFromTopToBottom() {
			while (firstRectTransform.MinY() > _recyclableViewBounds.max.y && lowestRecyclingIndex < recycleDataSource.count) {
				contentPosition -= new Vector2(0, _itemHeight);
				m_ContentStartPosition -= new Vector2(0, _itemHeight);

				var itemTransform = firstRectTransform;
				itemTransform.SetAsLastSibling();

				GetDataFromSource(itemTransform, lowestRecyclingIndex);
				_topmostRecyclingIndex++;
			}
		}

		private void RecycleFromBottomToTop() {
			while (lastRectTransform.MaxY() < _recyclableViewBounds.min.y && _topmostRecyclingIndex > 0) {
				contentPosition += new Vector2(0, _itemHeight);
				m_ContentStartPosition += new Vector2(0, _itemHeight);

				var itemTransform = lastRectTransform;
				itemTransform.SetAsFirstSibling();

				_topmostRecyclingIndex--;
				GetDataFromSource(itemTransform, _topmostRecyclingIndex);
			}
		}

		private void SetRecyclingBounds() {
			viewport.GetWorldCorners(_corners);
			_recyclableViewBounds.min = new Vector3(_corners[0].x, _corners[0].y - recycleBoundsThreshold);
			_recyclableViewBounds.max = new Vector3(_corners[2].x, _corners[2].y + recycleBoundsThreshold);
		}

		protected override void OnRectTransformDimensionsChange() {
			//if (Application.isPlaying)
			StartCoroutine(Reload());
		}

		public void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(_recyclableViewBounds.min - new Vector3(2000, 0), _recyclableViewBounds.min + new Vector3(2000, 0));
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_recyclableViewBounds.max - new Vector3(2000, 0), _recyclableViewBounds.max + new Vector3(2000, 0));
		}
	}
}