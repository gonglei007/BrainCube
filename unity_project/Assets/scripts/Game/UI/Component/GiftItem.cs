using UnityEngine;
using System.Collections;

public class GiftItem : MonoBehaviour {
	public UISprite	background;
	public UILabel	numberLabel;

	private int number;

	public int Number
	{
		get
		{
			return number;
		}
		set
		{
			number = value;
			if (GameSystem.GetInstance().IsVIP)
			{
				numberLabel.text = string.Format("{0} x 2", number);
			}
			else
			{
				numberLabel.text = number.ToString();
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
