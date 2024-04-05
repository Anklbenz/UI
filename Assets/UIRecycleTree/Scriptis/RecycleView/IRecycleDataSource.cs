
namespace UIRecycleTreeNamespace {
	public interface IRecycleDataSource {
		int expandedCount { get; }
		void MergeDataWithItem(RecycleItem recycleItem, int index);
	}
}
