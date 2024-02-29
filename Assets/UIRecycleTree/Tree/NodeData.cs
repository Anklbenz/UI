using System;

namespace UIRecycleTree {
	[Serializable]
	public class NodeData {
		public string text;
		public bool hasChildren;
		public bool expanded;
		public float indent;
	}
}