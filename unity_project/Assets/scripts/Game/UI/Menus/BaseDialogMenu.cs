using UnityEngine;
using System.Collections;

public class BaseDialogMenu : BaseMenu {

	public override void Show (bool active)
	{
		base.Show (active);
		GameSystem.GetInstance().gameUI.SetBackgroundBlur(active, this);
	}

	public override void ToggleShow ()
	{
		base.ToggleShow ();
		GameSystem.GetInstance().gameUI.SetBackgroundBlur(isActive, this);
	}
}
