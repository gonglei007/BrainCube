
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using KTPlaySDKJson;

public class KTPlayLeaderboard : MonoBehaviour
{
	public delegate void Callback(string s);

	/// <summary>
    ///  获取好友排行榜数据
    /// </summary>
    /// <param name="leaderboardId">排行榜id，此数据在开发者网站获取</param>
    /// <param name="startIndex">排行榜起始位置，如果传-1 返回当前登录用户排名所处位置的排行榜数据</param>
    /// <param name="count">获取排行榜数据记录条数</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>

	public static void FriendsLeaderboard(string leaderboardId, int startIndex,int count,MonoBehaviour obj, KTPlayLeaderboard.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTLeaderboardAndroid.FriendsLeaderBoard(leaderboardId, startIndex, count, obj, callbackMethod);
#elif UNITY_IOS
		KTLeaderboardiOS.FriendsLeaderBoard(leaderboardId,startIndex,count,obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  获取游戏排行榜数据
    /// </summary>
    /// <param name="leaderboardId">排行榜id，此数据在开发者网站获取</param>
    /// <param name="startIndex">排行榜起始位置，如果传-1 返回当前登录用户排名所处位置的排行榜数据</param>
    /// <param name="count">获取排行榜数据记录条数</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void GlobalLeaderboard(string leaderboardId,int startIndex,int count,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTLeaderboardAndroid.GlobalLeaderboard(leaderboardId, startIndex, count, obj, callbackMethod);
#elif UNITY_IOS
		KTLeaderboardiOS.GlobalLeaderboard(leaderboardId,startIndex,count,obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  上传得分
    /// </summary>
    /// <param name="score">游戏得分</param>
    /// <param name="leaderboardId">排行榜id，此数据在开发者网站获取</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void ReportScore(int score,string leaderboardId,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTLeaderboardAndroid.ReportScore(score, leaderboardId, obj, callbackMethod);
#elif UNITY_IOS
		KTLeaderboardiOS.ReportScore(score,leaderboardId,obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  获取上周好友排行
    /// </summary>
    /// <param name="leaderboardId">排行榜id，此数据在开发者网站获取</param>
    /// <param name="startIndex">排行榜起始位置，如果传-1 返回当前登录用户排名所处位置的排行榜数据</param>
    /// <param name="count">获取排行榜数据记录条数</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void LastFriendsLeaderboard(string leaderboardId, int startIndex,int count,MonoBehaviour obj, KTPlayLeaderboard.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTLeaderboardAndroid.LastFriendsLeaderBoard(leaderboardId, startIndex, count, obj, callbackMethod);
#elif UNITY_IOS
		KTLeaderboardiOS.LastFriendsLeaderBoard(leaderboardId,startIndex,count,obj,callbackMethod);
#else
#endif
	}
	/// <summary>
    ///  获取上周游戏排行榜数据
    /// </summary>
    /// <param name="leaderboardId">排行榜id，此数据在开发者网站获取</param>
    /// <param name="startIndex">排行榜起始位置，如果传-1 返回当前登录用户排名所处位置的排行榜数据</param>
    /// <param name="count">获取排行榜数据记录条数</param>
    /// <param name="obj">MonoBehaviour子类对象</param>
    /// <param name="callbackMethod">监听事件响应</param>
	public static void LastGlobalLeaderboard(string leaderboardId,int startIndex,int count,MonoBehaviour obj,KTPlayLeaderboard.Callback callbackMethod)
	{
#if UNITY_ANDROID
		KTLeaderboardAndroid.LastGlobalLeaderboard(leaderboardId, startIndex, count, obj, callbackMethod);
#elif UNITY_IOS
		KTLeaderboardiOS.LastGlobalLeaderboard(leaderboardId,startIndex,count,obj,callbackMethod);
#else
#endif
	}

	public class KTLeaderboardPaginator
	{
		/// <summary>
    	///  上榜人数
    	/// </summary>
		public double total;
		/// <summary>
    	///   下一页起始位置
    	/// </summary>
		public string nextCursor;

		/// <summary>
    	///   上一页起始位置
    	/// </summary>
		public string previousCursor;
		/// <summary>
    	///   KTUser数组
    	/// </summary>
		public ArrayList items;
		/// <summary>
    	///   排行榜名字
    	/// </summary>
		public string leaderboardName;
		/// <summary>
    	///   排行榜图标
    	/// </summary>
		public string leaderboardIcon;
		/// <summary>
    	///   排行榜ID
    	/// </summary>
		public string leaderboardId;
		/// <summary>
    	///   我的排名
    	/// </summary>
		public string myScore;
		/// <summary>
    	///   我的积分
    	/// </summary>
		public double myRank;
		/// <summary>
    	///   排行榜单唯一标示符
    	/// </summary>
		public string periodicalSummaryId;

		public KTLeaderboardPaginator(Hashtable param)
		{
			if(param["total"]!=  null)
			{
				this.total = (double)param["total"];
			}

			if((string)param["nextCursor"]!=  null)
			{
				this.nextCursor = (string)param["nextCursor"];
			}

			if((string)param["previousCursor"]!=  null)
			{
				this.previousCursor = (string)param["previousCursor"];
			}

			if(param["myRank"]!=  null)
			{
				this.myRank = (double)param["myRank"];
			}

			if((string)param["myScore"]!=  null)
			{
				this.myScore = (string)param["myScore"];
			}

			if(param["items"] != null)
			{
				IList list = (IList)param["items"];
				this.items = new ArrayList();
				foreach(Hashtable user in list)
				{
					KTUser one = new KTUser(user);
					this.items.Add(one);
				}
			}

			if((string)param["leaderboardName"]!=  null)
			{
				this.leaderboardName = (string)param["leaderboardName"];
			}

			if((string)param["leaderboardIcon"]!=  null)
			{
				this.leaderboardIcon = (string)param["leaderboardIcon"];
			}

			if((string)param["leaderboardId"]!=  null)
			{
				this.leaderboardId = (string)param["leaderboardId"];
			}

			if((string)param["periodicalSummaryId"]!=  null)
			{
				this.periodicalSummaryId = (string)param["periodicalSummaryId"];
			}
		}
	}

	public class KTLeaderboardCallbackParams
	{
		private const string KEY_WHAT = "what";
		private const string KEY_PARAMS = "params";
		private const string KEY_REQINFO = "requestInfo";

		public enum KTLeaderboardEvent
		{
			/// <summary>
    		///   好友排行榜数据事件
    		/// </summary>
			KTPlayLeaderboardEventFriendsLeaderboard = 300,
			/// <summary>
    		///   游戏排行榜数据事件
    		/// </summary>
			KTPlayLeaderboardEventGlobalLeaderboard,
			/// <summary>
    		///   上传得分事件
    		/// </summary>
			KTPlayLeaderboardEventReportScore,
			/// <summary>
    		///   上周好友排行榜数据事件
    		/// </summary>
			KTPlayLeaderboardEventLastFriendsLeaderboard,
			/// <summary>
    		///  上周游戏排行榜数据事件
    		/// </summary>
			KTPlayLeaderboardEventLastGlobalLeaderboard,
			/// <summary>
    		///   错误事件
    		/// </summary>
			OnKTError = 1000
		};

		public KTLeaderboardEvent leaderboardEventResult;
		public KTLeaderboardPaginator friendLeaderboardPaginator = null;
		public KTLeaderboardPaginator globalLeaderboardPaginator = null;
		public KTLeaderboardPaginator lastFriendLeaderboardPaginator = null;
		public KTLeaderboardPaginator lastGlobalLeaderboardPaginator = null;
		public string leaderboardId = null;
		public double score;
		public KTError playError = null;

		public void ParseFromString(string s)
		{
			Hashtable data = (Hashtable)KTJSON.jsonDecode(s);

			if(data[KEY_WHAT] != null)
			{
				this.leaderboardEventResult = (KTLeaderboardEvent)((double)data[KEY_WHAT]);

				switch(this.leaderboardEventResult)
				{
				case KTLeaderboardEvent.KTPlayLeaderboardEventFriendsLeaderboard:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					friendLeaderboardPaginator =  new KTLeaderboardPaginator(param);

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						leaderboardId = (string)requestInfo[@"leaderboardId"];
					}
				}
				break;
				case KTLeaderboardEvent.KTPlayLeaderboardEventGlobalLeaderboard:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					globalLeaderboardPaginator =  new KTLeaderboardPaginator(param);

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						leaderboardId = (string)requestInfo[@"leaderboardId"];
					}
				}
				break;
				case KTLeaderboardEvent.KTPlayLeaderboardEventReportScore:
				{
					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						leaderboardId = (string)requestInfo[@"leaderboardId"];
						score = (double)requestInfo[@"score"];
					}
				}
				break;
				case KTLeaderboardEvent.KTPlayLeaderboardEventLastFriendsLeaderboard:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					lastFriendLeaderboardPaginator =  new KTLeaderboardPaginator(param);

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						leaderboardId = (string)requestInfo[@"leaderboardId"];
					}
				}
				break;
				case KTLeaderboardEvent.KTPlayLeaderboardEventLastGlobalLeaderboard:
				{
					Hashtable param = (Hashtable)data[KEY_PARAMS];
					lastGlobalLeaderboardPaginator =  new KTLeaderboardPaginator(param);

					if(data[KEY_REQINFO] != null)
					{
						Hashtable requestInfo = (Hashtable)data[KEY_REQINFO];
						leaderboardId = (string)requestInfo[@"leaderboardId"];
					}
				}
				break;
				case KTLeaderboardEvent.OnKTError:
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
						leaderboardId = (string)requestInfo[@"leaderboardId"];
						if(requestInfo[@"score"] != null)
						{
							score = (double)requestInfo[@"score"];
						}
					}
				}
				break;
				}
			}
		}

		public KTLeaderboardCallbackParams(string s)
		{
			this.ParseFromString(s);
		}
	}
}

