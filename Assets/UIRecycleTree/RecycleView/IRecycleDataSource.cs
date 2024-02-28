
namespace UIRecycleTree {
	public interface IRecycleDataSource {
		int count { get; }
		void GetDataByIndex(IItem item, int index);
	}
}
