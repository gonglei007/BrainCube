using UnityEngine;
using System;

public class FixedConfirmMenu : BaseMenu {
	
	public UILabel			titleLabel;
	public UILabel			messageLabel;
	
	public UILabel			yesButtonLabel;
	public UILabel			noButtonLabel;

	public GameObject		closeButton;
	
	protected Action<bool> 	confirmCallback = null;
	protected Action 		closeCallback = null;
	protected bool			closeButtonVisible = false;

	private bool 			nextYesSoundAvailable = true;
	private bool 			nextNoSoundAvailable = true;
	private bool 			nextCloseSoundAvailable = true;

	private bool			nextCloseMenuAfterClickYesAvaiable = true;
	private bool			nextCloseMenuAfterClickNoAvaiable = true;

	//有时候在弹出窗口的回调中会再次调用弹出窗口，此时前一次回调的逻辑还未执行完成，该值用于标示此种情况并处理
	private bool			newInstanceCalled = false;

	public bool CloseButtonVisible
	{
		get
		{
			return closeButtonVisible;
		}
		set
		{
			closeButtonVisible = value;
			if (closeButton != null)
			{
				closeButton.SetActive(closeButtonVisible);
			}
		}
	}

	void Start()
	{
		this.gameObject.SetActive(false);
	}
	
	public virtual void SetContent(string titleText, string messageText)
	{
		titleLabel.text = titleText;
		messageLabel.text = messageText;
	}

	public void SetConfirmCallback(Action<bool> callback)
	{
		this.confirmCallback = callback;
	}
	
	public void SetCloseCallback(Action callback)
	{
		this.closeCallback = callback;
	}
	
	public void YesButtonOnClick()
	{
		if (nextYesSoundAvailable)
		{
			GameSoundSystem.GetInstance().PlayRandomSound();
		}
		else
		{
			nextYesSoundAvailable = true;
		}
		if (nextCloseMenuAfterClickYesAvaiable)
		{
			DoShow(false);
		}
		if (confirmCallback != null)
		{
			confirmCallback(true);
		}
		if (newInstanceCalled == false)
		{
			ClearCallback();
		}
	}
	
	public void NoButtonOnClick()
	{
		if (nextNoSoundAvailable)
		{
			GameSoundSystem.GetInstance().PlayRandomSound();
		}
		else
		{
			nextNoSoundAvailable = true;
		}
		if (nextCloseMenuAfterClickNoAvaiable)
		{
			DoShow(false);
		}
		if (confirmCallback != null)
		{
			confirmCallback(false);
		}
		if (newInstanceCalled == false)
		{
			ClearCallback();
		}
	}

	public void CloseButtonOnClick()
	{
		if (nextCloseSoundAvailable)
		{
			GameSoundSystem.GetInstance().PlayRandomSound();
		}
		else
		{
			nextCloseSoundAvailable = true;
		}
		DoShow(false);
		if (closeCallback != null)
		{
			closeCallback();
		}
		if (newInstanceCalled == false)
		{
			ClearCallback();
		}
	}
	
	public override void Show(bool active)
	{
		DoShow(active);
		if (active == false)
		{
			EnableAllButtonSound();
			ClearCallback();
		}
		else
		{
			newInstanceCalled = true;
		}
	}

	protected void DoShow(bool active)
	{
		base.Show (active);
		this.gameObject.SetActive(active);
		newInstanceCalled = false;
		GameSystem.GetInstance().gameUI.SetBackgroundBlur(active, this);
	}
	
	public override void ToggleShow ()
	{
		base.ToggleShow ();
		this.gameObject.SetActive(isActive);
		GameSystem.GetInstance().gameUI.SetBackgroundBlur(isActive, this);
		if (isActive == false)
		{
			EnableAllButtonSound();
			ClearCallback();
		}
	}
	
	public void SetYesButtonText(string text)
	{
		yesButtonLabel.text = text;
	}
	
	public void SetNoButtonText(string text)
	{
		noButtonLabel.text = text;
	}

	public void DisableNextYesSound()
	{
		nextYesSoundAvailable = false;
	}
	
	public void DisableNextNoSound()
	{
		nextNoSoundAvailable = false;
	}
	
	public void DisableNextCloseSound()
	{
		nextCloseSoundAvailable = false;
	}

	public void SetCloseMenuAfterClickYesEnable(bool enabled)
	{
		nextCloseMenuAfterClickYesAvaiable = enabled;
	}
	
	public void SetCloseMenuAfterClickNoEnbale(bool enabled)
	{
		nextCloseMenuAfterClickNoAvaiable = enabled;
	}
	
	private void EnableAllButtonSound()
	{
		nextYesSoundAvailable = true;
		nextNoSoundAvailable = true;
		nextCloseSoundAvailable = true;
	}
	
	private void ClearCallback()
	{
		confirmCallback = null;
		closeCallback = null;
	}
}
