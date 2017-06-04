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

public class TwitterAndroidUseExample : MonoBehaviour {

	

	private static bool IsUserInfoLoaded = false;
	private static bool IsAuthenticated = false;

	public Texture2D ImageToShare;
	public DefaultPreviewButton connectButton;
	public SA_Texture avatar;
	public SA_Label Location;
	public SA_Label Language;
	public SA_Label Status;
	public SA_Label Name;
	public DefaultPreviewButton[] AuthDependedButtons;



	void Awake() {



		AndroidTwitterManager.instance.OnTwitterInitedAction += OnTwitterInitedAction;
		AndroidTwitterManager.instance.OnPostingCompleteAction += OnPostingCompleteAction;
		AndroidTwitterManager.instance.OnUserDataRequestCompleteAction += OnUserDataRequestCompleteAction;
		AndroidTwitterManager.instance.OnAuthCompleteAction += OnAuthCompleteAction;




		//You can use:
		//AndroidTwitterManager.instance.Init();
		//if TWITTER_CONSUMER_KEY and TWITTER_CONSUMER_SECRET was alredy set in 
		//Window -> Mobile Social Plugin -> Edit Settings menu.


		AndroidTwitterManager.instance.Init();



	}

	void FixedUpdate() {
		if(IsAuthenticated) {
			connectButton.text = "Disconnect";
			Name.text = "Player Connected";
			foreach(DefaultPreviewButton button in AuthDependedButtons) {
				button.EnabledButton();
			}
		} else {
			foreach(DefaultPreviewButton button in AuthDependedButtons) {
				button.DisabledButton();
			}
			connectButton.text = "Connect";
			Name.text = "Player Disconnected";

			return;
		}


		if(IsUserInfoLoaded) {


			if(AndroidTwitterManager.instance.userInfo.profile_image != null) {
				avatar.texture = AndroidTwitterManager.instance.userInfo.profile_image;
			}

			Name.text = AndroidTwitterManager.instance.userInfo.name + " aka " + AndroidTwitterManager.instance.userInfo.screen_name;
			Location.text = AndroidTwitterManager.instance.userInfo.location;
			Language.text = AndroidTwitterManager.instance.userInfo.lang;
			Status.text = AndroidTwitterManager.instance.userInfo.status.text;
			
		
		}

	}

	private void Connect() {
		if(!IsAuthenticated) {
			AndroidTwitterManager.instance.AuthenticateUser();
		} else {
			LogOut();
		}
	}

	private void PostWithAuthCheck() {
		AndroidTwitterManager.instance.PostWithAuthCheck("Hello, I'am posting this from my app");
	}

	private void PostNativeScreenshot() {
		StartCoroutine(PostTWScreenshot());
	}

	private void PostMSG() {
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share",  "twi");
	}


	private void PostImage() {
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", ImageToShare,  "twi");
	}
	
	
	
	private IEnumerator PostTWScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", tex,  "twi");
		
