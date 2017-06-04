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

public class GoogleCloudUseExample : MonoBehaviour {

	public GameObject[] hideOnConnect;
	public GameObject[] showOnConnect;

	void Awake() {
		GoogleCloudManager.instance.addEventListener (GoogleCloudManager.ALL_STATES_LOADED, OnAllLoaded);

		GoogleCloudManager.instance.addEventListener (GoogleCloudManager.STATE_LOADED,   OnStateUpdated);
		GoogleCloudManager.instance.addEventListener (GoogleCloudManager.STATE_RESOLVED, OnStateUpdated);
		GoogleCloudManager.instance.addEventListener (GoogleCloudManager.STATE_UPDATED,  OnStateUpdated);

		GoogleCloudManager.instance.addEventListener (GoogleCloudManager.STATE_CONFLICT,  OnStateConflict);

		GooglePlayConnection.instance.connect ();
	}

	void FixedUpdate() {
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			foreach(GameObject o in hideOnConnect) {
				o.SetActive(false);
			}

			foreach(GameObject o in showOnConnect) {
				o.SetActive(true);
			}
		} else {
			foreach(GameObject o in hideOnConnect) {
				o.SetActive(true);
			}
			
			foreach(GameObject o in showOnConnect) {
				o.SetActive(false);
			}
		}
	}


	private void LoadAllStates() {
		GoogleCloudManager.instance.loadAllStates ();
	}

	private void LoadState() {
		GoogleCloudManager.instance.loadState (GoogleCloudSlot.SLOT_0);
	}

	private void UpdateState() {
		string msg = "Hello bytes data";
		System.Text.UTF8Encoding  encoding = new System.Text.UTF8Encoding();
		byte[] data = encoding.GetBytes(msg);
		GoogleCloudManager.instance.updateState (GoogleCloudSlot.SLOT_0, data);
	}

	private void DeleteState() {
		GoogleCloudManager.instance.deleteState(GoogleCloudSlot.SLOT_0);
		GoogleCloudManager.instance.addEventListener (GoogleCloudManager.STATE_DELETED, OnStateDeleted);
	}



	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnStateConflict(CEvent e) {
		GoogleCloudResult result = e.data as GoogleCloudResult;
		AN_PoupsProxy.showMessage ("OnStateUpdated", result.message 
		                           + "\n State ID: " + result.stateKey 
		                           + "\n State Data: " + result.stateData
		                           + "\n State Conflict: " + result.serverConflictData
		                           + "\n State resolve: " + result.resolvedVersion);

		//Resolving conflict with our local data
		//you should create your own resolve logic for your game. Read more about resolving conflict on Android developer website

		GoogleCloudManager.instance.resolveState (result.stateKey, result.stateData, result.resolvedVersion);
	}



	private void OnStateUpdated(CEvent e) {
		GoogleCloudResult result = e.data as GoogleCloudResult;

		AN_PoupsProxy.showMessage ("State Updated", result.message + "\n State ID: " + result.stateKey + "\n State Data: " + result.stateDataString);
	}


	private void OnAllLoaded(CEvent e) {
		GoogleCloudResult result = e.data as GoogleCloudResult;
		AN_PoupsProxy.showMessage ("All States Loaded", result.message + "\n" + "Total states: " + GoogleCloudManager.instance.states.Count);
	}

	private void OnStateDeleted(CEvent e) {
		GoogleCloudManager.instance.removeEventListener (GoogleCloudManager.STATE_DELETED, OnStateDeleted);
		GoogleCloudResult result = e.data as GoogleCloudResult;


		AN_PoupsProxy.showMessage ("KeyDeleted", result.message + "\n for state key: " + result.stateKey.ToString());
	}

	
}
