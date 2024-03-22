using System;
using UnityEngine;

namespace UIRecycleTree {
	[Serializable]
	public class ExpandStyle {
		public Sprite noChildSprite;
		public Color noChildColor = Color.white;
		public Sprite collapseSprite;
		public Color collapseColor = Color.white;
		public Sprite expandSprite;
		public Color expandColor = Color.white;
	}
}