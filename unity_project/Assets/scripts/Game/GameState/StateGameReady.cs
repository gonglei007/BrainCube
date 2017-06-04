using UnityEngine;
using System.Collections;

//All states should use the Serializable attribute if you want them to be visible in the inspector
[System.Serializable]
public class StateGameReady : FSMState<GameSystem, GameSystem.States>
{
	//FSM needs to function to keep track of the different states
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.GameReady;
		}
	}
	
	public override void Enter ()
	{
		entity.Reset();
		entity.gameUI.countDownMenu.Show(true);
		entity.gameUI.countDownMenu.StarCountDownAnim();
		GameSoundSystem.GetInstance ().ChooseMusic ();
	}

	public override void Execute ()
	{
		if(entity.gameUI.countDownMenu.IsCountDownAnimFinished){
			entity.gameUI.countDownMenu.IsCountDownAnimFinished = false;
			entity.InitGameCore();
			entity.gameUI.resultMenu.Show(false);
			entity.ChangeState(GameSystem.States.GamePreview);
		}
	}

	public override void Exit ()
	{
		entity.gameUI.countDownMenu.Show(false);
	}

}