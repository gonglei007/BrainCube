using UnityEngine;
using System.Collections;

public class AnOtherFeaturesPreview : MonoBehaviour {

	public GameObject image;
	public Texture2D helloWorldTexture;





	public void SaveToGalalry() {
		AndroidCamera.instance.OnImageSaved += OnImageSaved;
		AndroidCamera.instance.SaveImageToGallery(helloWorldTexture);

	}

	public void SaveScreenshot() {
		AndroidCamera.instance.OnImageSaved += OnImageSaved;
		AndroidCamera.instance.SaveScreenshotToGallery();

	}


	public void GetImageFromGallery() {
		AndroidCamera.instance.OnImagePicked += OnImagePicked;
		AndroidCamera.instance.GetImageFromGallery();
	}
	
	
	
	public void GetImageFromCamera() {
		AndroidCamera.instance.OnImagePicked += OnImagePicked;
		AndroidCamera.instance.GetImageFromCamera();
	}



	public void CheckAppInstalation() {
		AndroidNativeUtility.instance.OnPackageCheckResult += OnPackageCheckResult;
		AndroidNativeUtility.instance.CheckIsPackageInstalled("com.google.android.youtube");
	}

	public void RunApp() {
		AndroidNativeUtility.instance.RunPackage("com.google.android.youtube");
	}


	public void CheckAppLicense() {
		AN_LicenseManager.OnLicenseRequestResult += LicenseRequestResult;
		AN_LicenseManager.instance.StartLicenseRequest (AndroidNativeSettings.Instance.base64EncodedPublicKey);
		SA_StatusBar.text =  "Get App License Request STARTED";
	}


	private void LicenseRequestResult(AN_LicenseRequestResult result) {
		SA_StatusBar.text =  "App License Status: " + result.ToString();
		AndroidMessage.Create("License Check Result: ", "AN_LicenseRequestResult: " +  result.ToString());
	}


	private void EnableImmersiveMode() {
		ImmersiveMode.instance.EnableImmersiveMode();
	}
	



	private void LoadAppInfo() {

		AndroidAppInfoLoader.instance.addEventListener (AndroidAppInfoLoader.PACKAGE_INFO_LOADED, OnPackageInfoLoaded);
		AndroidAppInfoLoader.instance.LoadPackageInfo ();
	}


	private void LoadAdressBook() {
		AddressBookController.instance.LoadContacts ();
		AddressBookController.instance.OnContactsLoadedAction += OnContactsLoaded;

	}





	void OnPackageCheckResult (AN_PackageCheckResult res) {
		if(res.IsSucceeded) {
			AN_PoupsProxy.showMessage("On Package Check Result" , "Application  " + res.packageName + " is installed on this device");
		} else {
			AN_PoupsProxy.showMessage("On Package Check Result" , "Application  " + res.packageName + " is not installed on this device");
		}

		AndroidNativeUtility.instance.OnPackageCheckResult -= OnPackageCheckResult;
	}



	void OnContactsLoaded () {
		AddressBookController.instance.OnContactsLoadedAction -= OnContactsLoaded;
		AN_PoupsProxy.showMessage("On Contacts Loaded" , "Andress book has " + AddressBookController.instance.contacts.Count + " Contacts");
	}
	

	private void OnImagePicked(AndroidImagePickResult result) {
		Debug.Log("OnImagePicked");
		if(result.IsSucceeded) {
			image.GetComponent<Renderer>().material.mainTexture = result.image;
		}

		AndroidCamera.instance.OnImagePicked -= OnImagePicked;
	}

	void OnImageSaved (GallerySaveResult result) {

		AndroidCamera.instance.OnImageSaved -= OnImageSaved;

		if(result.IsSucceeded) {
			AN_PoupsProxy.showMessage("Saved", "Image saved to gallery \n" + "Path: " + result.imagePath);
			SA_StatusBar.text =  "Image saved to gallery";
		} else {
			AN_PoupsProxy.showMessage("Failed", "Image save to gallery failed");
			SA_StatusBar.text =  "Image save to gallery failed";
		}

	}

	private void OnPackageInfoLoaded() {
		AndroidAppInfoLoader.instance.removeEventListener (AndroidAppInfoLoader.PACKAGE_INFO_LOADED, OnPackageInfoLoaded);

		string msg = "";
		msg += "versionName: " + AndroidAppInfoLoader.instance.PacakgeInfo.versionName + "\n";
		msg += "versionCode: " + AndroidAppInfoLoader.instance.PacakgeInfo.versionCode + "\n";
		msg += "packageName" + AndroidAppInfoLoader.instance.PacakgeInfo.packageName + "\n";
		msg += "lastUpdateTime:" + System.Convert.ToString(AndroidAppInfoLoader.instance.PacakgeInfo.lastUpdateTime) + "\n";
		msg += "sharedUserId" + AndroidAppInfoLoader.instance.PacakgeInfo.sharedUserId + "\n";
		msg += "sharedUserLabel"  + AndroidAppInfoLoader.instance.PacakgeInfo.sharedUserLabel;

		AN_PoupsProxy.showMessage("App Info Loaded", msg);
	}

}
