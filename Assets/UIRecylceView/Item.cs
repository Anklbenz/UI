using TMPro;
using UnityEngine;
namespace UIRecycleView {
	public class Item : MonoBehaviour, IItem {
		[SerializeField] private TMP_Text textField;

		public string text {
			get => textField.text;
			set => textField.text = value;
		}
	}
}