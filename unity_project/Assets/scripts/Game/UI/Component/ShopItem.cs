using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour {
	public UISprite	buyButtonBg;
	public UILabel	buyLabel;
	public UILabel	priceLabel;
	public UILabel	descLabel;

	private ShopData	data;

	public ShopData Data
	{
		get
		{
			return data;
		}
		set
		{
			data = value;
			priceLabel.text = string.Format("￥{0}", data.price.ToString());
			if (data.product == IAPManager.IAPProduct.VIP)
			{
				descLabel.text = string.Format(TextManager.GetText("vip_gift_desc"), data.number);
			}
			else
			{
				descLabel.text = string.Format(TextManager.GetText("shop_item_desc"), data.number);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeDisplayState(bool isPurchasing)
	{
		if (isPurchasing)
		{
			buyLabel.text = TextManager.GetText("purchasing");
		}
		else
		{
			buyLabel.text = TextManager.GetText("buy");
		}
		buyButtonBg.enabled = !isPurchasing;
	}
}
