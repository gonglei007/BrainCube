using UnityEngine;
using System.Collections;

public class RankItem : MonoBehaviour {
	public UILabel rankLabel;
	public UILabel nameLabel;
	public UILabel scoreLabel;
	public UISprite rankBg;
	public UISprite itemBg;
	public UISprite innerGlow;

	private RankData data;

	public RankData Data
	{
		get
		{
			return data;
		}
		set
		{
			data = value;
			if (data != null)
			{
				rankLabel.text = data.rank.ToString();
				nameLabel.text = data.username;
				scoreLabel.text = data.score;
				if ((data.rank % 2) == 1)
				{
					rankBg.color = Color.black;
					rankLabel.color = Color.white;
					itemBg.color = Color.white;
				}
				else
				{
					rankBg.color = Color.white;
					rankLabel.color = Color.black;
					itemBg.color = new Color(0.75f, 0.75f, 0.75f);
				}
				this.IsPlayer = false;
			}
			else
			{
				rankLabel.text = "";
				nameLabel.text = "";
				scoreLabel.text = "";
			}
		}
	}

	public bool IsPlayer
	{
		get
		{
			return innerGlow.gameObject.activeSelf;
		}
		set
		{
			innerGlow.gameObject.SetActive(value);
		}
	}

	// Use this for initialization
	void Start () {
		innerGlow.width = (int)(SystemUtility.GetFinalWHAspect(Constant.STANDARD_WH_ASPECT) / Constant.STANDARD_WH_ASPECT * Constant.SCREEN_WIDTH);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
