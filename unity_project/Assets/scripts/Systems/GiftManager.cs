using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JsonFx.Json;

public class GiftManager {

	private static GiftManager instance = null;
	
	public static GiftManager GetInstance()
	{
		if (instance == null) 
		{
			instance = new GiftManager();
		}
		return instance;
	}

	[Serializable]
	public class GiftConfig{
		public int[] receivedSection = null;
		public int[] cooldownTime = null;

		public GiftConfig(){
			receivedSection = Constant.GIFT_RECEIVED_SECTION;
			cooldownTime = Constant.GIFT_COOLDOWN_TIME;
		}
	};
}
