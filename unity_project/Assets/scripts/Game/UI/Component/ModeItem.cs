using UnityEngine;
using System.Collections;

public class ModeItem : MonoBehaviour {
	public GameSystem.Mode		mode;
	public UISprite				background1;
	public UISprite				background2;
	public UILabel				modeNameLabel;
	public UILabel[]			bestRecordLabels;
	
	public UILabel				unlockWithScoreValueLabel;
	public UILabel				unlockWithCoinValueLabel;

	public GameObject			unlockWithScoreContents;
	public UIShake				unlockWithCoinContents;
	public GameObject			newText;

	public GameObject[]			rankButtons;

	public GameObject			modeDetailObj;
	public BoxCollider			modeDetailBlockBg;
	public GameObject			challengeButton;
	public GameObject			passLevelButton;
	public GameObject			hellButton;

	public UILabel				passedLevelLabel;

	public GameObject			pressDownObj;

	private bool				isUnlocked;
	private bool				canUnlock;
	private bool				isEntered;
	private bool				isModeDetailAnimating = false;
	private bool				isDetailVisible = false;

	public bool IsDetailVisible
	{
		get
		{
			return isDetailVisible;
		}
	}

	// Use this for initialization
	void Start () {
		modeDetailObj.SetActive(false);
		pressDownObj.SetActive(false);
		modeNameLabel.text = TextManager.GetText(string.Format("mode_name_{0}", (int)mode));
		unlockWithCoinValueLabel.text = string.Format("x{0}", Constant.MODE_UNLOCK_COIN[(int)mode]);
		modeDetailBlockBg.enabled = false;
		if (mode == GameSystem.Mode.Inverse)
		{
			unlockWithScoreValueLabel.text = string.Format(TextManager.GetText("unlock_inverse"), Constant.MODE_UNLOCK_COIN[(int)mode]);
		}
		else
		{
			unlockWithCoinValueLabel.text = string.Format("x{0}", Constant.MODE_UNLOCK_COIN[(int)mode]);
		}
		GameSystem.GetInstance().OnCoinChanged += HandleOnCoinChanged;
		GameSystem.GetInstance().OnBestRecordChanged += HandleOnBestRecordChanged;
		RefreshBestRecordLabel();
		CheckUnlock();
		RefreshRankButton ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void RefreshRankButton(){
		bool needShowRankButton = (LocalVersion.local == LocalVersion.Local.CN_ZH);
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			needShowRankButton = needShowRankButton && KTPlay.IsEnabled();
		}

		int index = 0;
		foreach(GameObject rankButton in rankButtons)
		{
			if (mode == GameSystem.Mode.Dual)
			{
				rankButton.SetActive(false);
			}
			else
			{
				if ((mode == GameSystem.Mode.BlackJack || mode == GameSystem.Mode.Word) && index != 0)
				{
					rankButton.SetActive(false);
				}
				else
				{
					rankButton.SetActive (needShowRankButton);
				}
			}
			index++;
		}

		if (LocalVersion.local == LocalVersion.Local.EN)
		{
			for(int i = 0 ; i < rankButtons.Length ; i++)
			{
				Vector3 rankButtonPos = rankButtons[i].transform.localPosition;
				Vector3 bestRecordLabelPos = bestRecordLabels[i].transform.localPosition;
				bestRecordLabelPos.y = rankButtonPos.y;
				bestRecordLabels[i].transform.localPosition = bestRecordLabelPos;
			}
		}
	}
		
	void RefreshBestRecordLabel()
	{
		int index = 0;
		foreach(UILabel bestRecordLabel in bestRecordLabels)
		{
			if (index == (int)GameSystem.ModeType.PassLevel)
			{
				bestRecordLabel.text = string.Format("{0}/{1}", PlayerProfile.LoadBestRecord(mode, (GameSystem.ModeType)index), Constant.MAX_PASSLEVEL);
			}
			else
			{
				bestRecordLabel.text = PlayerProfile.LoadBestRecord(mode, (GameSystem.ModeType)index).ToString();
			}
			index++;
		}
	}

