using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Umeng;
using KTPlaySDKJson;

public class MainMenu : BaseMenu {
	public static int 	ENTER_TIMES = -1;

	//下面四个是用于播放动画的
	public GameObject	background;
	public GameObject	title;
	public GameObject	toolBar;
	public List<GameObject>	modeObjects;
	public GameObject	vipButton;
	public GameObject	vipState;
	public GameObject	giftButton;
	public UILabel		moreGameLabel;
	public UILabel		versionLabel;

	public UIPanel		modePanel;
	public UILabel		coinLabel;
	public GameObject	snsButton;
	public UIToggle		muteButton;
	public GameObject	shopButton;
	public ModeItem[]	modeItems;

	public UILabel		giftStatusLabel;
	public UILabel		giftTimeLabel;
	public UIRotate		giftRotate;

	private ADTrigger	adTrigger = new ADTrigger(2.0f, 40);

	private bool		isStartGame = true;

	void Awake()
	{
		TextManager.LoadLanguage("Localization/zh-Hans/loc-kit");
		LeaderboardManager.Init();
		AdjustModePanel();
		GameSystem.GetInstance().gameUI.mainMenu = this;
	}
	
	void Start(){
		coinLabel.text = string.Format("{0}", GameSystem.GetInstance().Coin);
		GameSystem.GetInstance().OnCoinChanged += HandleOnCoinChanged;
		muteButton.value = PlayerProfile.LoadMute();
		EventDelegate.Add(muteButton.onChange, MuteButtonValueOnChange);
		GameSoundSystem.GetInstance().Mute = muteButton.value;
		shopButton.SetActive(Config.isStoreActive);
		moreGameLabel.text = Config.isMoreGameActive ? TextManager.GetText("more_game") : "...";
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			versionLabel.text = "v" + iOSInterfaces.getAPPVersion ();

			foreach(GameObject modeObject in modeObjects){
				if(modeObject.GetComponent<MoreModeButton>() != null){
					modeObjects.Remove(modeObject);
					modeObject.SetActive(false);
					break;
				}
			}
		} else if (Application.platform == RuntimePlatform.Android) {
			versionLabel.text = "v" + AndroidInterfaces.CallGetAppVersion ();
		}

#if UNITY_IPHONE || UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			KTPlay.SetAvailabilityChangedCallback(this, KTPlayAvailabilityChanged);
			GA.UpdateOnlineConfig();
		}
