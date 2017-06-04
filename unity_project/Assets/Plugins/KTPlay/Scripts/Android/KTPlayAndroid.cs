
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;



public class KTPlayAndroid
{

#if UNITY_ANDROID
	private static AndroidJavaObject joKTPlayInternal = null;
	public static AndroidJavaObject joKTPlayAdapter = null;
#endif

	public static void Init()
	{
#if UNITY_ANDROID
		joKTPlayInternal = new AndroidJavaObject("com.ktplay.core.KryptaniumInternal");
		joKTPlayAdapter = new AndroidJavaObject("com.ktplay.open.KryptaniumAdapter");
#endif
	}

	public static void SetViewDidAppearCallback(MonoBehaviour obj,KTPlay.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
#if UNITY_ANDROID
					joKTPlayAdapter.CallStatic("setOnAppearListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void SetViewDidDisappearCallback(MonoBehaviour obj,KTPlay.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
#if UNITY_ANDROID
					joKTPlayAdapter.CallStatic("setOnDisappearListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void SetDidDispatchRewardsCallback(MonoBehaviour obj,KTPlay.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
#if UNITY_ANDROID
					joKTPlayAdapter.CallStatic("setOnDispatchRewardsListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void SetActivityStatusChangedCallback(MonoBehaviour obj,KTPlay.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
#if UNITY_ANDROID
					joKTPlayAdapter.CallStatic("setOnActivityStatusChangedListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void SetAvailabilityChangedCallback(MonoBehaviour obj,KTPlay.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
#if UNITY_ANDROID
					joKTPlayAdapter.CallStatic("setOnAvailabilityChangedListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}
	public static void SetDeepLinkCallback(MonoBehaviour obj,KTPlay.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
#if UNITY_ANDROID
					joKTPlayAdapter.CallStatic("setOnDeepLinkListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void Show()
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("showKryptaniumView");
#endif
	}

	public static void ShowInterstitialNotification()
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("showInterstitialNotification");
#endif
	}

	public static void Dismiss()
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("dismiss");
#endif
	}

	public static bool IsEnabled()
	{
#if UNITY_ANDROID
		return joKTPlayAdapter.CallStatic<bool>("isEnabled");
#else
		return false;
#endif
	}

	public static bool IsShowing()
	{
#if UNITY_ANDROID
		return joKTPlayAdapter.CallStatic<bool>("isShowing");
#else
		return false;
#endif
	}

	public static void SetScreenshotRotation(float degress)
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("setScreenshotRotation", degress);
#endif
	}

	public static void ShareImageToKT(string imagePath,string description)
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("shareImageToKT", imagePath, description);
#endif
	}

	public static void ShareScreenshotToKT(string description)
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("shareScreenshotToKT", description);
#endif
	}

	public static void SetNotificationEnabled(bool isEnabled)
	{
#if UNITY_ANDROID
		joKTPlayAdapter.CallStatic("setNotificationEnabled", isEnabled);
#endif
	}

	public static void Update()
	{
#if UNITY_ANDROID
		bool queryScreenshot = joKTPlayInternal.CallStatic<bool>("queryScreenshotState");
		if(queryScreenshot)
		{
			string st_pic = Application.persistentDataPath + "/KTPlay_screenshot.jpg";
			System.IO.File.Delete(st_pic);
			string filename = "KTPlay_screenshot.jpg";
			Application.CaptureScreenshot(filename);

			joKTPlayInternal.CallStatic("setScreenshotDir", Application.persistentDataPath);
		}
#endif
	}
}

