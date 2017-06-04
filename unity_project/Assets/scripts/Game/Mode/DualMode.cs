using UnityEngine;
using System;
using System.Collections;

public class DualMode : BaseMode {
	private static DualMode instance;
	
	public Action<int>	OnLeftHPChanged;
	public Action<int>	OnRightHPChanged;
	public Action<bool>	OnTurnChanged;
	
	private int leftHp = 0;
	private int rightHp = 0;

	private int leftCount = 0;
	private int rightCount = 0;

	private bool isLeftTurn = true;
	private bool isLeftWin	= false;
	
	public override bool EnableRescue
	{
		get
		{
			return false;
		}
	}

	public override bool ShouldGainBouns
	{
		get
		{
			return false;
		}
	}
	
	public int LeftHP
	{
		get
		{
			return leftHp;
		}
		set
		{
			leftHp = Mathf.Clamp(value, 0, Constant.DUAL_HP_INIT);
			if (OnLeftHPChanged != null)
			{
				OnLeftHPChanged(leftHp);
			}
		}
	}
	
	public int RightHP
	{
		get
		{
			return rightHp;
		}
		set
		{
			rightHp = Mathf.Clamp(value, 0, Constant.DUAL_HP_INIT);
			if (OnRightHPChanged != null)
			{
				OnRightHPChanged(rightHp);
			}
		}
	}

	public bool IsLeftTurn
	{
		get
		{
			return isLeftTurn;
		}
		set
		{
			isLeftTurn = value;
			if (OnTurnChanged != null)
			{
				OnTurnChanged(isLeftTurn);
			}
		}
	}

	public bool IsLeftWin
	{
		get
		{
			return isLeftWin;
		}
	}
	
	public static DualMode GetInstance()
	{
		if (instance == null)
		{
			instance = new DualMode();
		}
		return instance;
	}
	
	protected DualMode()
	{
	}
	
	public override void Reset ()
	{		
		this.LeftHP = Constant.DUAL_HP_INIT;
		this.RightHP = Constant.DUAL_HP_INIT;
		this.IsLeftTurn = true;
	}
	
	public override bool IsRightCell(Cell cell)
	{
		if (isLeftTurn)
		{
			return cell.Type == Cell.CellType.Block && cell.CurrentColor == Constant.LEFT_COLOR;
		}
		else
		{
			return cell.Type == Cell.CellType.Block && cell.CurrentColor == Constant.RIGHT_COLOR;
		}
	}
	
	public override bool IsRightCellNotFliped(Cell cell)
	{
		return cell.Type == Cell.CellType.Block && cell.State != Cell.CellState.Opened;
	}
	
	public override void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			GameSystem.GetInstance().Score++;
		}
		else
		{
			if (isLeftTurn)
			{
				this.LeftHP--;
			}
			else
			{
				this.RightHP--;
			}
			/*
			if (this.LeftHP <= 0 || this.RightHP <= 0)
			{
				isLeftWin = this.RightHP <= 0;
			}
			*/
			isLeftWin = !isLeftTurn;
			GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
			GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
		}
	}
	
	public override void AfterFlipCell(Cell cell, bool isRight)
	{
		this.IsLeftTurn = !this.IsLeftTurn;
		if (isRight == false)
		{
			if(GameSystem.GetInstance().gameCore.IsAllBlockFinded())
			{
				HandleAllBlockFinded();
			}
		}
	}
	
	public override void InitCell(Cell cell)
	{
		if (cell.Type == Cell.CellType.Block)
		{
			if (leftCount > rightCount)
			{
				cell.CurrentColor = Constant.RIGHT_COLOR;
				rightCount++;
			}
			else
			{
				cell.CurrentColor = Constant.LEFT_COLOR;
				leftCount++;
			}
		}
	}

	public override void Init(Wave wave)
	{
		base.Init(wave);
		leftCount = 0;
		rightCount = 0;
	}

	public override void HandlePlaySound(Cell cell, bool isRight)
	{
		if (cell.CurrentColor == Constant.LEFT_COLOR || cell.CurrentColor == Constant.RIGHT_COLOR)
		{
			GameSoundSystem.GetInstance().PlayFlipRightSound();
		}
		else
		{
			GameSoundSystem.GetInstance().PlayFlipWrongSound();
		}
	}

	public override void HandleAllBlockFinded ()
	{
		base.HandleAllBlockFinded ();
		this.LeftHP++;
		this.RightHP++;
	}
}
