using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class SavedGamesExample : MonoBehaviour {
	


	public GameObject avatar;
	private Texture defaulttexture;
	
	public DefaultPreviewButton connectButton;
	public SA_Label playerLabel;
	
	public DefaultPreviewButton[] ConnectionDependedntButtons;

	

	


	void Start() {
		
		playerLabel.text = "Player Disconnected";
		defaulttexture = avatar.GetComponent<Renderer>().material.mainTexture;
		
		//listen for GooglePlayConnection events
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		GooglePlayConnection.instance.addEventListener(GooglePlayConnection.CONNECTION_RESULT_RECEIVED, OnConnectionResult);
		
		GooglePlaySavedGamesManager.ActionNewGameSaveRequest += ActionNewGameSaveRequest;
		GooglePlaySavedGamesManager.ActionGameSaveLoaded += ActionGameSaveLoaded;
		GooglePlaySavedGamesManager.ActionConflict += ActionConflict;

		
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			//checking if player already connected
			OnPlayerConnected ();
		} 
		
	}

	void OnDestroy() {
		if(GooglePlayConnection.HasInstance) {
			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
			GooglePlayConnection.instance.addEventListener(GooglePlayConnection.CONNECTION_RESULT_RECEIVED, OnConnectionResult);
			
		}

		if(GooglePlaySavedGamesManager.HasInstance) {
			GooglePlaySavedGamesManager.ActionNewGameSaveRequest -= ActionNewGameSaveRequest;
			GooglePlaySavedGamesManager.ActionGameSaveLoaded -= ActionGameSaveLoaded;
			GooglePlaySavedGamesManager.ActionConflict -= ActionConflict;
		}
		
	}


	
	private void ConncetButtonPress() {
		Debug.Log("GooglePlayManager State  -> " + GooglePlayConnection.state.ToString());
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			SA_StatusBar.text = "Disconnecting from Play Service...";
			GooglePlayConnection.instance.disconnect ();
		} else {
			SA_StatusBar.text = "Connecting to Play Service...";
			GooglePlayConnection.instance.connect ();
		}
	}
	

	
	

	
	void FixedUpdate() {
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			if(GooglePlayManager.instance.player.icon != null) {
				avatar.GetComponent<Renderer>().material.mainTexture = GooglePlayManager.instance.player.icon;
			}
		}  else {
			avatar.GetComponent<Renderer>().material.mainTexture = defaulttexture;
		}
		
		
		string title = "Connect";
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			title = "Disconnect";
			
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.EnabledButton();
			}
			
			
		} else {
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.DisabledButton();
				
			}
			if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED || GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED) {
				
				title = "Connect";
			} else {
				title = "Connecting..";
			}
		}
		
		connectButton.text = title;
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------


	public void CreateNewSnapshot() {
		StartCoroutine(MakeScreenshotAndSaveGameData());
	}

	private void ShowSavedGamesUI() {
		int maxNumberOfSavedGamesToShow = 5;
		GooglePlaySavedGamesManager.instance.ShowSavedGamesUI("See My Saves", maxNumberOfSavedGamesToShow);
	}


	public void LoadSavedGames() {
		GooglePlaySavedGamesManager.ActionAvailableGameSavesLoaded += ActionAvailableGameSavesLoaded;
		GooglePlaySavedGamesManager.instance.LoadAvailableSavedGames();

		SA_StatusBar.text = "Loading saved games.. ";
	}

	private void ActionAvailableGameSavesLoaded (GooglePlayResult res) {

		GooglePlaySavedGamesManager.ActionAvailableGameSavesLoaded += ActionAvailableGameSavesLoaded;
		if(res.isSuccess) {
			foreach(GP_SnapshotMeta meta in GooglePlaySavedGamesManager.instance.AvailableGameSaves) {
				Debug.Log("Meta.Title: " 					+ meta.Title);
				Debug.Log("Meta.Description: " 				+ meta.Description);
				Debug.Log("Meta.CoverImageUrl): " 			+ meta.CoverImageUrl);
				Debug.Log("Meta.LastModifiedTimestamp: " 	+ meta.LastModifiedTimestamp);

			}

			if(GooglePlaySavedGamesManager.instance.AvailableGameSaves.Count > 0) {
				GP_SnapshotMeta s =  GooglePlaySavedGamesManager.instance.AvailableGameSaves[0];
				AndroidDialog dialog = AndroidDialog.Create("Load Snapshot?", "Would you like to load " + s.Title);
				dialog.OnComplete += OnSpanshotLoadDialogComplete;
			}

		} else {
			AndroidMessage.Create("Fail", "Available Game Saves Load failed");
		}
	}

	void OnSpanshotLoadDialogComplete (AndroidDialogResult res) {
		if(res == AndroidDialogResult.YES) {
			GP_SnapshotMeta s =  GooglePlaySavedGamesManager.instance.AvailableGameSaves[0];
			GooglePlaySavedGamesManager.instance.LoadSpanshotByName(s.Title);
		}
	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void ActionNewGameSaveRequest () {
		SA_StatusBar.text = "New  Game Save Requested, Creating new save..";
		Debug.Log("New  Game Save Requested, Creating new save..");
		StartCoroutine(MakeScreenshotAndSaveGameData());

		AndroidMessage.Create("Result", "New Game Save Request");
	}

	private void ActionGameSaveLoaded (GP_SpanshotLoadResult result) {

		Debug.Log("ActionGameSaveLoaded: " + result.message);
		if(result.isSuccess) {

			Debug.Log("Snapshot.Title: " 					+ result.Snapshot.meta.Title);
			Debug.Log("Snapshot.Description: " 				+ result.Snapshot.meta.Description);
			Debug.Log("Snapshot.CoverImageUrl): " 			+ result.Snapshot.meta.CoverImageUrl);
			Debug.Log("Snapshot.LastModifiedTimestamp: " 	+ result.Snapshot.meta.LastModifiedTimestamp);

			Debug.Log("Snapshot.stringData: " 				+ result.Snapshot.stringData);
			Debug.Log("Snapshot.bytes.Length: " 			+ result.Snapshot.bytes.Length);

			AndroidMessage.Create("Snapshot Loaded", "Data: " + result.Snapshot.stringData);
		} 

		SA_StatusBar.text = "Games Loaded: " + result.message;

	}

	private void ActionGameSaveResult (GP_SpanshotLoadResult result) {
		GooglePlaySavedGamesManager.ActionGameSaveResult -= ActionGameSaveResult;
		Debug.Log("ActionGameSaveResult: " + result.message);

		if(result.isSuccess) {
			SA_StatusBar.text = "Games Saved: " + result.Snapshot.meta.Title;
		} else {
			SA_StatusBar.text = "Games Save Failed";
		}

		AndroidMessage.Create("Game Save Result", SA_StatusBar.text);
	}	

	private void ActionConflict (GP_SnapshotConflict result) {

		Debug.Log("Conflict Detected: ");

		GP_Snapshot snapshot = result.Snapshot;
		GP_Snapshot conflictSnapshot = result.ConflictingSnapshot;
		
		// Resolve between conflicts by selecting the newest of the conflicting snapshots.
		GP_Snapshot mResolvedSnapshot = snapshot;
		
		if (snapshot.meta.LastModifiedTimestamp < conflictSnapshot.meta.LastModifiedTimestamp) {
			mResolvedSnapshot = conflictSnapshot;
		}

		result.Resolve(mResolvedSnapshot);
	}


	private void OnPlayerDisconnected() {
		SA_StatusBar.text = "Player Disconnected";
		playerLabel.text = "Player Disconnected";
	}
	
	private void OnPlayerConnected() {
		SA_StatusBar.text = "Player Connected";
		playerLabel.text = GooglePlayManager.instance.player.name;
	}
	
	private void OnConnectionResult(CEvent e) {
		
		GooglePlayConnectionResult result = e.data as GooglePlayConnectionResult;
		SA_StatusBar.text = "ConnectionResul:  " + result.code.ToString();
		Debug.Log(result.code.ToString());
	}



	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	private IEnumerator MakeScreenshotAndSaveGameData() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D Screenshot = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		Screenshot.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		Screenshot.Apply();
		

		long TotalPlayedTime = 20000;
		//string currentSaveName =  "snapshotTemp-" + Random.Range(1, 281).ToString();
		string currentSaveName =  "TestingSameName";
		string description  = "Modified data at: " + System.DateTime.Now.ToString("MM/dd/yyyy H:mm:ss");


		GooglePlaySavedGamesManager.ActionGameSaveResult += ActionGameSaveResult;
		GooglePlaySavedGamesManager.instance.CreateNewSnapshot(currentSaveName, description, Screenshot, "some save data, for example you can use JSON or byte array", TotalPlayedTime);
		
		
		
		Destroy(Screenshot);
	}


}
