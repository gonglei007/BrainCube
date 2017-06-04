using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChimpMode : ClassicMode {
	private static ChimpMode instance;
	
	private int 			nextCorrectNumber;
	private int				currentMaxNumber;
	private List<int>		availableNumbers = new List<int>(24);
	
	public static new ChimpMode GetInstance()
	{
		if (instance == null)
		{
			instance = new ChimpMode();
		}
		return instance;
	}
	
	protected ChimpMode()
	{
	}
	
	public override void Reset ()
	{
		nextCorrectNumber = 1;
	}
	
	public override bool IsRightCell(Cell cell)
	{
		return cell.Type == Cell.CellType.Block && string.IsNullOrEmpty(cell.Text) == false;
	}
	
	public override void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			int number = Convert.ToInt32(cell.Text);
			if (number == nextCorrectNumber)
			{
				GameSystem.GetInstance().Score++;
				nextCorrectNumber++;
				
				if (number == currentMaxNumber)
				{
					GameSystem.GetInstance().gameCore.IsLevelWavePassed = true;
					GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
				}
			}
			else
			{
				GameSoundSystem.GetInstance().StopFlipRightSound();
				GameSoundSystem.GetInstance().PlayFlipWrongSound();
				GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
				GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
			}
		}
		else
		{
			cell.Type = Cell.CellType.Block;
			cell.Text = string.Empty;
			cell.TextureName = string.Empty;
		}
	}
	
	public override void AfterFlipCell(Cell cell, bool isRight)
	{
		if (isRight == false)
		{
			cell.Type = Cell.CellType.Empty;
		}
	}
	
	public override void InitCell(Cell cell)
	{
		if (cell.Type == Cell.CellType.Block)
		{
			int numberIndex = UnityEngine.Random.Range(0, availableNumbers.Count);
			cell.Text = availableNumbers[numberIndex].ToString();
			availableNumbers.RemoveAt(numberIndex);
		}
	}
	
	public override void Init(Wave wave)
	{
		currentMaxNumber = wave.tipNumber;
		availableNumbers.Clear();
		while(currentMaxNumber > 0)
		{
			availableNumbers.Add(currentMaxNumber);
			currentMaxNumber--;
		}
		currentMaxNumber = wave.tipNumber;

		if (currentMaxNumber <= 12)
		{
			fullBlockDisplayTime = currentMaxNumber * 0.6f / 12 + 0.4f;
		}
		else
		{
			fullBlockDisplayTime = 1;
		}
		
		nextCorrectNumber = 1;
	}
	
	public override void HandleAllBlockFinded()
	{
		
	}
	
	public override void Rescue ()
	{
		base.Rescue();
		this.Reset();
	}
}
