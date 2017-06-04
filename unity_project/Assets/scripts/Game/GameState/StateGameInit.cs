using UnityEngine;
using System.Collections;

//All states should use the Serializable attribute if you want them to be visible in the inspector
[System.Serializable]
public class StateGameInit : FSMState<GameSystem, GameSystem.States>
{
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.GameInit;
		}
	}

	public override void Enter ()
	{
		Input.multiTouchEnabled = true;
	}
	
	public override void Execute ()
	{
		entity.ChangeState (GameSystem.States.GameMenu);
	}

	public override void Exit ()
	{
	}
}
