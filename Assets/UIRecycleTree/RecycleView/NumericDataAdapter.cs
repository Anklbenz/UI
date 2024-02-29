using System.Collections.Generic;

namespace UIRecycleTree {
	public class NumericDataAdapter : IRecycleDataSource {
		public int count => _list.Count;
		private List<int> _list;
		public void Initialize(int itemsCount) {
			_list = new List<int>();
			for (int i = 0; i < itemsCount; i++)
				_list.Add(i);
		}

		public void GetDataByIndex(IRecycleItem recycleItem, int index) {
			var item1 = recycleItem as NodeView;
			item1.data = new NodeData() {text = _list[index].ToString()};
		}
	}
}