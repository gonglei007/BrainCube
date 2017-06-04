using UnityEngine;
using System.Collections;

public class GiftMenu : BaseDialogMenu {
	public GameObject	vipButton;

	public GiftItem[] 	giftItems;

	public GameObject	receiveButtonAvailableContent;
	public GameObject	receiveButtonUnAvailableContent;
	public UILabel		giftTimeLabel;

	public GameObject	giftItemSelector;

	private int			coin;

	private float		timer;
	private int			jumpTimes;
	private int			currentTimes;
	private float		jumpInterval = 0.05f;
	private int			startIndex = 0;
	private bool 		startAnim = false;
	private bool		isSelectorScaleing = false;

	void Awake()
	{
		GameSystem.GetInstance().gameUI.giftMenu = this;
		this.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
		giftItemSelector.SetActive(false);
		RefreshVIPButtonState();
		RefreshGiftItem();
	}
	
	void Update(){
		receiveButtonAvailableContent.SetActive(GameSystem.GetInstance().IsGiftAvailableNow);
		receiveButtonUnAvailableContent.SetActive(!GameSystem.GetInstance().IsGiftAvailableNow);

		if (GameSystem.GetInstance().IsGiftAvailableNow == false
		    && Time.frameCount % 15 == 0)
		{
			UpdateNextGifTime();
		}

		if (startAnim)
		{
			timer += Time.deltaTime;
			if (timer > jumpInterval)
			{
				timer = 0;
				
				if (currentTimes >= jumpTimes)
				{
					startAnim = false;
					SelectFinish();
				}
				else
				{
					currentTimes++;
					UpdateJumpInterval();
					int finalIndex = (startIndex + currentTimes) % giftItems.Length;
					giftItemSelector.transform.localPosition = giftItems[finalIndex].transform.localPosition;
					GameSoundSystem.GetInstance().PlayRandomSound();
				}
			}
		}
	}

	public void ReceiveButtonOnClick()
	{
		if (GameSystem.GetInstance().IsGiftAvailableNow)
		{
			coin = GameSystem.GetInstance().ReceiveGift();
			if (GameSystem.GetInstance().IsVIP)
			{
				coin /= 2;
			}
			int endIndex = -1;
			int index = 0;
			foreach(GiftItem giftItem in giftItems)
			{
				if (giftItem.Number == coin && RandomTool.Int(2) == 1)
				{
					endIndex = index;
					break;
				}
				index++;
			}

			if (endIndex == -1)
			{
				index = 0;
				foreach(GiftItem giftItem in giftItems)
				{
					if (giftItem.Number == coin)
					{
						endIndex = index;
						break;
					}
					index++;
				}
			}
			timer = 0;
			currentTimes = 0;
			jumpTimes = 90;
			startIndex = endIndex - jumpTimes % giftItems.Length;
			UpdateJumpInterval();
			if (startIndex < 0)
			{
				startIndex += giftItems.Length;
			}
			
			giftItemSelector.SetActive(true);
			startAnim = true;
		}
	}
	
	public void VIPButtonOnClick()
	{
		if (GameSystem.GetInstance().CheckIsActivated())
		{
			GameSoundSystem.GetInstance().PlayRandomSound();
			string title = TextManager.GetText("confirm_buy_vip_gift_title");
			string content = string.Format(TextManager.GetText("vip_gift_desc"), ShopData.GetShopData(IAPManager.IAPProduct.VIP).number);
			content = content.Replace('\n', ' ');
			//GameSystem.GetInstance().gameUI.vipConfirmMenu.SetConfirmCallback(ConfirmBuyVIPCallback);
			GameSystem.GetInstance().gameUI.vipConfirmMenu.SetContent(title, content);
			GameSystem.GetInstance().gameUI.vipConfirmMenu.Show(true);
		}
	}
	
	public void CloseButtonOnClick()
	{
		if (startAnim || isSelectorScaleing)
		{
			return;
		}
		this.Close();
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	private void UpdateNextGifTime()
	{
		int seconds = GameSystem.GetInstance().NextGiftTimeLeft;
		giftTimeLabel.text = string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds % 3600) / 60, seconds % 60);
		
		if (seconds <= 0)
		{
			giftTimeLabel.gameObject.SetActive(false);
		}
		else
		{
			giftTimeLabel.gameObject.SetActive(true);
		}
	}

	public void RefreshGiftItem()
	{
		int index = 0;
		int[] numberArray = new int[Constant.GIFT_NUMBER_PROBABILITY.Count];
		Constant.GIFT_NUMBER_PROBABILITY.Keys.CopyTo(numberArray, 0);
		foreach(GiftItem giftItem in giftItems)
		{
			int number = numberArray[Constant.GIFT_NUMBERS_ORDER[index]];
			giftItem.Number = number;
			giftItem.background.color = (index % 2 == 0) ? Constant.COLOR_DARK_BLACK: Color.white;
			giftItem.numberLabel.color = (index % 2 == 1) ? Constant.COLOR_DARK_BLACK: Color.white;
			index++;
		}
	}
	
	public void RefreshVIPButtonState()
	{
		vipButton.SetActive(!GameSystem.GetInstance().IsVIP);
	}

	private void UpdateJumpInterval()
	{
		if (currentTimes < 40)
		{
			jumpInterval = 0.02f;
		}
		else if (currentTimes < 60)
		{
			jumpInterval = 0.04f;
		}
		else if (currentTimes < 70)
		{
			jumpInterval = 0.06f;
		}
		else if (currentTimes <= 85)
		{
			jumpInterval = (currentTimes - 70.0f) / (jumpTimes - 70.0f) * (0.3f - 0.06f) + 0.06f;
		}
		else if (currentTimes < jumpTimes - 1)
		{
			jumpInterval = (currentTimes - 85.0f) / (jumpTimes - 85.0f) * (1f - 0.3f) + 0.3f;
		}
		else
		{
			jumpInterval = 1.5f;
		}
	}

	private void SelectFinish()
	{
		isSelectorScaleing = true;
		TweenScale tweenScale = TweenScale.Begin(giftItemSelector, 0.3f, Vector3.one * 1.3f);
		tweenScale.method = UITweener.Method.EaseOut;
		EventDelegate.Add(tweenScale.onFinished, SelectorScaleOutFinish, true);
	}

	private void SelectorScaleOutFinish()
	{
		TweenScale tweenScale = TweenScale.Begin(giftItemSelector, 0.2f, Vector3.one);
		tweenScale.method = UITweener.Method.EaseIn;
		EventDelegate.Add(tweenScale.onFinished, SelectorScaleInFinish, true);
	}
	
	private void SelectorScaleInFinish()
	{
		StartCoroutine(FinishReceiveGift());
	}

	IEnumerator FinishReceiveGift()
	{
		yield return new WaitForSeconds(0.2f);
		isSelectorScaleing = false;
		string title = TextManager.GetText("receive_gift");
		string content = null;
		if (GameSystem.GetInstance().IsVIP)
		{
			content = string.Format(TextManager.GetText("get_coin"), coin) + " x 2";
		}
		else
		{
			content = string.Format(TextManager.GetText("get_coin"), coin);
		}
		GameSystem.GetInstance().gameUI.blockConfirmMenu.SetContent(title, content, ConfirmStyle.OnlyYes);
		GameSystem.GetInstance().gameUI.blockConfirmMenu.Show(true);
	}
	
	private void ConfirmBuyVIPCallback(bool result)
	{
		if (result)
		{
			IAPManager.GetInstance().Pay(IAPManager.IAPProduct.VIP);
		}
	}
}
