using UnityEngine;
using System.Collections;

public class UIButtonColorAdvance : MonoBehaviour
{
	/// <summary>
	/// Target with a widget, renderer, or light that will have its color tweened.
	/// </summary>

	public GameObject[] targets;

	/// <summary>
	/// Color to apply on hover event (mouse only).
	/// </summary>

	public Color hover = new Color(1f, 1f, 1f, 1f);
	public bool enableHover = false;

	/// <summary>
	/// Color to apply on the pressed event.
	/// </summary>

	public Color pressed = Color.grey;

	/// <summary>
	/// Duration of the tween process.
	/// </summary>
	/// 

	public float duration = 0;

	Color[] mColors;
	bool mInitDone = false;
	bool mStarted = false;
	bool mHighlighted = false;

	void Awake()
	{
		if (!mInitDone) Init();
	}

	void Start () { mStarted = true; }

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnDisable ()
	{
		if (!mInitDone)
		{
			return;
		}
		if (targets.Length > 0)
		{
			for(int i = 0 ; i < targets.Length ; i++)
			{
				if (targets[i] != null)
				{
					TweenColor tc = targets[i].GetComponent<TweenColor>();
	
					if (tc != null)
					{
						tc.value = mColors[i];
						tc.enabled = false;
					}
				}
			}
		}
	}

	void Init ()
	{
		mInitDone = true;
		mColors = new Color[targets.Length];
		for(int i = 0 ; i < targets.Length ; i++)
		{
			UIWidget widget = targets[i].GetComponent<UIWidget>();
	
			if (widget != null)
			{
				mColors[i] = widget.color;
			}
			else
			{
				Renderer ren = targets[i].renderer;
	
				if (ren != null)
				{
					mColors[i] = ren.material.color;
				}
				else
				{
					Light lt = targets[i].light;
	
					if (lt != null)
					{
						mColors[i] = lt.color;
					}
					else
					{
						Debug.LogWarning(NGUITools.GetHierarchy(gameObject) + " has nothing for UIButtonColor to color", this);
						enabled = false;
					}
				}
			}
		}
	}

	void OnPress (bool isPressed)
	{
		if (!mInitDone) Init();
		if (duration > 0)
		{
			if (enabled)
			{
				for(int i = 0 ; i < targets.Length ; i++)
				{
					TweenColor.Begin(targets[i], duration, isPressed ? pressed : mColors[i]);
				}
			}
		}
		else
		{
			for(int i = 0 ; i < targets.Length ; i++)
			{
				ChangeColor(targets[i], isPressed ? pressed : mColors[i]);
			}
		}
	}

	void OnHover (bool isOver)
	{
		if (enabled && enableHover)
		{
			if (!mInitDone) 
			{
				Init();
			}
			
			if (duration > 0)
			{
				if (enabled)
				{
					for(int i = 0 ; i < targets.Length ; i++)
					{
						TweenColor.Begin(targets[i], duration, isOver ? hover : mColors[i]);
					}
				}
			}
			else
			{
				for(int i = 0 ; i < targets.Length ; i++)
				{
					ChangeColor(targets[i], isOver ? hover : mColors[i]);
				}
			}
			mHighlighted = isOver;
		}
	}
	
	void ChangeColor(GameObject target, Color color)
	{
		if (enabled) 
		{
			UIWidget widget = target.GetComponent<UIWidget>();
	
			if (widget != null)
			{
				widget.color = color;
			}else
			{
				Renderer ren = target.renderer;
	
				if (ren != null)
				{
					ren.material.color = color;
				}
				else
				{
					Light lt = target.light;
	
					if (lt != null)
					{
						lt.color = color;
					}
					else
					{
						Debug.LogWarning(NGUITools.GetHierarchy(gameObject) + " has nothing for UIButtonColor to color", this);
						enabled = false;
					}
				}
			}
		}
	}
}
