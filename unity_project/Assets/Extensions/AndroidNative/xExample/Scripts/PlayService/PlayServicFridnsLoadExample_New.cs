using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class PlayServicFridnsLoadExample_New : MonoBehaviour {
	

	public GameObject avatar;

	public SA_Label playerLabel;
	public DefaultPreviewButton connectButton;
	

	private Texture defaulttexture;
	public DefaultPreviewButton[] ConnectionDependedntButtons;
	public CustomPlayerUIRow[] rows;


	void Awake() {

		playerLabel.text = "Player Disconnected";
		defaulttexture = avatar.GetComponent<Renderer>().material.mainTexture;



		//listen for GooglePlayConnection events
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		GooglePlayConnection.instance.addEventListener(GooglePlayConnection.CONNECTION_RESULT_RECEIVED, OnConnectionResult);
		

	
		GooglePlayManager.ActionFriendsListLoaded +=  OnFriendListLoaded;


		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			//checking if player already connected
			OnPlayerConnected ();
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

	void Update() {

		foreach(CustomPlayerUIRow row in rows) {
			row.Disable();
		}

		if(GooglePlayConnection.state != GPConnectionState.STATE_CONNECTED) {
			return;
		} 

		int i = 0;
		foreach(string fId in GooglePlayManager.instance.friendsList) {
			GooglePlayerTemplate p = GooglePlayManager.instance.GetPlayerById(fId);
			if(p != null) {
				rows[i].playerId.text = p.playerId;
				rows[i].playerName.text = p.name;
				if(p.hasIconImage && p.icon != null) {
					rows[i].hasIcon.text = "Yes";
				} else {
					rows[i].hasIcon.text = "No";
				}

				if(p.hasHiResImage &&  p.image != null) {
					rows[i].hasImage.text = "Yes";
				} else {
					rows[i].hasImage.text = "No";
				}

				rows[i].avatar.GetComponent<Renderer>().enabled = true;
				if(p.hasIconImage && p.icon != null) {
					rows[i].avatar.GetComponent<Renderer>().material.mainTexture = p.icon;
				} else {
					rows[i].avatar.GetComponent<Renderer>().material.mainTexture = defaulttexture;
				}
			}

			i++;

			if(i > 5) {
				return;
			}
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



	public void LoadFriendsList() {
		GooglePlayManager.instance.LoadConnectedPlayers();
	}



	private void OnFriendListLoaded(GooglePlayResult result) {
		GooglePlayManager.ActionFriendsListLoaded -=  OnFriendListLoaded;
		SA_StatusBar.text = "Load Friends Result:  " + result.response.ToString();
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


}
