////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AndroidAdMobController : SA_Singleton<AndroidAdMobController>, GoogleMobileAdInterface  {

	
	private bool _IsInited = false ;
	private Dictionary<int, AndroidADBanner> _banners; 

	
	private string _BannersUunitId;
	private string _InterstisialUnitId;


	//Actions
	private Action _OnInterstitialLoaded 			= delegate {};
	private Action _OnInterstitialFailedLoading 	= delegate {};
	private Action _OnInterstitialOpened 			= delegate {};
	private Action _OnInterstitialClosed 			= delegate {};
	private Action _OnInterstitialLeftApplication  	= delegate {};
	private Action<string> _OnAdInAppRequest		= delegate {};



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}



	public void Init(string ad_unit_id) {
		if(_IsInited) {
			Debug.LogWarning ("Init shoudl be called only once. Call ignored");
			return;
		}
		_IsInited = true;
		_BannersUunitId 	= ad_unit_id;
		_InterstisialUnitId = ad_unit_id;

		_banners =  new Dictionary<int, AndroidADBanner>();

		AN_GoogleAdProxy.InitMobileAd(ad_unit_id);
	}


	public void Init(string banners_unit_id, string interstisial_unit_id) {
		if(_IsInited) {
			Debug.LogWarning ("Init shoudl be called only once. Call ignored");
			return;
		}
		
		Init(banners_unit_id);
		SetInterstisialsUnitID(interstisial_unit_id);
	}




	public void SetBannersUnitID(string ad_unit_id) {
		_BannersUunitId = ad_unit_id;
		AN_GoogleAdProxy.ChangeBannersUnitID(ad_unit_id);
	}

	public void SetInterstisialsUnitID(string ad_unit_id) {
		_InterstisialUnitId = ad_unit_id;
		AN_GoogleAdProxy.ChangeInterstisialsUnitID(ad_unit_id);
	}




	//--------------------------------------
	//  BUILDER METHODS
	//--------------------------------------



	//Add a keyword for targeting purposes.
	public void AddKeyword(string keyword)  {
		if(!_IsInited) {
			Debug.LogWarning ("AddKeyword shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.AddKeyword(keyword);
	}


	public void SetBirthday(int year, AndroidMonth month, int day)  {
		if(!_IsInited) {
			Debug.LogWarning ("SetBirthday shoudl be called only after Init function. Call ignored");
			return;
		}
		
		AN_GoogleAdProxy.SetBirthday(year, (int) month, day);
	}

	public void TagForChildDirectedTreatment(bool tagForChildDirectedTreatment)  {
		if(!_IsInited) {
			Debug.LogWarning ("TagForChildDirectedTreatment shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.TagForChildDirectedTreatment(tagForChildDirectedTreatment);
	}



	//Causes a device to receive test ads. The deviceId can be obtained by viewing the logcat output after creating a new ad.
	public void AddTestDevice(string deviceId) {
		if(!_IsInited) {
			Debug.LogWarning ("AddTestDevice shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.AddTestDevice(deviceId);
	}


	private const string DEVICES_SEPARATOR = ",";
	//Causes a device to receive test ads. The deviceId can be obtained by viewing the logcat output after creating a new ad.
	public void AddTestDevices(params string[] ids) {
		if(!_IsInited) {
			Debug.LogWarning ("AddTestDevice shoudl be called only after Init function. Call ignored");
			return;
		}

		if(ids.Length == 0) {
			return;
		}


		AN_GoogleAdProxy.AddTestDevices(string.Join(DEVICES_SEPARATOR, ids));
	}



	//Set the user's gender for targeting purposes. This should be GADGenger.GENDER_MALE, GADGenger.GENDER_FEMALE, or GADGenger.GENDER_UNKNOWN
	public void SetGender(GoogleGender gender) {
		if(!_IsInited) {
			Debug.LogWarning ("SetGender shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.SetGender((int) gender);
	}




	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public GoogleMobileAdBanner CreateAdBanner(TextAnchor anchor, GADBannerSize size)  {
		if(!_IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}

		AndroidADBanner bannner = new AndroidADBanner(anchor, size, GADBannerIdFactory.nextId);
		_banners.Add(bannner.id, bannner);

		return bannner;
		
	}


	public GoogleMobileAdBanner CreateAdBanner(int x, int y, GADBannerSize size)  {
		if(!_IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		
		AndroidADBanner bannner = new AndroidADBanner(x, y, size, GADBannerIdFactory.nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
		
	}



	public void DestroyBanner(int id) {
		if(_banners != null) {
			if(_banners.ContainsKey(id)) {

				AndroidADBanner banner = _banners[id];
				if(banner.IsLoaded) {
					_banners.Remove(id);
					AN_GoogleAdProxy.DestroyBanner(id);
				} else {
					banner.DestroyAfterLoad();
				}

			}
		}
	}

	
	public void StartInterstitialAd() {
		if(!_IsInited) {
			Debug.LogWarning ("StartInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.StartInterstitialAd();
	}
	
	public void LoadInterstitialAd() {
		if(!_IsInited) {
			Debug.LogWarning ("LoadInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.LoadInterstitialAd();
	}
	
	public void ShowInterstitialAd() {
		if(!_IsInited) {
			Debug.LogWarning ("ShowInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}

		AN_GoogleAdProxy.ShowInterstitialAd();
	}


	public void RecordInAppResolution(GADInAppResolution resolution) {
		AN_GoogleAdProxy.RecordInAppResolution((int) resolution);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public GoogleMobileAdBanner GetBanner(int id) {
		if(_banners.ContainsKey(id)) {
			return _banners[id];
		} else {
			Debug.LogWarning("Banner id: " + id.ToString() + " not found");
			return null;
		}
	}


	public List<GoogleMobileAdBanner> banners {
		get {

			List<GoogleMobileAdBanner> allBanners =  new List<GoogleMobileAdBanner>();
			if(_banners ==  null) {
				return allBanners;
			}

			foreach(KeyValuePair<int, AndroidADBanner> entry in _banners) {
				allBanners.Add(entry.Value);
			}

			return allBanners;


		}
	}

	public bool IsInited {
		get {
			return _IsInited;
		}
	}

	public string BannersUunitId {
		get {
			return _BannersUunitId;
		}
	}

	public string InterstisialUnitId {
		get {
			return _InterstisialUnitId;
		}
	}


	//--------------------------------------
	//  Actions 
	//--------------------------------------

	public Action OnInterstitialLoaded {
		get {
			return _OnInterstitialLoaded;
		}

		set {
			_OnInterstitialLoaded = value;
		}
	}

	public Action OnInterstitialFailedLoading {
		get {
			return _OnInterstitialFailedLoading;
		}
		
		set {
			_OnInterstitialFailedLoading = value;
		}
	}


	public Action OnInterstitialOpened {
		get {
			return _OnInterstitialOpened;
		}
		
		set {
			_OnInterstitialOpened = value;
		}
	}

	public Action OnInterstitialClosed {
		get {
			return _OnInterstitialClosed;
		}
		
		set {
			_OnInterstitialClosed = value;
		}
	}


	public Action OnInterstitialLeftApplication {
		get {
			return _OnInterstitialLeftApplication;
		}
		
		set {
			_OnInterstitialLeftApplication = value;
		}
	}


	public Action<string> OnAdInAppRequest {
		get {
			return _OnAdInAppRequest;
		}
		
		set {
			_OnAdInAppRequest = value;
		}
	}
	

	//--------------------------------------
	//  EVENTS BANNER AD
	//--------------------------------------
	

	private void OnBannerAdLoaded(string data)  {

		string[] bannerData;
		bannerData = data.Split(AndroidNative.DATA_SPLITTER [0]);


		int id = System.Convert.ToInt32(bannerData[0]);
		int w = System.Convert.ToInt32(bannerData[1]);
		int h = System.Convert.ToInt32(bannerData[2]);

		AndroidADBanner banner = GetBanner(id) as AndroidADBanner;
		if(banner != null) {
			banner.SetDimentions(w, h);
			banner.OnBannerAdLoaded();
		}
	
	}
	
	private void OnBannerAdFailedToLoad(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		AndroidADBanner banner = GetBanner(id) as AndroidADBanner;
		if(banner != null) {
			banner.OnBannerAdFailedToLoad();
		}
	}
	
	private void OnBannerAdOpened(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		AndroidADBanner banner = GetBanner(id) as AndroidADBanner;
		if(banner != null) {
			banner.OnBannerAdOpened();
		}
	}

	private void OnBannerAdClosed(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		AndroidADBanner banner = GetBanner(id) as AndroidADBanner;
		if(banner != null) {
			banner.OnBannerAdClosed();
		}
	}

	private void OnBannerAdLeftApplication(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		AndroidADBanner banner = GetBanner(id) as AndroidADBanner;
		if(banner != null) {
			banner.OnBannerAdLeftApplication();
		}
	}


	
	//--------------------------------------
	//  EVENTS INTERSTITIAL AD
	//--------------------------------------

	
	private void OnInterstitialAdLoaded()  {
		_OnInterstitialLoaded();
		dispatch(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_LOADED);
	}
	
	private void OnInterstitialAdFailedToLoad() {
		_OnInterstitialFailedLoading();
		dispatch(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_FAILED_LOADING);
	}
	
	private void OnInterstitialAdOpened() {
		_OnInterstitialOpened();
		dispatch(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_OPENED);
	}
	
	private void OnInterstitialAdClosed() {
		_OnInterstitialClosed();
		dispatch(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_CLOSED);
	}
	
	private void OnInterstitialAdLeftApplication() {
		_OnInterstitialLeftApplication();
		dispatch(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_LEFT_APPLICATION);
	}
	
	//--------------------------------------
	//  GENERAL EVENTS
	//--------------------------------------

	private void OnInAppPurchaseRequested(string productId) {
		_OnAdInAppRequest(productId);
		dispatch(GoogleMobileAdEvents.ON_AD_IN_APP_REQUEST, productId);
	}



	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
