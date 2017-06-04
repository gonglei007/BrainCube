using UnityEngine;
using System;
using System.Collections;

public class RescueConfirmMenu : FixedConfirmMenu {
	public UILabel	timeLabel;

	private float 	autoCloseTime;

	void Awake()
	{
		GameSystem.GetInstance().gameUI.rescueConfirmMenu = this;
	}

	public float AutoCloseTime
	{
		get
		{
			return autoCloseTime;
		}
		set
		{
			autoCloseTime = value;
			timeLabel.text = string.Format("({0:F1}s)", autoCloseTime);
		}
	}

	void Update()
	{
		if (this.AutoCloseTime > 0)
		{
			this.AutoCloseTime -= Time.deltaTime;
			if (this.AutoCloseTime <= 0)
			{
				NoButtonOnClick();
			}
		}
	}
	
	public override void Show (bool active)
	{
		if (active)
		{
			GameSystem.GetInstance().gameUI.confirmMenu.Show(false);
		}
		base.Show (active);
	}
}