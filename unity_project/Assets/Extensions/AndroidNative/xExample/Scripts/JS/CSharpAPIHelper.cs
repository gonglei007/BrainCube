using UnityEngine;
using System.Collections;

public class CSharpAPIHelper : MonoBehaviour {



	public void ConnectToPlaySertivce() {
		//listen for GooglePlayConnection events
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);

		
		
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			//checking if player already connected
			OnPlayerConnected ();
		}  else {
			Debug.Log("Connecting....");
			GooglePlayConnection.instance.connect();
		}




	}


	private void OnPlayerConnected() {

		//Sedning Event back to JS
		gameObject.SendMessage("PlayerConnectd");
	}

	private void OnPlayerDisconnected() {

		//Sedning Event back to JS
		gameObject.SendMessage("PlayerDisconected");
	}
}
