////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class BillingResult  {


	private int _response;
	private string _message;

	private GooglePurchaseTemplate _purchase = null;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------



	public BillingResult(int code, string msg, GooglePurchaseTemplate p) : this(code, msg) {
		_purchase = p;
	}


	public BillingResult(int code, string msg) {
		_response = code;
		_message = msg;
	}



	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public GooglePurchaseTemplate purchase {
		get {
			return _purchase;
		}
	}


	public int response {
		get {
			return _response;
		}
	}


	public string message {
		get {
			return _message;
		}
	}
	

	public bool isSuccess  {
		get {
			return _response == BillingResponseCodes.BILLING_RESPONSE_RESULT_OK;
		}
	}

	public bool isFailure {
		get {
			return !isSuccess;
		}
	}


}
