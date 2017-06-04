using UnityEngine;
using System.Collections;

public abstract class BaseMode {

	protected float fullBlockDisplayTime = 0.4f;

	public virtual float FullBlockDisplayTime
	{
		get
		{
			return fullBlockDisplayTime;
		}
	}

	public virtual bool EnableRescue
	{
		get
		{
			return true;
		}
	}

	public virtual bool ShouldGainBouns
	{
		get
		{
			return true;
		}
	}

	public virtual void Reset()
	{
	}

	public virtual void Update()
	{
	}
	
	public virtual bool IsRightCell(Cell cell)
	{
		return false;
	}

	public virtual bool IsRightCellNotFliped(Cell cell)
	{
		return false;
	}

	public virtual void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			GameSystem.GetInstance().Score++;
		}
		else
		{
			GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
			GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
		}
	}
	
	public virtual void AfterFlipCell(Cell cell, bool isRight)
	{
	}

	public virtual void InitCell(Cell cell)
	{
	}

	public virtual void Init(Wave wave)
	{
		int totalTipCount = wave.tipNumber * wave.rowNumber;
		if (totalTipCount <= 12)
		{
			fullBlockDisplayTime = totalTipCount * 0.6f / 12 + 0.4f;
		}
		else
		{
			fullBlockDisplayTime = 1;
		}
	}

	public virtual void HandleAllBlockFinded()
	{
		GameSystem.GetInstance().gameCore.IsLevelWavePassed = true;
		GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
	}

	public virtual void Rescue()
	{
		GameSoundSystem.GetInstance().ChooseMusic();
	}

	public virtual void HandlePlaySound(Cell cell, bool isRight)
	{
		if (isRight)
		{
			GameSoundSystem.GetInstance().PlayFlipRightSound();
		}
		else
		{
			GameSoundSystem.GetInstance().PlayFlipWrongSound();
		}
	}
}
