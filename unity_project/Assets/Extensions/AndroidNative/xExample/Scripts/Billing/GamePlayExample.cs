////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class GamePlayExample : MonoBehaviour {



	public GameObject[] objectToEnbaleOnInit;
	public GameObject[] objectToDisableOnInit;

	public DefaultPreviewButton[] initBoundButtons;


	public SA_Label coinsLable;
	public SA_Label boostLabel;



	void Awake() {
		GameBillingManagerExample.init();
	}

	void FixedUpdate() {
		coinsLable.text = "Total Coins: " + GameDataExample.coins.ToString();

		if(GameDataExample.IsBoostPurchased) {
			boostLabel.text = "Boost Enabled";
		} else {
			boostLabel.text = "Boost Disabled";
		}

		if(GameBillingManagerExample.isInited) {
			foreach(GameObject o in objectToEnbaleOnInit) {
				o.SetActive(true);
			}

			foreach(GameObject o in objectToDisableOnInit) {
				o.SetActive(false);
			}

			foreach(DefaultPreviewButton btn in initBoundButtons) {
				btn.EnabledButton();
			}
		} else {
			foreach(DefaultPreviewButton btn in initBoundButtons) {
				btn.DisabledButton();
			}
		}
	}

	public void AddCoins () {
		GameBillingManagerExample.purchase(GameBillingManagerExample.COINS_ITEM);
	}

	public void Boost () {
		GameBillingManagerExample.purchase(GameBillingManagerExample.COINS_BOOST);
	}



	
}
