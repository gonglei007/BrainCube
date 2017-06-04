
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

#if UNITY_IOS || UNITY_EDITOR

public class KTPlayiOS : MonoBehaviour
{
	public const string LIB_NAME = "__Internal";

	[DllImport (LIB_NAME)]
	private static extern void KT_SetOnAppearCallback(string GameobjectName,string methodName);

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
					KT_SetOnAppearCallback(gameObj.name,methodName);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_SetOnDisappearCallback(string GameobjectName,string methodName);

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
					KT_SetOnDisappearCallback(gameObj.name,methodName);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_SetOnDispatchRewardsCallback(string GameobjectName,string methodName);

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
					KT_SetOnDispatchRewardsCallback(gameObj.name,methodName);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_SetOnActivityStatusChangedCallback(string GameobjectName,string methodName);

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
					KT_SetOnActivityStatusChangedCallback(gameObj.name,methodName);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_SetOnAvailabilityChangedCallback(string GameobjectName,string methodName);

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
					KT_SetOnAvailabilityChangedCallback(gameObj.name,methodName);
				}
			}
		}
	}
	[DllImport (LIB_NAME)]
	private static extern void KT_SetDeepLinkCallback(string GameobjectName,string methodName);

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
					KT_SetDeepLinkCallback(gameObj.name,methodName);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_ShowKTPlayView();

	public static void Show()
	{
		KT_ShowKTPlayView();
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_DismissKTPlayView();

	public static void Dismiss()
	{
		KT_DismissKTPlayView();
	}

	[DllImport (LIB_NAME)]
	private static extern bool KT_IsEnabled();

	public static bool IsEnabled()
	{
		return KT_IsEnabled();
	}

	[DllImport (LIB_NAME)]
	private static extern bool KT_IsKTPlayViewShowing();

	public static bool IsShowing()
	{
		return KT_IsKTPlayViewShowing();
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_SetScreenshotRotation(float degress);

	public static void SetScreenshotRotation(float degress)
	{
		KT_SetScreenshotRotation(degress);
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_ShareScreenshotToKT( string description);

	public static void ShareScreenshotToKT( string description)
	{
		 KT_ShareScreenshotToKT(description);
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_SetNotificationEnabled( bool enabled);

	public static void SetNotificationEnabled(bool enabled)
	{
		 KT_SetNotificationEnabled(enabled);
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_ShareImageToKT(string imagePath,string description);

	public static void ShareImageToKT(string imagePath,string description)
	{
		 KT_ShareImageToKT(imagePath,description);
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_ShowInterstitialNotification();

	public static void ShowInterstitialNotification()
	{
		KT_ShowInterstitialNotification();
	}
}

#endif
