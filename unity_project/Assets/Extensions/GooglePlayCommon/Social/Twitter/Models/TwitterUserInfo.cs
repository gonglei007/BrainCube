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

public class TwitterUserInfo : EventDispatcherBase {

	public const string PROFILE_IMAGE_LOADED		 = "profile_image_loaded";
	public const string PROFILE_BACKGROUND_LOADED 	 = "profile_background_loaded";
	

	private string _id;
	private string _description;
	private string _name;
	private string _screen_name;

	private string _location;
	private string _lang;

	private string _rawJSON;

	private string _profile_image_url;
	private string _profile_image_url_https;
	private string _profile_background_image_url;
	private string _profile_background_image_url_https;


	private Texture2D _profile_image = null;
	private Texture2D _profile_background = null;

	private Color _profile_background_color = Color.clear;
	private Color _profile_text_color = Color.clear;

	

	private int _friends_count;
	private int _statuses_count;

	private TwitterStatus _status;



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public TwitterUserInfo(string data) {
		_rawJSON = data;


		IDictionary JSON =  ANMiniJSON.Json.Deserialize(_rawJSON) as IDictionary;	


		_id 								= System.Convert.ToString(JSON["id"]);
		_name 								= System.Convert.ToString(JSON["name"]);
		_description 						= System.Convert.ToString(JSON["description"]);
		_screen_name 						= System.Convert.ToString(JSON["screen_name"]);


		_lang 								= System.Convert.ToString(JSON["lang"]);
		_location 							= System.Convert.ToString(JSON["location"]);


		_profile_image_url 					= System.Convert.ToString(JSON["profile_image_url"]);
		_profile_image_url_https 			= System.Convert.ToString(JSON["profile_image_url_https"]);
		_profile_background_image_url 		= System.Convert.ToString(JSON["profile_background_image_url"]);
		_profile_background_image_url_https = System.Convert.ToString(JSON["profile_background_image_url_https"]);


		_friends_count  					= System.Convert.ToInt32(JSON["friends_count"]);
		_statuses_count 					= System.Convert.ToInt32(JSON["statuses_count"]);

		_profile_text_color 	 			= HexToColor(System.Convert.ToString(JSON["profile_text_color"]));
		_profile_background_color 			= HexToColor(System.Convert.ToString(JSON["profile_background_color"]));


		_status =  new TwitterStatus(JSON["status"] as IDictionary);
	}

	public TwitterUserInfo(IDictionary JSON) {
		_id 								= System.Convert.ToString(JSON["id"]);
		_name 								= System.Convert.ToString(JSON["name"]);
		_description 						= System.Convert.ToString(JSON["description"]);
		_screen_name 						= System.Convert.ToString(JSON["screen_name"]);
		
		
		_lang 								= System.Convert.ToString(JSON["lang"]);
		_location 							= System.Convert.ToString(JSON["location"]);
		
		
		_profile_image_url 					= System.Convert.ToString(JSON["profile_image_url"]);
		_profile_image_url_https 			= System.Convert.ToString(JSON["profile_image_url_https"]);
		_profile_background_image_url 		= System.Convert.ToString(JSON["profile_background_image_url"]);
		_profile_background_image_url_https = System.Convert.ToString(JSON["profile_background_image_url_https"]);
		
		
		_friends_count  					= System.Convert.ToInt32(JSON["friends_count"]);
		_statuses_count 					= System.Convert.ToInt32(JSON["statuses_count"]);
		
		_profile_text_color 	 			= HexToColor(System.Convert.ToString(JSON["profile_text_color"]));
		_profile_background_color 			= HexToColor(System.Convert.ToString(JSON["profile_background_color"]));
		

	}


	//--------------------------------------
	// PUBLI METHODS
	//--------------------------------------


	public void LoadProfileImage() {

		if(_profile_image != null) {
			dispatch(PROFILE_IMAGE_LOADED);
			return;
		}


		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.addEventListener(BaseEvent.LOADED, OnProfileImageLoaded);
		loader.LoadTexture(_profile_image_url_https);
	}

	public void LoadBackgroundImage() {

		if(_profile_background != null) {
			dispatch(PROFILE_BACKGROUND_LOADED);
			return;
		}

		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.addEventListener(BaseEvent.LOADED, OnProfileBackgroundLoaded);
		loader.LoadTexture(_profile_background_image_url_https);
	}


	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public string rawJSON {
		get {
			return _rawJSON;
		}
	}


	public string id {
		get {
			return _id;
		}
	}
	public string name {
		get {
			return _name;
		}
	}


	public string description {
		get {
			return _description;
		}
	}


	public string screen_name {
		get {
			return _screen_name;
		}
	}


	
	public string location {
		get {
			return _location;
		}
	}


	public string lang {
		get {
			return _lang;
		}
	}
	

	public string profile_image_url {
		get {
			return _profile_image_url;
		}
	}

	public string profile_image_url_https {
		get {
			return _profile_image_url_https;
		}
	}

	public string profile_background_image_url {
		get {
			return _profile_background_image_url;
		}
	}
	public string profile_background_image_url_https {
		get {
			return _profile_background_image_url_https;
		}
	}
	

	public int friends_count {
		get {
			return _friends_count;
		}
	}


	public int statuses_count {
		get {
			return _statuses_count;
		}
	}


	public TwitterStatus status {
		get {
			return _status;
		}
	}


	public Texture2D profile_image {
		get {
			return _profile_image;
		}
	}

	public Texture2D profile_background {
		get {
			return _profile_background;
		}
	}


	public Color profile_background_color {
		get {
			return _profile_background_color;
		}
	}

	public Color profile_text_color {
		get {
			return _profile_text_color;
		}
	}


	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnProfileImageLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnProfileImageLoaded);
		_profile_image = e.data as Texture2D;

		dispatch(PROFILE_IMAGE_LOADED);
	}

	private void OnProfileBackgroundLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnProfileBackgroundLoaded);
		_profile_background = e.data as Texture2D;

		dispatch(PROFILE_BACKGROUND_LOADED);
	}


	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------

	private Color HexToColor(string hex) {
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

}
