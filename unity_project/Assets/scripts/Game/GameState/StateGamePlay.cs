using UnityEngine;
using System.Collections;

//All states should use the Serializable attribute if you want them to be visible in the inspector
[System.Serializable]
public class StateGamePlay : FSMState<GameSystem, GameSystem.States>
{
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.GamePlay;
		}
	}

	public override void Enter ()
	{
		entity.AllowMultiTouch = true;
	}

	public override void Execute ()
	{
	}

	public override void Exit ()
	{
		entity.AllowMultiTouch = true;
	}

	void HandleInstaWin (object sender, System.EventArgs e)
	{
		entity.ChangeState(GameSystem.States.WaveComplete);
	}
	
}
