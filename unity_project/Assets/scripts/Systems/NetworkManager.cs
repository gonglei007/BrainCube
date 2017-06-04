using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Umeng;
using JsonFx.Json;

public class NetworkManager {

	private static NetworkManager instance = null;

	public static NetworkManager GetInstance()
	{
		if (instance == null) 
		{
			#if UNITY_IPHONE || UNITY_ANDROID
			GA.UpdateOnlineConfig ();
			#endif
			instance = new NetworkManager();
		}
		return instance;
	}

	public GiftManager.GiftConfig giftConfig = new GiftManager.GiftConfig();
	public Dictionary<int, float> giftNumberProbability = Constant.GIFT_NUMBER_PROBABILITY;
	public int[] trialLevels = Constant.MODE_TRIAL_LIMIT;
	
	public void LoadCacheData()
	{
		giftConfig = GetGiftConfig ();
		giftNumberProbability = GetGiftProbability ();
		trialLevels = GetTrialLevels ();
	}

	public bool IsActiveAD(){
		string appVersion = GetAPPVersion();

		#if UNITY_IPHONE || UNITY_ANDROID
		string currentVersionADActiveKey = "ad_switch_" + appVersion;
		
		string ADActiveState = GA.GetConfigParamForKey(currentVersionADActiveKey);
		#else
		string ADActiveState = "0";
		#endif
		
		bool available = (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) 
			&& (string.IsNullOrEmpty(ADActiveState) || ADActiveState == "1");
		
		Debug.Log(string.Format("AD state is {0}! version:{1} online param:{2}", available?"on":"off", appVersion, ADActiveState));
		return available;
	}

	public bool IsSNSAvailable()
	{
		string appVersion = GetAPPVersion();

		#if UNITY_IPHONE || UNITY_ANDROID
		string currentVersionSNSActiveKey = "sns_switch_" + appVersion;
		
		string SNSActiveState = GA.GetConfigParamForKey(currentVersionSNSActiveKey);
		#else
		string SNSActiveState = "0";
		#endif
		bool available = (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			&& (string.IsNullOrEmpty(SNSActiveState) || SNSActiveState == "1") 
				&& LocalVersion.local == LocalVersion.Local.CN_ZH 
				&& KTPlay.IsEnabled();
		Debug.Log(string.Format("SNS state is {0}! version:{1} online param:{2}", available?"on":"off", appVersion, SNSActiveState));
		return available;
	}

	/**
	 * Get config params and cache to local
	 */
	public void UpdateCache()
	{
		UpdateCacheTrialLevels ();
		UpdateCacheGiftConfig ();
		UpdateCacheGiftProbability ();
	}

	private void UpdateCacheTrialLevels()
	{
#if UNITY_IPHONE || UNITY_ANDROID
		string appVersion = GetAPPVersion();
		
		string currentVersionTrialConfigValue = string.Empty;
		string currentVersionTrialConfigKey = "trial_config_" + appVersion;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			currentVersionTrialConfigValue = GA.GetConfigParamForKey(currentVersionTrialConfigKey);
		} else {
			currentVersionTrialConfigValue = JsonWriter.Serialize(trialLevels);
		}
		PlayerProfile.UpdateCacheTrialConfig (currentVersionTrialConfigValue);
#endif
	}

	private int[] GetTrialLevels()
	{
		int[] trialConfig;
		string config = PlayerProfile.LoadCacheTrialConfig ();
		if (config == string.Empty) {
			trialConfig = Constant.MODE_TRIAL_LIMIT;
		} else 
		{
			try{
				trialConfig = JsonReader.Deserialize<int[]>(config);
			}
			catch(JsonDeserializationException e)
			{
				Debug.LogWarning("exception:" + e.ToString());
				trialConfig = Constant.MODE_TRIAL_LIMIT;
			}
			// verify data
			if(trialConfig.Length < Constant.MODE_TRIAL_LIMIT.Length){
				trialConfig = Constant.MODE_TRIAL_LIMIT;
			}
		}
		return trialConfig;
	}
	
	private void UpdateCacheGiftConfig()
	{
#if UNITY_IPHONE || UNITY_ANDROID
		string appVersion = GetAPPVersion();
		
		string currentVersionGiftConfigValue = string.Empty;
		string currentVersionGiftConfigKey = "gift_config_" + appVersion;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			currentVersionGiftConfigValue = GA.GetConfigParamForKey (currentVersionGiftConfigKey);
		} else {
			currentVersionGiftConfigValue= JsonWriter.Serialize(new GiftManager.GiftConfig ());
		}
		PlayerProfile.UpdateCacheGiftConfig (currentVersionGiftConfigValue);
#endif
	}
	
	private GiftManager.GiftConfig GetGiftConfig()
	{
		GiftManager.GiftConfig giftConfig = null;
		string config = PlayerProfile.LoadCacheGiftConfig ();
		if (config == string.Empty) {
			giftConfig = new GiftManager.GiftConfig ();
		} else 
		{
			try{
				giftConfig = JsonReader.Deserialize<GiftManager.GiftConfig>(config);
			}
			catch(JsonDeserializationException e)
			{
				Debug.LogWarning("exception:" + e.ToString());
				giftConfig = new GiftManager.GiftConfig ();
			}
		}
		return giftConfig;
	}
	
	private void UpdateCacheGiftProbability()
	{
#if UNITY_IPHONE || UNITY_ANDROID
		string appVersion = GetAPPVersion();
		
		string value = string.Empty;
		string key = "gift_probability_" + appVersion;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			value = GA.GetConfigParamForKey (key);
		} else {
			value= JsonWriter.Serialize(giftNumberProbability);
		}
		PlayerProfile.UpdateCacheGiftProbability (value);
#endif
	}
	
	private Dictionary<int, float> GetGiftProbability()
	{
		Dictionary<int, float> giftProbability = null;
		string config = PlayerProfile.LoadCacheGiftProbability ();
		if (config == string.Empty) {
			giftProbability = giftNumberProbability;
		} else 
		{
			try{
				giftProbability = new Dictionary<int, float>();
				Dictionary<string, string> tempDics = JsonReader.Deserialize<Dictionary<string, string>>(config);
				foreach(KeyValuePair<string, string> item in tempDics)
				{
					giftProbability[Convert.ToInt32(item.Key)] = (float)Convert.ToDouble(item.Value);
				}
			}
			catch(JsonDeserializationException e)
			{
				Debug.LogWarning("exception:" + e.ToString());
				giftProbability = giftNumberProbability;
			}
		}
		return giftProbability;
	}

	//private 
	private string GetAPPVersion()
	{
		string appVersion = "1.0.0";
		#if UNITY_IPHONE
		appVersion = iOSInterfaces.getAPPVersion();
		#elif UNITY_ANDROID
		appVersion = AndroidInterfaces.CallGetAppVersion();
		#endif
		return appVersion;
	}
}
