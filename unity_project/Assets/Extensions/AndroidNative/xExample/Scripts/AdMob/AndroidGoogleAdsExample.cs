////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class AndroidGoogleAdsExample : MonoBehaviour {



	
	//replace with your ids
	private const string MY_BANNERS_AD_UNIT_ID		 = "ca-app-pub-6101605888755494/1824764765"; 
	private const string MY_INTERSTISIALS_AD_UNIT_ID =  "ca-app-pub-6101605888755494/3301497967"; 

	
	private GoogleMobileAdBanner banner1;
	private GoogleMobileAdBanner banner2;

	private bool IsInterstisialsAdReady = false;


	public DefaultPreviewButton ShowIntersButton;

	public DefaultPreviewButton[] b1CreateButtons;
	public DefaultPreviewButton b1Hide;
	public DefaultPreviewButton b1Show;
	public DefaultPreviewButton b1Refresh;
	public DefaultPreviewButton ChangePost1;
	public DefaultPreviewButton ChangePost2;
	public DefaultPreviewButton b1Destroy;


	public DefaultPreviewButton[] b2CreateButtons;
	public DefaultPreviewButton b2Hide;
	public DefaultPreviewButton b2Show;
	public DefaultPreviewButton b2Refresh;
	public DefaultPreviewButton b2Destroy;



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Start() {


		AndroidAdMobController.instance.Init(MY_BANNERS_AD_UNIT_ID);

		//If yoi whant to use Interstisial ad also, you need to set additional ad unin id for Interstisial as well
		AndroidAdMobController.instance.SetInterstisialsUnitID(MY_INTERSTISIALS_AD_UNIT_ID);

		
		//Optional, add data for better ad targeting
		AndroidAdMobController.instance.SetGender(GoogleGender.Male);
		AndroidAdMobController.instance.AddKeyword("game");
		AndroidAdMobController.instance.SetBirthday(1989, AndroidMonth.MARCH, 18);
		AndroidAdMobController.instance.TagForChildDirectedTreatment(false);

		//Causes a device to receive test ads. The deviceId can be obtained by viewing the logcat output after creating a new ad
		//AndroidAdMobController.instance.AddTestDevice("6B9FA8031AEFDC4758B7D8987F77A5A6");


		AndroidAdMobController.instance.addEventListener(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_LOADED, OnInterstisialsLoaded);
		AndroidAdMobController.instance.addEventListener(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_OPENED, OnInterstisialsOpen);

		//listening for InApp Event
		//You will only receive in-app purchase (IAP) ads if you specifically configure an IAP ad campaign in the AdMob front end.
		AndroidAdMobController.instance.addEventListener(GoogleMobileAdEvents.ON_AD_IN_APP_REQUEST, OnInAppRequest);


	}


	private void StartInterstitialAd() {
		AndroidAdMobController.instance.StartInterstitialAd ();
	}

	private void LoadInterstitialAd() {
		AndroidAdMobController.instance.LoadInterstitialAd ();
	}

	private void ShowInterstitialAd() {
		AndroidAdMobController.instance.ShowInterstitialAd ();
	}


	private void CreateBannerCustomPos() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(300, 100, GADBannerSize.BANNER);
	}

	private void CreateBannerUpperLeft() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.UpperLeft, GADBannerSize.BANNER);
	}

	private void CreateBannerUpperCneter() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.UpperCenter, GADBannerSize.BANNER);
	}

	private void CreateBannerBottomLeft() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.LowerLeft, GADBannerSize.BANNER);
	}

	private void CreateBannerBottomCenter() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.BANNER);
	}

	private void CreateBannerBottomRight() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.LowerRight, GADBannerSize.BANNER);
	}

	private void B1Hide() {
		banner1.Hide();
	}


	private void B1Show() {
		banner1.Show();
	}

	private void B1Refresh() {
		banner1.Refresh();
	}

	private void B1Destrouy() {
		AndroidAdMobController.instance.DestroyBanner(banner1.id);
		banner1 = null;
	}


	private void SmartTOP() {
		banner2 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.UpperCenter, GADBannerSize.SMART_BANNER);
	}

	private void SmartBottom() {
		banner2 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
	}

	
	private void B2Hide() {
		banner2.Hide();
	}
	
	
	private void B2Show() {
		banner2.Show();
	}
	
	private void B2Refresh() {
		banner2.Refresh();
	}
	
	private void B2Destrouy() {
		AndroidAdMobController.instance.DestroyBanner(banner2.id);
		banner2 = null;
	}

	private void ChnagePostToMiddle() {
		banner1.SetBannerPosition(TextAnchor.MiddleCenter);
	}

	private void ChangePostRandom() {
		banner1.SetBannerPosition(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
	}




	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	


	void FixedUpdate() {
		if(IsInterstisialsAdReady) {
			ShowIntersButton.EnabledButton();
		} else {
			ShowIntersButton.DisabledButton();
		}

		if(banner1 != null) {
			foreach(DefaultPreviewButton pb in b1CreateButtons) {
				pb.DisabledButton();
			}

			b1Destroy.EnabledButton();

			if(banner1.IsLoaded) {
				b1Refresh.EnabledButton();
				ChangePost1.EnabledButton();
				ChangePost2.EnabledButton();
				if(banner1.IsOnScreen) {
					b1Hide.EnabledButton();
					b1Show.DisabledButton();
				} else {
					b1Hide.DisabledButton();
					b1Show.EnabledButton();
				}
			} else { 
				b1Refresh.DisabledButton();
				ChangePost1.DisabledButton();
				ChangePost2.DisabledButton();
				b1Hide.DisabledButton();
				b1Show.DisabledButton();
			}



		} else {
			foreach(DefaultPreviewButton pb in b1CreateButtons) {
				pb.EnabledButton();
			}

			b1Hide.DisabledButton();
			b1Show.DisabledButton();
			b1Refresh.DisabledButton();
			b1Destroy.DisabledButton();
		}





		if(banner2 != null) {
			foreach(DefaultPreviewButton pb in b2CreateButtons) {
				pb.DisabledButton();
			}
			
			b2Destroy.EnabledButton();
			
			if(banner2.IsLoaded) {
				b2Refresh.EnabledButton();
				if(banner2.IsOnScreen) {
					b2Hide.EnabledButton();
					b2Show.DisabledButton();
				} else {
					b2Hide.DisabledButton();
					b2Show.EnabledButton();
				}
			} else { 
				b2Refresh.DisabledButton();
				b2Hide.DisabledButton();
				b2Show.DisabledButton();
			}
			
			
			
		} else {
			foreach(DefaultPreviewButton pb in b2CreateButtons) {
				pb.EnabledButton();
			}
			
			b2Hide.DisabledButton();
			b2Show.DisabledButton();
			b2Refresh.DisabledButton();
			b2Destroy.DisabledButton();
		}


	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnInterstisialsLoaded() {
		IsInterstisialsAdReady = true;
	}

	private void OnInterstisialsOpen() {
		IsInterstisialsAdReady = false;
	}

	private void OnInAppRequest(CEvent e) {
		//getting product id
		string productId = (string) e.data;
		AN_PoupsProxy.showMessage ("In App Request", "In App Request for product Id: " + productId + " received");


		//Then you should perfrom purchase  for this product id, using this or another game billing plugin
		//Once the purchase is complete, you should call RecordInAppResolution with one of the constants defined in GADInAppResolution:

		AndroidAdMobController.instance.RecordInAppResolution(GADInAppResolution.RESOLUTION_SUCCESS);

	}


	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
