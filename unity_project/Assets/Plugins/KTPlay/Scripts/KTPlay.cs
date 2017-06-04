using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;


public class KTPlay : MonoBehaviour
{


	private static bool isScreenShooting = false;

	/// <summary>
	///   奖励类
	/// </summary>
	public class KTPlayRewardsItem
	{
		/// <summary>
		///   奖励名称
		/// </summary>
		public string name;
		/// <summary>
    ///   奖励ID
		/// </summary>
		public string typeId;
		/// <summary>
		///   奖励值
		/// </summary>
		public double value;
	}

	public delegate void Callback(string s);

	public void Awake()
	{
#if UNITY_ANDROID
		KTPlayAndroid.Init();
#endif
	}
 	/// <summary>
	///  设置监听者，监听打开KTPlay主窗口事件
	/// </summary>
	/// <param name="obj">MonoBehaviour子类对象</param>
	/// <param name="callbackMethod">监听事件响应</param>
	public static void SetViewDidAppearCallback(MonoBehaviour obj, KTPlay.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetViewDidAppearCallback(obj, callbackMethod);
#elif UNITY_IOS
		KTPlayiOS.SetViewDidAppearCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
	///  设置监听者，监听关闭KTPlay主窗口事件
	/// </summary>
	/// <param name="obj">MonoBehaviour子类对象</param>
	/// <param name="callbackMethod">监听事件响应</param>
	public static void SetViewDidDisappearCallback(MonoBehaviour obj, KTPlay.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetViewDidDisappearCallback(obj, callbackMethod);
#elif UNITY_IOS
		KTPlayiOS.SetViewDidDisappearCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
	///   设置监听者，监听奖励发放事件
	/// </summary>
	/// <param name="obj">MonoBehaviour子类对象</param>
	/// <param name="callbackMethod">监听事件响应</param>
	public static void SetDidDispatchRewardsCallback(MonoBehaviour obj, KTPlay.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetDidDispatchRewardsCallback(obj, callbackMethod);
#elif UNITY_IOS
		KTPlayiOS.SetDidDispatchRewardsCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
	///   设置监听者，监听用户新动态
	/// </summary>
	/// <param name="obj">MonoBehaviour子类对象</param>
	/// <param name="callbackMethod">监听事件响应</param>
	public static void SetActivityStatusChangedCallback(MonoBehaviour obj, KTPlay.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetActivityStatusChangedCallback(obj, callbackMethod);
#elif UNITY_IOS
		KTPlayiOS.SetActivityStatusChangedCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
	///   设置监听者，监听KTPlay SDK的可用状态变更
	/// </summary>
	/// <param name="obj">MonoBehaviour子类对象</param>
	/// <param name="callbackMethod">监听事件响应</param>
	public static void SetAvailabilityChangedCallback(MonoBehaviour obj, KTPlay.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetAvailabilityChangedCallback(obj, callbackMethod);
#elif UNITY_IOS
		KTPlayiOS.SetAvailabilityChangedCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
	///   设置监听者，监听DeepLink事件
	/// </summary>
	/// <param name="obj">MonoBehaviour子类对象</param>
	/// <param name="callbackMethod">监听事件响应</param>
	public static void SetDeepLinkCallback(MonoBehaviour obj, KTPlay.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetDeepLinkCallback(obj,callbackMethod);
#elif UNITY_IOS
		KTPlayiOS.SetDeepLinkCallback(obj,callbackMethod);
#else
#endif
	}
	/// <summary>
	///   打开KTPlay主窗口
	/// </summary>
	public static void Show()
	{
#if UNITY_ANDROID
		KTPlayAndroid.Show();
#elif UNITY_IOS
		KTPlayiOS.Show();
#else
#endif
	}
	/// <summary>
	///   打开KTPlay强通知窗口（插屏通知窗口）
	/// </summary>
	public static void ShowInterstitialNotification()
	{
#if UNITY_ANDROID
		KTPlayAndroid.ShowInterstitialNotification();
#elif UNITY_IOS
		KTPlayiOS.ShowInterstitialNotification();
#else
#endif
	}
	/// <summary>
	///   关闭KTPlay主窗口
	/// </summary>
	public static void Dismiss()
	{
#if UNITY_ANDROID
		KTPlayAndroid.Dismiss();
#elif UNITY_IOS
		KTPlayiOS.Dismiss();
#else
#endif
	}
	/// <summary>
	///   判断KTplay是否可用
	///    	KTPlay不可用的情况包括：
 	///    	1、设备不被支持
	///     2、在Portal上关闭
 	///    	3、未获取到服务器配置信息（断网）
	/// </summary>
	/// <return >KTplay是否可用</return>

	public static bool IsEnabled()
	{
#if UNITY_ANDROID
		return KTPlayAndroid.IsEnabled();
#elif UNITY_IOS
		return KTPlayiOS.IsEnabled();
#else
		return false;
#endif
	}
	/// <summary>
	///   判断KTplay主窗口是否处于打开状态
	/// </summary>
	/// <return>KTplay主窗口是否打开</return>
	public static bool IsShowing()
	{
#if UNITY_ANDROID
		return KTPlayAndroid.IsShowing();
#elif UNITY_IOS
		return KTPlayiOS.IsShowing();
#else
		return false;
#endif
	}

	/// <summary>
	///   设置截图旋转角度
	/// </summary>
	/// <param name="degress">截图旋转角度（注意，是角度而不是弧度，取值如90,180等）</param>
	public static void SetScreenshotRotation(float degress)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetScreenshotRotation(degress);
#elif UNITY_IOS
		isScreenShooting = true;
		KTPlayiOS.SetScreenshotRotation(degress);
#else
#endif
	}

	/// <summary>
	///   分享图片/文本到KTPlay社区
	/// </summary>
	/// <param name="imagePath">图片的绝对路径,为nil时，没有默认图片</param>
	/// <param name="description">图片的描述,为nil时，没有默认内容描述</param>
	public static void ShareImageToKT(string imagePath,string description)
	{
#if UNITY_ANDROID
		KTPlayAndroid.ShareImageToKT(imagePath, description);
#elif UNITY_IOS
		KTPlayiOS.ShareImageToKT(imagePath, description);
#else
#endif
	}


	/// <summary>
	///   启用/禁用通知功能
	/// </summary>
	/// <param name="enabled">YES/NO 启用/禁用</param>
	public static void SetNotificationEnabled(bool enabled)
	{
#if UNITY_ANDROID
		KTPlayAndroid.SetNotificationEnabled(enabled);
#elif UNITY_IOS
		KTPlayiOS.SetNotificationEnabled(enabled);
#else
#endif
	}

	public class KTPlayCallbackParams
	{
		private const string KEY_WHAT = "what";
		private const string KEY_PARAMS = "params";
		private const string KEY_REWARD_NAME = "name";
		private const string KEY_REWARD_TYPE = "typeId";
		private const string KEY_REWARD_VALUE = "value";

		public enum KTPlayEvent
		{
			/// <summary>
			///   打开KTPlay主窗口事件
			/// </summary>
			OnAppear = 0,
			/// <summary>
			///   关闭KTPlay主窗口事件
			/// </summary>
			OnDisappear,
			/// <summary>
			///   收到奖励事件
			/// </summary>
			OnDispatchRewards,
			/// <summary>
			///   收到新动态事件
			/// </summary>
			OnActivityStatusChanged,
			/// <summary>
			///   KTPlay 是否可用状态改版事件
			/// </summary>
			OnAvailabilityChanged,
			/// <summary>
			///   收到深度链接事件
			/// </summary>
			OnDeepLink,
			/// <summary>
			///   错误事件
			/// </summary>
			OnKTError = 1000
		};

		public KTPlayEvent KTPlayEventResult;
		public ArrayList rewards;
		public bool hasNewActivity = false;
		public bool isEnabled = false;
		public string linkScheme = null;
		public KTError playError = null;

		public void ParseFromString(string s)
		{
			Hashtable data = (Hashtable)KTJSON.jsonDecode(s);
			this.rewards = null;

			if(data[KEY_WHAT] != null)
			{
				this.KTPlayEventResult = (KTPlayEvent)((double)data[KEY_WHAT]);
				Debug.Log("param.KTPlayEvent=" + this.KTPlayEventResult);

				switch(this.KTPlayEventResult)
				{
				case KTPlayEvent.OnAppear:
				{
#if UNITY_IOS
					isScreenShooting = true;
#endif
				}
				break;
				case KTPlayEvent.OnDisappear:
					break;
				case KTPlayEvent.OnDispatchRewards:
				{
					IList list = (IList)data[KEY_PARAMS];
					rewards = new ArrayList();
					foreach (IDictionary item in list)
					{
						KTPlayRewardsItem rewardItem = new KTPlayRewardsItem();
						if(item[KEY_REWARD_NAME] != null)
							rewardItem.name = (string)item[KEY_REWARD_NAME];
						if(item[KEY_REWARD_TYPE] != null)
							rewardItem.typeId =  (string)item[KEY_REWARD_TYPE];
						if(item[KEY_REWARD_VALUE] != null)
							rewardItem.value = (double)item[KEY_REWARD_VALUE];
						rewards.Add(rewardItem);
					}
				}
				break;
				case KTPlayEvent.OnActivityStatusChanged:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if((bool)param["hasNewActivity"] == true)
					{
						hasNewActivity = true;
					}
					else
					{
						hasNewActivity = false;
					}
				}
				break;
				case KTPlayEvent.OnAvailabilityChanged:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if((bool)param["isEnabled"] == true)
					{
						isEnabled = true;
					}
					else
					{
						isEnabled = false;
					}
				}
				break;
				case KTPlayEvent.OnDeepLink:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					if(param["linkScheme"] != null)
					{
						linkScheme = (string)param["linkScheme"];
					}
					else
					{
						linkScheme = null;
					}
				}
				break;
				case KTPlayEvent.OnKTError:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					playError = new KTError();
					if(param["description"] != null)
					{
						playError.description = (string)param["description"];
					}
					if(param["code"] != null)
					{
						playError.code = (double)param["code"];
					}
				}
				break;
				}
			}
		}

		public KTPlayCallbackParams(string s)
		{
			this.ParseFromString(s);
		}
	};

#if UNITY_IOS
	IEnumerator CapturePNG()
	{
		yield return new WaitForEndOfFrame();
		Debug.Log("post");
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);

		string path = Application.temporaryCachePath + "/kt_captureScreenshot";
		System.IO.File.WriteAllBytes(path,bytes);

		//Application.CaptureScreenshot ("../Library/Caches/kt_captureScreenshot");
	}
#endif

	void Update()
	{
#if UNITY_IOS
		if(isScreenShooting)
		{
			StartCoroutine(CapturePNG());

			isScreenShooting = false;
		}
#endif

#if UNITY_ANDROID
		KTPlayAndroid.Update();
#endif
	}
}
