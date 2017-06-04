
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

#if UNITY_IOS || UNITY_EDITOR

public class KTLeaderboardiOS : MonoBehaviour
{
	public const string LIB_NAME = "__Internal";
	public delegate void Callback(string s);

	[DllImport (LIB_NAME)]
	private static extern void KT_FriendsLeaderBoardCallback(string GameobjectName,string methodName,string leaderboardId,int startIndex,int count);
	
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
					KT_FriendsLeaderBoardCallback(gameObj.name,methodName,leaderboardId,startIndex,count);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_GlobalLeaderBoardCallback(string GameobjectName,string methodName,string leaderboardId,int startIndex,int count);
	
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
					KT_GlobalLeaderBoardCallback(gameObj.name,methodName,leaderboardId,startIndex,count);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_LastFriendsLeaderBoardCallback(string GameobjectName,string methodName,string leaderboardId,int startIndex,int count);

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
					KT_LastFriendsLeaderBoardCallback(gameObj.name,methodName,leaderboardId,startIndex,count);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]
	private static extern void KT_LastGlobalLeaderBoardCallback(string GameobjectName,string methodName,string leaderboardId,int startIndex,int count);
	
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
					KT_LastGlobalLeaderBoardCallback(gameObj.name,methodName,leaderboardId,startIndex,count);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]	
	private static extern void KT_ReportScoreCallback(string GameobjectName,string methodName,int score,string leaderboardId);
	
	public static void ReportScore(int score,string leaderboardId,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{
					KT_ReportScoreCallback(gameObj.name,methodName,score,leaderboardId);
				}
			}
		}
	}
}

#endif
