using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace AcceptSlider {
	[Serializable]
	public class AcceptedEvent : UnityEvent {
	}
	public class UIAcceptSlider : Slider, IEndDragHandler {
		private const float TOLERANCE = 0.01f;
		public AcceptedEvent onAccept = new AcceptedEvent();
		public AcceptedEvent onReject = new AcceptedEvent();
		public event Action AcceptedEvent, RejectedEvent;
		[SerializeField] private bool _resetIfReached;

		public bool resetIfReached { get => _resetIfReached; set => _resetIfReached = value; }

		private bool isReached => Math.Abs(value - maxValue) < TOLERANCE;
		private bool _canDrag;

		protected override void OnEnable() {
			base.OnEnable();
			value = 0;
		}

		public override void OnPointerDown(PointerEventData eventData) {
			if (handleRect == null || !RectTransformUtility.RectangleContainsScreenPoint(handleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
				return;
			base.OnPointerDown(eventData);
			_canDrag = true;
		}

		public override void OnPointerUp(PointerEventData eventData) {
			base.OnPointerUp(eventData);
			_canDrag = false;
		}

		public override void OnDrag(PointerEventData eventData) {
			if (!_canDrag) return;
			base.OnDrag(eventData);

			if (!isReached) return;
			AcceptNotify();
			_canDrag = false;

			if (resetIfReached)
				ResetValue();
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (value != 0 && !isReached)
				RejectNotify();
			ResetValue();
		}
		private void ResetValue() =>
				value = 0;

		private void AcceptNotify() {
			AcceptedEvent?.Invoke();
			onAccept?.Invoke();
		}
		private void RejectNotify() {
			RejectedEvent?.Invoke();
			onReject?.Invoke();
		}
	}
}