using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class AndroidInterfaces {
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.gltop.findblock.UnityPlayerNativeActivity");
#endif

    public static string CallGetAppVersion()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return androidJavaClass.CallStatic<string>("getAppVersion");
#else
		return "0.0.0";
#endif
    }
	
	public static void CallShare(string shareContent, string shareImagePath, string shareUrl)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		androidJavaClass.CallStatic("share", shareContent, shareImagePath, shareUrl);
#endif
	}
	
	public static void CallPay(string productID, string callbackGameObject, string callbackMethod)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		androidJavaClass.CallStatic("pay", productID, callbackGameObject, callbackMethod);
#endif
	}
	
	public static void CallRestore(string productID, string callbackGameObject, string callbackMethod)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		androidJavaClass.CallStatic("restore", productID, callbackGameObject, callbackMethod);
		#endif
	}
}
