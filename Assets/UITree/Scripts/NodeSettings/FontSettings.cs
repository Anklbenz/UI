using System;
using TMPro;
using UnityEngine;

[Serializable]

public class FontSettings {
	public TMP_FontAsset font;
	public float fontSize = 46;
	
	public Color textColor = Color.white;
	public TMPro.FontStyles fontStyle;
	public TextAlignmentOptions textAlignment;
}
