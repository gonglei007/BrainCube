
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

#if UNITY_IOS || UNITY_EDITOR

public class KTAccountManageriOS : MonoBehaviour
{
	public const string LIB_NAME = "__Internal";
	
	[DllImport (LIB_NAME)]
	private static extern void KT_SetLoginStatusChangeCallback(string GameobjectName,string methodName);

	public static void SetLoginStatusChangeCallback(MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
					KT_SetLoginStatusChangeCallback(gameObj.name,methodName);
				}
			}
		}
	}
	
	
	[DllImport (LIB_NAME)]
	private static extern void KT_UserProfileCallback(string GameobjectName,string methodName,string userId);

	public static void UserProfile(string userId,MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
					KT_UserProfileCallback(gameObj.name,methodName,userId);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]
	private static extern void KT_LoginWithGameUser(string GameobjectName,string methodName,string gameUserId);
	
	public static void loginWithGameUser(string gameUserId, MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
					KT_LoginWithGameUser(gameObj.name,methodName,gameUserId);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]
	private static extern void KT_SetNickName(string GameobjectName,string methodName,string nickName);
	
	public static void setNickname(string nickName, MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
					KT_SetNickName(gameObj.name,methodName,nickName);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]
	private static extern void KT_Logout();
	
	public static void logout()
	{	
		KT_Logout();
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_ShowLoginView(bool closeable, string GameobjectName,string methodName);

	public static void ShowLoginView(bool closeable, MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{									
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
					KT_ShowLoginView(closeable, gameObj.name,methodName);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]
	private static extern bool KT_IsLoggedIn();

	public static bool IsLoggedIn()
	{
		return KT_IsLoggedIn();
	}
	
	[DllImport (LIB_NAME)]
	private static extern string  KT_CurrentAccount();

	public static KTUser CurrentAccount()
	{
		string str = (string)KT_CurrentAccount();

		Hashtable data = (Hashtable)KTJSON.jsonDecode(str);

		return new KTUser(data);
	}
}

#endif
