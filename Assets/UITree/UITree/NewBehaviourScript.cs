using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
	[SerializeField] private ScrollRect _scrollRect;

	private void Start() {
		_scrollRect.onValueChanged.AddListener(Update1);
	}
	private void Update1(Vector2 arg0) {
		Debug.Log(_scrollRect.content.anchoredPosition);
		
	}
	//		
}
