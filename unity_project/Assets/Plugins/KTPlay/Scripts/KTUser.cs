using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


// 50x50	http://dw.img.ktplay.cn/group1/M00/62/5D/CgIAEVPL7AGAItQaAAHic5V3UQU0010831_50x50
// 80x80	http://dw.img.ktplay.cn/group1/M00/62/5D/CgIAEVPL7AGAItQaAAHic5V3UQU0010831_80x80
// 120x120	http://dw.img.ktplay.cn/group1/M00/62/5D/CgIAEVPL7AGAItQaAAHic5V3UQU0010831_120x120
// 200x200
public class KTUser
{
	/// <summary>
	///   KT用户唯一标识符
	/// </summary>
	public string userId;
	/// <summary>
	///   KT用户头像Url，如果想要获取头像缩率图请再url后面追加支持的缩率图大小。
	///   支持大小列表：_32x32,_50x50,_64x64,_80x80,_120x120,_128x128,_200x200,_256x256
	/// </summary>
	public string headerUrl;
	/// <summary>
	///   KT用户昵称
	/// </summary>
	public string nickname;
	/// <summary>
	///   性别  0:未知;1:男;2:女
	/// </summary>
	public double gender;
	/// <summary>
	///   城市
	/// </summary>
	public string city;
	/// <summary>
	///   扩展属性，获取排行榜时返回用户的积分
	/// </summary>
	public string score;
	/// <summary>
	///   扩展属性，获取排行榜时返回用户的排行
	/// </summary>
	public double rank;
	/// <summary>
	///	保留字段
	/// </summary>
	public string snsUserId;
	/// <summary>
	///	保留字段
	/// </summary>
	public string loginType;
	/// <summary>
	///   游戏用户唯一标识符,网游登录用户有效
	/// </summary>
	public string gameUserId;
	/// <summary>
	///   当前用户昵称是否为KTPlay平台生成
	/// </summary>
	public bool needPresentNickname;

	public KTUser()
	{
	}

	public void setUserId(string userId)
	{
		this.userId = userId;
	}
	public void setHeaderUrl(string headerUrl)
	{
		this.headerUrl = headerUrl;
	}
	public void setNickname(string nickname)
	{
		this.nickname = nickname;
	}
	public void setGender(int gender)
	{
		this.gender = (double)gender;
	}
	public void setCity(string city)
	{
		this.city = city;
	}
	public void setScore(string score)
	{
		this.score = score;
	}
	public void setRank(long rank)
	{
		this.rank = (double)rank;
	}
	public void setSnsUserId(string snsUserId)
	{
		this.snsUserId = snsUserId;
	}
	public void setLoginType(string loginType)
	{
		this.loginType = loginType;
	}
	public void setGameUserId(string gameUserId)
	{
		this.gameUserId = gameUserId;
	}
	public void setNeedPresentNickname(bool needPresentNickname)
	{
		this.needPresentNickname = needPresentNickname;
	}

	public  KTUser(Hashtable param)
	{
		if((string)param["userId"]!= null)
			this.userId = (string)param["userId"];

		if((string)param["headerUrl"]!=  null)
			this.headerUrl = (string)param["headerUrl"];

		if((string)param["nickname"]!= null)
			this.nickname = (string)param["nickname"];

		if((string)param["city"]!=  null)
			this.city = (string)param["city"];

		if((string)param["score"]!= null)
			this.score = (string)param["score"];

		if((string)param["snsUserId"]!=  null)
			this.snsUserId = (string)param["snsUserId"];

		if((string)param["loginType"]!=  null)
			this.loginType = (string)param["loginType"];

		if(param["gender"]!= null)
		{
			this.gender = (double)param["gender"] ;
		}

		if(param["rank"]!= null)
		{
			this.rank = (double)param["rank"] ;
		}

		if(param["gameUserId"]!=null)
		{
			this.gameUserId = (string)param["gameUserId"];
		}

		if(param["needPresentNickname"]!=null)
		{
			this.needPresentNickname = (bool)param["needPresentNickname"];
		}
	}
}
