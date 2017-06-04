using UnityEngine;
using System.Collections;

[System.Serializable]
public class StateGamePreview : FSMState<GameSystem, GameSystem.States>{	
	//FSM needs to function to keep track of the different states
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.GamePreview;
		}
	}
	
	public override void Enter ()
	{
		entity.gameUI.gameMenu.Show(true);
		entity.gameCore.SetAllCellState(Cell.CellState.Closed);
		entity.gameCore.StarWaveEnterAnim();
		entity.fullScreenBlock.enabled = true;
	}

	public override void Execute()
	{
		if (entity.gameCore.IsWaveEnterAnimFinished)
		{
			entity.gameCore.IsWaveEnterAnimFinished = false;
			entity.ChangeState(GameSystem.States.GamePlay);
		}
	}

	public override void Exit ()
	{
		entity.fullScreenBlock.enabled = false;
	}
}
