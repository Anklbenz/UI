using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndentBox : MonoBehaviour {
	[SerializeField] private LayoutElement layoutElement;

	public float indent { set => layoutElement.minWidth = value; }
}
