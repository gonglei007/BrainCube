using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class SocialPlatfromSettings : ScriptableObject {

	public const string VERSION_NUMBER = "4.3";
	public const string FB_SDK_VERSION_NUMBER = "6.1.0";

	public bool showPermitions = true;
	public bool ShowActions = true;

	public bool ShowAPIS = true;

	public List<string> fb_scopes_list =  new List<string>();
	

	public string TWITTER_CONSUMER_KEY 	= "REPLACE_WITH_YOUR_CONSUMER_KEY";
	public string TWITTER_CONSUMER_SECRET 	= "REPLACE_WITH_YOUR_CONSUMER_SECRET";
	

	private const string ISNSettingsAssetName = "SocialSettings";
	private const string ISNSettingsPath = "Extensions/GooglePlayCommon/Resources";
	private const string ISNSettingsAssetExtension = ".asset";


	public bool TwitterAPI = true;
	public bool NativeSharingAPI = true;
	public bool InstagramAPI = true;


	public bool KeepManifestClean = true;

	
	private static SocialPlatfromSettings instance = null;
	
	
	public static SocialPlatfromSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(ISNSettingsAssetName) as SocialPlatfromSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<SocialPlatfromSettings>();
					#if UNITY_EDITOR
					//string properPath = Path.Combine(Application.dataPath, ISNSettingsPath);

					FileStaticAPI.CreateFolder(ISNSettingsPath);

					/*
					if (!Directory.Exists(properPath)) {
						AssetDatabase.CreateFolder("Extensions/", "GooglePlayCommon");
						AssetDatabase.CreateFolder("Extensions/GooglePlayCommon", "Resources");
					}
					*/
					
					string fullPath = Path.Combine(Path.Combine("Assets", ISNSettingsPath),
					                               ISNSettingsAssetName + ISNSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);


					instance.AddDefaultScopes();

					#endif
				}
			}
			return instance;
		}
	}


	public string fb_scopes {
		get {
			string scopes = "";
			foreach(string s in fb_scopes_list) {
				scopes+= s + ",";
			}

			if(scopes.Length > 0) {
				scopes = scopes.Substring(0, scopes.Length - 1);
			}

			return scopes;
		}
	}

	public void AddDefaultScopes() {
	
		instance.fb_scopes_list.Add("user_about_me");
		instance.fb_scopes_list.Add("user_friends");
		instance.fb_scopes_list.Add("email");
		instance.fb_scopes_list.Add("publish_actions");
		instance.fb_scopes_list.Add("read_friendlists");
		instance.fb_scopes_list.Add("user_games_activity");
		instance.fb_scopes_list.Add("user_activities");
		instance.fb_scopes_list.Add("user_likes");
	}
	


}

