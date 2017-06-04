using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;


public class Example : MonoBehaviour {



	void Start () {
		#if UNITY_IPHONE || UNITY_ANDROID
			//请到 http://www.umeng.com/analytics 获取app key
			GA.StartWithAppKeyAndChannelId ("app key" , "App Store");
			
			//调试时开启日志
			GA.SetLogEnabled (true);
			
			
			//触发统计事件 开始关卡
			GA.StartLevel ("your level name");
#endif
	}





}


