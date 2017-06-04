////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class AndroidNative {

	//--------------------------------------
	// Constants
	//--------------------------------------

	public const string DATA_SPLITTER = "|";
	public const string DATA_EOF = "endofline";
	


	// --------------------------------------
	// Twitter
	// --------------------------------------
	
	public static void TwitterInit(string consumer_key, string consumer_secret) {
		CallAndroidNativeBridge("TwitterInit", consumer_key, consumer_secret);
	}
	
	public static void AuthificateUser() {
		CallAndroidNativeBridge("AuthificateUser");
	}
	
	public static void LoadUserData() {
		CallAndroidNativeBridge("LoadUserData");
	}
	
	public static void TwitterPost(string status) {
		CallAndroidNativeBridge("TwitterPost", status);
	}
	
	public static void TwitterPostWithImage(string status, string data) {
		CallAndroidNativeBridge("TwitterPostWithImage", status, data);
	}
	
	public static void LogoutFromTwitter() {
		CallAndroidNativeBridge("LogoutFromTwitter");
	}




	// --------------------------------------
	// Camera And Gallery
	// --------------------------------------
	

	public static void InitCameraAPI(string folderName, int maxSize, int mode) {
		CallAndroidNativeBridge("InitCameraAPI", folderName, maxSize.ToString(), mode.ToString());
	}

	public static void SaveToGalalry(string ImageData, string name) {
		CallAndroidNativeBridge("SaveToGalalry", ImageData, name);
	}


	public static void GetImageFromGallery() {
		CallAndroidNativeBridge("GetImageFromGallery");
	}
	
	public static void GetImageFromCamera() {
		CallAndroidNativeBridge("GetImageFromCamera");
	}


	// --------------------------------------
	// Utils
	// --------------------------------------
	
	public static void isPackageInstalled(string packagename) {
		CallAndroidNativeBridge("isPackageInstalled", packagename);
	}
	
	public static void runPackage(string packagename) {
		CallAndroidNativeBridge("runPackage", packagename);
	}


	
	//--------------------------------------
	// Other Features
	//--------------------------------------
	
	public static void LoadContacts() {
		CallAndroidNativeBridge("loadAddressBook");
	}
	
	
	public static void LoadPackageInfo() {
		CallAndroidNativeBridge("LoadPackageInfo");
	}
	


	// --------------------------------------
	// Native Bridge
	// --------------------------------------


	private const string CLASS_NAME = "com.androidnative.AN_Bridge";
	
	private static void CallAndroidNativeBridge(string methodName, params object[] args) {
		AN_ProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}

}
