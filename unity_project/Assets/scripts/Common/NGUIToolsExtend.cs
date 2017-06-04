using UnityEngine;
using System.Collections;

public class NGUIToolsExtend {

	public static void SetAlphaRecursively(GameObject gameObject, float alpha)
	{
		UIWidget[] widgets = gameObject.GetComponentsInChildren<UIWidget>(true);
		foreach(UIWidget widget in widgets)
		{
			widget.alpha = alpha;
		}
	}
	
	public static void SetColorecursively(GameObject gameObject, Color color)
	{
		UIWidget[] widgets = gameObject.GetComponentsInChildren<UIWidget>(true);
		foreach(UIWidget widget in widgets)
		{
			widget.color = color;
		}
	}
}
