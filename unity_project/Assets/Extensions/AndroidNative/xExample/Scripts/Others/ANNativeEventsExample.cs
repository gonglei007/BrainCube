using UnityEngine;
using System.Collections;

public class ANNativeEventsExample : MonoBehaviour {


	void Start () {
		AndroidApp.instance.OnStop += OnStop;
		AndroidApp.instance.OnStart += OnStart;
		AndroidApp.instance.OnNewIntent += OnNewIntent;
		AndroidApp.instance.OnActivityResult += OnActivityResult;
	}
	


	void OnStop () {
		Debug.Log("Activity event: OnStop");
	}

	void OnStart () {
		Debug.Log("Activity event: OnStart");
	}

	void OnNewIntent () {
		Debug.Log("Activity event: OnNewIntent");
	}

	void OnActivityResult (AndroidActivityResult result) {
	
		Debug.Log("Activity event: OnActivityResult");
		Debug.Log("result.code: " + result.code);
		Debug.Log("result.requestId: " + result.requestId);
	}
}
