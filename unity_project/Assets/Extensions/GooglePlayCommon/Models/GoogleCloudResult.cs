////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using System;
using UnityEngine;

public class GoogleCloudResult {

	private GP_AppStateStatusCodes _response;
	private string _message;

	private int _stateKey;


	public byte[] stateData;
	public byte[] serverConflictData;
	public string resolvedVersion;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public GoogleCloudResult(string code) {
		_response = (GP_AppStateStatusCodes) System.Convert.ToInt32(code);
		_message = _response.ToString ();
	}

	public GoogleCloudResult (string code, string key) {
		_response = (GP_AppStateStatusCodes) System.Convert.ToInt32(code);
		_message = _response.ToString ();

		_stateKey = System.Convert.ToInt32 (key);
	}
	

	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public GP_AppStateStatusCodes response {
		get {
			return _response;
		}
	}
	
	public string stateDataString {
		get {
			if(stateData == null) {
				return String.Empty;
			} else {

				#if UNITY_WP8 || UNITY_METRO
				return String.Empty;
				#else
				return System.Text.Encoding.UTF8.GetString(stateData); 
				#endif
			}
			
		}
	}
	
	public string serverConflictDataString {
		get {
			if(serverConflictData == null) {
				return String.Empty;
			} else {
				#if UNITY_WP8 || UNITY_METRO
				return String.Empty;
				#else
				return System.Text.Encoding.UTF8.GetString(stateData); 
				#endif
			}
			
		}
	}

	public string message {
		get {
			return _message;
		}
	}

	public int stateKey {
		get {
			return _stateKey;
		}
	}


	public bool isSuccess  {
		get {
			return _response == GP_AppStateStatusCodes.STATUS_OK;
		}
	}

	public bool isFailure {
		get {
			return !isSuccess;
		}
	}

}


