using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour {

	public static GameSystem instance;
	ADTrigger	adTrigger = new ADTrigger(1.0f, 100);

	public enum Mode
	{
		Classic = 0,
		Inverse = 1,
		Survival = 2,
		TimeRush = 3,
		ColorFull = 4,
		BlackJack = 5,
		Word = 6,
		Chimp = 7,
		Dual = 8,
		Count
	}

	public enum ModeType
	{
		Challenge = 0,
		PassLevel = 1,
		Hell = 2,
		Count = 3
	}
	
	public enum States 
	{
		GameInit,
		GameMenu,
		GameReady,
		GamePreview,
		GamePlay,
		WaveComplete,
		GameResult,
	}

	public enum FoodSetting
	{
		Confirm,
		AutoFeed,
		NoFeed
	}

	public Action<int>				OnWaveNumberChanged;
	public Action<int>				OnScoreChanged;
	public Action<int>				OnCoinChanged;
	public Action					OnBestRecordChanged;

	public UICamera					uiCamera;
	public UICamera					uiCamera1;

	public GameCore					gameCore;
	public GameUI					gameUI;
	public BoxCollider				fullScreenBlock;

	private FiniteStateMachine<GameSystem, States> fsm	= null;
	private StateGameInit 			stateGameInit		= new StateGameInit();
	private StateGameMenu 			stateGameMenu		= new StateGameMenu();
	private StateGameReady 			stateGameReady		= new StateGameReady();
	private StateGamePreview		stateGamePreview	= new StateGamePreview();
	private StateGamePlay 			stateGamePlay		= new StateGamePlay();
	private StateWaveComplete 		stateWaveComplete	= new StateWaveComplete();
	private StateGameResult			stateGameResult		= new StateGameResult();
	private States					currentState;

	private int						realWaveIndex		= 0;//关卡数据的索引，递增到最后一关后不再变化
	private int						displayWaveNumber	= 1;//显示在界面上的第X波，一直递增
	private int						score				= 0;//当前的分数
	private int						preWaveScore		= 0;//上一波结束时的分数
	private int						coin				= 0;
	private bool					brokenRecord		= false;

	private Mode					currentMode			= Mode.Classic;
	private ModeType				currentModeType		= ModeType.Challenge;
	
	private BaseMode[]				modeLogics 			= new BaseMode[]{ClassicMode.GetInstance(), 
																		InverseMode.GetInstance(), 
																		SurvivalMode.GetInstance(),
																		TimeRushMode.GetInstance(),
																		ColorFullMode.GetInstance(),
																		BlackJackMode.GetInstance(),
																		WordMode.GetInstance(),
																		ChimpMode.GetInstance(),
																		DualMode.GetInstance()};

	private int						rescuedTimes		= 0;

	private bool					isActivated			= false;
	private bool					isTrial				= false;
	private bool					isVIP				= false;

	private Dictionary<int, float>	giftData			= null;
	private int 					giftReceivedCount;
	private int 					giftCoolDownTime;
	private DateTime 				lastGiftTime;
	private DateTime				nextGiftTime;
	private bool					isGiftAvailableNow	= false;
	private int						nextGiftTimeLeft;

	private bool					isStartGame			= true;

	private Action<bool>			activeGameConfirmCallback	= null;
	private Action					activeGameSucceedCallback 	= null;
	
	private GameSystem.Mode 		recommendMode;
	private bool					unlockRecommendMode = false;

	public int DisplayWaveNumber
	{
		get
		{
			return displayWaveNumber;
		}
		set
		{
			displayWaveNumber = value;
			if (OnWaveNumberChanged != null)
			{
				OnWaveNumberChanged(this.DisplayWaveNumber);
			}
		}
	}

	public int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
			if (OnScoreChanged != null)
			{
				OnScoreChanged(this.Score);
			}
		}
	}

	public int Coin
	{
		get
		{
			return coin;
		}
		set
		{
			coin = value;
			if (OnCoinChanged != null)
			{
				OnCoinChanged(this.Coin);
			}
		}
	}

	public bool BrokenRecord
	{
		get
		{
			return brokenRecord;
		}
		set
		{
			brokenRecord = value;
		}
	}

	public Mode CurrentMode
	{
		get
		{
			return currentMode;
		}
		set
		{
			currentMode = value;
		}
	}
	
	public ModeType CurrentModeType
	{
		get
		{
			return currentModeType;
		}
		set
		{
			currentModeType = value;
		}
	}

	public States CurrentState
	{
		get
		{
			return currentState;
		}
	}
	
	public BaseMode CurrentModeLogic
	{
		get
		{
			return modeLogics[(int)CurrentMode];
		}
	}

	public bool AllowMultiTouch
	{
		get
		{
			return uiCamera.allowMultiTouch;
		}
		set
		{
			uiCamera.allowMultiTouch = value;
		}
	}

	public int RescuedTimes
	{
		get
		{
			return rescuedTimes;
		}
		set
		{
			rescuedTimes = value;
		}
	}

	public bool IsActivated
	{
		get
		{
			return isActivated;
		}
	}

	public bool IsTrial
	{
		get
		{
			return isTrial;
		}
		set
		{
			isTrial = value;
		}
	}

	public bool IsVIP
	{
		get
		{
			return isVIP;
		}
	}

	public bool IsGiftAvailableNow
	{
		get
		{
			return isGiftAvailableNow;
		}
	}

	public int NextGiftTimeLeft
	{
		get
		{
			return nextGiftTimeLeft;
		}
	}

	public Dictionary<int, float> GiftData
	{
		get
		{
			return giftData;
		}
	}

	public Mode RecommendMode
	{
		get
		{
			return recommendMode;
		}
		set
		{
			recommendMode = value;
		}
	}

	public bool UnlockRecommendMode
	{
		get
		{
			return unlockRecommendMode;
		}
		set
		{
			unlockRecommendMode = value;
		}
	}

	public bool BlockFullScreen
	{
		get
		{
			return fullScreenBlock.enabled;
		}
		set
		{
			fullScreenBlock.enabled = value;
		}
	}

	public static GameSystem GetInstance()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
		PlayerProfile.UpgradeProfile();
		this.Coin = PlayerProfile.LoadCoin();
		if (Config.enableHardActive) {
			this.isActivated = PlayerProfile.LoadIsActivated ();
		} else {
			this.isActivated = true;
		}
		this.isVIP = PlayerProfile.LoadIsVIP();
		this.giftData = PlayerProfile.LoadGiftData();
		iOSInterfaces.CallGameFullyLoaded();
		LoadScenes();
		AchieveData.init ();
	}

	// Use this for initialization
	void Start () {
		MergeScenes();

		fsm = new FiniteStateMachine<GameSystem, GameSystem.States>(this);
		fsm.RegisterState(stateGameInit);
		fsm.RegisterState(stateGameMenu);
		fsm.RegisterState(stateGameReady);
		fsm.RegisterState(stateGamePreview);
		fsm.RegisterState(stateGamePlay);
		fsm.RegisterState(stateWaveComplete);
		fsm.RegisterState(stateGameResult);

		ChangeState(States.GameInit);

		UMengManager.Init();
	}
	
	// Update is called once per frame
	void Update () {
		fsm.Update();

		this.CurrentModeLogic.Update();

		if (Input.GetKeyDown(KeyCode.Escape) 
		    && gameUI.vipConfirmMenu.IsActive == false 
		    && gameUI.blockConfirmMenu.IsActive == false 
		    && gameUI.rescueConfirmMenu.IsActive == false
		    && gameUI.recommendConfirmMenu.IsActive == false)
		{
			if (fsm.GetCurrentState().StateID == GameSystem.States.GamePlay)
			{
				gameUI.confirmMenu.Show(false);
				gameUI.pauseMenu.ToggleShow();
			}
			else
			{
				gameUI.confirmMenu.CloseButtonVisible = false;
				gameUI.confirmMenu.SetConfirmCallback(ConfirmQuitGame);
				gameUI.confirmMenu.SetContent(TextManager.GetText("confirm_quit_game"), null, ConfirmStyle.YesNo);
				gameUI.confirmMenu.SetYesButtonText(TextManager.GetText("no"));
				gameUI.confirmMenu.SetNoButtonText(TextManager.GetText("yes"));
				gameUI.confirmMenu.ToggleShow();
			}
			GameSoundSystem.GetInstance().PlayRandomSound();
		}

		if (isGiftAvailableNow == false)
		{
			CheckGiftTime();
		}
	}

	public void Reset()
	{
		this.realWaveIndex = 0;
		if (currentModeType == ModeType.PassLevel)
		{
			this.DisplayWaveNumber = PlayerProfile.LoadBestRecord(this.CurrentMode, this.CurrentModeType);
		}
		else
		{
			this.DisplayWaveNumber = 1;
		}
		this.Score = 0;
		this.preWaveScore = 0;
		this.RescuedTimes = 0;
		this.BrokenRecord = false;
		CurrentModeLogic.Reset();
	}

	public void InitGameCore(){
		if (currentModeType == ModeType.PassLevel)
		{
			if (this.DisplayWaveNumber < LevelData.wavesInfo[(int)currentMode].Length)
			{
				this.realWaveIndex = this.DisplayWaveNumber - 1;
			}
			else
			{
				this.realWaveIndex = LevelData.NormalLevelMaxIndex(currentMode);
			}
		}
		else if (currentModeType == ModeType.Hell)
		{
			this.realWaveIndex = LevelData.HellLevelIndex(currentMode);
		}
		Wave	wave = this.GetWave();
		gameCore.Init(wave);
	}

	public void GameEnd()
	{
		if (this.CurrentModeType == ModeType.PassLevel)
		{
			if(brokenRecord)
			{
				GameSoundSystem.GetInstance().PlayCheerSound();
			}
			LeaderboardManager.ReportScore(this.CurrentMode, this.CurrentModeType, this.DisplayWaveNumber);
		}
		else
		{
			int	bestRecord = PlayerProfile.LoadBestRecord(this.CurrentMode, this.CurrentModeType);
			if(this.Score > bestRecord){
				brokenRecord = true;
				GameSoundSystem.GetInstance().PlayCheerSound();
				PlayerProfile.SaveBestRecord(this.CurrentMode, this.CurrentModeType, this.Score);
				if (OnBestRecordChanged != null)
				{
					OnBestRecordChanged();
				}
			}

			if (isActivated)
			{
				int gainedCoin = this.Score / Constant.SCORE_TO_COIN;
				if (isVIP)
				{
					gainedCoin *= 2;
				}
				this.Coin += gainedCoin;
				PlayerProfile.SaveCoin(this.Coin);

				UMengManager.Bonus(gainedCoin, (Umeng.GA.BonusSource)UMengManager.Bonus_Source_Pass_Level);
			}
			else
			{
				//Trick Code : 使界面刷新用
				this.Coin += 0;
			}
			LeaderboardManager.ReportScore(this.CurrentMode, this.CurrentModeType, this.Score);
		}

		//友盟统计游戏结束后的数据
		Dictionary<string,string> dict = new Dictionary<string,string>();
		string keyString = "mode|modeType|wave|score";
		string valueString = string.Format("{0}|{1}|{2}|{3}", this.CurrentMode, this.CurrentModeType, this.DisplayWaveNumber, this.Score);
		dict.Add(keyString, valueString);
		UMengManager.Event(UMengManager.Event_GameEnd, dict);
	}
	
	public void ChangeState(States newState)
	{
		currentState = newState;
		fsm.ChangeState(newState);
	}

	public void StartNextWave(){
		int[] trialLevelNumberList = NetworkManager.GetInstance ().trialLevels;
		int trialLevelNumber = 20;
		if (trialLevelNumberList.Length > (int)currentMode) 
		{
			trialLevelNumber = trialLevelNumberList[(int)currentMode];
		}
		if ((isActivated == false)
			&& this.DisplayWaveNumber >= trialLevelNumber)
		{
			gameUI.confirmMenu.SetConfirmCallback(ConfirmActivateGameInTrialEndCallback);
			gameUI.confirmMenu.SetCloseCallback(JumpToGameEnd);
			gameUI.confirmMenu.SetContent(TextManager.GetText("confirm_activate_game_title"), string.Format(TextManager.GetText("confirm_activate_game_content"), Constant.ACTIVATE_GAME_BONUS), ConfirmStyle.YesClose);
			gameUI.confirmMenu.Show(true);
			return;
		}
		else if ((isTrial)
	         && this.DisplayWaveNumber >= trialLevelNumber)
		{
			if (currentMode == Mode.Inverse)
			{
				string title = TextManager.GetText("trial_end_when_activated_title");
				string content = string.Format(TextManager.GetText("trial_end_of_inverse_content"), Constant.MODE_UNLOCK_COIN[(int)currentMode]);
				gameUI.confirmMenu.SetConfirmCallback((result)=>{
					JumpToGameEnd();
				});
				gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.OnlyYes);
				gameUI.confirmMenu.Show(true);
				return;
			}
			else
			{
				int price = Constant.MODE_UNLOCK_COIN[(int)currentMode];
#if UNITY_IOS
				if (GameSystem.GetInstance().Coin < price)
				{
					string title = TextManager.GetText("trial_end_when_activated_title");
					string content = string.Format(TextManager.GetText("trial_end_when_coin_not_enough_unlock"), price);
					gameUI.confirmMenu.SetConfirmCallback(ConfirmTrialEndCallback);
					gameUI.confirmMenu.SetCloseCallback(()=>{					
						gameUI.confirmMenu.SetCloseMenuAfterClickYesEnable(true);
						JumpToGameEnd();
					});
					gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.YesClose);
					gameUI.confirmMenu.SetYesButtonText(TextManager.GetText("go_to_shop"));
					gameUI.confirmMenu.SetCloseMenuAfterClickYesEnable(false);
					gameUI.confirmMenu.Show(true);
					return;
				}
				else
#endif
				{
					string title = TextManager.GetText("trial_end_when_activated_title");
					string content = string.Format(TextManager.GetText("trial_end_when_activated_msg"), price);
					gameUI.confirmMenu.SetConfirmCallback(ConfirmTrialEndCallback);
					gameUI.confirmMenu.SetCloseCallback(JumpToGameEnd);
					gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.YesClose);
					gameUI.confirmMenu.Show(true);
					return;
				}
			}
		}

		if(this.realWaveIndex < LevelData.NormalLevelMaxIndex(currentMode)){
			this.realWaveIndex++;
		}
		this.DisplayWaveNumber++;
		
		if (this.CurrentModeType == ModeType.PassLevel)
		{
			brokenRecord = true;
			//冲关状态时，每过一关就要存储一次，避免意外结束游戏导致进度未保存
			PlayerProfile.SaveBestRecord(this.CurrentMode, this.CurrentModeType, this.DisplayWaveNumber);
			if (OnBestRecordChanged != null)
			{
				OnBestRecordChanged();
			}
			int scoreDiff = this.Score - this.preWaveScore;
			if(scoreDiff >= Constant.SCORE_TO_COIN)
			{
				int coinGained = scoreDiff / Constant.SCORE_TO_COIN;
				if (isVIP)
				{
					coinGained *= 2;
				}
				this.Coin += coinGained;
				this.preWaveScore = this.Score;
				PlayerProfile.SaveCoin(this.Coin);
				
				UMengManager.Bonus(coinGained, (Umeng.GA.BonusSource)UMengManager.Bonus_Source_Pass_Level);
			}
			LeaderboardManager.ReportScore(this.CurrentMode, this.CurrentModeType, this.DisplayWaveNumber);
		}

		StartTheWave();
	}
	
	public void StartTheWave(){
		InitGameCore();
		ChangeState(States.GamePreview);
	}

	public BaseMode GetModeLogic(Mode modeName)
	{
		return modeLogics[(int)modeName];
	}

	public bool CheckIsActivated(Action<bool> confirmCallback = null, Action succeedCallback = null)
	{
		if (isActivated == false)
		{
			activeGameConfirmCallback = confirmCallback;
			activeGameSucceedCallback = succeedCallback;
			GameSoundSystem.GetInstance().PlayRandomSound();
			gameUI.confirmMenu.SetCloseCallback(CloseActivateGameCallback);
			gameUI.confirmMenu.SetConfirmCallback(ConfirmActivateGameCallback);
			gameUI.confirmMenu.SetContent(TextManager.GetText("confirm_activate_game_title"), string.Format(TextManager.GetText("confirm_activate_game_content"), Constant.ACTIVATE_GAME_BONUS), ConfirmStyle.YesClose);
			gameUI.confirmMenu.Show(true);
		}
		return isActivated;
	}

	public void ActivateGame()
	{
		isActivated = true;
		PlayerProfile.SaveIsActivated(true);

		this.Coin += Constant.ACTIVATE_GAME_BONUS;
		PlayerProfile.SaveCoin(this.Coin);
		
		UMengManager.Bonus(Constant.ACTIVATE_GAME_BONUS, (Umeng.GA.BonusSource)UMengManager.Bonus_Source_Activate_Game);
		
		if (activeGameSucceedCallback != null)
		{
			activeGameSucceedCallback();
		}

		if(CurrentState == States.WaveComplete)
		{
			StartNextWave();
		}
	}

	public void BecomeVIP()
	{
		isVIP = true;
		PlayerProfile.SaveIsVIP(true);
		gameUI.giftMenu.RefreshGiftItem();
		gameUI.shopMenu.CheckVIPItem();
		gameUI.mainMenu.RefreshVIPButtonState();
		gameUI.giftMenu.RefreshVIPButtonState();
	}

	public int ReceiveGift()
	{
		isGiftAvailableNow = false;
		Dictionary<int, float> giftNumberProbability = NetworkManager.GetInstance ().giftNumberProbability;
		int coin = RandomTool.Chances(giftNumberProbability);
		if (isVIP)
		{
			coin *= 2;
		}
		this.Coin += coin;
		PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);

		giftReceivedCount++;
		PlayerProfile.SaveGiftReceivedCount(giftReceivedCount);

		PlayerProfile.SaveLastGiftTime(DateTime.Now);

		ReloadGiftTimeInfo();

		UMengManager.Bonus(coin, (Umeng.GA.BonusSource)UMengManager.Bonus_Source_Gift);

		return coin;
	}

	public void UnlockMode(Mode mode)
	{
		if(GameSystem.GetInstance().Coin >= Constant.MODE_UNLOCK_COIN[(int)mode])
		{
			PlayerProfile.SaveModeUnlock(mode, true);
			GameSystem.GetInstance().Coin -= Constant.MODE_UNLOCK_COIN[(int)mode];
			PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);
			
			UMengManager.Buy(UMengManager.Item_Unlock_Mode[(int)mode], 1, Constant.MODE_UNLOCK_COIN[(int)mode]);
		}
	}

	public void ReloadGiftTimeInfo()
	{
		giftReceivedCount = PlayerProfile.LoadGiftReceivedCount();
		int index = 0;
		int[] giftReceivedSection = NetworkManager.GetInstance().giftConfig.receivedSection;
		while(index < giftReceivedSection.Length)
		{
			if (giftReceivedSection[index] > giftReceivedCount)
			{
				break;
			}
			index++;
		}
		int[] giftCooldownTime = NetworkManager.GetInstance().giftConfig.cooldownTime;
		giftCoolDownTime = giftCooldownTime[index];
		
		lastGiftTime = PlayerProfile.LoadLastGiftTime();
		nextGiftTime = lastGiftTime.AddMinutes(giftCoolDownTime);
		
		int notificationId = PlayerProfile.LoadNotificationId();
		if (isStartGame)
		{
			isStartGame = false;
			if(Application.platform == RuntimePlatform.Android)
			{
				if (notificationId == -1)
				{
					notificationId = AndroidNotificationManager.instance.ScheduleLocalNotification(TextManager.GetText("gift_notification_title"), TextManager.GetText("gift_notification_content"), giftCoolDownTime * 60);
					PlayerProfile.SaveNotificationId(notificationId);
				}
			}
		}
		else
		{
			if(Application.platform == RuntimePlatform.Android)
			{
				AndroidNotificationManager.instance.CancelLocalNotification(notificationId);
				notificationId = AndroidNotificationManager.instance.ScheduleLocalNotification(TextManager.GetText("gift_notification_title"), TextManager.GetText("gift_notification_content"), giftCoolDownTime * 60);
				PlayerProfile.SaveNotificationId(notificationId);
			}
		}

		CheckGiftTime();
	}
	
	private void CheckGiftTime()
	{
		TimeSpan timeLeft = nextGiftTime - DateTime.Now;			
		nextGiftTimeLeft = (int)timeLeft.TotalSeconds;
		if (nextGiftTimeLeft < 0)
		{
			isGiftAvailableNow = true;
			gameUI.mainMenu.UpdateGiftButtonState();
		}
	}
	
	private Wave GetWave(){
		if(realWaveIndex < 0 && realWaveIndex >= LevelData.NormalLevelMaxIndex(currentMode)){
			return null;
		}
		if (currentModeType == ModeType.PassLevel)
		{
			return LevelData.GetPassLevelWaveData(currentMode, this.DisplayWaveNumber);
		}
		else
		{
			return LevelData.wavesInfo[(int)currentMode][realWaveIndex];
		}
	}
	
	private void OnApplicationPause(bool pause){
//		Debug.Log("OnApplicationPause:" + pause.ToString());
		if(pause &&
		   fsm != null && 
		   fsm.GetCurrentState() != null && 
		   fsm.GetCurrentState().StateID == GameSystem.States.GamePlay){
			gameUI.pauseMenu.Show(true);
			adTrigger.ToShow (true);
		}
	}

	private void ConfirmQuitGame(bool result)
	{
		if (result == false)
		{
			Application.Quit();
		}
	}

	void CloseActivateGameCallback()
	{
		if(activeGameConfirmCallback != null)
		{
			activeGameConfirmCallback(false);
		}
	}
	
	void ConfirmActivateGameCallback(bool result)
	{
		if (result)
		{
			IAPManager.GetInstance().Pay(IAPManager.IAPProduct.ActiveGame);
		}
		if(activeGameConfirmCallback != null)
		{
			activeGameConfirmCallback(result);
		}
	}
	
	void ConfirmActivateGameInTrialEndCallback(bool result)
	{
		if (result)
		{
			IAPManager.GetInstance().Pay(IAPManager.IAPProduct.ActiveGame);
		}
	}

	void ConfirmTrialEndCallback(bool result)
	{
		if (result)
		{
			int price = Constant.MODE_UNLOCK_COIN[(int)currentMode];
			if (GameSystem.GetInstance().Coin >= price)
			{
				isTrial = false;
				UnlockMode(currentMode);
				StartNextWave();
			}
#if UNITY_IOS
			else
			{
				gameUI.shopMenu.RegisterCloseCallback(RefreshConfirmUnlockModeMenu);
				gameUI.shopMenu.Show(true);
			}
#else
			else
			{
				for(int i = 0 ; i < ShopData.gameShopData.Length - 1 ; i++)
				{
					ShopData shopData = ShopData.gameShopData[i];
					if (shopData.number >= price)
					{
						IAPManager.GetInstance().Pay(shopData.product);
						break;
					}
				}
			}
#endif
		}
	}

	public void RefreshConfirmUnlockModeMenu()
	{
		int price = Constant.MODE_UNLOCK_COIN[(int)currentMode];
		if (GameSystem.GetInstance().Coin >= price)
		{
			string title = TextManager.GetText("trial_end_when_activated_title");
			string content = string.Format(TextManager.GetText("trial_end_when_activated_msg"), price);
			gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.YesClose);
			gameUI.confirmMenu.SetYesButtonText(TextManager.GetText("unlock"));
			gameUI.confirmMenu.SetCloseMenuAfterClickYesEnable(true);
		}
	}

	public void PayUnlockModeCallback(bool isSucceed)
	{
		if (isSucceed)
		{
			isTrial = false;
			UnlockMode(currentMode);
			StartNextWave();
		}
		else
		{
			JumpToGameEnd();
		}
	}
	
	public void PayUnlockRecommendModeCallback(bool isSucceed)
	{
		if (isSucceed)
		{
			GameSystem.GetInstance().IsTrial = false;
			GameSystem.GetInstance().UnlockMode(GameSystem.GetInstance().RecommendMode);
			gameUI.mainMenu.modeItems[(int)GameSystem.GetInstance().RecommendMode].CheckUnlock();
			gameUI.mainMenu.modeItems[(int)GameSystem.GetInstance().RecommendMode].EnterGame();
		}
	}

	public void JumpToGameEnd()
	{
		GameSoundSystem.GetInstance().StopFlipRightSound();
		ChangeState(States.GameResult);
	}

	private void LoadScenes()
	{
		Application.LoadLevelAdditive("BlockConfirmMenuScene");
		Application.LoadLevelAdditive("ConfirmMenuScene");
		Application.LoadLevelAdditive("CountDownMenuScene");
		Application.LoadLevelAdditive("DailyRewardMenuScene");
		Application.LoadLevelAdditive("GameMenuScene");
		Application.LoadLevelAdditive("GiftMenuScene");
		Application.LoadLevelAdditive("MainMenuScene");
		Application.LoadLevelAdditive("PauseMenuScene");
		Application.LoadLevelAdditive("RankMenuScene");
		Application.LoadLevelAdditive("RecommendConfirmMenuScene");
		Application.LoadLevelAdditive("RescueConfirmMenuScene");
		Application.LoadLevelAdditive("ResultMenuScene");
		Application.LoadLevelAdditive("ShopMenuScene");
		Application.LoadLevelAdditive("VipConfirmMenuScene");
	}
	
	private void MergeScenes()
	{
		GameObject[] allObjects = FindObjectsOfType<GameObject>();
		foreach(GameObject gameObject in allObjects)
		{
			if (gameObject.name.Equals(this.name) && gameObject != this.gameObject)
			{
				for(int i = 0 ; i < gameObject.transform.childCount ; i++)
				{
					Transform childTransform = gameObject.transform.GetChild(i);
					childTransform.parent = this.transform;
				}
				Destroy(NGUITools.GetRoot(gameObject));
			}
			
			if (gameObject.name.Equals(uiCamera.name) && gameObject != uiCamera.gameObject)
			{
				for(int i = 0 ; i < gameObject.transform.childCount ; i++)
				{
					Transform layerTransform = gameObject.transform.GetChild(i);
					Transform targetLayerTransform = uiCamera.transform.FindChild(layerTransform.name);
					for(int j = 0 ; j < layerTransform.childCount ; j++)
					{
						Transform layerChild = layerTransform.GetChild(j);
						layerChild.transform.parent = targetLayerTransform;
						layerChild.transform.localScale = Vector3.one;
					}
				}
				Destroy(NGUITools.GetRoot(gameObject));
			}
			
			if (gameObject.name.Equals(uiCamera1.name) && gameObject != uiCamera1.gameObject)
			{
				for(int i = 0 ; i < gameObject.transform.childCount ; i++)
				{
					Transform layerTransform = gameObject.transform.GetChild(i);
					Transform targetLayerTransform = uiCamera1.transform.FindChild(layerTransform.name);
					for(int j = 0 ; j < layerTransform.childCount ; j++)
					{
						Transform layerChild = layerTransform.GetChild(j);
						layerChild.transform.parent = targetLayerTransform;
						layerChild.transform.localScale = Vector3.one;
					}
				}
				Destroy(NGUITools.GetRoot(gameObject));
			}
		}
	}
}
