using UnityEngine;
using System;
using System.Collections;

public class Cell : MonoBehaviour {	
	public enum CellType{
		Block,
		Empty
	};
	
	public enum CellState{
		Showing,
		Opened,
		Closed,
	}

	public UISprite			background;
	public UISprite			cellSprite;
	public UILabel			cellLabel;
	public UISprite			cellTexture;
	public UISprite			glowBorder;
	
	private int				rowIndex;
	private int				columnIndex;
	private CellType		type;
	private CellState		state = CellState.Showing;
	private float			backgroundAlpha;
	private float			foregroundAlpha;

	public int RowIndex
	{
		get
		{
			return rowIndex;
		}
	}

	public int ColumnIndex
	{
		get
		{
			return columnIndex;
		}
	}

	public CellType Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}
	
	public CellState State{
		get{
			return state;
		}
		
		set{
			state = value;
			switch(state)
			{
			case CellState.Showing:
				bool isBlock = (type == CellType.Block);
				background.enabled = true;
				cellSprite.enabled = isBlock;
				cellLabel.enabled = isBlock;
				cellTexture.enabled = isBlock;
				cellSprite.spriteName = isBlock ? "img_cell_white" : string.Empty;
				break;
			case CellState.Opened:
				background.enabled = false;
				cellSprite.enabled = true;
				cellLabel.enabled = true;
				cellTexture.enabled = true;
				cellSprite.spriteName = (type == CellType.Block) ? "img_cell_white" : "img_cell_wrong";
				break;
			case CellState.Closed:
				background.enabled = true;
				cellSprite.enabled = false;
				cellLabel.enabled = false;
				cellTexture.enabled = false;
				break;
			default:
				break;
			}
		}
	}

	public Color CurrentColor
	{
		get
		{
			return cellSprite.color;
		}
		set
		{
			cellSprite.color = value;
		}
	}

	public string Text
	{
		get
		{
			return cellLabel.text;
		}
		set
		{
			cellLabel.text = value;
		}
	}

	public string TextureName
	{
		get
		{
			return cellTexture.spriteName;
		}
		set
		{
			cellTexture.spriteName = value;
		}
	}

	public float BackgroundAlpha
	{
		get
		{
			return backgroundAlpha;
		}
		set
		{
			backgroundAlpha = value;
			background.alpha = value;
		}
	}

	public float ForegroundAlpha
	{
		get
		{
			return foregroundAlpha;
		}
		set
		{
			foregroundAlpha = value;
			cellSprite.alpha = value;
			cellLabel.alpha = value;
			cellTexture.alpha = value;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Init(int size, CellType cellType, int rowIndex, int columnIndex){
		this.background.width = size;
		this.background.height = size;
		this.cellSprite.width = size;
		this.cellSprite.height = size;
		this.cellTexture.width = size / 2;
		this.glowBorder.width = (int)(size * 1.35f);
		//this.glowBorder.width = (int)(size * 151f / 126);
		this.glowBorder.height = this.glowBorder.width;
		this.State = Cell.CellState.Closed;
		this.type = cellType;
		this.rowIndex = rowIndex;
		this.columnIndex = columnIndex;
		this.cellSprite.color = Color.white;
		this.cellLabel.text = string.Empty;
		this.cellLabel.fontSize = size / 2;
		this.cellTexture.spriteName = string.Empty;
	}
	
	public void AnimateAlpha(float from, float to, float time, EventDelegate.Callback callback, UITweener.Method method = UITweener.Method.Linear, float delay = 0)
	{
		backgroundAlpha = to;
		foregroundAlpha = to;
		TweenAlphaAdvance tweenAlpha = TweenAlphaAdvance.Begin(this.gameObject, time, to);
		tweenAlpha.from = from;
		tweenAlpha.method = method;
		tweenAlpha.delay = delay;
		EventDelegate.Add(tweenAlpha.onFinished, callback, true);
	}

	public void AnimateForegroundAlpha(float from, float to, float time, EventDelegate.Callback callback, UITweener.Method method = UITweener.Method.Linear, float delay = 0)
	{
		foregroundAlpha = to;
		TweenAlpha tweenAlpha1 = TweenAlpha.Begin(cellLabel.gameObject, time, to);
		tweenAlpha1.from = from;
		tweenAlpha1.method = method;
		tweenAlpha1.delay = delay;
		TweenAlpha tweenAlpha2 = TweenAlpha.Begin(cellTexture.gameObject, time, to);
		tweenAlpha2.from = from;
		tweenAlpha2.method = method;
		tweenAlpha2.delay = delay;
		TweenAlpha tweenAlpha3 = TweenAlpha.Begin(cellSprite.gameObject, time, to);
		tweenAlpha3.from = from;
		tweenAlpha3.method = method;
		tweenAlpha3.delay = delay;
		EventDelegate.Add(tweenAlpha3.onFinished, callback, true);
	}

	public void PlayGlowBorderAnimation()
	{
		glowBorder.color = this.CurrentColor;
		if (this.cellSprite.spriteName.Equals("img_cell_wrong"))
		{
			glowBorder.color = Color.red;
		}
		glowBorder.alpha = 0.1f;
		glowBorder.gameObject.SetActive(true);
		/*
		TweenAlpha tweenAlpha = TweenAlpha.Begin(glowBorder.gameObject, 0.15f, 1.0f);
		EventDelegate.Add(tweenAlpha.onFinished, GlowBorderShowFinish, true);
		*/
		iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.0f,
		                                            "to", 1.0f,
		                                            "time", 0.1f,
		                                            //"delay", 0.05f,
		                                            "onupdatetarget", this.gameObject,
		                                            "easetype", iTween.EaseType.easeOutExpo,
		                                            "looptype", iTween.LoopType.none,
		                                            "onupdate", "GlowBorderAnimationUpdate",
		                                            "oncompletetarget", this.gameObject,
		                                            "oncomplete", "GlowBorderShowFinish"));

	}

	private void GlowBorderAnimationUpdate(float value)
	{
		glowBorder.alpha = value;
	}
	
	private void GlowBorderShowFinish()
	{	
		glowBorder.gameObject.SetActive(false);
	}
}
