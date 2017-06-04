using UnityEngine;
using System.Collections;

public class DailyRewardMenu : BaseDialogMenu {

	public DailyRewardItem[] dailyRewardItems;
	public UILabel			 confirmLabel;
	
	void Awake()
	{
		GameSystem.GetInstance().gameUI.dailyRewardMenu = this;
		this.gameObject.SetActive(false);
	}

	void Start()
	{
		if (LocalVersion.local == LocalVersion.Local.CN_ZH)
		{
			confirmLabel.spacingX = 10;
		}
		else
		{
			confirmLabel.spacingX = 0;
		}
	}

	public void Show(bool active, int continusDay)
	{
		this.Show(active);
		if (active)
		{
			continusDay--;
			for(int i = 0 ; i < dailyRewardItems.Length ; i++)
			{
				dailyRewardItems[i].DayIndex = i;
				if(i < continusDay)
				{
					dailyRewardItems[i].CurrentState = DailyRewardItem.State.Past;
				}
				else if (i == continusDay)
				{
					dailyRewardItems[i].CurrentState = DailyRewardItem.State.Today;
					GameSystem.GetInstance().Coin += Constant.DAILY_REWARD_DATA[i];
					PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);
				}
				else
				{
					dailyRewardItems[i].CurrentState = DailyRewardItem.State.Future;
				}
			}
		}
	}
	
	public override void Show (bool active)
	{
		base.Show (active);
		this.gameObject.SetActive(active);
	}
	
	public override void ToggleShow ()
	{
		base.ToggleShow ();
		this.gameObject.SetActive(isActive);
	}

	public void ConfirmButtonOnClick()
	{
		this.Close();
		GameSoundSystem.GetInstance().PlayRandomSound();
	}
}
