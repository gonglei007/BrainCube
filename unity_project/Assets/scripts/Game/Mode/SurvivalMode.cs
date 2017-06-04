using UnityEngine;
using System;
using System.Collections;

public class SurvivalMode : ClassicMode {
	private static SurvivalMode instance;

	public 	Action<int>	OnHPChanged;

	private int hp = 0;
	
	public int HP
	{
		get
		{
			return hp;
		}
		set
		{
			hp = Mathf.Clamp(value, 0, Constant.HP_MAX);
			if (OnHPChanged != null)
			{
				OnHPChanged(hp);
			}
		}
	}
	
	public static new SurvivalMode GetInstance()
	{
		if (instance == null)
		{
			instance = new SurvivalMode();
		}
		return instance;
	}
	
	protected SurvivalMode()
	{
	}

	public override void Reset ()
	{
		this.HP = Constant.HP_INITIAL;
	}
	
	public override void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			GameSystem.GetInstance().Score++;
			this.HP += Constant.HP_BONUS;
		}
		else
		{
			this.HP += Constant.HP_PUNISH;
			if (this.HP <= 0)
			{
				GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
				GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
			}
		}
	}

	public override void Rescue ()
	{
		base.Rescue();
		this.Reset();
	}
}
