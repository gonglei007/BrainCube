
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

#if UNITY_IOS || UNITY_EDITOR

public class KTFriendshipiOS : MonoBehaviour
{
	public const string LIB_NAME = "__Internal";

	[DllImport (LIB_NAME)]
	private static extern void KT_FriendRequestCallback(string GameobjectName,string methodName,string userIds);

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
					KT_FriendRequestCallback(gameObj.name,methodName,userIds);
				}
			}
		}
	}

	[DllImport (LIB_NAME)]
	private static extern void KT_FriendListCallback(string GameobjectName,string methodName);
	
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
					KT_FriendListCallback(gameObj.name,methodName);
				}
			}
		}
	}
	
	[DllImport (LIB_NAME)]
	private static extern void KT_ShowFriendRequestsView();

	public static void ShowFriendRequestsView()
	{
		 KT_ShowFriendRequestsView();
	}
}

#endif
