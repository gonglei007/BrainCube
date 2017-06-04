using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class AndroidNativeSettings : ScriptableObject {

	public const string VERSION_NUMBER = "6.0";
	public const string GOOGLE_PLAY_SDK_VERSION_NUMBER = "6171000";


	public bool EnablePlusAPI 		= true;
	public bool EnableGamesAPI 		= true;
	public bool EnableAppStateAPI 	= true;
	public bool EnableDriveAPI 		= false;
	public bool LoadProfileIcons 	= true;
	public bool LoadProfileImages 	= true;

	public bool LoadQuestsImages 	= true;
	public bool LoadQuestsIcons 	= true;
	public bool LoadEventsIcons 	= true;



	public bool UseProductNameAsFolderName = true;
	public string GalleryFolderName = string.Empty;
	public int MaxImageLoadSize = 512;
	public AN_CameraCaptureType CameraCaptureMode;



	public bool ShowPluginSettings = false;
	public bool ShowAppPermissions = false;
	public bool EnableBillingAPI = true;
	public bool EnablePSAPI = true;
	public bool EnableSocialAPI = true;
	public bool EnableCameraAPI = true;


	public bool ExpandNativeAPI = false;
	public bool ExpandPSAPI = false;
	public bool ExpandBillingAPI = false;
	public bool ExpandSocialAPI = false;
	public bool ExpandCameraAPI = false;



	public bool ShowStoreKitParams = false;
	public bool ShowCameraAndGalleryParams = false;
	public bool ShowLocalNotificationParams = false;
	public bool ShowPushNotificationParams = false;
	public bool ShowPSSettings = false;
	public bool ShowPSSettingsResources = false;
	public bool ShowActions = false;
	public bool GCMSettingsActinve = false;


	//APIs:
	public bool LocalNotificationsAPI = true; 
	public bool ImmersiveModeAPI = true;
	public bool ApplicationInformationAPI = true;
	public bool ExternalAppsAPI = true;
	public bool PoupsandPreloadersAPI = true;
	public bool CheckAppLicenseAPI = true;

	public bool InAppPurchasesAPI = true;


	public bool GooglePlayServicesAPI = true;
	public bool PlayServicesAdvancedSignInAPI = true;
	public bool GoogleButtonAPI = true;
	public bool AnalyticsAPI = true;
	public bool GoogleCloudSaveAPI = true;
	public bool PushNotificationsAPI = true;
	public bool GoogleMobileAdAPI = true;
	

	public bool GalleryAPI = true;
	public bool CameraAPI = true;

	public bool KeepManifestClean = true;
	

	public string GCM_SenderId = "YOUR_SENDER_ID_HERE";

	public string GooglePlayServiceAppID = "0";

	public string base64EncodedPublicKey = "REPLACE_WITH_YOUR_PUBLIC_KEY";
	public List<string> InAppProducts = new List<string>();

	public bool ShowWhenAppIsForeground = true;
	public bool EnableVibrationLocal = false;
	public Texture2D LocalNotificationIcon = null;
	public AudioClip LocalNotificationSound = null;

	public bool UseParsePushNotifications = false;
	public string ParseAppId = "YOUR_PARSE_APP_ID";
	public string DotNetKey = "YOUR_PARSE_DOT_NET_KEY";

	public bool EnableVibrationPush = false;
	public Texture2D PushNotificationIcon = null;
	public AudioClip PushNotificationSound = null;

	public const string ANSettingsAssetName = "AndroidNativeSettings";
	public const string ANSettingsPath = "Extensions/AndroidNative/Resources";
	public const string ANSettingsAssetExtension = ".asset";

	private static AndroidNativeSettings instance = null;

	
	public static AndroidNativeSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(ANSettingsAssetName) as AndroidNativeSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<AndroidNativeSettings>();
					#if UNITY_EDITOR
					//string properPath = Path.Combine(Application.dataPath, ANSettingsPath);

					FileStaticAPI.CreateFolder(ANSettingsPath);

					/*
					if (!Directory.Exists(properPath)) {
						AssetDatabase.CreateFolder("Extensions/", "AndroidNative");
						AssetDatabase.CreateFolder("Extensions/AndroidNative", "Resources");
					}
					*/
					
					string fullPath = Path.Combine(Path.Combine("Assets", ANSettingsPath),
					                               ANSettingsAssetName + ANSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}


	public bool IsBase64KeyWasReplaced {
		get {
			if(base64EncodedPublicKey.Equals("REPLACE_WITH_YOUR_PUBLIC_KEY")) {
				return false;
			} else {
				return true;
			}
		}
	}



}
