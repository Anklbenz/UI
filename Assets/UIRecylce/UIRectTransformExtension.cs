using UnityEngine;

public static class UIRectTransformExtension {
	public static Vector3[] GetWorldCorners(this RectTransform rectTransform) {
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		return corners;
	}
	public static float MaxY(this RectTransform rectTransform) =>
			rectTransform.GetWorldCorners()[1].y;

	public static float MinY(this RectTransform rectTransform) =>
			rectTransform.GetWorldCorners()[0].y;

	public static float MaxX(this RectTransform rectTransform) =>
			rectTransform.GetWorldCorners()[2].x;

	public static float MinX(this RectTransform rectTransform) =>
			rectTransform.GetWorldCorners()[0].x;
}