	public void CheckUnlock()
	{
		isUnlocked = PlayerProfile.LoadModeUnlock(mode);
		isEntered = PlayerProfile.LoadModeEntered(mode);
		background1.color = isUnlocked ? Constant.COLOR_WHITE : Constant.COLOR_GRAY;
		modeNameLabel.color = isUnlocked ? Constant.COLOR_DARK_BLACK : Constant.COLOR_LIGHT_WHITE;
		background2.color = background1.color;
		this.GetComponent<BoxCollider>().enabled = isUnlocked;

		if (mode == GameSystem.Mode.Inverse)
		{
			unlockWithCoinContents.gameObject.SetActive(false);
			unlockWithScoreContents.gameObject.SetActive(!isUnlocked);
			if (isUnlocked == false)
			{
				canUnlock = PlayerProfile.LoadBestRecord(GameSystem.Mode.Classic, GameSystem.ModeType.Challenge) >= Constant.MODE_UNLOCK_COIN[(int)mode];
				unlockWithScoreContents.SetActive(!canUnlock);
				newText.SetActive(canUnlock);
				
				if (canUnlock)
				{
					PlayerProfile.SaveModeUnlock(mode, true);
					CheckUnlock();
					
					//友盟统计解锁模式
					UMengManager.Event(UMengManager.Event_UnlockMode, mode.ToString());
				}
			}
			else
			{
				newText.SetActive(!isEntered);
			}
		}
		else
		{
			unlockWithScoreContents.gameObject.SetActive(false);
			unlockWithCoinContents.gameObject.SetActive(!isUnlocked);
			if (isUnlocked == false)
			{
				canUnlock = GameSystem.GetInstance().Coin >= Constant.MODE_UNLOCK_COIN[(int)mode];
				unlockWithCoinContents.enabled = canUnlock;
				newText.SetActive(false);
			}
			else
			{
				newText.SetActive(!isEntered);
			}
		}
	}

	public void ModeButtonOnClick()
	{
		if (mode == GameSystem.Mode.Classic
		    ||GameSystem.GetInstance().CheckIsActivated())
		{
			if (isUnlocked)
			{
				GameSoundSystem.GetInstance().PlayRandomSound();
				ShowModeDetail();
			}
		}
	}

	public void ModeButtonOnPress()
	{
		pressDownObj.SetActive(true);
	}

	public void ModeButtonOnRelease()
	{
		pressDownObj.SetActive(false);
	}
	
	public void UnlockButtonOnClick()
	{
		if (GameSystem.GetInstance().CheckIsActivated())
		{
			GameSoundSystem.GetInstance().PlayRandomSound();

			if(mode == GameSystem.Mode.Inverse)
			{
				string tipText = string.Format(TextManager.GetText("unlock_inverse"), Constant.MODE_UNLOCK_COIN[(int)mode]);
				GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback(CannotUnlockModeCallback);
				GameSystem.GetInstance().gameUI.confirmMenu.DisableNextYesSound();
				GameSystem.GetInstance().gameUI.confirmMenu.SetContent(tipText, null, ConfirmStyle.YesClose);
				GameSystem.GetInstance().gameUI.confirmMenu.SetYesButtonText(TextManager.GetText("trial"));
				GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
			}
			else
			{
				if (canUnlock)
				{
					string tipText = string.Format(TextManager.GetText("cost_coin"), Constant.MODE_UNLOCK_COIN[(int)mode]);
					GameSystem.GetInstance().gameUI.blockConfirmMenu.CloseButtonVisible = true;
					GameSystem.GetInstance().gameUI.blockConfirmMenu.DisableNextNoSound();
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetConfirmCallback(ConfirmUnlockModeWithCoinCallback);
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetContent(TextManager.GetText("confirm_unlock_mode_title"), tipText, ConfirmStyle.YesNoClose);
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetYesButtonText(TextManager.GetText("unlock"));
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetNoButtonText(TextManager.GetText("trial"));
					GameSystem.GetInstance().gameUI.blockConfirmMenu.Show(true);
				}
				else
				{
					string tipText = string.Format(TextManager.GetText("need_coin"), Constant.MODE_UNLOCK_COIN[(int)mode]);
					GameSystem.GetInstance().gameUI.blockConfirmMenu.CloseButtonVisible = false;
					GameSystem.GetInstance().gameUI.blockConfirmMenu.DisableNextYesSound();
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetConfirmCallback(CannotUnlockModeCallback);
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetContent(TextManager.GetText("not_enough_coin"), tipText, ConfirmStyle.YesClose);
					GameSystem.GetInstance().gameUI.blockConfirmMenu.SetYesButtonText(TextManager.GetText("trial"));
					GameSystem.GetInstance().gameUI.blockConfirmMenu.Show(true);
				}
			}
		}
	}
	
	public void ChallengeRankButtonOnClick()
	{
		GameSystem.GetInstance().CurrentMode = mode;
		GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.Challenge;
		GameSystem.GetInstance().gameUI.rankMenu.Show(true);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}
	
	public void PassLevelRankButtonOnClick()
	{
		GameSystem.GetInstance().CurrentMode = mode;
		GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.PassLevel;
		GameSystem.GetInstance().gameUI.rankMenu.Show(true);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}
	
