using UnityEngine;
using System.Collections;

public class MusicItem : MonoBehaviour {
	public UISprite	itemBg;
	public UILabel	nameLabel;
	public UIToggle	checkBox;

	private MusicData data;
	private bool	  isBgm = false;

	public bool IsBgm
	{
		get
		{
			return isBgm;
		}
		set
		{
			isBgm = value;
		}
	}

	public MusicData Data
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
				if (isBgm)
				{
					nameLabel.text = TextManager.GetText(string.Format("bgm_music_name_{0}", data.id));
				}
				else
				{
					nameLabel.text = TextManager.GetText(string.Format("game_music_name_{0}", data.id));
				}
				itemBg.color = data.id % 2 == 0 ? Color.white : new Color(0.91372f,0.91372f,0.91372f);
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
