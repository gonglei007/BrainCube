
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

public class KTFriendshipAndroid : MonoBehaviour
{
#if KTUNUSE
	public static void SendFriendRequest(ArrayList userIds,MonoBehaviour obj,KTFriendship.Callback callbackMethod)
	{
		if(obj!=null && callbackMethod!=null)
		{
			GameObject gameObj = obj.gameObject;
			if(gameObj != null && callbackMethod != null)
			{
				string methodName = ((System.Delegate)callbackMethod).Method.Name;
				if(methodName != null)
				{

					AndroidJavaObject joArrayList = null;
					joArrayList = new AndroidJavaObject("java.util.ArrayList");
					if(joArrayList == null)
						Debug.Log("===SendFriendRequest joArrayList ==null");
					else
					{
						foreach (var param in userIds)
						{
							joArrayList.Call<bool>("add", param);
						}
					}
					KTPlayAndroid.joKTPlayAdapter.CallStatic("sendFriendRequests", userIds.Count, joArrayList, gameObj.name, methodName);
/
				}
			}
		}
	}
#endif	
	
	public static void SendFriendRequest(string userIds,MonoBehaviour obj,KTFriendship.Callback callbackMethod)
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
					AndroidJavaObject joArrayList = null;
					joArrayList = new AndroidJavaObject("java.util.ArrayList");
					if(joArrayList == null)
						Debug.Log("===SendFriendRequest joArrayList ==null");
					else
					{
						string[] userIdsstr = userIds.Split(new char[] {','});
						
						foreach(string userId in userIdsstr)
						{
							joArrayList.Call<bool>("add", userId);
						}
						
						KTPlayAndroid.joKTPlayAdapter.CallStatic("sendFriendRequests", joArrayList, userIdsstr.Length, gameObj.name, methodName);
					}
#endif
				}
			}
		}
	}

	public static void FriendList(MonoBehaviour obj,KTFriendship.Callback callbackMethod)
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
					KTPlayAndroid.joKTPlayAdapter.CallStatic("friendList", gameObj.name, methodName);
#endif
				}
			}
		}
	}

	public static void ShowFriendRequestsView()
	{
#if UNITY_ANDROID
		KTPlayAndroid.joKTPlayAdapter.CallStatic("showFriendRequestsView");
#endif
	}
}

