using UnityEngine;
using System;
using System.Collections;


public class AndroidApp : SA_Singleton<AndroidApp> {

	public Action OnStart =  delegate{};
	public Action OnStop =  delegate{};
	public Action OnNewIntent =  delegate{};
	public Action<AndroidActivityResult> OnActivityResult =  delegate{};


	public const string ON_START 			= "on_start";
	public const string ON_STOP				= "on_stop";
	public const string ON_NEW_INTENT 		= "on_new_intent";
	public const string ON_ACTIVITY_RESULT 	= "on_activity_result";






	//--------------------------------------
	// LISTNERS
	//--------------------------------------


	private void onStart() {
		dispatch(ON_START);
		OnStart();
	}

	private void onStop() {
		dispatch(ON_STOP);
		OnStop();
	}

	private void onNewIntent() {
		dispatch(ON_NEW_INTENT);
		OnNewIntent();
	}

	private void onActivityResult(string data) {
		string[] storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		AndroidActivityResult result =  new AndroidActivityResult(storeData[0], storeData[1]);


		dispatch(ON_ACTIVITY_RESULT, result);
		OnActivityResult(result);
	}


	void OnApplicationPause(bool IsPaused) {
		if(IsPaused) {
			onStop();
		} else {
			onStart();
		}
	}
}
