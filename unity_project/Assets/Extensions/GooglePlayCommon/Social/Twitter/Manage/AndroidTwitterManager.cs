////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////
 
using UnityEngine;
using System;
using System.Collections;

public class AndroidTwitterManager : SA_Singleton<AndroidTwitterManager>, TwitterManagerInterface {


	private bool _IsAuthed = false;
	private bool _IsInited = false;

	private TwitterUserInfo _userInfo;


	//Actinos
	public Action<TWResult> OnTwitterInitedAction 				= delegate {};
	public Action<TWResult> OnAuthCompleteAction 				= delegate {};
	public Action<TWResult> OnPostingCompleteAction 			= delegate {};
	public Action<TWResult> OnUserDataRequestCompleteAction 	= delegate {};


	// --------------------------------------
	// INITIALIZATION
	// --------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	public void Init() {
		Init(SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY, SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET);
	}

	public void Init(string consumer_key, string consumer_secret) {
		if(_IsInited) {
			return;
		}

		_IsInited = true;
		AndroidNative.TwitterInit(consumer_key, consumer_secret);
	}


	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	


	public void AuthenticateUser() {
		if(_IsAuthed) {
			OnAuthSuccess();
		} else {
			AndroidNative.AuthificateUser();
		}
	}

	public void LoadUserData() {
		if(_IsAuthed) {
			AndroidNative.LoadUserData();
		} else {
			Debug.LogWarning("Auth user before loadin data, fail event generated");

			TWResult res =  new TWResult(false, null);
			dispatch(TwitterEvents.USER_DATA_FAILED_TO_LOAD, res);
			OnUserDataRequestCompleteAction(res);
		}
	}
	
	public void Post(string status) {
		if(!_IsAuthed) {
			Debug.LogWarning("Auth user before posting data, fail event generated");
			TWResult res =  new TWResult(false, null);
			dispatch(TwitterEvents.POST_FAILED, res);
			OnPostingCompleteAction(res);
			return;
		} 

		AndroidNative.TwitterPost(status);
	}

	public void Post(string status, Texture2D texture) {

		if(!_IsAuthed) {
			Debug.LogWarning("Auth user before posting data, fail event generated");
			TWResult res =  new TWResult(false, null);
			dispatch(TwitterEvents.POST_FAILED, res);
			OnPostingCompleteAction(res);
			return;
		} 


		byte[] val = texture.EncodeToPNG();
		string bytesString = System.Convert.ToBase64String (val);

		AndroidNative.TwitterPostWithImage(status, bytesString);
	}


	public TwitterPostingTask PostWithAuthCheck(string status)  {
		return PostWithAuthCheck(status, null);
	}

	public TwitterPostingTask PostWithAuthCheck(string status, Texture2D texture) {
		TwitterPostingTask task =  TwitterPostingTask.Cretae();
		task.Post(status, texture, this);
		return task;

	}

	public void LogOut() {
		_IsAuthed = false;
		AndroidNative.LogoutFromTwitter();
	}

	// --------------------------------------
	// GET / SET
	// --------------------------------------

	public bool IsAuthed {
		get {
			return _IsAuthed;
		}
	}

	public bool IsInited {
		get {
			return _IsInited;
		}
	}

	public TwitterUserInfo userInfo {
		get {
			return _userInfo;
		}
	}


	
	// --------------------------------------
	// EVENTS
	// --------------------------------------



	private void OnInited(string data) {
		if(data.Equals("1")) {
			_IsAuthed = true;
		}

		TWResult res =  new TWResult(true, null);
		dispatch(TwitterEvents.TWITTER_INITED, res);
		OnTwitterInitedAction(res);
	}

	private void OnAuthSuccess() {
		_IsAuthed = true;
		TWResult res =  new TWResult(true, null);
		dispatch(TwitterEvents.AUTHENTICATION_SUCCEEDED, res);
		OnAuthCompleteAction(res);
	}


	private void OnAuthFailed() {
		TWResult res =  new TWResult(false, null);
		dispatch(TwitterEvents.AUTHENTICATION_FAILED, res);
		OnAuthCompleteAction(res);
	}

	private void OnPostSuccess() {
		TWResult res =  new TWResult(true, null);
		dispatch(TwitterEvents.POST_SUCCEEDED, res);
		OnPostingCompleteAction(res);
	}
	
	
	private void OnPostFailed() {
		TWResult res =  new TWResult(false, null);
		dispatch(TwitterEvents.POST_FAILED, res);
		OnPostingCompleteAction(res);
	}


	private void OnUserDataLoaded(string data) {
		_userInfo =  new TwitterUserInfo(data);

		TWResult res =  new TWResult(true, data);
		dispatch(TwitterEvents.USER_DATA_LOADED, res);
		OnUserDataRequestCompleteAction(res);


	}


	private void OnUserDataLoadFailed() {
		TWResult res =  new TWResult(false, null);
		dispatch(TwitterEvents.USER_DATA_FAILED_TO_LOAD, res);
		OnUserDataRequestCompleteAction(res);
	}


}

