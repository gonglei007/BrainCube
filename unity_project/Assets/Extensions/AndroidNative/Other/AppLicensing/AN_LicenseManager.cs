using UnityEngine;
using System;
using System.Collections;

public class AN_LicenseManager : SA_Singleton<AN_LicenseManager> {
	
	public static Action<AN_LicenseRequestResult> 	OnLicenseRequestResult = 	delegate {};

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
		
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------


	public void StartLicenseRequest() {
		StartLicenseRequest(AndroidNativeSettings.Instance.base64EncodedPublicKey);
	}

	public void StartLicenseRequest(string base64PublicKey) {
		AN_LicenseManagerProxy.StartLicenseRequest (base64PublicKey);
	}

	private void OnLicenseRequestRes(string data) {
		AN_LicenseRequestResult result = (AN_LicenseRequestResult)Enum.Parse (typeof(AN_LicenseRequestResult), data);
		OnLicenseRequestResult (result);
	}
}
