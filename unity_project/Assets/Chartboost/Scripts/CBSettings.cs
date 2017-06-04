using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ChartboostSDK {

#if UNITY_EDITOR	
	[InitializeOnLoad]
#endif
	public class CBSettings : ScriptableObject
	{
		const string cbSettingsAssetName = "ChartboostSettings";
	    const string cbSettingsPath = "Chartboost/Resources";
	    const string cbSettingsAssetExtension = ".asset";

	    private static CBSettings instance;

	    static CBSettings Instance
	    {
	        get
	        {
	            if (instance == null)
	            {
	                instance = Resources.Load(cbSettingsAssetName) as CBSettings;
	                if (instance == null)
	                {
	                    // If not found, autocreate the asset object.
	                    instance = CreateInstance<CBSettings>();
	#if UNITY_EDITOR
	                    string properPath = Path.Combine(Application.dataPath, cbSettingsPath);
	                    if (!Directory.Exists(properPath))
	                    {
	                        AssetDatabase.CreateFolder("Assets/Chartboost", "Resources");
	                    }

	                    string fullPath = Path.Combine(Path.Combine("Assets", cbSettingsPath),
	                                                   cbSettingsAssetName + cbSettingsAssetExtension
	                                                  );
	                    AssetDatabase.CreateAsset(instance, fullPath);
	#endif
	                }
	            }
	            return instance;
	        }
	    }

	#if UNITY_EDITOR
	    [MenuItem("Chartboost/Edit Settings")]
	    public static void Edit()
	    {
	        Selection.activeObject = Instance;
	    }

	    [MenuItem("Chartboost/SDK Documentation")]
	    public static void OpenDocumentation()
	    {
	        string url = "https://help.chartboost.com/documentation/unity";
	        Application.OpenURL(url);
	    }
	#endif

	    #region App Settings
		[SerializeField]
		public string iOSAppId = "0";
		[SerializeField]
		public string iOSAppSecret = "0";
		[SerializeField]
		public string androidAppId = "0";
		[SerializeField]
		public string androidAppSecret = "0";
		[SerializeField]
		public string amazonAppId = "0";
		[SerializeField]
		public string amazonAppSecret = "0";
		[SerializeField]
		public bool isLoggingEnabled = false;
		[SerializeField]
		public string[] androidPlatformLabels = new[] { "Google Play", "Amazon" };
		[SerializeField]
		public int selectedAndroidPlatformIndex = 0;

		public void SetAndroidPlatformIndex(int index)
		{
			if (selectedAndroidPlatformIndex != index)
			{
				selectedAndroidPlatformIndex = index;
				DirtyEditor();
			}
		}

		public int SelectedAndroidPlatformIndex
		{
			get { return selectedAndroidPlatformIndex; }
		}

		public string[] AndroidPlatformLabels
		{
			get { return androidPlatformLabels; }
			set
			{
				if (androidPlatformLabels != value)
				{
					androidPlatformLabels = value;
					DirtyEditor();
				}
			}
		}

		// iOS
	    public void SetIOSAppId(string id)
	    {
	        if (Instance.iOSAppId != id)
	        {
	            Instance.iOSAppId = id;
	            DirtyEditor();
	        }
	    }

		public static string getIOSAppId()
		{
			return Instance.iOSAppId;
		}

	    public void SetIOSAppSecret(string secret)
	    {
	        if (Instance.iOSAppSecret != secret)
	        {
	            Instance.iOSAppSecret = secret;
	            DirtyEditor();
	        }
	    }

		public static string getIOSAppSecret()
		{
			return Instance.iOSAppSecret;
		}

		// Android
		public void SetAndroidAppId(string id)
		{
			if (Instance.androidAppId != id)
			{
				Instance.androidAppId = id;
				DirtyEditor();
			}
		}
		
		public static string getAndroidAppId()
		{
			return Instance.androidAppId;
		}
		
		public void SetAndroidAppSecret(string secret)
		{
			if (Instance.androidAppSecret != secret)
			{
				Instance.androidAppSecret = secret;
				DirtyEditor();
			}
		}
		
		public static string getAndroidAppSecret()
		{
			return Instance.androidAppSecret;
		}

		// Amazon
		public void SetAmazonAppId(string id)
		{
			if (Instance.amazonAppId != id)
			{
				Instance.amazonAppId = id;
				DirtyEditor();
			}
		}
		
		public static string getAmazonAppId()
		{
			return Instance.amazonAppId;
		}
		
		public void SetAmazonAppSecret(string secret)
		{
			if (Instance.amazonAppSecret != secret)
			{
				Instance.amazonAppSecret = secret;
				DirtyEditor();
			}
		}
		
		public static string getAmazonAppSecret()
		{
			return Instance.amazonAppSecret;
		}

		public static string getSelectAndroidAppId()
		{
			// Google
			if (Instance.selectedAndroidPlatformIndex == 0)
			{
				return Instance.androidAppId;
			}
			// Amazon
			else 
			{
				return Instance.amazonAppId;
			}
		}

		public static string getSelectAndroidAppSecret()
		{
			// Google
			if (Instance.selectedAndroidPlatformIndex == 0)
			{
				return Instance.androidAppSecret;
			}
			// Amazon
			else 
			{
				return Instance.amazonAppSecret;
			}
		}

		public static void enableLogging(bool enabled)
		{
			Instance.isLoggingEnabled = enabled;
		}

		public static bool isLogging()
		{
			return Instance.isLoggingEnabled;
		}

	    private static void DirtyEditor()
	    {
	#if UNITY_EDITOR
	        EditorUtility.SetDirty(Instance);
	#endif
	    }

	    #endregion
	}
}