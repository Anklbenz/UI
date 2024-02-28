using UnityEngine;

public class Demo : MonoBehaviour {
	[SerializeField] private RecycleView view;
	private NumericDataAdapter _dataSource = new ();

	private void Start() {
		_dataSource.Initialize();
		view.recycleDataSource = _dataSource;
		
		StartCoroutine(view.Reload());
	}

}