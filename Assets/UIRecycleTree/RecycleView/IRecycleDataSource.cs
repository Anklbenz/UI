
namespace UIRecycleTree {
	public interface IRecycleDataSource {
		int count { get; }
		void GetDataByIndex(IRecycleItem recycleItem, int index);
	}
}
