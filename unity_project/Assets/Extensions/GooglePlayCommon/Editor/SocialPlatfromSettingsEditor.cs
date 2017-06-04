
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(SocialPlatfromSettings))]
public class SocialPlatfromSettingsEditor : Editor {




	static GUIContent TConsumerKey   = new GUIContent("API Key [?]:", "Twitter register app consumer key");
	static GUIContent TConsumerSecret   = new GUIContent("API Secret [?]:", "Twitter register app consumer secret");
	static GUIContent FBdkVersion   = new GUIContent("Facebook SDK Version [?]", "Version of Unity Facebook SDK Plugin");

	
	
	static GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	static GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");



	private const string IOS_SOURCE_PATH 			= "Plugins/StansAssets/IOS/";
	private const string IOS_DESTANATION_PATH 		= "Plugins/IOS/";
	private const string ANDROID_SOURCE_PATH 		= "Plugins/StansAssets/Android/";
	private const string ANDROID_DESTANATION_PATH 	= "Plugins/Android/";

	private const string version_info_file = "Plugins/StansAssets/Versions/MSP_VersionInfo.txt"; 
	


	void Awake() {
		if(IsInstalled && IsUpToDate) {
			UpdateManifest();
		}
	}

	public override void OnInspectorGUI() {


		
		#if UNITY_WEBPLAYER
		EditorGUILayout.HelpBox("Editing Mobile Social Settins not avaliable with web player platfrom. Please swith to any other platfrom under Build Seting menu", MessageType.Warning);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Switch To Android",  GUILayout.Width(130))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		}

		if(GUILayout.Button("Switch To IOS",  GUILayout.Width(130))) {

			#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);
			#else
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
			#endif

		}
		EditorGUILayout.EndHorizontal();
		
		if(Application.isEditor) {
			return;
		}

		#endif

		GUI.changed = false;



		GeneralOptions();
		EditorGUILayout.Space();


		FacebookSettings();
		EditorGUILayout.Space();
		TwitterSettings();
		EditorGUILayout.Space();

		AboutGUI();
	



		if(GUI.changed) {
			DirtyEditor();
		}
	}


	public static bool IsFullVersion {
		get {
			if(FileStaticAPI.IsFileExists(PluginsInstalationUtil.IOS_SOURCE_PATH + "MGInstagram.h")) {
				return true;
			} else {
				return false;
			}
		}
	}
	

