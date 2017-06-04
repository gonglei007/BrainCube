////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class GooglePlayerTemplate {
	
	private string _id;
	private string _name;
	private string _iconImageUrl;
	private string _hiResImageUrl;

	private Texture2D _icon = null;
	private Texture2D _image = null;

	private bool _hasIconImage = false;
	private bool _hasHiResImage = false;




	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	public GooglePlayerTemplate(string pId, string pName, string iconUrl, string imageUrl, string pHasIconImage, string pHasHiResImage) {
		_id = pId;
		_name = pName;

		_iconImageUrl = iconUrl;
		_hiResImageUrl = imageUrl;

		if(pHasIconImage.Equals("1")) {
			_hasIconImage = true;
		}

		if(pHasHiResImage.Equals("1")) {
			_hasHiResImage = true;
		}


		if(AndroidNativeSettings.Instance.LoadProfileIcons) {
			LoadIcon();
		}

		if(AndroidNativeSettings.Instance.LoadProfileImages) {
			LoadImage();
		}
	} 


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------


	public void LoadImage() {
		
		if(image != null) {
			return;
		}
		
		
		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.addEventListener(BaseEvent.LOADED, OnProfileImageLoaded);
		loader.LoadTexture(_hiResImageUrl);
	}


	public void LoadIcon() {
		
		if(icon != null) {
			return;
		}


		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.addEventListener(BaseEvent.LOADED, OnProfileIconLoaded);
		loader.LoadTexture(_iconImageUrl);
	}

	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public string playerId {
		get {
			return _id;
		}
	}

	public string name {
		get {
			return _name;
		}
	}


	public bool hasIconImage {
		get {
			return _hasIconImage;
		}
	}
	
	public bool hasHiResImage {
		get {
			return _hasHiResImage;
		}
	}


	public string iconImageUrl {
		get {
			return _iconImageUrl;
		}
	}

	public string hiResImageUrl {
		get {
			return _hiResImageUrl;
		}
	}


	public Texture2D icon {
		get {
			return _icon;
		}
	}


	public Texture2D image {
		get {
			return _image;
		}
	}
	

	//--------------------------------------
	// EVENTS
	//--------------------------------------


	private void OnProfileImageLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnProfileImageLoaded);
		if(e.data != null) {
			_image = e.data as Texture2D;
		}

	}

	private void OnProfileIconLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnProfileIconLoaded);
		if(e.data != null) {
			_icon = e.data as Texture2D;
		}

	}



}