		Destroy(tex);
		
	}


	private void LoadUserData() {
		AndroidTwitterManager.instance.LoadUserData();
	}
	
	private void PostMessage() {
		AndroidTwitterManager.instance.Post("Hello, I'am posting this from my app");
	}
	
	private void PostScreehShot() {
		StartCoroutine(PostScreenshot());
	}


	// --------------------------------------
	// EVENTS
	// --------------------------------------



	void OnUserDataRequestCompleteAction (TWResult result) {
		if(result.IsSucceeded) {
			IsUserInfoLoaded = true;
			AndroidTwitterManager.instance.userInfo.LoadProfileImage();
			AndroidTwitterManager.instance.userInfo.LoadBackgroundImage();
		} else {
			Debug.Log("Opps, user data load failed, something was wrong");
		}
	}
	

	void OnPostingCompleteAction (TWResult result) {
		if(result.IsSucceeded) {
			Debug.Log("Congrats. You just posted something to Twitter!");
		} else {
			Debug.Log("Oops! Posting failed. Something went wrong.");
		}
	}

	void OnAuthCompleteAction (TWResult result) {
		if(result.IsSucceeded) {
			//user authed
			OnAuth();
		}
	}

	void OnTwitterInitedAction (TWResult result) {

		if(AndroidTwitterManager.instance.IsAuthed) {
			//user authed
			OnAuth();
		}
	}

	private void OnAuth() {
		IsAuthenticated = true;
	}



	// --------------------------------------
	// Aplication Only API Maehtods
	// --------------------------------------


	private void RetrieveTimeLine() {
		TW_UserTimeLineRequest r =  TW_UserTimeLineRequest.Create();
		r.addEventListener(BaseEvent.COMPLETE, OnTimeLineRequestComplete);
		r.AddParam("screen_name", "unity3d");
		r.AddParam("count", "1");
		r.Send();
	}


	private void UserLookUpRequest() {
		TW_UsersLookUpRequest r =  TW_UsersLookUpRequest.Create();
		r.addEventListener(BaseEvent.COMPLETE, OnLookUpRequestComplete);
		r.AddParam("screen_name", "unity3d");
		r.Send();
	}


	private void FriedsidsRequest() {
		TW_FriendsIdsRequest r =  TW_FriendsIdsRequest.Create();
		r.addEventListener(BaseEvent.COMPLETE, OnIdsLoaded);
		r.AddParam("screen_name", "unity3d");
		r.Send();
	}

	private void FollowersidsRequest() {
		TW_FollowersIdsRequest r =  TW_FollowersIdsRequest.Create();
		r.addEventListener(BaseEvent.COMPLETE, OnIdsLoaded);
		r.AddParam("screen_name", "unity3d");
		r.Send();
	}

	private void TweetSearch() {
		TW_SearchTweetsRequest r =  TW_SearchTweetsRequest.Create();
		r.addEventListener(BaseEvent.COMPLETE, OnSearchRequestComplete);
		r.AddParam("q", "@noradio");
		r.AddParam("count", "1");
		r.Send();
	}



	
	// --------------------------------------
	// Events
	// --------------------------------------

	private void OnIdsLoaded(CEvent e) {
		
		TW_APIRequstResult result = e.data as TW_APIRequstResult;
		
		
		if(result.IsSucceeded) {

			
			AN_PoupsProxy.showMessage("Ids Request Succeeded", "Totals ids loaded: " + result.ids.Count);
			Debug.Log(result.ids.Count);
		} else {
			Debug.Log(result.responce);
			AN_PoupsProxy.showMessage("Ids Request Failed", result.responce);
		}
	}


	private void OnLookUpRequestComplete(CEvent e) {
	
		TW_APIRequstResult result = e.data as TW_APIRequstResult;
		
		
		if(result.IsSucceeded) {
			string msg = "User Id: ";
			msg+= result.users[0].id;
			msg+= "\n";
			msg+= "User Name:" + result.users[0].name;
			
			AN_PoupsProxy.showMessage("User Info Loaded", msg);
			Debug.Log(msg);
		} else {
			Debug.Log(result.responce);
			AN_PoupsProxy.showMessage("User Info Failed", result.responce);
		}
	}


	private void OnSearchRequestComplete(CEvent e) {
		TW_APIRequstResult result = e.data as TW_APIRequstResult;
		
		
		if(result.IsSucceeded) {
			string msg = "Tweet text:" + "\n";
			msg+= result.tweets[0].text;
			
			AN_PoupsProxy.showMessage("Tweet Search Request Succeeded", msg);
			Debug.Log(msg);
		} else {
			Debug.Log(result.responce);
			AN_PoupsProxy.showMessage("Tweet Search Request Failed", result.responce);
		}
		
	}


	private void OnTimeLineRequestComplete(CEvent e) {
		TW_APIRequstResult result = e.data as TW_APIRequstResult;


		if(result.IsSucceeded) {
			string msg = "Last Tweet text:" + "\n";
			msg+= result.tweets[0].text;
			
			AN_PoupsProxy.showMessage("Time Line Request Succeeded", msg);
			Debug.Log(msg);
		} else {
			Debug.Log(result.responce);
			AN_PoupsProxy.showMessage("Time Line Request Failed", result.responce);
		}

	}


	// --------------------------------------
	// PRIVATE METHODS
	// --------------------------------------

	private IEnumerator PostScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		AndroidTwitterManager.instance.Post("My app ScreehShot", tex);
		
		Destroy(tex);
		
	}

	private void LogOut() {
		IsUserInfoLoaded = false;
		
		IsAuthenticated = false;

		AndroidTwitterManager.instance.LogOut();
	}
}
