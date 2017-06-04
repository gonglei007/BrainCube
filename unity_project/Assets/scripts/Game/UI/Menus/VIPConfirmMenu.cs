using UnityEngine;
using System.Collections;

public class VIPConfirmMenu : FixedConfirmMenu {
	public UILabel buyButtonLabel;
	public UILabel restoreButtonLabel;
	public GameObject buyButton;
	public GameObject restoreButton;
	
	void Awake()
	{
		GameSystem.GetInstance().gameUI.vipConfirmMenu = this;
	}

	void Start()
	{
		this.gameObject.SetActive(false);
		if (!Config.enableRestore) {
			restoreButton.SetActive(false);
			Vector3 newPosition = restoreButton.transform.localPosition;
			newPosition.y += 40.0f;
			buyButton.transform.localPosition = newPosition;
		} 
	}

	public void OnBuyButtonClick()
	{
		GameSoundSystem.GetInstance().PlayRandomSound();
		IAPManager.GetInstance().Pay(IAPManager.IAPProduct.VIP);
		DoShow(false);
	}
	public void OnRestoreButtonClick()
	{
		GameSoundSystem.GetInstance().PlayRandomSound();
		IAPManager.GetInstance().Restore(IAPManager.IAPProduct.VIP);
		DoShow(false);
	}

	public override void SetContent(string titleText, string messageText)
	{
		base.SetContent (titleText, messageText);

		buyButtonLabel.text = TextManager.GetText ("buy");
		restoreButtonLabel.text = TextManager.GetText ("restore");
	}
}
