
namespace UIRecycleTree {
	public interface IRecycleDataSource {
		int count { get; }
		void SetDataToItem(IRecycleItem recycleItem, int index);
	}
}
