using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public class UMengManager : MonoBehaviour {
	public static string Event_GameEnd 		= "GameEnd";
	public static string Event_EnterMode 	= "EnterMode";
	public static string Event_UnlockMode 	= "UnlockMode";
	
	public static int	 Bonus_Source_Buy_VIP			= 4;
	public static int	 Bonus_Source_Pass_Level		= 5;
	public static int	 Bonus_Source_Gift				= 6;
	public static int	 Bonus_Source_Activate_Game		= 7;

	public static string[] Item_IAP					= new string[]{
		"激活游戏",
		"VIP特权",
		"100白块",
		"200白块",
		"800白块",
		"1500白块",
		"2300白块",
		"复活"};
	
	public static string[] Item_Unlock_Mode				= new string[]{
		"解锁经典模式",
		"解锁反转模式",
		"解锁生存模式",
		"解锁限时模式",
		"解锁多彩模式",
		"解锁21点模式",
		"解锁单词模式",
		"解锁猩猩模式",
		"解锁双人模式"};
	
	public static string shareUrl 			= string.Empty;//"https://itunes.apple.com/cn/app/id909997508?mt=8";
	
	private static UMengManager	instance;
	
	public GameObject		shareScreenBlock;
	
	// Use this for initialization
	void Awake () {
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GA.StartWithAppKeyAndChannelId("53f0bdc9fd98c586a9004d4d", "App Store");
			GA.CheckUpdate();
		}
		#endif
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
		{
			GA.StartWithAppKeyAndChannelId("547aacd0fd98c56e270006fd", "Google Play");
		}
		#endif
		NetworkManager.GetInstance();
		instance = this;
	}
	
	public static void Init()
	{
		NetworkManager.GetInstance ().UpdateCache ();
		NetworkManager.GetInstance ().LoadCacheData ();
		GameSystem.GetInstance ().ReloadGiftTimeInfo ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void FullScreenBlockOnClick()
	{
		Debug.Log ("Manually Hide FullScreenBlock");
		shareScreenBlock.SetActive(false);
	}
	
	private void ShareFinishCallback(string result)
	{
		if (instance != null)
		{
			Debug.Log ("Auto Hide FullScreenBlock");
			instance.shareScreenBlock.SetActive(false);
			if(result.Equals("success"))
			{
				GameSystem.GetInstance().Coin += Constant.SHARE_GAME_BONUS;
				PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);
				string shareBonusContent = string.Format(TextManager.GetText("share_bonus_content"), Constant.SHARE_GAME_BONUS);
				GameSystem.GetInstance().gameUI.confirmMenu.SetContent(TextManager.GetText("share_bonus_title"), shareBonusContent, ConfirmStyle.OnlyYes);
				GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
			}
		}
	}		
	
	public static void StartLevel(string level)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.StartLevel(level);
		#endif
	}
	
	public static void Event(string eventId)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Event(eventId);
		#endif
	}
	
	public static void Event(string eventId, string label)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Event(eventId, label);
		#endif
	}
	
	public static void Event(string eventId, Dictionary<string, string> attributes)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Event(eventId, attributes);
		#endif
	}
	
	public static void Event(string eventId, Dictionary<string, string> attributes, int value)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Event(eventId, attributes, value);
		#endif
	}
	
	public static void Bonus(double coin, GA.BonusSource bonusSource)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Bonus(coin, bonusSource);
		#endif
	}
	
	public static void Pay(double cash, GA.PaySource paySource, double coin)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Pay(cash, paySource, coin);
		#endif
	}
	
	public static void Pay(double cash, GA.PaySource paySource, string item, int amount, double price)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Pay(cash, paySource, item, amount, price);
		#endif
	}
	
	public static void Buy(string item, int amount, double price)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		GA.Buy(item, amount, price);
		#endif
	}
	
	public static IEnumerator ShareScore(int score)
	{
		string shareContent = string.Format(TextManager.GetText("result_share_text"), score.ToString());
		Application.CaptureScreenshot("Sceenshot.png");
		string shareImagePath = Application.persistentDataPath + "/Sceenshot.png";
		yield return new WaitForSeconds(0.6f);	//wait for saving screenshot to file.

		#if UNITY_IPHONE || UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidInterfaces.CallShare(shareContent, shareImagePath, shareUrl);
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//ios上弹出分享时，点击空白处后面的按钮也会响应，需屏蔽
			if (instance != null)
			{
				Debug.Log ("Show FullScreenBlock");
				instance.shareScreenBlock.SetActive(true);
			}
			iOSInterfaces.CallShareWithScreenshot(shareContent, shareImagePath);
		}
		#endif
	}
	
	public static IEnumerator ShareGame()
	{
		string shareContent = TextManager.GetText("game_share_text");
		Application.CaptureScreenshot("Sceenshot.png");
		string shareImagePath = Application.persistentDataPath + "/Sceenshot.png";
		yield return new WaitForSeconds(0.6f);

		#if UNITY_IPHONE || UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidInterfaces.CallShare(shareContent, shareImagePath, shareUrl);
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (instance != null)
			{
				//ios上弹出分享时，点击空白处后面的按钮也会响应，需屏蔽
				Debug.Log ("Show FullScreenBlock");
				instance.shareScreenBlock.SetActive(true);
			}
			iOSInterfaces.CallShareWithScreenshot(shareContent, shareImagePath);
		}
		#endif
	}

}
