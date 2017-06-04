////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 


public class GooglePlayResult {


	private GP_GamesStatusCodes _response;
	private string _message;




	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public GooglePlayResult(GP_GamesStatusCodes code) {
		SetCode(code);
	}

	public GooglePlayResult(string code) {
		SetCode((GP_GamesStatusCodes) System.Convert.ToInt32(code));

	}

	private void SetCode(GP_GamesStatusCodes code) {
		_response = code;
		_message = _response.ToString ();
	}



	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public GP_GamesStatusCodes response {
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
			return _response == GP_GamesStatusCodes.STATUS_OK;
		}
	}

	public bool isFailure {
		get {
			return !isSuccess;
		}
	}


		 
}