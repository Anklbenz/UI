using System.Collections.Generic;

public class NumericDataAdapter : IRecycleDataSource {
	public int count => _list.Count;
	private List<int> _list;
	public void Initialize() {
		_list = new List<int>();
		for (int i = 0; i < 30; i++)
			_list.Add(i);
	}

	public void GetDataByIndex(IItem item, int index) {
		var item1 = item as Item;
		item1.text = _list[index].ToString();
	}
}