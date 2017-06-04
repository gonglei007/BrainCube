using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constant {
	public static int		SCREEN_WIDTH			= 640;		//屏幕标准设计宽度
	public static int		SCREEN_HEIGHT			= 960;		//屏幕标准设计高度
	public static float		STANDARD_WH_ASPECT		= SCREEN_WIDTH * 1.0f / SCREEN_HEIGHT; //标准长宽比

	public const int		HP_INITIAL				= 20;		//生存模式初始血量
	public const int		HP_BONUS				= 1;		//生存模式点对格子增加的生命值
	public const int		HP_PUNISH				= -5;		//生存模式点错格子扣掉的生命值
	public const int		HP_MAX					= 30;		//生存模式的血量上限

	public const int		LIFE_TIME_INITIAL		= 3;		//限时模式的初始时间
	public const float		LIFE_TIME_BONUS			= 0.3f;		//限时模式点对格子的时间奖励值
	public const int		LIFE_TIME_PUNISH		= -2;		//限时模式点错格子的时间惩罚值
	public const int		LIFE_TIME_MAX			= 5;		//限时模式的时间上限

	public const int		DUAL_HP_INIT			= 3;		//双人模式的初始血量
	public const int		DUAL_HP_MAX			 	= 6;		//双人模式胜利血量
	public static Color		LEFT_COLOR				= new Color(1, 102/255.0f, 1);
	public static Color		RIGHT_COLOR				= new Color(102/255.0f, 204/255.0f, 1);

	public const int		SUM_TARGET				= 21;		//21点模式求和的目标值

	public const int		MAX_PASSLEVEL			= 999;		//闯关模式最多999关

	//解锁各模式需要的白块数量, 除反转模式外。反转模式对应的数值是经典模式需要达到的分数才可解锁
	public static int[]		MODE_UNLOCK_COIN		= new int[]{0,60,100,100,500,600,700,800,0};
	//每日奖励的白块数量
	public static int[]		DAILY_REWARD_DATA		= new int[]{30,50,80,160,200,350,1000};
	//礼包界面中白块数量的索引
	public static int[]		GIFT_NUMBERS_ORDER		= new int[]{0,1,2,0,4,3,2,1};

	public const int		SCORE_TO_COIN			= 1;		//分数除以该值为获得的白块数

	public static int		RESCUE_BASIC_PRICE		= 40;		//原地复活的基础价格	
	public static int		RESCUE_COST_COEFF		= 2;		//原地的复活消耗的白块是该数值的N次方乘以基础值(N是已经复活过的次数)
	public const float		RESCUE_TIME				= 5;		//原地复活的确认框自动关闭的时间
	public const int		RESCUE_MAX_TIMES		= 3;		//原地复活最大次数（一次游戏中）

	public const int 		TOP_RANK_COUNT 			= 10;		//全局排行中显示的排名个数
	public const int 		MY_RANK_COUNT 			= 7;		//我的排行中显示的排名个数

	public static Color		COLOR_WHITE				= Color.white;
	public static Color		COLOR_LIGHT_WHITE		= new Color(230.0f/255.0f, 230.0f/255.0f, 230.0f/255.0f);
	public static Color		COLOR_DARK_BLACK		= new Color(51.0f/255.0f, 51.0f/255.0f, 51.0f/255.0f);
	public static Color		COLOR_GRAY				= new Color(128.0f/256.0f, 128.0f/256.0f, 136.0f/256.0f);

	// server config
	//每个模式试玩的关卡数限制
	public static int[]		MODE_TRIAL_LIMIT		= new int[]{13,5,5,5,5,5,5,5,5};
	//礼包领取次数对冷却时间影响的区间
	public static int[]		GIFT_RECEIVED_SECTION	= new int[]{3,5,10,20};
	//礼包冷却时间, 单位为分钟
	public static int[]		GIFT_COOLDOWN_TIME		= new int[]{5,20,60,120,240};
	//礼包中赠送的白块的数量及概率
	public static Dictionary<int, float> GIFT_NUMBER_PROBABILITY = new Dictionary<int, float>()
	{
		{5, 0.05f},
		{10, 0.2f},
		{15, 0.5f},
		{20, 0.2f},
		{30, 0.05f}
	};

	public const int		RECOMMEND_TRIGGER_TIMES	= 3;		//推荐模式触发间隔（进入主页面的次数）
	//模式推荐高优先级，随机选一个没解锁的推荐
	public static GameSystem.Mode[]	RECOMMEND_ORDER_HIGH_PRIORITY = new GameSystem.Mode[]{GameSystem.Mode.Chimp,
																						GameSystem.Mode.Word,
																						GameSystem.Mode.ColorFull,
																						GameSystem.Mode.BlackJack,};
	//模式推荐低优先级，随机选一个没解锁的推荐
	public static GameSystem.Mode[]	RECOMMEND_ORDER_LOW_PRIORITY = new GameSystem.Mode[]{GameSystem.Mode.TimeRush,
																						GameSystem.Mode.Survival};
	
	//激活游戏赠送的白块数
	public static int ACTIVATE_GAME_BONUS		= 90;
	//激活游戏的RMB购买价格
	public static int ACTIVATE_GAME_RMB_PRICE	= 6;
	//复活RMB购买的价格
	public static int RESCUE_RMB_PRICE			= 2;
	//分享游戏奖励白块儿
	public static int SHARE_GAME_BONUS			= 50;
}
