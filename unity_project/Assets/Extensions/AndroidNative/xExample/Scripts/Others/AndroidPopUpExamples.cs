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
using System.Collections.Generic;

public class AndroidPopUpExamples : MonoBehaviour {


	private string rateText = "If you enjoy using Google Earth, please take a moment to rate it. Thanks for your support!";


	//example link to your app on android market
	private string rateUrl = "market://details?id=com.unionassets.android.plugin.preview";




	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	private void RateDialogPopUp() {
		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create("Rate Us", rateText, rateUrl);
		rate.OnComplete += OnRatePopUpClose;
	}

	private void DialogPopUp() {
		AndroidDialog dialog = AndroidDialog.Create("Dialog Titile", "Dialog message");
		dialog.OnComplete += OnDialogClose;
	}

	private void MessagePopUp() {
		AndroidMessage msg = AndroidMessage.Create("Message Titile", "Message message");
		msg.OnComplete += OnMessageClose;
	}

	private void ShowPreloader() {
		Invoke("HidePreloader", 2f);
		AndroidNativeUtility.ShowPreloader("Loading", "Wait 2 seconds please");
	}

	private void HidePreloader() {
		AndroidNativeUtility.HidePreloader();
	}

	private void OpenRatingPage() {
		AndroidNativeUtility.OpenAppRatingPage(rateUrl);
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	

	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private void OnRatePopUpClose(AndroidDialogResult result) {

		switch(result) {
		case AndroidDialogResult.RATED:
			Debug.Log ("RATED button pressed");
			break;
		case AndroidDialogResult.REMIND:
			Debug.Log ("REMIND button pressed");
			break;
		case AndroidDialogResult.DECLINED:
			Debug.Log ("DECLINED button pressed");
			break;
			
		}

		AN_PoupsProxy.showMessage("Result", result.ToString() + " button pressed");
	}



	private void OnDialogClose(AndroidDialogResult result) {

		//parsing result
		switch(result) {
		case AndroidDialogResult.YES:
			Debug.Log ("Yes button pressed");
			break;
		case AndroidDialogResult.NO:
			Debug.Log ("No button pressed");
			break;

		}
			
		AN_PoupsProxy.showMessage("Result", result.ToString() + " button pressed");
	}



	private void OnMessageClose() {
		AN_PoupsProxy.showMessage("Result", "Message Closed");
	}
	

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
