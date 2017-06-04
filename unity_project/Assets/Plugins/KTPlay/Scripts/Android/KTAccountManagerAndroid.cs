
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

public class KTAccountManagerAndroid : MonoBehaviour
{
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
#if UNITY_ANDROID
					KTPlayAndroid.joKTPlayAdapter.CallStatic("setLoginStatusChangedListener", gameObj.name, methodName);
#endif
				}
			}
		}
	}

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
#if UNITY_ANDROID
					KTPlayAndroid.joKTPlayAdapter.CallStatic("userProfile", userId, gameObj.name, methodName);
#endif
				}
			}
		}
	}

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
#if UNITY_ANDROID
					KTPlayAndroid.joKTPlayAdapter.CallStatic("showLoginView", closeable, gameObj.name, methodName);
#endif
				}
			}
		}
	}

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
#if UNITY_ANDROID
					KTPlayAndroid.joKTPlayAdapter.CallStatic("loginWithGameUser", gameUserId, gameObj.name, methodName);
#endif
				}
			}
		}
	}

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
#if UNITY_ANDROID
					KTPlayAndroid.joKTPlayAdapter.CallStatic("setNickName", nickName, gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static bool IsLoggedIn()
	{
#if UNITY_ANDROID
		return KTPlayAndroid.joKTPlayAdapter.CallStatic<bool>("isLoggedIn");
#else
		return false;
#endif
	}

	public static void logout()
	{
#if UNITY_ANDROID
		KTPlayAndroid.joKTPlayAdapter.CallStatic("logout");
#endif
	}

	public static KTUser CurrentAccount()
	{
		//string str = (string)KT_CurrentAccount();

		//Hashtable data = (Hashtable)KTJSON.jsonDecode(str);

		//return new KTUser(data);
#if UNITY_ANDROID
		AndroidJavaObject joUser = null;
		try
		{
			joUser = KTPlayAndroid.joKTPlayAdapter.CallStatic<AndroidJavaObject>("currentAccount");
		}
		catch (System.Exception e)
		{
			Debug.Log("Failed to CurrentAccount: " + e.Message);
			return null;
		}

		if(joUser == null)
			return null;

		string userid = joUser.Call<string>("getUserId");
		string headerurl = joUser.Call<string>("getHeaderUrl");
		string nickname = joUser.Call<string>("getNickname");
		int gender = joUser.Call<int>("getGender");
		string city = joUser.Call<string>("getCity");
		string score = joUser.Call<string>("getScore");
		long rank = joUser.Call<long>("getRank");
		string snsuserid = joUser.Call<string>("getSnsUserId");
		string logintype = joUser.Call<string>("getLoginType");
		string gameUserId = joUser.Call<string>("getGameUserId");
		bool needPresentNickname = joUser.Call<bool>("getNeedPresentNickname");

		KTUser user = new KTUser();
		user.setUserId(userid);
		user.setHeaderUrl(headerurl);
		user.setNickname(nickname);
		user.setGender(gender);
		user.setCity(city);
		user.setScore(score);
		user.setRank(rank);
		user.setSnsUserId(snsuserid);
		user.setLoginType(logintype);
		user.setGameUserId(gameUserId);
		user.setNeedPresentNickname(needPresentNickname);

		return user;
#else
		return null;
#endif
	}
}