	public void HellRankButtonOnClick()
	{
		GameSystem.GetInstance().CurrentMode = mode;
		GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.Hell;
		GameSystem.GetInstance().gameUI.rankMenu.Show(true);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void ChallengeButtonOnClick()
	{
		if (isModeDetailAnimating)
		{
			return;
		}
		GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.Challenge;
		GameSystem.GetInstance().IsTrial = false;
		EnterGame();
	}

	public void PassLevelButtonOnClick()
	{
		if (isModeDetailAnimating)
		{
			return;
		}

		if (GameSystem.GetInstance().CheckIsActivated())
		{
			GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.PassLevel;
			GameSystem.GetInstance().IsTrial = false;
			EnterGame();
		}
	}

	public void HellButtonOnClick()
	{
		if (isModeDetailAnimating)
		{
			return;
		}
		
		if (GameSystem.GetInstance().CheckIsActivated())
		{
			GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.Hell;
			GameSystem.GetInstance().IsTrial = false;
			EnterGame();
		}
	}
	
	public void ShowModeDetail()
	{
		isModeDetailAnimating = true;
		isDetailVisible = true;
		modeDetailBlockBg.enabled = true;
		modeDetailObj.SetActive(true);

		passLevelButton.SetActive(true);
		hellButton.SetActive(true);

		challengeButton.transform.localPosition = new Vector3(-430, 0, 0);
		TweenPosition tweenPosition = TweenPosition.Begin(challengeButton, 0.3f, new Vector3(-213, 0, 0));
		tweenPosition.method = UITweener.Method.EaseOutBack;
		
		hellButton.transform.localPosition = new Vector3(430, 0, 0);
		tweenPosition = TweenPosition.Begin(hellButton, 0.3f, new Vector3(213, 0, 0));
		tweenPosition.method = UITweener.Method.EaseOutBack;
		
		passLevelButton.transform.localScale = new Vector3(0.01f, 0.01f, 1);
		TweenScale tweenScale = TweenScale.Begin(passLevelButton, 0.3f, Vector3.one);
		tweenScale.method = UITweener.Method.EaseOutBack;
		
		EventDelegate.Add(tweenScale.onFinished, ShowModeDetailFinish, true);

		GameSystem.GetInstance().gameUI.mainMenu.CloseOtherOpenedMode(this);
	}

	public void HideModeDetail()
	{
		isModeDetailAnimating = true;
		isDetailVisible = false;
		modeDetailObj.SetActive(true);

		TweenPosition tweenPosition = TweenPosition.Begin(challengeButton, 0.3f, new Vector3(-430, 0, 0));
		tweenPosition.method = UITweener.Method.EaseIn;

		tweenPosition = TweenPosition.Begin(hellButton, 0.3f, new Vector3(430, 0, 0));
		tweenPosition.method = UITweener.Method.EaseIn;

		TweenScale tweenScale = TweenScale.Begin(passLevelButton, 0.3f, new Vector3(0.01f, 0.01f, 1));
		tweenScale.method = UITweener.Method.EaseIn;
		
		EventDelegate.Add(tweenScale.onFinished, HideModeDetailFinish, true);
	}
	
	void HideModeDetailImmediately()
	{
		HideModeDetailFinish();
	}
	
	void ShowModeDetailFinish()
	{
		isModeDetailAnimating = false;
	}
	
	void HideModeDetailFinish()
	{
		isDetailVisible = false;
		modeDetailBlockBg.enabled = false;
		isModeDetailAnimating = false;
		modeDetailObj.SetActive(false);
	}

	public void EnterGame()
	{
		GameSystem.GetInstance().CurrentMode = mode;
		GameSystem.GetInstance().ChangeState(GameSystem.States.GameReady);
		if (isEntered == false)
		{
			isEntered = true;
			newText.SetActive(false);
			PlayerProfile.SaveModeEntered(mode, true);
			
			//友盟统计进入模式的次数
			UMengManager.Event(UMengManager.Event_EnterMode, string.Format("{0}|{1}", mode, GameSystem.GetInstance().CurrentModeType));
		}
		HideModeDetailImmediately();
	}
	
	void HandleOnCoinChanged(int coin)
	{
		CheckUnlock();
	}
	
	void HandleOnBestRecordChanged()
	{
		if (GameSystem.GetInstance().CurrentMode == mode)
		{
			RefreshBestRecordLabel();
		}
	}
	
	void ConfirmUnlockModeWithCoinCallback(bool result)
	{
		if (result)
		{
			GameSystem.GetInstance().UnlockMode(mode);
			newText.SetActive(true);
			
			//友盟统计解锁模式
			UMengManager.Event(UMengManager.Event_UnlockMode, mode.ToString());
		}
		else
		{
			TrialPlay();
		}
	}

	public void TrialPlay()
	{
		GameSystem.GetInstance().CurrentModeType = GameSystem.ModeType.Challenge;
		GameSystem.GetInstance().IsTrial = true;
		EnterGame();
	}

	void CannotUnlockModeCallback(bool result)
	{
		TrialPlay();
	}
}
