////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using System;
using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class WWWTextureLoader : EventDispatcher {

	private string _url;

	public Action<Texture2D> OnLoad = delegate{}; 

	public static WWWTextureLoader Create() {
		return new GameObject("WWWTextureLoader").AddComponent<WWWTextureLoader>();
	}

	public void LoadTexture(string url) {
		_url = url;
		StartCoroutine(LoadCoroutin());
	}


	private IEnumerator LoadCoroutin () {
		// Start a download of the given URL
		WWW www = new WWW (_url);

		// Wait for download to complete
		yield return www;

		if(www.error == null) {
			dispatch(BaseEvent.LOADED, www.texture);
			OnLoad(www.texture);

		} else {
			dispatch(BaseEvent.LOADED, null);
			OnLoad(null);
		}

	}

}
