using UnityEngine;
using System.Collections;

public class DailyRewardItem : MonoBehaviour {
	public enum State
	{
		Past,
		Today,
		Future
	}

	public UISprite	background;
	public UISprite	blockSprite;
	public UILabel	valueLable;
	public UILabel	dayLabel;
	
	private int		dayIndex;
	private State	currentState;

	public int DayIndex
	{
		get
		{
			return dayIndex;
		}
		set
		{
			dayIndex = value;
			dayLabel.text = TextManager.GetText(string.Format("day_{0}", dayIndex));
			valueLable.text = Constant.DAILY_REWARD_DATA[dayIndex].ToString();
		}
	}

	public State CurrentState
	{
		get
		{
			return currentState;
		}
		set
		{
			currentState = value;
			switch(currentState)
			{
			case State.Past:
				background.color = Color.gray;
				blockSprite.color = Color.gray;
				valueLable.color = Color.white;
				valueLable.color = Constant.COLOR_DARK_BLACK;
				break;
			case State.Today:
				background.color = new Color(0.45f,0.78f,0.45f);
				blockSprite.color = Color.white;
				valueLable.color = Constant.COLOR_DARK_BLACK;
				break;
			case State.Future:
				if(dayIndex == 6)
				{
					background.color = Constant.COLOR_DARK_BLACK;
					valueLable.color = Color.white;
				}
				else
				{
					background.color = Color.white;
					valueLable.color = Constant.COLOR_DARK_BLACK;
				}
				blockSprite.color = Color.white;
				break;
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