	public static bool IsInstalled {
		get {
			if(FileStaticAPI.IsFileExists(PluginsInstalationUtil.ANDROID_DESTANATION_PATH + "androidnative.jar") && FileStaticAPI.IsFileExists(PluginsInstalationUtil.IOS_DESTANATION_PATH + "MGInstagram.h")) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	public static bool IsUpToDate {
		get {
			if(SocialPlatfromSettings.VERSION_NUMBER.Equals(DataVersion)) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	
	public static string DataVersion {
		get {
			if(FileStaticAPI.IsFileExists(version_info_file)) {
				return FileStaticAPI.Read(version_info_file);
			} else {
				return "Unknown";
			}
		}
	}

	public static float Version {
		get {
			return System.Convert.ToSingle(DataVersion);
		}
	}

	public static bool IsFacebookInstalled {
		get {
			if(!FileStaticAPI.IsFolderExists("Facebook")) {
				return false;
			} else {
				return true;
			}
		}
	}



	private void GeneralOptions() {
		
		if(!IsFullVersion) {
			EditorGUILayout.HelpBox("Mobile Social Plugin v" + SocialPlatfromSettings.VERSION_NUMBER + " is installed", MessageType.Info);
			Actions();
			return;
		}
		
		if(!IsInstalled) {
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			if(GUILayout.Button("Install Plugin",  GUILayout.Width(120))) {
				PluginsInstalationUtil.Android_InstallPlugin();
				PluginsInstalationUtil.IOS_InstallPlugin();
				UpdateVersionInfo();
			}
			GUI.color = c;
			EditorGUILayout.EndHorizontal();
		}
		
		if(IsInstalled) {
			if(!IsUpToDate) {
				EditorGUILayout.HelpBox("Update Required \nResources version: " + DataVersion + " Plugin version: " + SocialPlatfromSettings.VERSION_NUMBER, MessageType.Warning);

				if(Version <= 4.4f) {
					EditorGUILayout.HelpBox("New version contains AndroidManifest.xml chnages, Please remove Assets/Plugins/Android/AndroidManifest.xml file before update or add manualy File Sharing Block from Assets/Plugins/StansAssets/Android/AndroidManifest.xml", MessageType.Warning);
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					
					if(GUILayout.Button("Remove AndroidManifest and Update to " + SocialPlatfromSettings.VERSION_NUMBER,  GUILayout.Width(250))) {
						
						string file = "AndroidManifest.xml";
						FileStaticAPI.DeleteFile(PluginsInstalationUtil.ANDROID_DESTANATION_PATH + file);
						
						PluginsInstalationUtil.Android_UpdatePlugin();
						UpdateVersionInfo();
					}
					
					
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}


				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				Color c = GUI.color;
				GUI.color = Color.cyan;
				if(GUILayout.Button("Update to " + SocialPlatfromSettings.VERSION_NUMBER,  GUILayout.Width(250))) {
					PluginsInstalationUtil.Android_InstallPlugin();
					PluginsInstalationUtil.IOS_InstallPlugin();
					UpdateVersionInfo();
				}
				
				GUI.color = c;
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
			} else {
				EditorGUILayout.HelpBox("Mobile Social Plugin v" + SocialPlatfromSettings.VERSION_NUMBER + " is installed", MessageType.Info);
				Actions();

			}
		}

		EditorGUILayout.Space();
		
	}


	public static void DrawAPIsList() {
		EditorGUILayout.BeginHorizontal();
		GUI.enabled = false;
		EditorGUILayout.Toggle("Facebook",  IsFacebookInstalled);
		GUI.enabled = true;
		SocialPlatfromSettings.Instance.TwitterAPI = EditorGUILayout.Toggle("Twitter",  SocialPlatfromSettings.Instance.TwitterAPI);
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
		SocialPlatfromSettings.Instance.NativeSharingAPI = EditorGUILayout.Toggle("Native Sharing",  SocialPlatfromSettings.Instance.NativeSharingAPI);
		SocialPlatfromSettings.Instance.InstagramAPI = EditorGUILayout.Toggle("Instagram",  SocialPlatfromSettings.Instance.InstagramAPI);
		EditorGUILayout.EndHorizontal();
	}

	public static void UpdateManifest() {
		
		if(!SocialPlatfromSettings.Instance.KeepManifestClean) {
			return;
		}
		
		AN_ManifestManager.Refresh();
		
		AN_ManifestTemplate Manifest =  AN_ManifestManager.GetManifest();
		AN_ApplicationTemplate application =  Manifest.ApplicationTemplate;
		AN_ActivityTemplate launcherActivity = application.GetLauncherActivity();

		AN_ActivityTemplate AndroidNativeProxy = application.GetOrCreateActivityWithName("com.androidnative.AndroidNativeProxy");
		AndroidNativeProxy.SetValue("android:launchMode", "singleTask");
		AndroidNativeProxy.SetValue("android:label", "@string/app_name");
		AndroidNativeProxy.SetValue("android:configChanges", "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen");
		AndroidNativeProxy.SetValue("android:theme", "@android:style/Theme.Translucent.NoTitleBar");


		if(launcherActivity.Name == "com.androidnative.AndroidNativeBridge") {
			launcherActivity.SetName("com.unity3d.player.UnityPlayerNativeActivity");
		}
	
		
		////////////////////////
		//TwitterAPI
		////////////////////////


		foreach(KeyValuePair<int, AN_ActivityTemplate> entry in application.Activities) {
			//TODO get intents array
			AN_ActivityTemplate act = entry.Value;
			AN_PropertyTemplate intent = act.GetIntentFilterWithName("android.intent.action.VIEW");
			if(intent != null) {
				AN_PropertyTemplate data = intent.GetPropertyWithTag("data");
				if(data.GetValue("android:scheme") == "oauth") {
					act.RemoveProperty(intent);
				}
			}
		} 

		if(SocialPlatfromSettings.Instance.TwitterAPI) {
			if(AndroidNativeProxy != null) {

				AN_PropertyTemplate intent_filter = AndroidNativeProxy.GetOrCreateIntentFilterWithName("android.intent.action.VIEW");
				intent_filter.GetOrCreatePropertyWithName("category", "android.intent.category.DEFAULT");
				intent_filter.GetOrCreatePropertyWithName("category", "android.intent.category.BROWSABLE");
				AN_PropertyTemplate data = intent_filter.GetOrCreatePropertyWithTag("data");
				data.SetValue("android:scheme", "oauth");
				data.SetValue("android:host", PlayerSettings.bundleIdentifier);
			} 
		} else {
			if(AndroidNativeProxy != null) {
				AN_PropertyTemplate intent_filter = AndroidNativeProxy.GetOrCreateIntentFilterWithName("android.intent.action.VIEW");
				AndroidNativeProxy.RemoveProperty(intent_filter);
			}
		}


		////////////////////////
		//FB API
		////////////////////////
		AN_PropertyTemplate ApplicationId_meta = application.GetOrCreatePropertyWithName("meta-data", "com.facebook.sdk.ApplicationId");
		AN_ActivityTemplate LoginActivity = application.GetOrCreateActivityWithName("com.facebook.LoginActivity");
		AN_ActivityTemplate FBUnityLoginActivity = application.GetOrCreateActivityWithName("com.facebook.unity.FBUnityLoginActivity");
		AN_ActivityTemplate FBUnityDeepLinkingActivity = application.GetOrCreateActivityWithName("com.facebook.unity.FBUnityDeepLinkingActivity");
		AN_ActivityTemplate FBUnityDialogsActivity = application.GetOrCreateActivityWithName("com.facebook.unity.FBUnityDialogsActivity");


		if(IsFacebookInstalled) {
		


			ApplicationId_meta.SetValue("android:value", "\\ " + FBSettings.AppId);

			LoginActivity.SetValue("android:label", "@string/app_name");
			LoginActivity.SetValue("android:theme", "@android:style/Theme.Translucent.NoTitleBar");
			LoginActivity.SetValue("android:configChanges", "keyboardHidden|orientation");


			FBUnityLoginActivity.SetValue("android:theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
			FBUnityLoginActivity.SetValue("android:configChanges", "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen");

			FBUnityDialogsActivity.SetValue("android:theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
			FBUnityDialogsActivity.SetValue("android:configChanges", "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen");

			FBUnityDeepLinkingActivity.SetValue("android:exported", "true");


			
		} else {
			application.RemoveProperty(ApplicationId_meta);
			application.RemoveActivity(LoginActivity);
			application.RemoveActivity(FBUnityLoginActivity);
			application.RemoveActivity(FBUnityDeepLinkingActivity);
			application.RemoveActivity(FBUnityDialogsActivity);
		}
		
		
		////////////////////////
		//NativeSharingAPI
		////////////////////////
		AN_PropertyTemplate provider = application.GetOrCreatePropertyWithName("provider", "android.support.v4.content.FileProvider");
		if(SocialPlatfromSettings.Instance.NativeSharingAPI) {

#if !(UNITY_4_0	|| UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6)
			//Remove FileProvider description from AndroidManifest.xml in Unity 5
			application.RemoveProperty (provider);
#else
			provider.SetValue("android:authorities", PlayerSettings.bundleIdentifier + ".fileprovider");
			provider.SetValue("android:exported", "false");
			provider.SetValue("android:grantUriPermissions", "true");
			AN_PropertyTemplate provider_meta = provider.GetOrCreatePropertyWithName("meta-data", "android.support.FILE_PROVIDER_PATHS");
			provider_meta.SetValue("android:resource", "@xml/file_paths");		
#endif
		} else {
			application.RemoveProperty(provider);
		}	

		
		List<string> permissions = GetRequiredPermissions();
		foreach(string p in permissions) {
			Manifest.AddPermission(p);
		}
		
		AN_ManifestManager.SaveManifest();
	}

	public static List<string> GetRequiredPermissions() {
		List<string> permissions =  new List<string>();
		permissions.Add("android.permission.INTERNET");

		if(SocialPlatfromSettings.Instance.NativeSharingAPI || SocialPlatfromSettings.Instance.InstagramAPI) {
			permissions.Add("android.permission.WRITE_EXTERNAL_STORAGE");
		}
		
		return permissions;
	}

	private void Actions() {
		EditorGUILayout.Space();


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Keep Android Mnifest Clean");
		
		EditorGUI.BeginChangeCheck();
		SocialPlatfromSettings.Instance.KeepManifestClean = EditorGUILayout.Toggle(SocialPlatfromSettings.Instance.KeepManifestClean);
		if(EditorGUI.EndChangeCheck()) {
			UpdateManifest();
		}
		
		if(GUILayout.Button("[?]",  GUILayout.Width(25))) {
			Application.OpenURL("http://goo.gl/018lnQ");
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.EndHorizontal();


		SocialPlatfromSettings.Instance.ShowAPIS = EditorGUILayout.Foldout(SocialPlatfromSettings.Instance.ShowAPIS, "Mobile Social Plugin APIs");
		if(SocialPlatfromSettings.Instance.ShowAPIS) {
			EditorGUI.indentLevel++;

			EditorGUI.BeginChangeCheck();
			DrawAPIsList();
			if(EditorGUI.EndChangeCheck()) {
				UpdateManifest();
			}
			
			
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
		}

		SocialPlatfromSettings.Instance.ShowActions = EditorGUILayout.Foldout(SocialPlatfromSettings.Instance.ShowActions, "More Actions");
		if(SocialPlatfromSettings.Instance.ShowActions) {
				
			if(!IsFacebookInstalled) {
				GUI.enabled = false;
			}	
				
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
				
			if(GUILayout.Button("Remove Facebook SDK",  GUILayout.Width(160))) {
				bool result = EditorUtility.DisplayDialog(
					"Removing Facebook SDK",
					"Warning action can not be undone without reimporting the plugin",
					"Remove",
					"Cansel");
				if(result) {
					FileStaticAPI.DeleteFolder(PluginsInstalationUtil.ANDROID_DESTANATION_PATH + "facebook");
					FileStaticAPI.DeleteFolder("Facebook");
					FileStaticAPI.DeleteFolder("Extensions/GooglePlayCommon/Social/Facebook");
					FileStaticAPI.DeleteFile("Extensions/MobileSocialPlugin/Example/Scripts/MSPFacebookUseExample.cs");
					FileStaticAPI.DeleteFile("Extensions/MobileSocialPlugin/Example/Scripts/MSP_FacebookAnalyticsExample.cs");
					FileStaticAPI.DeleteFile("Extensions/MobileSocialPlugin/Example/Scripts/MSP_FacebookAndroidTurnBasedAndGiftsExample.cs");
					FileStaticAPI.CopyFile("Extensions/StansAssetsCommon/SA_FB_PlaceHolder.txt", "Extensions/StansAssetsCommon/SA_FB_PlaceHolder.cs");
				}
					
			}
				
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
				
				
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("Reset Settings",  GUILayout.Width(160))) {
				ResetSettings();
			}
				
			if(GUILayout.Button("Load Example Settings",  GUILayout.Width(160))) {
				LoadExampleSettings();
			}
				
				
			EditorGUILayout.EndHorizontal();
				
		}
	}

	public static void LoadExampleSettings() {

		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY = "wEvDyAUr2QabVAsWPDiGwg";
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET = "igRxZbOrkLQPNLSvibNC3mdNJ5tOlVOPH3HNNKDY0";

	}

	public static void ResetSettings() {
		FileStaticAPI.DeleteFile("Extensions/GooglePlayCommon/Resources/SocialSettings.asset");
		SocialPlatfromSettings.Instance.ShowActions = true;
		Selection.activeObject = SocialPlatfromSettings.Instance;
	}
	
	
	private static string newPermition = "";
	public static void FacebookSettings() {
		EditorGUILayout.HelpBox("Facebook Settings", MessageType.None);

		if (SocialPlatfromSettings.Instance.fb_scopes_list.Count == 0) {
			SocialPlatfromSettings.Instance.AddDefaultScopes();
		}

		SocialPlatfromSettings.Instance.showPermitions = EditorGUILayout.Foldout(SocialPlatfromSettings.Instance.showPermitions, "Facebook Permissions");
		if(SocialPlatfromSettings.Instance.showPermitions) {
			foreach(string s in SocialPlatfromSettings.Instance.fb_scopes_list) {
				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.SelectableLabel(s, GUILayout.Height(16));
				
				if(GUILayout.Button("x",  GUILayout.Width(20))) {
					SocialPlatfromSettings.Instance.fb_scopes_list.Remove(s);
					return;
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.EndVertical();
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Add new permition: ");
			newPermition = EditorGUILayout.TextField(newPermition);
			
			
			EditorGUILayout.EndHorizontal();
			
			
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.Space();
			if(GUILayout.Button("Documentation",  GUILayout.Width(100))) {
				Application.OpenURL("https://developers.facebook.com/docs/facebook-login/permissions/v2.0");
			}
			
			
			
			if(GUILayout.Button("Add",  GUILayout.Width(100))) {
				
				if(newPermition != string.Empty) {
					newPermition = newPermition.Trim();
					if(!SocialPlatfromSettings.Instance.fb_scopes_list.Contains(newPermition)) {
						SocialPlatfromSettings.Instance.fb_scopes_list.Add(newPermition);
					}
					
					newPermition = string.Empty;
				}
			}
			EditorGUILayout.EndHorizontal();
		
		}

	}

	public static void TwitterSettings() {
		EditorGUILayout.HelpBox("Twitter Settings", MessageType.None);


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(TConsumerKey);
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY	 	= EditorGUILayout.TextField(SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY);
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY 		= SocialPlatfromSettings.Instance.TWITTER_CONSUMER_KEY.Trim();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(TConsumerSecret);
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET	 	= EditorGUILayout.TextField(SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET);
		SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET	 	= SocialPlatfromSettings.Instance.TWITTER_CONSUMER_SECRET.Trim();
		EditorGUILayout.EndHorizontal();
	}




	private void AboutGUI() {


		EditorGUILayout.HelpBox("About Mobile Social Plugin", MessageType.None);
		EditorGUILayout.Space();
		
		SelectableLabelField(SdkVersion, SocialPlatfromSettings.VERSION_NUMBER);
		if(IsFacebookInstalled) {
			SelectableLabelField(FBdkVersion, SocialPlatfromSettings.FB_SDK_VERSION_NUMBER);
		}
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");

		
	}
	
	private static void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}

	public static void UpdateVersionInfo() {
		FileStaticAPI.Write(version_info_file, SocialPlatfromSettings.VERSION_NUMBER);
		UpdateManifest();
	}



	public static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(SocialPlatfromSettings.Instance);
		#endif
	}
	
	
}
