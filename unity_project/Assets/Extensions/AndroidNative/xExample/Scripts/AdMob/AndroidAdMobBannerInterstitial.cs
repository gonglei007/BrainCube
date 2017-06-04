using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//Attach the script to the empty gameobject on your sceneS
public class AndroidAdMobBannerInterstitial : MonoBehaviour {


	public string InterstitialUnityId;


	// --------------------------------------
	// Unity Events
	// --------------------------------------
	
	void Awake() {

		if(AndroidAdMobController.instance.IsInited) {
			if(!AndroidAdMobController.instance.InterstisialUnitId.Equals(InterstitialUnityId)) {
				AndroidAdMobController.instance.SetInterstisialsUnitID(InterstitialUnityId);
			} 
		} else {
			AndroidAdMobController.instance.Init(InterstitialUnityId);
		}


	}

	void Start() {
		ShowBanner();
	}




	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	public void ShowBanner() {
		AndroidAdMobController.instance.StartInterstitialAd();
	}



	// --------------------------------------
	// GET / SET
	// --------------------------------------



	public string sceneBannerId {
		get {
			return Application.loadedLevelName + "_" + this.gameObject.name;
		}
	}

	
}
