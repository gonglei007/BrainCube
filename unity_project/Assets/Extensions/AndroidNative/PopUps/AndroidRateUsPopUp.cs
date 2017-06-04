////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class AndroidRateUsPopUp : BaseAndroidPopup {
	


	public string yes;
	public string later;
	public string no;
	public string url;

	public Action<AndroidDialogResult> OnComplete = delegate {} ;

	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static AndroidRateUsPopUp Create(string title, string message, string url) {
		return Create(title, message, url, "Rate app", "Later", "No, thanks");
	}
	
	public static AndroidRateUsPopUp Create(string title, string message, string url, string yes, string later, string no) {
		AndroidRateUsPopUp rate = new GameObject("AndroidRateUsPopUp").AddComponent<AndroidRateUsPopUp>();
		rate.title = title;
		rate.message = message;
		rate.url = url;

		rate.yes = yes;
		rate.later = later;
		rate.no = no;

		rate.init();
			
		return rate;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void init() {
		AN_PoupsProxy.showRateDialog(title, message, yes, later, no);
	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onPopUpCallBack(string buttonIndex) {
		int index = System.Convert.ToInt16(buttonIndex);
		switch(index) {
			case 0: 
				AN_PoupsProxy.OpenAppRatePage(url);
				OnComplete(AndroidDialogResult.RATED);
				dispatch(BaseEvent.COMPLETE, AndroidDialogResult.RATED);
				break;
			case 1:
				OnComplete(AndroidDialogResult.REMIND);
				dispatch(BaseEvent.COMPLETE, AndroidDialogResult.REMIND);
				break;
			case 2:
				OnComplete(AndroidDialogResult.DECLINED);
				dispatch(BaseEvent.COMPLETE, AndroidDialogResult.DECLINED);
				break;
		}
		
		
		
		Destroy(gameObject);
	} 
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
