using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class PlayerProfile {
	public static string KeyBestScore			= "BestScore_{0}_{1}";
	public static string KeyCoin				= "Coin";
	public static string KeyModeUnlock			= "ModeUnlock_{0}";
	public static string KeyModeEntered			= "ModeEntered_{0}";
	public static string KeyMute				= "Mute";
	public static string KeyEnterMainMenuTimes	= "EnterMainMenuTimes";
	public static string KeyBgmState			= "BgmState_{0}";
	public static string KeyGameMusicState		= "GameMusic_{0}";
	public static string KeyDailyRewardDays		= "DailyRewardDays";
	public static string KeyLastRewardTime		= "LastRewardTime";
	public static string KeyIsActivated			= "IsTrialVersion";
	public static string KeyGiftData			= "GiftData";
	public static string KeyGiftReceivedCount	= "GiftReceivedCount";
	public static string KeyLastGiftTime		= "LastGiftTime";
	public static string KeyIsVIP				= "IsVIP";
	public static string KeyNotificationId		= "NotificationId";

	public static string KeyCacheTrialConfig	= "CacheTrialConfig";
	public static string KeyCacheGiftConfig		= "CacheGiftConfig";
	public static string KeyCacheGiftProbability = "CacheGiftProbability";

	public static string KeyPurchasingList		= "PurchasingList";

	public static bool	 Use_Encrypt			= true;

	public static void UpgradeProfile()
	{
		foreach(GameSystem.Mode mode in Enum.GetValues(typeof(GameSystem.Mode)))
		{
			string key = string.Format("BestScore_{0}", (int)mode);
			string encrypedKey = Use_Encrypt ? SecurityUtility.GetInstance().EncryptString(key) : key;
			if (PlayerPrefs.HasKey(encrypedKey))
			{
				int score = LoadIntValue(key, 0);
				SaveBestRecord(mode, GameSystem.ModeType.Challenge, score);
				PlayerPrefs.DeleteKey(encrypedKey);
			}
		}
	}
	
	public static void ClearProfile() {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
	
	#region Game Related Methods
	public static void SaveBestRecord(GameSystem.Mode mode, GameSystem.ModeType modeType, int record){
		string finalKey = string.Format(KeyBestScore, (int)mode, (int)modeType);
		SaveIntValue(finalKey, record);
	}

	public static int LoadBestRecord(GameSystem.Mode mode, GameSystem.ModeType modeType){
		string finalKey = string.Format(KeyBestScore, (int)mode, (int)modeType);
		int defaultValue = (modeType == GameSystem.ModeType.PassLevel ? 1 : 0);
		return LoadIntValue(finalKey, defaultValue);
	}

	public static void SaveCoin(int coin)
	{
		SaveIntValue(KeyCoin, coin);
	}
	
	public static int LoadCoin(){
		return LoadIntValue(KeyCoin, 0);
	}

	public static void SaveModeUnlock(GameSystem.Mode mode, bool unlock){
		string finalKey = string.Format(KeyModeUnlock, (int)mode);
		SaveBoolValue(finalKey, unlock);
	}
	
	public static bool LoadModeUnlock(GameSystem.Mode mode){
		string finalKey = string.Format(KeyModeUnlock, (int)mode);
		if (mode == GameSystem.Mode.Classic || mode == GameSystem.Mode.Dual)
		{
			return true;
		}
		else
		{
			return LoadBoolValue(finalKey, false);
		}
	}

	public static void SaveModeEntered(GameSystem.Mode mode, bool entered)
	{
		string finalKey = string.Format(KeyModeEntered, (int)mode);
		SaveBoolValue(finalKey, entered);
	}

	public static bool LoadModeEntered(GameSystem.Mode mode)
	{
		string finalKey = string.Format(KeyModeEntered, (int)mode);
		if (mode == GameSystem.Mode.Classic || mode == GameSystem.Mode.Dual)
		{
			return true;
		}
		else
		{
			return LoadBoolValue(finalKey, false);
		}
	}

	public static void SaveEnterMainMenuTimes(int times){
		SaveIntValue(KeyEnterMainMenuTimes, times);
	}

	public static int LoadEnterMainMenuTimes(){
		return LoadIntValue (KeyEnterMainMenuTimes, ADManager.InterstitialAdStartCount);
	}

	public static void SaveMute(bool mute)
	{
		SaveBoolValue(KeyMute, mute);
	}

	public static bool LoadMute()
	{
		return LoadBoolValue(KeyMute, false);
	}

	public static void SaveBgmState(int index, bool enabled)
	{
		string finalKey = string.Format(KeyBgmState, index);
		SaveBoolValue(finalKey, enabled);
	}

	public static bool LoadBgmState(int index)
	{
		string finalKey = string.Format(KeyBgmState, index);
		return LoadBoolValue(finalKey, index == 0);
	}
	
	public static void SaveGameMusicState(int index, bool enabled)
	{
		string finalKey = string.Format(KeyGameMusicState, index);
		SaveBoolValue(finalKey, enabled);
	}
	
	public static bool LoadGameMusicState(int index)
	{
		string finalKey = string.Format(KeyGameMusicState, index);
		return LoadBoolValue(finalKey, true);
	}
	public static void SaveDailyRewardDays(int continuesDay)
	{
		SaveIntValue(KeyDailyRewardDays, continuesDay);
	}

	public static int LoadDailyRewardDays()
	{
		return LoadIntValue(KeyDailyRewardDays, 1);
	}

	public static void SaveLastRewardTime(DateTime dateTime)
	{
		SaveStringValue(KeyLastRewardTime, dateTime.ToString("yyyy-MM-dd"));
	}

	public static DateTime LoadLastRewardTime()
	{
		string timeString = LoadStringValue(KeyLastRewardTime);
		if (string.IsNullOrEmpty(timeString))
		{
			timeString = "1970-01-01";
		}
		return DateTime.Parse(timeString);
	}

	public static void SaveIsActivated(bool isActivated)
	{
		SaveBoolValue(KeyIsActivated, isActivated);
	}

	public static bool LoadIsActivated()
	{
		return LoadBoolValue(KeyIsActivated, false);
	}

	public static void SaveGiftData(Dictionary<int, float> giftData)
	{
		string[] dataStrings = new string[giftData.Count*2];
		int index = 0;
		foreach(KeyValuePair<int, float> kvp in giftData)
		{
			if (index < dataStrings.Length - 1)
			{
				dataStrings[index] = string.Format("{0},{1},", kvp.Key, kvp.Value);
			}
			else
			{
				dataStrings[index] = string.Format("{0},{1}", kvp.Key, kvp.Value);
			}
		}
		string dataString = string.Join(",", dataStrings);
		SaveStringValue(KeyGiftData, dataString);
	}
	
	public static void SaveGiftData(string giftData)
	{
		SaveStringValue(KeyGiftData, giftData);
	}

	public static Dictionary<int, float> LoadGiftData()
	{
		string dataString = LoadStringValue(KeyGiftData);

		if (string.IsNullOrEmpty(dataString))
		{
			return Constant.GIFT_NUMBER_PROBABILITY;
		}
		else
		{
			string[] dataStrings = dataString.Split(new char[]{','});
			Dictionary<int, float> giftData = new Dictionary<int, float>(dataStrings.Length / 2);

			for(int i = 0 ; i < dataStrings.Length - 1 ; i+=2)
			{
				int number = Convert.ToInt32(dataStrings[i]);
				float probability = Convert.ToSingle(dataStrings[i+1]);
				giftData.Add(number, probability);
			}

			return giftData;
		}
	}

	public static void SaveGiftReceivedCount(int giftTimes)
	{
		SaveIntValue(KeyGiftReceivedCount, giftTimes);
	}

	public static int LoadGiftReceivedCount()
	{
		return LoadIntValue(KeyGiftReceivedCount, 0);
	}

	public static void SaveLastGiftTime(DateTime dateTime)
	{		
		SaveStringValue(KeyLastGiftTime, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
	}

	public static DateTime LoadLastGiftTime()
	{
		DateTime dateTime;
		string timeString = LoadStringValue(KeyLastGiftTime);
		if (string.IsNullOrEmpty(timeString))
		{
			dateTime = DateTime.Now;
			SaveLastGiftTime(dateTime);
		}
		else
		{
			dateTime = DateTime.Parse(timeString);
		}
		return dateTime;
	}

	public static void SaveIsVIP(bool isVIP)
	{
		SaveBoolValue(KeyIsVIP, isVIP);
	}

	public static bool LoadIsVIP()
	{
		return LoadBoolValue(KeyIsVIP, false);
	}

	public static void SaveNotificationId(int notificationId)
	{
		SaveIntValue(KeyNotificationId, notificationId);
	}

	public static int LoadNotificationId()
	{
		return LoadIntValue(KeyNotificationId, -1);
	}
	#endregion

	#region Cache Data Methods
	public static void UpdateCacheTrialConfig(string config)
	{
		SaveStringValue (KeyCacheTrialConfig, config);
	}

	public static string LoadCacheTrialConfig()
	{
		return LoadStringValue (KeyCacheTrialConfig);
	}
	
	public static void UpdateCacheGiftConfig(string config)
	{
		SaveStringValue (KeyCacheGiftConfig, config);
	}
	
	public static string LoadCacheGiftConfig()
	{
		return LoadStringValue (KeyCacheGiftConfig);
	}

	public static void UpdateCacheGiftProbability(string config)
	{
		SaveStringValue (KeyCacheGiftProbability, config);
	}
	
	public static string LoadCacheGiftProbability()
	{
		return LoadStringValue (KeyCacheGiftProbability);
	}

	#endregion

	#region Private Methods
	private static void SaveBoolValue(string key, bool finalValue)
	{
		SaveIntValue(key, finalValue ? 1 : 0);
	}

	private static void SaveIntValue(string key, int finalValue)
	{
		SaveStringValue(key, finalValue.ToString());
	}

	private static void SaveLongValue(string key, long finalValue)
	{
		SaveStringValue(key, finalValue.ToString());
	}

	private static void SaveStringValue(string key, string finalValue)
	{
		string encrypedKey = Use_Encrypt ? SecurityUtility.GetInstance().EncryptString(key) : key;
		string encryptedValue = Use_Encrypt ? CombineEncryptKeyAndValue(key, finalValue) : finalValue;
		PlayerPrefs.SetString(encrypedKey, encryptedValue);
		PlayerPrefs.Save();
	}

	private static bool LoadBoolValue(string key, bool defaultValue)
	{
		string savedString = LoadStringValue(key);
		if (string.IsNullOrEmpty(savedString) == false)
		{
			int realValue = int.Parse(savedString);
			return (realValue == 1);
		}
		else
		{				
			SaveBoolValue(key, defaultValue);
			return defaultValue;
		}
	}

	private static int LoadIntValue(string key, int defaultValue)
	{
		string savedString = LoadStringValue(key);
		if (string.IsNullOrEmpty(savedString) == false)
		{
			int realValue = int.Parse(savedString);
			return realValue;
		}
		else
		{				
			SaveIntValue(key, defaultValue);
			return defaultValue;
		}
	}

	public static long LoadLongValue(string key, long defaultValue)
	{
		string savedString = LoadStringValue(key);
		if (string.IsNullOrEmpty(savedString) == false)
		{
			long realValue = long.Parse(savedString);
			return realValue;
		}
		else
		{				
			SaveLongValue(key, defaultValue);
			return defaultValue;
		}
	}

	private static string LoadStringValue(string key)
	{
		string encrypedKey = Use_Encrypt ? SecurityUtility.GetInstance().EncryptString(key) : key;
		if (PlayerPrefs.HasKey(encrypedKey))
		{
			string savedString = PlayerPrefs.GetString(encrypedKey);
			if (Use_Encrypt)
			{
				string[] decryptStrings = SplitDecryptKeyAndValue(savedString);
				if (decryptStrings != null && decryptStrings.Length == 2)
				{
					if (decryptStrings[0].Equals(key))
					{
						return decryptStrings[1];
					}
					else
					{
						return String.Empty;
					}
				}
				else
				{
					return String.Empty;
				}
			}
			else
			{
				return savedString;
			}
		}
		else
		{
			return String.Empty;
		}
	}

	private static string CombineEncryptKeyAndValue(string key, string val)
	{
		return SecurityUtility.GetInstance().EncryptString(key + "@" + val);
	}

	private static string[] SplitDecryptKeyAndValue(string encryptString)
	{
		if (string.IsNullOrEmpty(encryptString) == false)
		{
			string decryptedString = SecurityUtility.GetInstance().DecryptString(encryptString);
			return decryptedString.Split(new char[]{'@'});
		}
		else
		{
			return null;
		}
	}
	#endregion
}