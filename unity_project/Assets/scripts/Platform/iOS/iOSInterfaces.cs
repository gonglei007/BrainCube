using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JsonFx.Json;

public class iOSInterfaces  {
	//[DllImport("__Internal")]
	//private static extern string getAPPVersion();
	[DllImport("__Internal")]
	private static extern void Pay(string productId);
	[DllImport("__Internal")]
	private static extern void Restore(string productId);
	[DllImport("__Internal")]
	private static extern void ShareWithScreenshot(string text, string screenshotPath);
	[DllImport("__Internal")]
	private static extern void GameFullyLoaded();
	[DllImport("__Internal")]
	private static extern void StartRefreshShopData();

	public static string getAPPVersion(){
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return Interfaces.getAPPVersion();
		}
		return "0.0.0";
	}
	
	public static void CallPay(string productID)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Pay(productID);
		}
	}
	
	public static void CallRestore(string productID)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Restore(productID);
		}
	}

	public static void CallShareWithScreenshot(string text, string screenshotPath){
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			ShareWithScreenshot(text, screenshotPath);
		}
	}

	public static void CallGameFullyLoaded()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameFullyLoaded();
		}
	}

	public static void CallStartRefreshShopData()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			StartRefreshShopData();
		}
	}
}
