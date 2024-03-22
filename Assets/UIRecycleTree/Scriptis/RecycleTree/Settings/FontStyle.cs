using System;
using UnityEngine;

[Serializable]
public class FontStyle {
	public Color noChildColor = Color.white;
	public Color collapsedColor = Color.white;
	public Color expandedColor = Color.white;

	public TMPro.FontStyles noChildStyle;
	public TMPro.FontStyles collapsedStyle;
	public TMPro.FontStyles expandedStyle;
}
