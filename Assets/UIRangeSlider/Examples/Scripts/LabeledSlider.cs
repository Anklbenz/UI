	using System;
	using TMPro;
	using UnityEngine;

	namespace UIMinMaxSliderExamples {
		using UIMinMaxSlider;
		public class LabeledSlider : MonoBehaviour {
			[SerializeField] private UIMinMaxSlider slider;
			[SerializeField] private TMP_Text minValue, maxValue;

			private void SetValues(float min, float max) {
				minValue.text = min.ToString();
				maxValue.text = max.ToString();
			}

			private void OnEnable() {
				slider.onValueChanged.AddListener(SetValues);
			}

			private void OnDisable() {
				slider.onValueChanged.RemoveListener(SetValues);
			}

			private void Awake() {
				SetValues(slider.valueMin, slider.valueMax);
			}

		}
	}

