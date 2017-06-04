using UnityEngine;
using System.Collections;

public class PauseMenu : BaseDialogMenu {

	// Use this for initialization
	void Awake () {
		GameSystem.GetInstance().gameUI.pauseMenu = this;	
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Show (bool active)
	{
		base.Show (active);
		Time.timeScale = active ? 0 : 1;
	}
	
	public void RestartButtonOnClick()
	{
		this.Show(false);
		GameSoundSystem.GetInstance().StopFlipRightSound();
		GameSystem.GetInstance().ChangeState(GameSystem.States.GameReady);
	}
	
	public void MainButtonOnClick(){
		this.Show(false);
		GameSoundSystem.GetInstance().StopFlipRightSound();
		StateGameMenu.IS_ENTER_FROM_GAME = true;
		GameSystem.GetInstance().ChangeState(GameSystem.States.GameInit);
		GameSystem.GetInstance().gameUI.mainMenu.ShowEnterAnimation(()=>{
			GameSystem.GetInstance().gameUI.gameMenu.Show(false);
		});
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void ContinueButtonOnClick()
	{
		this.Show(false);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}
}
