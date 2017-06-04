using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShopMenu : BaseDialogMenu {
	public GameObject userCenterButton;
	public ShopItem[] shopItems;
	public UISprite	  foreground;

	private int noVIPBgLength = 580;
	private int hasVIPBgLength = 676;

	private List<string> purchasingList = new List<string>(8);

	private Action	  closeCallback;

	void Awake()
	{
		GameSystem.GetInstance().gameUI.shopMenu = this;
		this.gameObject.SetActive(false);
		RefreshShopData();
	}
	
	void Start () {
		userCenterButton.SetActive(Config.isUseCenterActive);
		iOSInterfaces.CallStartRefreshShopData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Show (bool active)
	{
		base.Show (active);
		if (active)
		{
			iOSInterfaces.CallStartRefreshShopData();
		}
	}
	
	public void CloseButtonOnClick(){
		this.Show(false);
		GameSoundSystem.GetInstance().PlayRandomSound();
		if (closeCallback != null)
		{
			closeCallback();
			closeCallback = null;
		}
	}
	
	public void UseCenterButtonOnClick()
	{
	}

	public void RegisterCloseCallback(Action closeCallback)
	{
		this.closeCallback = closeCallback;
	}
	
	public void PressShopItem()
	{
		GameSoundSystem.GetInstance().PlayRandomSound();
		ShopData shopData = UIEventTrigger.current.GetComponent<ShopItem>().Data;
		BuyItem(shopData);
	}

	void BuyItem(ShopData shopData)
	{
		if(CheckIsPurchasing(shopData.product) == false)
		{
			IAPManager.GetInstance().Pay(shopData.product);
		}
	}

	bool CheckIsPurchasing(IAPManager.IAPProduct product)
	{
		string productId = IAPManager.GetInstance().productIDMap[product];
		foreach(string existId in purchasingList)
		{
			if (existId.Equals(productId))
			{
				return true;
			}
		}
		return false;
	}

	public void CheckVIPItem()
	{
		int height = hasVIPBgLength;
		foreach(ShopItem shopItem in shopItems)
		{
			if (shopItem.Data.product == IAPManager.IAPProduct.VIP && GameSystem.GetInstance().IsVIP)
			{
				shopItem.gameObject.SetActive(false);
				height = noVIPBgLength;
				break;
			}
		}
		foreground.height = height;
	}

	public void ChangeItemDisplayState(IAPManager.IAPProduct product, bool isPurchasing)
	{
		foreach(ShopItem shopItem in shopItems)
		{
			if (shopItem.Data.product == product)
			{
				shopItem.ChangeDisplayState(isPurchasing);
				break;
			}
		}
	}

	void RequestProductsFinished(string shopDataString)
	{		
		ArrayList shopDataList = KTPlaySDKJson.KTJSON.jsonDecode(shopDataString) as ArrayList;
		foreach(Hashtable hashtable in shopDataList)
		{
			if (hashtable.Contains("price"))
			{
				string productId = hashtable["id"].ToString();
				int price = Convert.ToInt32(hashtable["price"]);
				IAPManager.IAPProduct product = IAPManager.GetInstance().IDProductMap[productId];
				foreach(ShopData shopData in ShopData.gameShopData)
				{
					if (shopData.product == product)
					{
						shopData.price = price;
						break;
					}
				}
			}
		}
		RefreshShopData();
	}

	void RefreshShopData()
	{
		int index = 0;
		foreach(ShopItem shopItem in shopItems)
		{
			if (index < ShopData.gameShopData.Length)
			{
				shopItem.Data = ShopData.gameShopData[index];
				shopItem.gameObject.SetActive(true);
			}
			else
			{
				shopItem.gameObject.SetActive(false);
			}
			index++;
		}
		CheckVIPItem();
	}

	#region Below three methods are called from xcode objective-c side
	void InitializePurchsingList(string purchasingData)
	{
		string[] dataStrings = purchasingData.Split(new char[]{','});
		foreach(string data in dataStrings)
		{
			purchasingList.Add(data);
			IAPManager.IAPProduct product = IAPManager.GetInstance().IDProductMap[data];
			ChangeItemDisplayState(product, true);
		}
	}
	
	void AddPurchasingItem(string productId)
	{
		if (purchasingList.Contains(productId) == false)
		{
			purchasingList.Add(productId);
			IAPManager.IAPProduct product = IAPManager.GetInstance().IDProductMap[productId];
			ChangeItemDisplayState(product, true);
		}
	}
	
	void RemovePurchasingItem(string productId)
	{
		if (purchasingList.Contains(productId))
		{
			purchasingList.Remove(productId);
			IAPManager.IAPProduct product = IAPManager.GetInstance().IDProductMap[productId];
			ChangeItemDisplayState(product, false);
		}
	}
	#endregion
}
