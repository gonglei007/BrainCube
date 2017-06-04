
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

public class KTLeaderboardAndroid : MonoBehaviour
{
	public delegate void Callback(string s);

	public static void FriendsLeaderBoard(string leaderboardId, int startIndex,int count,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
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
					KTPlayAndroid.joKTPlayAdapter.CallStatic("friendsLeaderboard", leaderboardId, startIndex, count, gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void GlobalLeaderboard(string leaderboardId,int startIndex,int count,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
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
					KTPlayAndroid.joKTPlayAdapter.CallStatic("globalLeaderboard", leaderboardId, startIndex, count, gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void ReportScore(long score,string leaderboardId,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
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
					KTPlayAndroid.joKTPlayAdapter.CallStatic("reportScore", score, leaderboardId, gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void LastFriendsLeaderBoard(string leaderboardId, int startIndex,int count,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
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
					KTPlayAndroid.joKTPlayAdapter.CallStatic("lastFriendsLeaderboard", leaderboardId, startIndex, count, gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void LastGlobalLeaderboard(string leaderboardId,int startIndex,int count,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
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
					KTPlayAndroid.joKTPlayAdapter.CallStatic("lastGlobalLeaderboard", leaderboardId, startIndex, count, gameObj.name, methodName);
#endif
				}
			}
		}
	}
}
