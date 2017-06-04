////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class GameDataExample  {

	public static int coins {
		get {
			if(PlayerPrefs.HasKey("coins")) {
				return PlayerPrefs.GetInt("coins");
			} else {
				 return 0;
			}

		}
	}

	public static void AddCoins(int amount) {
		if(IsBoostPurchased) {
			amount += Mathf.FloorToInt(amount * 0.2f);
		}

		PlayerPrefs.SetInt("coins", coins + amount);
	}

	public static void EnableCoinsBoost() {
		PlayerPrefs.SetInt("coins_boost", 1);
	}

	public static bool IsBoostPurchased {
		get {
			if(PlayerPrefs.HasKey("coins_boost")) {
				return true;
			} else {
				return false;
			}
		}
	}
}
