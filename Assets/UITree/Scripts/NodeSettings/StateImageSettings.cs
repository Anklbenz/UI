using System;
using UnityEngine;

[Serializable]
public class StateImageSettings {
	public Sprite expand;
	public Color expandColor = Color.white;
	[Space(5)]
	public Sprite collapse;
	public Color collapseColor = Color.white;
	[Space(5)]
	public Sprite empty;
	public Color emptyColor = Color.white;
	[Space(5)]
	public Vector2 imageSizeDelta = new Vector2(60, 60);
}

