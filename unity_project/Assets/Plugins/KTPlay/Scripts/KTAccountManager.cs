
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;



public class KTAccountManager : MonoBehaviour
{
	public delegate void Callback(string s);
	/// <summary>
    ///  设置监听者，监听KTPlay登录状态变更
    /// </summary>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void SetLoginStatusChange(MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTAccountManagerAndroid.SetLoginStatusChangeCallback(obj, callbackMethod);
#elif UNITY_IOS

		KTAccountManageriOS.SetLoginStatusChangeCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  获取任意KTplay用户信息
    /// </summary>
    /// <param name="userId">KT用户唯一标示符</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void UserProfile(string userId,MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTAccountManagerAndroid.UserProfile(userId, obj, callbackMethod);
#elif UNITY_IOS
		KTAccountManageriOS.UserProfile(userId,obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  打开KTPlay登录界面
    /// </summary>
    /// <param name="closeable">closeable 登录界面是否可由玩家关闭， YES 可以，NO 不可以</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void ShowLoginView(bool closeable, MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTAccountManagerAndroid.ShowLoginView(closeable, obj,callbackMethod);
#elif UNITY_IOS
		KTAccountManageriOS.ShowLoginView(closeable, obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  使用网游帐户登录KTPlay平台
    /// </summary>
    /// <param name="gameUserId">游戏用户ID</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void loginWithGameUser(string gameUserId, MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTAccountManagerAndroid.loginWithGameUser(gameUserId, obj,callbackMethod);
#elif UNITY_IOS
		KTAccountManageriOS.loginWithGameUser(gameUserId, obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  修改当前登录帐户昵称
    /// </summary>
    /// <param name="nickname">新昵称</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void setNickname(string nickName, MonoBehaviour obj,KTAccountManager.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTAccountManagerAndroid.setNickname(nickName, obj,callbackMethod);
#elif UNITY_IOS
		KTAccountManageriOS.setNickname(nickName, obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  登出当前用户
    /// </summary>
	public static void logout()
	{
#if UNITY_ANDROID
		KTAccountManagerAndroid.logout();
#elif UNITY_IOS
		KTAccountManageriOS.logout();
#else
#endif
	}
	/// <summary>
    ///  判断是否已有用户登录
    /// </summary>
    /// <return>是否已有用户登录</return>
	public static bool IsLoggedIn()
	{
#if UNITY_ANDROID
		return KTAccountManagerAndroid.IsLoggedIn();
#elif UNITY_IOS
		return KTAccountManageriOS.IsLoggedIn();
#else
#endif
		return false;
	}
	/// <summary>
    ///  获取当前登录KTplay用户信息
    /// </summary>
    /// <return>返回KTplay用户信息，如果用户未登录返回为nil</return>
	public static KTUser CurrentAccount()
	{
#if UNITY_ANDROID
		return KTAccountManagerAndroid.CurrentAccount();
#elif UNITY_IOS
		return KTAccountManageriOS.CurrentAccount();
#else
#endif
		return null;
	}

	public class KTAccountManagerCallbackParams
	{
		private const string KEY_WHAT = "what";
		private const string KEY_PARAMS = "params";
		private const string KEY_REQINFO = "requestInfo";

		public enum KTAccountManagerEvent
		{
			/// <summary>
    		///   KTPlay登录状态变更事件
    		/// </summary>
			KTPlayAccountEventStatusChange = 100,
			/// <summary>
    		///   获取KTplay用户信息事件
    		/// </summary>
			KTPlayAccountEventUserProfile,
			/// <summary>
    		///   打开KTPlay登录界面的登录信息事件
    		/// </summary>
			KTPlayAccountEventLoginViewLogin,
			/// <summary>
    		///    使用网游帐户登录的登录信息事件
    		/// </summary>
			KTPlayAccountEventLoginWhithGameUser,
			/// <summary>
    		///   修改当前登录帐户昵称事件
    		/// </summary>
			KTPlayAccountEventSetNickName,
			/// <summary>
    		///   错误事件
    		/// </summary>
			OnKTError = 1000
		};

		public KTAccountManagerEvent accountManagerEventResult;
		public KTUser statusUser = null;
		public bool isLogin = false;
		public KTUser oneUser = null;
		public string userId = null;
		public string gameUserId = null;
		public string nickName = null;
		public KTError playError = null;

		public void ParseFromString(string s)
		{
			Hashtable data = (Hashtable)KTJSON.jsonDecode(s);

			if(data[KEY_WHAT] != null)
			{
				this.accountManagerEventResult = (KTAccountManagerEvent)((double)data[KEY_WHAT]);

				switch(this.accountManagerEventResult)
				{
				case KTAccountManagerEvent.KTPlayAccountEventStatusChange:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];

					if(param["userId"] == null)
					{
						statusUser = null;
					}
					else
					{
						statusUser = new KTUser(param);
					}

					if((bool)param["isLogin"] == true)
					{
						isLogin = true;
					}
					else
					{
						isLogin = false;
					}
				}
				break;
				case KTAccountManagerEvent.KTPlayAccountEventUserProfile:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if(param["userId"] == null)
					{
						oneUser = null;
					}
					else
					{
						oneUser = new KTUser(param);
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						userId = (string)requestInfo[@"userId"];
					}
				}
				break;
				case KTAccountManagerEvent.KTPlayAccountEventLoginViewLogin:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if(param["userId"] == null)
					{
						oneUser = null;
					}
					else
					{
						oneUser = new KTUser(param);
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						userId = (string)requestInfo[@"userId"];
					}
				}
				break;
				case KTAccountManagerEvent.KTPlayAccountEventLoginWhithGameUser:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if(param["userId"] == null)
					{
						oneUser = null;
					}
					else
					{
						oneUser = new KTUser(param);
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						gameUserId = (string)requestInfo[@"gameUserId"];
					}
				}
				break;
				case KTAccountManagerEvent.KTPlayAccountEventSetNickName:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if(param["userId"] == null)
					{
						oneUser = null;
					}
					else
					{
						oneUser = new KTUser(param);
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						nickName = (string)requestInfo[@"nickName"];
					}
				}
				break;
				case KTAccountManagerEvent.OnKTError:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					playError = new KTError ();
					if(param["description"] != null)
					{
						playError.description = (string)param["description"];
					}
					if(param["code"] != null)
					{
						playError.code = (double)param["code"];
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						userId = (string)requestInfo[@"userId"];
					}
					else
					{
						userId = null;
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						gameUserId = (string)requestInfo[@"gameUserId"];
					}
					else
					{
						gameUserId = null;
					}

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						nickName = (string)requestInfo[@"nickName"];
					}
					else
					{
						nickName = null;
					}
				}
				break;
				}
			}
		}

		public KTAccountManagerCallbackParams(string s)
		{
			this.ParseFromString(s);
		}
	};
}
