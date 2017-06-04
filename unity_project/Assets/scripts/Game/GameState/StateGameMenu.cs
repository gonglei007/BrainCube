using UnityEngine;
using System.Collections;

//All states should use the Serializable attribute if you want them to be visible in the inspector
[System.Serializable]
public class StateGameMenu : FSMState<GameSystem, GameSystem.States>
{
	public static bool IS_ENTER_FROM_GAME = false;
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.GameMenu;
		}
	}

	public override void Enter ()
	{
		entity.gameUI.mainMenu.Show(true);
		if (IS_ENTER_FROM_GAME)
		{
			IS_ENTER_FROM_GAME = false;
		}
		else
		{
			entity.gameUI.gameMenu.Show(false);
		}
	}
	
	public override void Execute ()
	{
	}

	public override void Exit ()
	{
		entity.gameUI.mainMenu.Show(false);
	}
}
