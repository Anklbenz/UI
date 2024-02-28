using UnityEngine;
using Random = UnityEngine.Random;

namespace UIRecycleView {
	public class Demo : MonoBehaviour {
		[SerializeField] private RecycleView view;
		private NumericDataAdapter _dataSource = new();

		private void Start() {
			_dataSource.Initialize(30);
			view.recycleDataSource = _dataSource;

			StartCoroutine(view.Reload());
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				var newCount = Random.Range(3, 3000);
				Debug.Log(newCount);
				_dataSource.Initialize(newCount);
				StartCoroutine(view.Reload());
			}
		}
	}
}
