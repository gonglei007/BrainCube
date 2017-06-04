using System;
using UnityEngine;
using System.Collections;

public class AndroidNativeUtility : SA_Singleton<AndroidNativeUtility> {


	//Events
	public static string PACKAGE_CHECK_RESPONCE = "package_check_responce";


	//Actions
	public Action<AN_PackageCheckResult> OnPackageCheckResult = delegate{};

	
	//--------------------------------------
	// Init
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
	
	public void CheckIsPackageInstalled(string packageName) {
		AndroidNative.isPackageInstalled(packageName);
	}

	public void RunPackage(string packageName) {
		AndroidNative.runPackage(packageName);
	}


	//--------------------------------------
	// Static Methods
	//--------------------------------------

	public static void ShowPreloader(string title, string message) {
		AN_PoupsProxy.ShowPreloader(title, message);
	}
	
	public static void HidePreloader() {
		AN_PoupsProxy.HidePreloader();
	}


	public static void OpenAppRatingPage(string url) {
		AN_PoupsProxy.OpenAppRatePage(url);
	}



	public static void HideCurrentPopup() {
		AN_PoupsProxy.HideCurrentPopup();
	}


	//--------------------------------------
	// Events
	//--------------------------------------

	private void OnPacakgeFound(string packageName) {
		AN_PackageCheckResult result = new AN_PackageCheckResult(packageName, true);
		OnPackageCheckResult(result);
		dispatch(PACKAGE_CHECK_RESPONCE, result);
	}

	private void OnPacakgeNotFound(string packageName) {
		AN_PackageCheckResult result = new AN_PackageCheckResult(packageName, false);
		OnPackageCheckResult(result);
		dispatch(PACKAGE_CHECK_RESPONCE, result);
	}




}