#endif
		UpdateGiftButtonState();
		RefreshVIPButtonState();
	}

	void Update(){
		adTrigger.Update();
		if (GameSystem.GetInstance().IsGiftAvailableNow == false
			&& Time.frameCount % 15 == 0)
		{
			UpdateGiftButtonState();
		}
	}

	void OnDestroy()
	{
		GameSystem.GetInstance().OnCoinChanged -= HandleOnCoinChanged;
	}

	public void KTPlayAvailabilityChanged(string s)
	{
		Debug.Log ("KTPlay availability state: " + s);
		Hashtable data = (Hashtable)KTJSON.jsonDecode (s);
		Hashtable paramsObj = (Hashtable)data["params"];
		bool isEnabled = Convert.ToBoolean (paramsObj["isEnabled"]);
		Debug.Log ("KTPlay state:" + isEnabled.ToString());
		// refresh sns button state
		bool available = false;
		if (isEnabled)
		{
			available = NetworkManager.GetInstance().IsSNSAvailable();
		}
		snsButton.SetActive(available);
		// refresh rank button state
		foreach (ModeItem modeItem in modeItems)
		{
			modeItem.RefreshRankButton();		
		}
	}

	public override void Show (bool active)
	{
		base.Show(active);
		if (active)
		{
			if (isStartGame)
			{
				this.gameObject.transform.localPosition = new Vector3(0,2000,this.gameObject.transform.localPosition.z);
				StartCoroutine(PlayEnterGameAnimation());
				isStartGame = false;
			}
			else
			{
				// Show Interstitial AD
				adTrigger.ToShow();
				
				bool available = NetworkManager.GetInstance().IsSNSAvailable();
				
				snsButton.SetActive(available);
			}
		}
	}

	IEnumerator PlayEnterGameAnimation()
	{
		yield return new WaitForSeconds(0.5f);
		this.Show(true);
		this.ShowEnterAnimation(MainMenuEnterAnimFirstFinish);
	}

	public void ShowEnterAnimation(Action callback)
	{
		GameSystem.GetInstance().BlockFullScreen = true;

		NGUIToolsExtend.SetAlphaRecursively(background, 0);
		NGUIToolsExtend.SetAlphaRecursively(vipState, 0);
		int lastIndex = modeObjects.Count - 2;
		if(Config.isMoreGameActive)
		{
			lastIndex = modeObjects.Count - 1;
		}
		TweenAlphaAdvance.Begin(background, 0.4f + lastIndex * 0.1f, 1);

		TweenAlphaAdvance.Begin(vipState, 0.4f, 1);
		
		Vector3 endPos = title.transform.localPosition;
		Vector3 startPos = endPos;
		startPos.y = 600;
		title.transform.localPosition = startPos;
		TweenPosition tweenPosition = TweenPosition.Begin(title, 0.4f, endPos);
		tweenPosition.method = UITweener.Method.EaseOut;

		endPos = vipButton.transform.localPosition;
		startPos = endPos;
		startPos.y = 600;
		vipButton.transform.localPosition = startPos;
		tweenPosition = TweenPosition.Begin(vipButton, 1f, endPos);
		tweenPosition.delay = 0.1f;
		tweenPosition.method = UITweener.Method.BounceOut;
		
		endPos = giftButton.transform.localPosition;
		startPos = endPos;
		startPos.y = 600;
		giftButton.transform.localPosition = startPos;
		tweenPosition = TweenPosition.Begin(giftButton, 1f, endPos);
		tweenPosition.delay = 0.1f;
		tweenPosition.method = UITweener.Method.BounceOut;
		
		endPos = toolBar.transform.localPosition;
		startPos = endPos;
		startPos.x = -800;
		toolBar.transform.localPosition = startPos;
		tweenPosition = TweenPosition.Begin(toolBar, 0.4f, endPos);
		tweenPosition.method = UITweener.Method.EaseOut;

		int index = 0;
		foreach(GameObject modeItem in modeObjects)
		{
			endPos = modeItem.transform.localPosition;
			startPos = endPos;
			startPos.x = 800;
			modeItem.transform.localPosition = startPos;
			tweenPosition = TweenPosition.Begin(modeItem, 0.4f, endPos);
			tweenPosition.delay = index * 0.1f;
			tweenPosition.method = UITweener.Method.EaseOut;
			if (index == lastIndex)
			{
				EventDelegate.Add(tweenPosition.onFinished, ()=>{
					GameSystem.GetInstance().BlockFullScreen = false;
					MainMenuEnterAnimFinish();
					if(callback != null)
					{
						callback();
					}
				}, true);
				break;
			}
			index++;
		}
	}

	public void CloseOtherOpenedMode(ModeItem currentItem)
	{
		foreach (ModeItem modeItem in modeItems)
		{
			if (modeItem != currentItem && modeItem.IsDetailVisible)
			{
				modeItem.HideModeDetail();
			}
		}
	}

	void HandleOnCoinChanged(int coin)
	{
		coinLabel.text = string.Format("{0}", coin);
	}

	public void SNSButtonOnClick(){
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			if (KTPlay.IsEnabled()) {
				KTPlay.Show();
			}
		}
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void ShareButtonOnClick(){
		StartCoroutine(UMengManager.ShareGame());
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void ShopButtonOnClick(){
		if (GameSystem.GetInstance().CheckIsActivated())
		{
			GameSystem.GetInstance().gameUI.shopMenu.Show (true);
			GameSoundSystem.GetInstance().PlayRandomSound();
		}
	}
	
	public void MoreModeButtonOnClick()
	{
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void MoreGameButtonOnClick()
	{
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void AboutButtonOnClick()
	{
		GameSoundSystem.GetInstance().PlayRandomSound();
		string title = TextManager.GetText("about");
		string content = TextManager.GetText("about_content");
		GameSystem.GetInstance ().gameUI.confirmMenu.SetStyle (28, NGUIText.Alignment.Left);
		GameSystem.GetInstance().gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.OnlyYes);
		GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
	}

	public void MuteButtonValueOnChange()
	{
		GameSoundSystem.GetInstance().Mute = muteButton.value;
		PlayerProfile.SaveMute(muteButton.value);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void VIPButtonOnClick()
	{
		if (GameSystem.GetInstance().CheckIsActivated())
		{
			if (GameSystem.GetInstance().IsVIP == false)
			{
				GameSoundSystem.GetInstance().PlayRandomSound();
				string title = TextManager.GetText("confirm_buy_vip_gift_title");
				string content = String.Format(TextManager.GetText("vip_gift_desc"), ShopData.GetShopData(IAPManager.IAPProduct.VIP).number);
				content = content.Replace('\n', ' ');
				//GameSystem.GetInstance().gameUI.vipConfirmMenu.SetConfirmCallback(ConfirmBuyVIPCallback);
				GameSystem.GetInstance().gameUI.vipConfirmMenu.SetContent(title, content);
				GameSystem.GetInstance().gameUI.vipConfirmMenu.Show(true);
			}
		}
	}

	public void GiftButtonOnClick()
	{
		if (GameSystem.GetInstance().CheckIsActivated())
		{
			GameSoundSystem.GetInstance().PlayRandomSound();
			GameSystem.GetInstance().gameUI.giftMenu.Show(true);
		}
	}

	public void RefreshVIPButtonState()
	{
		vipState.SetActive(GameSystem.GetInstance().IsVIP);
		vipButton.SetActive(!GameSystem.GetInstance().IsVIP);
	}

	void MainMenuEnterAnimFinish()
	{
		if (GameSystem.GetInstance().IsActivated)
		{
			ENTER_TIMES++;
			CheckRecommend();
		}
	}

	void MainMenuEnterAnimFirstFinish()
	{
		if (GameSystem.GetInstance().IsActivated)
		{
			CheckGiftReady();
		}
	}

	void CheckDailyReward()
	{
		int continuesDay = PlayerProfile.LoadDailyRewardDays();
		DateTime lastRewardTime = PlayerProfile.LoadLastRewardTime();
		if (lastRewardTime.Year == 1970)
		{
			continuesDay = 1;
			PlayerProfile.SaveLastRewardTime(DateTime.Now);
			GameSystem.GetInstance().gameUI.dailyRewardMenu.Show(true, continuesDay);
		}
		else
		{
			int dayDiff = GetDayDiff(DateTime.Now, lastRewardTime);
			Debug.Log("DayDiff " + dayDiff);
			if (dayDiff == 1)
			{
				continuesDay++;
				if (continuesDay > 7)
				{
					continuesDay = 1;
				}
				PlayerProfile.SaveDailyRewardDays(continuesDay);
				PlayerProfile.SaveLastRewardTime(DateTime.Now);
				GameSystem.GetInstance().gameUI.dailyRewardMenu.Show(true, continuesDay);
			}
			else if (dayDiff > 1)
			{
				continuesDay = 1;
				PlayerProfile.SaveDailyRewardDays(continuesDay);
				PlayerProfile.SaveLastRewardTime(DateTime.Now);
				GameSystem.GetInstance().gameUI.dailyRewardMenu.Show(true, continuesDay);
			}
		}
	}

	private int GetDayDiff(DateTime time1, DateTime time2)
	{
		DateTime time1970 = new DateTime(1970,1,1);
		TimeSpan timeSpan1 = (time1 - time1970);
		TimeSpan timeSpan2 = (time2 - time1970);
		return (int)(timeSpan1.TotalDays - timeSpan2.TotalDays);
	}

	public void UpdateGiftButtonState()
	{
		int seconds = GameSystem.GetInstance().NextGiftTimeLeft;
		
		if (seconds <= 0)
		{
			giftTimeLabel.text = string.Empty;
			giftRotate.enabled = true;
			giftStatusLabel.text = TextManager.GetText("gift_available");
			giftStatusLabel.color = Color.red;

			TweenScale tweenScale = TweenScale.Begin(giftRotate.gameObject, 0.7f, Vector3.one * 1.3f);
			tweenScale.enabled = true;
			tweenScale.method = UITweener.Method.BounceIn;
			tweenScale.style = UITweener.Style.PingPong;
			tweenScale.ignoreTimeScale = false;
		}
		else
		{
			giftTimeLabel.text = string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds % 3600) / 60, seconds % 60);
			giftRotate.enabled = false;
			giftStatusLabel.text = TextManager.GetText("gift");
			giftStatusLabel.color = Color.black;

			TweenScale tweenScale = giftRotate.GetComponent<TweenScale>();
			if (tweenScale != null)
			{
				tweenScale.enabled = false;
			}
			giftRotate.transform.localScale = Vector3.one;
		}
	}

	private void CheckRecommend()
	{
		Array modeArray = Enum.GetValues(typeof(GameSystem.Mode));
		List<GameSystem.Mode> lockedModesHP = new List<GameSystem.Mode>(modeArray.Length);
		List<GameSystem.Mode> lockedModesLP = new List<GameSystem.Mode>(modeArray.Length);
		foreach(GameSystem.Mode mode in modeArray)
		{
			if (PlayerProfile.LoadModeUnlock(mode) == false)
			{
				if (ArrayTool.Constains(Constant.RECOMMEND_ORDER_HIGH_PRIORITY, mode))
				{
					lockedModesHP.Add(mode);
				}
				else if (ArrayTool.Constains(Constant.RECOMMEND_ORDER_LOW_PRIORITY, mode))
				{
					lockedModesLP.Add(mode);
				}
			}
		}

		if (ENTER_TIMES > 0 && ENTER_TIMES % Constant.RECOMMEND_TRIGGER_TIMES == 0)
		{
			if (lockedModesHP.Count > 0)
			{
				GameSystem.GetInstance().RecommendMode = RandomTool.Element(lockedModesHP);
			}
			else if (lockedModesLP.Count > 0)
			{
				GameSystem.GetInstance().RecommendMode = RandomTool.Element(lockedModesLP);
			}
			else
			{
				return;
			}

			string title = string.Format(TextManager.GetText("recommend_mode_title"), TextManager.GetText(string.Format("mode_name_{0}", (int)GameSystem.GetInstance().RecommendMode)));
			string yesButtonText = string.Format(TextManager.GetText("unlock_condition"), Constant.MODE_UNLOCK_COIN[(int)GameSystem.GetInstance().RecommendMode]);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetRecommendMode(GameSystem.GetInstance().RecommendMode);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetConfirmCallback(RecommendConfirmCallback);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetContent(title, null, ConfirmStyle.YesNoClose);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetYesButtonText(yesButtonText);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetNoButtonText(TextManager.GetText("trial"));
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.DisableNextYesSound();
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.DisableNextNoSound();
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.Show(true);
		}
	}

	private void RecommendConfirmCallback(bool result)
	{
		CloseOtherOpenedMode(modeItems[(int)GameSystem.GetInstance().RecommendMode]);
		if (result)
		{
			int price = Constant.MODE_UNLOCK_COIN[(int)GameSystem.GetInstance().RecommendMode];
			if (GameSystem.GetInstance().Coin >= price)
			{
				GameSystem.GetInstance().IsTrial = false;
				GameSystem.GetInstance().UnlockMode(GameSystem.GetInstance().RecommendMode);
				modeItems[(int)GameSystem.GetInstance().RecommendMode].CheckUnlock();
				modeItems[(int)GameSystem.GetInstance().RecommendMode].EnterGame();
			}
			else
			{
#if UNITY_IOS
				string title = TextManager.GetText("not_enough_coin_title");
				string content = string.Format(TextManager.GetText("not_enough_coin_for_unlock_recommend"), price - GameSystem.GetInstance().Coin);
				GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback((param)=>{
					GameSystem.GetInstance().gameUI.shopMenu.Show(true);
				});
				GameSystem.GetInstance().gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.OnlyYes);
				GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
#else
				for(int i = 0 ; i < ShopData.gameShopData.Length - 1 ; i++)
				{
					ShopData shopData = ShopData.gameShopData[i];
					if (shopData.number >= price)
					{
						GameSystem.GetInstance().UnlockRecommendMode = true;
						IAPManager.GetInstance().Pay(shopData.product);
						break;
					}
				}
#endif
			}
		}
		else
		{
			modeItems[(int)GameSystem.GetInstance().RecommendMode].TrialPlay();
		}
	}

	private void RefreshConfirmUnlockModeMenu()
	{
		int price = Constant.MODE_UNLOCK_COIN[(int)GameSystem.GetInstance().RecommendMode];
		if (GameSystem.GetInstance().Coin >= price)
		{			
			string yesButtonText = string.Format(TextManager.GetText("unlock_condition"), Constant.MODE_UNLOCK_COIN[(int)GameSystem.GetInstance().RecommendMode]);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetYesButtonText(yesButtonText);
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.DisableNextYesSound();
			GameSystem.GetInstance().gameUI.recommendConfirmMenu.SetCloseMenuAfterClickYesEnable(true);
		}
	}

	private void CheckGiftReady()
	{
		if (GameSystem.GetInstance().IsGiftAvailableNow)
		{
			GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback(ConfirmGiftReadyCallback);
			GameSystem.GetInstance().gameUI.confirmMenu.SetContent(TextManager.GetText("gift_ready_title"), TextManager.GetText("gift_ready_content"), ConfirmStyle.YesClose);
			GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
		}
	}

	private void ConfirmGiftReadyCallback(bool result)
	{
		if (result)
		{
			GameSystem.GetInstance().gameUI.giftMenu.Show(true);
		}
	}

	private void ConfirmBuyVIPCallback(bool result)
	{
		if (result)
		{
			IAPManager.GetInstance().Pay(IAPManager.IAPProduct.VIP);
		}
	}

	private void AdjustModePanel()
	{
		Vector4 clipRegion = modePanel.baseClipRegion;
		int heightExtend = (int)((Constant.SCREEN_WIDTH / SystemUtility.GetDeviceWHAspect() - Constant.SCREEN_HEIGHT) / 2);
		heightExtend = heightExtend > 0 ? heightExtend : 0;
		int newPanelHeight = (int)clipRegion.w + heightExtend;
		clipRegion.w = newPanelHeight;
		clipRegion.y = -newPanelHeight / 2;
		modePanel.baseClipRegion = clipRegion;
	}
}