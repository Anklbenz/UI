using TMPro;
using UnityEngine;
namespace UIRecycleTree {
	public class RecycleItem : MonoBehaviour, IRecycleItem {
		[SerializeField] private TMP_Text textField;

		public string text {
			get => textField.text;
			set => textField.text = value;
		}
	}
}