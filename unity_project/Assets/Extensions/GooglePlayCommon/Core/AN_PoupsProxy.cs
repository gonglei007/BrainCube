using UnityEngine;
using System.Collections;

public class AN_PoupsProxy  {


	private const string CLASS_NAME = "com.androidnative.popups.PopUpsManager";
	
	private static void CallActivityFunction(string methodName, params object[] args) {
		AN_ProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}


	//--------------------------------------
	//  MESSAGING
	//--------------------------------------
	
	
	public static void showDialog(string title, string message) {
		showDialog (title, message, "Yes", "No");
	}
	
	public static void showDialog(string title, string message, string yes, string no) {
		CallActivityFunction("ShowDialog", title, message, yes, no);
	}
	
	
	public static void showMessage(string title, string message) {
		showMessage (title, message, "Ok");
	}
	
	
	public static void showMessage(string title, string message, string ok) {
		CallActivityFunction("ShowMessage", title, message, ok);
	}
	
	public static void OpenAppRatePage(string url) {
		CallActivityFunction("OpenAppRatingPage", url);
	}
	
	
	public static void showRateDialog(string title, string message, string yes, string laiter, string no) {
		CallActivityFunction("ShowRateDialog", title, message, yes, laiter, no);
	}
	
	public static void ShowPreloader(string title, string message) {
		CallActivityFunction("ShowPreloader",  title, message);
	}
	
	public static void HidePreloader() {
		CallActivityFunction("HidePreloader");
	}

	public static void HideCurrentPopup() {
		CallActivityFunction("HideCurrentPopup");
	}





}
