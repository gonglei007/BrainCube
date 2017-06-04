using UnityEngine;
using System;
using System.Collections;

public enum ConfirmStyle
{
	OnlyYes,
	YesNo,
	YesClose,
	YesNoClose
}

public class AutoConfirmMenu : FixedConfirmMenu 
{
	public UISprite		titleBg;
	public UISprite		menuBg;

	public GameObject 	yesButton;
	public GameObject 	noButton;

	protected int 		titleBgHeightParam 		= 58;
	protected int		menuBgHeightParam 		= 98;
	protected int		messagePositionParam 	= 39;
	protected int		buttonHeight			= 80;
	protected int		buttonPositionParam 	= 75;
	protected int		buttonGapParam 			= 20;

	void Awake()
	{
		GameSystem.GetInstance().gameUI.confirmMenu = this;
	}

	public virtual void SetStyle(int contentFontSize, NGUIText.Alignment alignment)
	{
		messageLabel.fontSize = contentFontSize;
		messageLabel.alignment = alignment;
	}

	public virtual void SetContent(string titleText, string messageText, ConfirmStyle confirmStyle)
	{
		titleLabel.text = titleText;
		messageLabel.text = messageText;

		yesButtonLabel.text = TextManager.GetText("yes");
		noButtonLabel.text = TextManager.GetText("no");

		int buttonNumber = 1;

		if (confirmStyle == ConfirmStyle.OnlyYes)
		{
			yesButtonLabel.text = TextManager.GetText("ok");
			noButton.SetActive(false);
			this.CloseButtonVisible = false;
		}
		else if (confirmStyle == ConfirmStyle.YesNo)
		{
			buttonNumber = 2;

			noButton.SetActive(true);
			this.CloseButtonVisible = false;
		}
		else if (confirmStyle == ConfirmStyle.YesClose)
		{
			noButton.SetActive(false);
			this.CloseButtonVisible = true;
		}
		else if (confirmStyle == ConfirmStyle.YesNoClose)
		{
			buttonNumber = 2;
			noButton.SetActive(true);
			this.CloseButtonVisible = true;
		}
		
		titleBg.height = titleLabel.height + titleBgHeightParam;
		int contentHeight = CustomContentHeight();
		if (contentHeight == 0)
		{
			contentHeight = messageLabel.height;
		}
		menuBg.height = titleBg.height + contentHeight + buttonHeight * buttonNumber + (buttonNumber - 1) * buttonGapParam + menuBgHeightParam;
		
		titleBg.transform.localPosition = new Vector3(0, menuBg.height / 2, 0);
		titleLabel.transform.localPosition = new Vector3(0, titleBg.transform.localPosition.y - titleBg.height / 2, 0);

		Vector3 messageLabelPos = messageLabel.transform.localPosition;
		messageLabelPos.y = menuBg.height / 2 - titleBg.height - messagePositionParam;
		messageLabel.transform.localPosition = messageLabelPos;

		CustomContent();

		float buttonPositionY = buttonPositionParam - menuBg.height / 2;
		
		Vector3 yesButtonPos = Vector3.zero;
		yesButtonPos.x = 0;
		yesButtonPos.y = buttonPositionY;
		yesButton.transform.localPosition = yesButtonPos;
		if (buttonNumber == 2)
		{
			Vector3 noButtonPos = yesButtonPos;
			noButtonPos.y += buttonGapParam + buttonHeight;
			noButton.transform.localPosition = noButtonPos;
		}
		
		Vector3 closeButtonPos = closeButton.transform.localPosition;
		closeButtonPos.y = menuBg.height / 2;
		closeButton.transform.localPosition = closeButtonPos;
	}

	protected virtual int CustomContentHeight()
	{
		return 0;
	}

	protected virtual void CustomContent()
	{

	}
}
