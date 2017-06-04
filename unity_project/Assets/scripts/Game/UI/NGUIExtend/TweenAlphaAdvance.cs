using UnityEngine;

public class TweenAlphaAdvance : UITweener
{
	#if UNITY_3_5
	public float from = 1f;
	public float to = 1f;
	#else
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;
	#endif
	
	Transform mTrans;
	UIWidget[] mWidgets;
	UIPanel mPanel;
	
	/// <summary>
	/// Current alpha.
	/// </summary>
	
	public float alpha
	{
		get
		{
			if (mPanel != null) return mPanel.alpha;
			if (mWidgets != null) return mWidgets[0].alpha;
			return 0f;
		}
		set
		{
			if (mPanel != null)
			{
				mPanel.alpha = value;
			}
			else
			{
				foreach(UIWidget widget in mWidgets)
				{
					widget.alpha = value;
				}
			}
		}
	}
	
	/// <summary>
	/// Find all needed components.
	/// </summary>
	
	void Awake ()
	{
		mPanel = GetComponent<UIPanel>();
		if (mPanel == null) mWidgets = GetComponentsInChildren<UIWidget>(true);
	}
	
	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>
	
	protected override void OnUpdate (float factor, bool isFinished) { alpha = Mathf.Lerp(from, to, factor); }
	
	/// <summary>
	/// Start the tweening operation.
	/// </summary>
	
	static public TweenAlphaAdvance Begin (GameObject go, float duration, float alpha)
	{
		TweenAlphaAdvance comp = UITweener.Begin<TweenAlphaAdvance>(go, duration);
		comp.from = comp.alpha;
		comp.to = alpha;
		
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public void UpdateWidgets()
	{
		mPanel = GetComponent<UIPanel>();
		if (mPanel == null) mWidgets = GetComponentsInChildren<UIWidget>(true);
	}
}