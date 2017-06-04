using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BlackJackMode : ClassicMode {
	public enum Result
	{
		NotFinish,
		Win,
		Lose
	}

	private static BlackJackMode instance;

	public 	Action	OnSelectCardChanged;

	private List<int> 		sumFactor = new List<int>(10);
	private List<int>		selectedCardValues = new List<int>(10);
	private List<string>	selectedCardNames = new List<string>(10);
	private Result 			result;

	public List<string> SelectedCardNames
	{
		get
		{
			return selectedCardNames;
		}
	}
	
	public static new BlackJackMode GetInstance()
	{
		if (instance == null)
		{
			instance = new BlackJackMode();
		}
		return instance;
	}
	
	protected BlackJackMode()
	{
	}

	public override void Reset ()
	{
		sumFactor.Clear();
		selectedCardValues.Clear();
		selectedCardNames.Clear();
	}
	
	public override bool IsRightCell(Cell cell)
	{
		return cell.Type == Cell.CellType.Block && string.IsNullOrEmpty(cell.TextureName) == false;
	}
	
	public override void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			selectedCardNames.Add(cell.TextureName);
			if (OnSelectCardChanged != null)
			{
				OnSelectCardChanged();
			}

			string cardNumberString = cell.TextureName.Substring(cell.TextureName.Length - 2, 2);
			int cardNumber = Convert.ToInt32(cardNumberString);
			int cardValue = PokerNumber2Value(cardNumber);
			selectedCardValues.Add(cardValue);

			result = GetSelectedCardResult();
			if (result == Result.Win)
			{
				GameSystem.GetInstance().Score++;
				GameSystem.GetInstance().gameCore.IsLevelWavePassed = true;
				GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
			}
			else if(result == Result.Lose)
			{
				GameSoundSystem.GetInstance().StopFlipRightSound();
				GameSoundSystem.GetInstance().PlayFlipWrongSound();
				GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
				GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
			}
			else
			{
				GameSystem.GetInstance().Score++;
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
			int factorIndex = UnityEngine.Random.Range(0, sumFactor.Count);
			int factor = sumFactor[factorIndex];
			factor = Value2PokerNumber(factor);
			cell.TextureName = PokerNumber2PokerName(factor);
			sumFactor.RemoveAt(factorIndex);
		}
	}
	
	public override void Init(Wave wave)
	{
		int totalTipCount = wave.tipNumber;
		if (totalTipCount <= 6)
		{
			fullBlockDisplayTime = totalTipCount * 1.6f / 6 + 0.4f;
		}
		else
		{
			fullBlockDisplayTime = 2;
		}
		sumFactor.Clear();
		selectedCardValues.Clear();
		selectedCardNames.Clear();

		int realFactorCount = 2;
		if (GameSystem.GetInstance().DisplayWaveNumber == 1)
		{
			realFactorCount = 2;
		}
		else if (GameSystem.GetInstance().DisplayWaveNumber <= 10)
		{
			realFactorCount = 3;
		}
		else if (GameSystem.GetInstance().DisplayWaveNumber <= 20)
		{
			realFactorCount = 4;
		}
		else
		{
			realFactorCount = 5;
		}
		int protectMax = realFactorCount - 3;

		int sumTarget = Constant.SUM_TARGET;
		while(realFactorCount > 0)
		{
			int factor = 0;
			if (realFactorCount == 1 || sumTarget == 1)
			{
				factor = sumTarget;
			}
			else if (realFactorCount == 2)
			{
				//剩余两个数字时，数字的取值不能过小使得最后一个数字超过11,也不能过大使最后一个数字为0
				if (sumTarget > 11)
				{
					factor = UnityEngine.Random.Range(sumTarget - 11, 12);
				}
				else
				{
					factor = UnityEngine.Random.Range(1, sumTarget);
				}
				sumTarget -= factor;
			}
			else
			{
				//取前几个组成数时，不能过大，使得后面的数字无值可取
				int max = sumTarget - protectMax < 12 ? sumTarget - protectMax : 12;
				factor = UnityEngine.Random.Range(1, max);
				sumTarget -= factor;
			}
			sumFactor.Add(factor);
			realFactorCount--;
		}


		string sumFactorString = "";
		foreach(int factor in sumFactor)
		{
			sumFactorString += factor + ",";
		}
		Debug.Log("sumFactors ************** " + sumFactorString);


		while(sumFactor.Count < wave.tipNumber)
		{
			int fakeFactor = UnityEngine.Random.Range(1, 12);
			sumFactor.Add(fakeFactor);
		}
	}
	
	public override void HandleAllBlockFinded()
	{
		if (result == Result.NotFinish)
		{
			GameSoundSystem.GetInstance().StopFlipRightSound();
			GameSoundSystem.GetInstance().PlayFlipWrongSound();
			GameSystem.GetInstance().gameCore.IsLevelWavePassed = false;
			GameSystem.GetInstance().ChangeState(GameSystem.States.WaveComplete);
		}
	}

	private int Value2PokerNumber(int value)
	{
		if (value == 1 || value == 11)
		{
			value = 1;
		}
		else if (value == 10)
		{
			value = UnityEngine.Random.Range(11,14);		
		}
		return value;
	}

	private string PokerNumber2PokerName(int number)
	{
		int mark = UnityEngine.Random.Range(0,4);
		string name = null;
		switch(mark)
		{
		case 0:
			name = string.Format("img_poker_heart_{0:D2}", number);
			break;
		case 1:
			name = string.Format("img_poker_diamond_{0:D2}", number);
			break;
		case 2:
			name = string.Format("img_poker_spade_{0:D2}", number);
			break;
		case 3:
			name = string.Format("img_poker_club_{0:D2}", number);
			break;
		}
		return name;
	}

	private int PokerNumber2Value(int pokerNumber)
	{
		if (pokerNumber == 11 || pokerNumber == 12 || pokerNumber == 13)
		{
			pokerNumber = 10;
		}
		return pokerNumber;
	}

	private Result GetSelectedCardResult()
	{
		int cardACount = 0;
		int noASum = 0;
		foreach(int cardNumber in selectedCardValues)
		{
			int value = PokerNumber2Value(cardNumber);
			if (value == 1)
			{
				cardACount++;
			}
			else
			{
				noASum += value;
			}
		}
		if (cardACount == 0)
		{
			if (noASum == 21)
			{
				return Result.Win;
			}
			else if (noASum < 21)
			{
				return Result.NotFinish;
			}
			else 
			{
				return Result.Lose;
			}
		}
		else
		{
			//因为A的存在，结果有2的[A数量]次方种
			int resultKind = (int)Mathf.Pow(2, cardACount);
			List<int> aList = new List<int>(cardACount);
			for(int i = 0 ; i < cardACount ; i++)
			{
				aList.Add(1);
			}
			List<int> resultList = new List<int>(resultKind);
			GetAllASum(aList, aList.Count, resultList);
			for(int j = 0 ; j < resultList.Count ; j++)
			{
				resultList[j] += noASum;
			}

			bool allExceed21 = true;
			foreach(int result in resultList)
			{
				if (result == 21)
				{
					return Result.Win;
				}
				else if (result < 21)
				{
					allExceed21 = false;
				}
			}
			if (allExceed21)
			{
				return Result.Lose;
			}
			else
			{
				return Result.NotFinish;
			}
		}
	}

	private void GetAllASum(List<int> aList, int index, List<int> resultList)
	{
		if (index < 1)
		{
			return;
		}

		aList[index-1] = 1;
		if (index == 1)
		{
			int sum = 0;
			foreach(int value in aList)
			{
				sum += value;
			}
			resultList.Add(sum);
		}
		GetAllASum(aList, index-1, resultList);

		aList[index-1] = 11;
		if (index == 1)
		{
			int sum = 0;
			foreach(int value in aList)
			{
				sum += value;
			}
			resultList.Add(sum);
		}
		GetAllASum(aList, index-1, resultList);
	}
	
	public override void Rescue ()
	{
		base.Rescue();
		this.Reset();
	}
}
