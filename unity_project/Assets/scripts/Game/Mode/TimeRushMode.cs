using UnityEngine;
using System;
using System.Collections;

public class TimeRushMode : ClassicMode {
	private static TimeRushMode instance;

	public Action<float> OnLifeTimeChanged;

	private float lifeTime = 0;
	
	public float LifeTime
	{
		get
		{
			return lifeTime;
		}
		set
		{
			lifeTime = Mathf.Clamp(value, 0, Constant.LIFE_TIME_MAX);
			if (OnLifeTimeChanged != null)
			{
				OnLifeTimeChanged(lifeTime);
			}
		}
	}
	
	public static new TimeRushMode GetInstance()
	{
		if (instance == null)
		{
			instance = new TimeRushMode();
		}
		return instance;
	}
	
	protected TimeRushMode()
	{
	}
	
	public override void Reset ()
	{		
		this.LifeTime = Constant.LIFE_TIME_INITIAL;
	}

	public override void Update ()
	{
		if (GameSystem.GetInstance().CurrentState == GameSystem.States.GamePlay)
		{
			this.LifeTime -= Time.deltaTime;
			if (this.LifeTime <= 0)
			{
				GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
				GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
			}
		}
	}
	
	public override void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			GameSystem.GetInstance().Score++;
			this.LifeTime += Constant.LIFE_TIME_BONUS;
		}
		else
		{
			this.LifeTime += Constant.LIFE_TIME_PUNISH;
			if (this.LifeTime <= 0)
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
