using UnityEngine;
using System.Collections;
using System;
using Umeng;
using ChartboostSDK;

public class ADTrigger{

	public ADTrigger(float delay, int percent){
		this.percent = percent;
		this.delay = delay;
	}

	int percent = 100;

	bool adShowed = true;
	float adTimer = 0.0f;
	float delay = 1.0f;

	public void ToShow(bool direct = false){
		if (direct) {
			ADManager.ShowInterstitialAD (this.percent);
		} else {
			adShowed = false;
		}
	}

	public void Update(){
		if (!adShowed) {
			adTimer += Time.deltaTime;
			if (adTimer > delay) {
				ADManager.ShowInterstitialAD(this.percent);
				adShowed = true;
			}
		}
	}
}

public class ADManager : MonoBehaviour {

	public static int InterstitialAdStartCount = 2;
	public static int InterstitialAdResetCount = 5;

	// Use this for initialization
	void Start () {
		UnityEngine.Random.seed = DateTime.Now.Millisecond;
#if UNITY_IPHONE || UNITY_ANDROID
		GA.UpdateOnlineConfig ();
#endif

		if (IsActiveAD ()) {
			if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android){
				Debug.Log("## Init AD modules...");
				UnityEngine.Object chartboostPrefab = Resources.Load("Prefabs/Chartboost");
				GameObject.Instantiate(chartboostPrefab);
				Chartboost.cacheInterstitial(CBLocation.Default);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static bool IsActiveAD(){
		string appVersion = "1.0.0";
#if UNITY_IPHONE
		appVersion = iOSInterfaces.getAPPVersion();
#elif UNITY_ANDROID
		appVersion = AndroidInterfaces.CallGetAppVersion();
#endif

#if UNITY_IPHONE || UNITY_ANDROID
		string currentVersionADActiveKey = "ad_switch_" + appVersion;
		
		GA.UpdateOnlineConfig ();
		string ADActiveState = GA.GetConfigParamForKey(currentVersionADActiveKey);
#else
		string ADActiveState = "0";
#endif

		bool available = true;//(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) 
			//&& (string.IsNullOrEmpty(ADActiveState) || ADActiveState == "1");
		
		Debug.Log(string.Format("AD state is {0}! version:{1} online param:{2}", available?"on":"off", appVersion, ADActiveState));
		return available;
	}
	

	public static void ShowInterstitialAD(int percent){
		Debug.Log ("Call ShowInterstitialAD");
		if (percent <= 0) {
			return;
		}
		int randomValue = UnityEngine.Random.Range(1, 100);
		Debug.Log ("rand:" + randomValue.ToString() + " percent:" + percent.ToString());
		if (randomValue > percent) {
			return;
		}
		if (!IsActiveAD ()) {
			return;
		}

		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			Chartboost.showInterstitial(CBLocation.Default);
		}
	}

	void OnEnable() {
		// Listen to all impression-related events
		Chartboost.didFailToLoadInterstitial += DidFailToLoadInterstitial;
		Chartboost.didDismissInterstitial += DidDismissInterstitial;
		Chartboost.didCloseInterstitial += DidCloseInterstitial;
		Chartboost.didClickInterstitial += DidClickInterstitial;
		Chartboost.didCacheInterstitial += DidCacheInterstitial;
		Chartboost.shouldDisplayInterstitial += ShouldDisplayInterstitial;
		Chartboost.didDisplayInterstitial += DidDisplayInterstitial;
		Chartboost.didFailToLoadMoreApps += DidFailToLoadMoreApps;
		Chartboost.didDismissMoreApps += DidDismissMoreApps;
		Chartboost.didCloseMoreApps += DidCloseMoreApps;
		Chartboost.didClickMoreApps += DidClickMoreApps;
		Chartboost.didCacheMoreApps += DidCacheMoreApps;
		Chartboost.shouldDisplayMoreApps += ShouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps += DidDisplayMoreApps;
		Chartboost.didFailToRecordClick += DidFailToRecordClick;
		Chartboost.didFailToLoadRewardedVideo += DidFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo += DidDismissRewardedVideo;
		Chartboost.didCloseRewardedVideo += DidCloseRewardedVideo;
		Chartboost.didClickRewardedVideo += DidClickRewardedVideo;
		Chartboost.didCacheRewardedVideo += DidCacheRewardedVideo;
		Chartboost.shouldDisplayRewardedVideo += ShouldDisplayRewardedVideo;
		Chartboost.didCompleteRewardedVideo += DidCompleteRewardedVideo;
		Chartboost.didDisplayRewardedVideo += DidDisplayRewardedVideo;
		Chartboost.didCacheInPlay += DidCacheInPlay;
		Chartboost.didFailToLoadInPlay += DidFailToLoadInPlay;
		Chartboost.didPauseClickForConfirmation += DidPauseClickForConfirmation;
		Chartboost.willDisplayVideo += WillDisplayVideo;
		#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow += DidCompleteAppStoreSheetFlow;
		#endif
	}

	void OnDisable() {
		// Remove event handlers
		Chartboost.didFailToLoadInterstitial -= DidFailToLoadInterstitial;
		Chartboost.didDismissInterstitial -= DidDismissInterstitial;
		Chartboost.didCloseInterstitial -= DidCloseInterstitial;
		Chartboost.didClickInterstitial -= DidClickInterstitial;
		Chartboost.didCacheInterstitial -= DidCacheInterstitial;
		Chartboost.shouldDisplayInterstitial -= ShouldDisplayInterstitial;
		Chartboost.didDisplayInterstitial -= DidDisplayInterstitial;
		Chartboost.didFailToLoadMoreApps -= DidFailToLoadMoreApps;
		Chartboost.didDismissMoreApps -= DidDismissMoreApps;
		Chartboost.didCloseMoreApps -= DidCloseMoreApps;
		Chartboost.didClickMoreApps -= DidClickMoreApps;
		Chartboost.didCacheMoreApps -= DidCacheMoreApps;
		Chartboost.shouldDisplayMoreApps -= ShouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps -= DidDisplayMoreApps;
		Chartboost.didFailToRecordClick -= DidFailToRecordClick;
		Chartboost.didFailToLoadRewardedVideo -= DidFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo -= DidDismissRewardedVideo;
		Chartboost.didCloseRewardedVideo -= DidCloseRewardedVideo;
		Chartboost.didClickRewardedVideo -= DidClickRewardedVideo;
		Chartboost.didCacheRewardedVideo -= DidCacheRewardedVideo;
		Chartboost.shouldDisplayRewardedVideo -= ShouldDisplayRewardedVideo;
		Chartboost.didCompleteRewardedVideo -= DidCompleteRewardedVideo;
		Chartboost.didDisplayRewardedVideo -= DidDisplayRewardedVideo;
		Chartboost.didCacheInPlay -= DidCacheInPlay;
		Chartboost.didFailToLoadInPlay -= DidFailToLoadInPlay;
		Chartboost.didPauseClickForConfirmation -= DidPauseClickForConfirmation;
		Chartboost.willDisplayVideo -= WillDisplayVideo;
		#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow -= DidCompleteAppStoreSheetFlow;
		#endif
	}
	
	void DidFailToLoadInterstitial(CBLocation location, CBImpressionError error) {
		Debug.Log(string.Format("didFailToLoadInterstitial: {0} at location {1}", error, location));
	}
	
	void DidDismissInterstitial(CBLocation location) {
		Debug.Log("didDismissInterstitial: " + location);
	}
	
	void DidCloseInterstitial(CBLocation location) {
		Debug.Log("didCloseInterstitial: " + location);
	}
	
	void DidClickInterstitial(CBLocation location) {
		Debug.Log("didClickInterstitial: " + location);
	}
	
	void DidCacheInterstitial(CBLocation location) {
		Debug.Log("didCacheInterstitial: " + location);
	}
	
	bool ShouldDisplayInterstitial(CBLocation location) {
		Debug.Log("shouldDisplayInterstitial: " + location);
		return true;
	}
	
	void DidDisplayInterstitial(CBLocation location){
		Debug.Log("didDisplayInterstitial: " + location);
	}
	
	void DidFailToLoadMoreApps(CBLocation location, CBImpressionError error) {
		Debug.Log(string.Format("didFailToLoadMoreApps: {0} at location: {1}", error, location));
	}
	
	void DidDismissMoreApps(CBLocation location) {
		Debug.Log(string.Format("didDismissMoreApps at location: {0}", location));
	}
	
	void DidCloseMoreApps(CBLocation location) {
		Debug.Log(string.Format("didCloseMoreApps at location: {0}", location));
	}
	
	void DidClickMoreApps(CBLocation location) {
		Debug.Log(string.Format("didClickMoreApps at location: {0}", location));
	}
	
	void DidCacheMoreApps(CBLocation location) {
		Debug.Log(string.Format("didCacheMoreApps at location: {0}", location));
	}
	
	bool ShouldDisplayMoreApps(CBLocation location) {
		Debug.Log(string.Format("shouldDisplayMoreApps at location: {0}", location));
		return true;
	}
	
	void DidDisplayMoreApps(CBLocation location){
		Debug.Log("didDisplayMoreApps: " + location);
	}
	
	void DidFailToRecordClick(CBLocation location, CBImpressionError error) {
		Debug.Log(string.Format("didFailToRecordClick: {0} at location: {1}", error, location));
	}
	
	void DidFailToLoadRewardedVideo(CBLocation location, CBImpressionError error) {
		Debug.Log(string.Format("didFailToLoadRewardedVideo: {0} at location {1}", error, location));
	}
	
	void DidDismissRewardedVideo(CBLocation location) {
		Debug.Log("didDismissRewardedVideo: " + location);
	}
	
	void DidCloseRewardedVideo(CBLocation location) {
		Debug.Log("didCloseRewardedVideo: " + location);
	}
	
	void DidClickRewardedVideo(CBLocation location) {
		Debug.Log("didClickRewardedVideo: " + location);
	}
	
	void DidCacheRewardedVideo(CBLocation location) {
		Debug.Log("didCacheRewardedVideo: " + location);
	}
	
	bool ShouldDisplayRewardedVideo(CBLocation location) {
		Debug.Log("shouldDisplayRewardedVideo: " + location);
		return true;
	}
	
	void DidCompleteRewardedVideo(CBLocation location, int reward) {
		Debug.Log(string.Format("didCompleteRewardedVideo: reward {0} at location {1}", reward, location));
	}
	
	void DidDisplayRewardedVideo(CBLocation location){
		Debug.Log("didDisplayRewardedVideo: " + location);
	}
	
	void DidCacheInPlay(CBLocation location) {
		Debug.Log("didCacheInPlay called: "+location);
	}
	
	void DidFailToLoadInPlay(CBLocation location, CBImpressionError error) {
		Debug.Log(string.Format("didFailToLoadInPlay: {0} at location: {1}", error, location));
	}
	
	void DidPauseClickForConfirmation() {
		Debug.Log("didPauseClickForConfirmation called");
	}
	
	void WillDisplayVideo(CBLocation location) {
		Debug.Log("willDisplayVideo: " + location);
	}
	
	#if UNITY_IPHONE
	void DidCompleteAppStoreSheetFlow() {
		Debug.Log("didCompleteAppStoreSheetFlow");
	}
	#endif

}