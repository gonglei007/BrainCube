using UnityEngine;
using System.Collections;

[System.Serializable]
public class StateGameResult : FSMState<GameSystem, GameSystem.States>
{
	public static bool IS_EXIT_TO_MAIN = false;
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.GameResult;
		}
	}
	
	public override void Enter ()
	{
		entity.GameEnd();
		entity.fullScreenBlock.enabled = false;
		entity.gameUI.resultMenu.Show(true);
	}
	
	public override void Execute ()
	{
		
	}
	
	public override void Exit ()
	{
		if (IS_EXIT_TO_MAIN)
		{
			IS_EXIT_TO_MAIN = false;
		}
		else
		{
			entity.gameUI.resultMenu.Show(false);
		}
	}
}
