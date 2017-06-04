using UnityEngine;
using System.Collections;

public class ShopData {
	public IAPManager.IAPProduct product;
	public int tier;
	public int price; //￥ RMB
	public int number;
	
	public ShopData(IAPManager.IAPProduct product, int tier, int price, int number){
		this.product = product;
		this.tier = tier;
		this.price = price;
		this.number = number;
	}
	
	public static ShopData[] gameShopData = new ShopData[]{
		new ShopData(IAPManager.IAPProduct.WhiteBlock1, 1, 1, 100),
		new ShopData(IAPManager.IAPProduct.WhiteBlock2, 2, 3, 300),
		new ShopData(IAPManager.IAPProduct.WhiteBlock3, 3, 6, 800),
		new ShopData(IAPManager.IAPProduct.WhiteBlock4, 4, 12, 1500),
		new ShopData(IAPManager.IAPProduct.WhiteBlock5, 5, 18, 2300),	//前面产品数量要跟IAPManager中IAPProduct中的一致
		new ShopData(IAPManager.IAPProduct.VIP, 6, 8, 1000),	//VIP礼包, 除白块外，此后领取礼包翻倍
	};

	public static ShopData GetShopData(IAPManager.IAPProduct product)
	{
		foreach(ShopData shopData in gameShopData)
		{
			if (shopData.product == product)
			{
				return shopData;
			}
		}
		return null;
	}
}
