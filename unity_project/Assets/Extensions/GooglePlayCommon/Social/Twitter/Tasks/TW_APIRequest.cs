using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public abstract class TW_APIRequest : EventDispatcher {

	private bool IsFirst = true;
	private string GetParams = string.Empty;


	private string requestUrl;

	#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 
	private Hashtable Headers = new Hashtable();
	#else
	private  Dictionary<string, string> Headers = new Dictionary<string, string>();
	#endif




	// --------------------------------------
	// Pulic Methods
	// --------------------------------------



	public void Send() {
		if(TwitterApplicationOnlyToken.instance.currentToken == null) {
			TwitterApplicationOnlyToken.instance.addEventListener(BaseEvent.COMPLETE, OnTokenLoaded);
			TwitterApplicationOnlyToken.instance.RetrieveToken();
		} else {
			StartCoroutine(Request());
		}
	}
	

	public void AddParam(string name, int value) {
		AddParam(name, value.ToString());
	}

	public void AddParam(string name, string value) {


		if(!IsFirst) {
			GetParams += "&";
		} else {
			GetParams += "?";
		}

		GetParams += name + "=" + value;


		IsFirst = false;
	}




	// --------------------------------------
	// Protected Methods
	// --------------------------------------


	protected void SetUrl(string url) {
		requestUrl = url;
	}

	private IEnumerator Request () {


		requestUrl = requestUrl + GetParams;
		

		Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
		Headers.Add("Authorization", "Bearer " + TwitterApplicationOnlyToken.instance.currentToken);
		

		
		WWW www = new WWW(requestUrl, null,  Headers);
		yield return www;
		
		if(www.error == null) {
			OnResult(www.text);
		} else {
			dispatch(BaseEvent.COMPLETE, new TW_APIRequstResult(false, www.error));
		}


		Destroy(gameObject);
	}


	// --------------------------------------
	// Events
	// --------------------------------------


	protected abstract void OnResult(string data);

	private void OnTokenLoaded() {

		TwitterApplicationOnlyToken.instance.removeEventListener(BaseEvent.COMPLETE, OnTokenLoaded);

		if(TwitterApplicationOnlyToken.instance.currentToken != null) {
			StartCoroutine(Request());
		} else {
			dispatch(BaseEvent.COMPLETE, new TW_APIRequstResult(false, "Retirving auth tocen failed"));
		}

	}


}
