using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPManager : MonoBehaviour{

	public static IAPManager instance;
	public static IAPManager GetInstance()
	{
		return instance;
	}

	public enum IAPProduct
	{
		ActiveGame,
		VIP,
		WhiteBlock1,
		WhiteBlock2,
		WhiteBlock3,
		WhiteBlock4,
		WhiteBlock5,
		Rescue,
	};
	public Dictionary<IAPProduct, string>	productIDMap = new Dictionary<IAPProduct, string>();
	public Dictionary<string, IAPProduct>	IDProductMap = new Dictionary<string, IAPProduct>();

	void Awake()
	{
		instance = this;
		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) 
		{
			Config.isSkipPurchase = true;
		}

		// 各个渠道分支根据自己的需求定义ID
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor) {
			productIDMap[IAPProduct.ActiveGame] = "com.gltop.activegame";
			productIDMap[IAPProduct.Rescue] = "com.gltop.rescue";
			productIDMap[IAPProduct.VIP] = "com.gltop.vip";
			productIDMap[IAPProduct.WhiteBlock1] = "com.gltop.whiteblock1";
			productIDMap[IAPProduct.WhiteBlock2] = "com.gltop.whiteblock2";
			productIDMap[IAPProduct.WhiteBlock3] = "com.gltop.whiteblock3";
			productIDMap[IAPProduct.WhiteBlock4] = "com.gltop.whiteblock4";
			productIDMap[IAPProduct.WhiteBlock5] = "com.gltop.whiteblock5";
		} else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
			productIDMap[IAPProduct.ActiveGame] = "001";
			productIDMap[IAPProduct.VIP] = "002";
			productIDMap[IAPProduct.WhiteBlock1] = "003";
			productIDMap[IAPProduct.WhiteBlock2] = "004";
			productIDMap[IAPProduct.WhiteBlock3] = "005";
			productIDMap[IAPProduct.WhiteBlock4] = "006";
			productIDMap[IAPProduct.WhiteBlock5] = "007";
			productIDMap[IAPProduct.Rescue] = "008";
		}
		foreach(KeyValuePair<IAPProduct, string> item in productIDMap){
			IDProductMap[item.Value] = item.Key;
		}
	}

	// Old method, remove it after android implemented.
	static public void Pay(string id, string callbackGameObject, string callbackMethod)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidInterfaces.CallPay(id, callbackGameObject, callbackMethod);
		}
	}

	public void Pay(IAPProduct product)
	{
		string productID = productIDMap[product];
		if (Config.isSkipPurchase)
		{
			PayCallback("succeed," + productID);
		}
		else
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				//TODO:GL - android平台尚未实现
				AndroidInterfaces.CallPay(productID, "", "");
			}
			else if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				iOSInterfaces.CallPay(productID);
			}
		}
	}
	
	public void Restore(IAPProduct product)
	{
		string productID = productIDMap[product];
		if (Config.isSkipPurchase)
		{
			PayCallback("succeed," + productID);
		}
		else
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				//TODO:GL - android平台尚未实现
				AndroidInterfaces.CallRestore(productID, "", "");
			}
			else if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				iOSInterfaces.CallRestore(productID);
			}
		}
	}

	// callback from platform purchse
	public void PayCallback(string param)
	{
		string[] returnParams = param.Split(","[0]);
		if (returnParams.Length != 2) 
		{
			Debug.LogError("Invalid pay callback params!");
			return;
		}
		string result = returnParams[0];
		string productID = returnParams[1];
		IAPProduct product = IDProductMap [productID];
		if (result.Equals("succeed"))
		{
			ShopData shopData = ShopData.GetShopData(product);
			switch(product)
			{
			case IAPProduct.ActiveGame:
				GameSystem.GetInstance().ActivateGame();
				UMengManager.Pay(Constant.ACTIVATE_GAME_RMB_PRICE, Umeng.GA.PaySource.支付宝, UMengManager.Item_IAP[(int)shopData.product], 1, Constant.ACTIVATE_GAME_BONUS);
				break;
			case IAPProduct.VIP:
				GameSystem.GetInstance().Coin += shopData.number;
				PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);
				UMengManager.Bonus(shopData.number, (Umeng.GA.BonusSource)UMengManager.Bonus_Source_Buy_VIP);
				UMengManager.Pay(shopData.price, Umeng.GA.PaySource.支付宝, UMengManager.Item_IAP[(int)shopData.product], 1, shopData.number);

				GameSystem.GetInstance().BecomeVIP();
				break;
			case IAPProduct.WhiteBlock1:
			case IAPProduct.WhiteBlock2:
			case IAPProduct.WhiteBlock3:
			case IAPProduct.WhiteBlock4:
			case IAPProduct.WhiteBlock5:
				GameSystem.GetInstance().Coin += shopData.number;
				PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);
				UMengManager.Pay(shopData.price, Umeng.GA.PaySource.支付宝, shopData.number);
				UMengManager.Pay(shopData.price, Umeng.GA.PaySource.支付宝, UMengManager.Item_IAP[(int)shopData.product], 1, shopData.number);

#if UNITY_IOS
				if (GameSystem.GetInstance().UnlockRecommendMode)
				{
					GameSystem.GetInstance().UnlockRecommendMode = false;
					GameSystem.GetInstance().PayUnlockRecommendModeCallback(true);
				}
#else
				if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
				{
					GameSystem.GetInstance().PayUnlockModeCallback(true);
				}
#endif
				break; 
			case IAPProduct.Rescue:
				GameSystem.GetInstance().CurrentModeLogic.Rescue();
				GameSystem.GetInstance().ChangeState(GameSystem.States.GamePreview);

				UMengManager.Pay(Constant.RESCUE_RMB_PRICE, Umeng.GA.PaySource.支付宝, UMengManager.Item_IAP[(int)shopData.product], 1, Constant.RESCUE_BASIC_PRICE);
				break;
			}
		}
		else if (result.Equals("failed"))
		{
#if !UNITY_IOS
			string rescueTitleText = TextManager.GetText("pay_failed");
			GameSystem.GetInstance().gameUI.confirmMenu.SetContent(rescueTitleText, null, ConfirmStyle.OnlyYes);
			GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
#endif
			switch(product)
			{
			case IAPProduct.ActiveGame:
				GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback((confirmResult)=>{
					if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
					{
						GameSystem.GetInstance().JumpToGameEnd();
					}
				});
				break;
			case IAPProduct.VIP:
				break;
			case IAPProduct.WhiteBlock1:
			case IAPProduct.WhiteBlock2:
			case IAPProduct.WhiteBlock3:
			case IAPProduct.WhiteBlock4:
			case IAPProduct.WhiteBlock5:
#if UNITY_IOS
				if (GameSystem.GetInstance().UnlockRecommendMode)
				{
					GameSystem.GetInstance().UnlockRecommendMode = false;
					GameSystem.GetInstance().PayUnlockRecommendModeCallback(false);
				}
#else
				if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
				{
					GameSystem.GetInstance().PayUnlockModeCallback(false);
				}
#endif
				break;
			case IAPProduct.Rescue:
				GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback((confirmResult)=>{
					if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
					{
						GameSystem.GetInstance().JumpToGameEnd();
					}
				});
				break;
			}
		}
		else if (result.Equals("cancel"))
		{
#if !UNITY_IOS
			string rescueTitleText = TextManager.GetText("pay_cancel");
			GameSystem.GetInstance().gameUI.confirmMenu.SetContent(rescueTitleText, null, ConfirmStyle.OnlyYes);
			GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
#endif
			switch(product)
			{
			case IAPProduct.ActiveGame:
				GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback((confirmResult)=>{
					if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
					{
						GameSystem.GetInstance().JumpToGameEnd();
					}
				});
				break;
			case IAPProduct.VIP:
				break;
			case IAPProduct.WhiteBlock1:
			case IAPProduct.WhiteBlock2:
			case IAPProduct.WhiteBlock3:
			case IAPProduct.WhiteBlock4:
			case IAPProduct.WhiteBlock5:
#if UNITY_IOS
				if (GameSystem.GetInstance().UnlockRecommendMode)
				{
					GameSystem.GetInstance().UnlockRecommendMode = false;
					GameSystem.GetInstance().PayUnlockRecommendModeCallback(false);
				}
#else
				if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
				{
					GameSystem.GetInstance().PayUnlockModeCallback(false);
				}
#endif
				break;
			case IAPProduct.Rescue:
				GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback((confirmResult)=>{
					if(GameSystem.GetInstance().CurrentState == GameSystem.States.WaveComplete)
					{
						GameSystem.GetInstance().JumpToGameEnd();
					}
				});
				break;
			}
		}
	}
}
