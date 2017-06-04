using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class WordMode : ClassicMode {
	private static WordMode instance;
	private static int			MAX_WORD_LENGTH = 12;
	
	public 	Action	OnWordTextChanged;

	private List<char> 		wordChars = new List<char>(24);
	private List<char>		tempChars = new List<char>(24);
	private WordData 		wordData;
	private string			wordText;

	private int 			correctCharCount;
	private StringBuilder	stringBuilder = new StringBuilder(40);
	private int				wordLength;
	private int				wordArrayIndex;

	public string WordText
	{
		get
		{
			return wordText;
		}
	}

	public string WordTranslation
	{
		get
		{
			return wordData.translation;
		}
	}
	
	public static new WordMode GetInstance()
	{
		if (instance == null)
		{
			instance = new WordMode();
		}
		return instance;
	}
	
	protected WordMode()
	{
	}
	
	public override void Reset ()
	{
		correctCharCount = 0;
		RefreshWordText();
	}
	
	public override bool IsRightCell(Cell cell)
	{
		return cell.Type == Cell.CellType.Block && string.IsNullOrEmpty(cell.Text) == false;
	}

	public override void FlipCell(Cell cell, bool isRight)
	{
		if (isRight)
		{
			char letter = Convert.ToChar(cell.Text);
			if (wordChars[correctCharCount].Equals(letter))
			{
				GameSystem.GetInstance().Score++;
				correctCharCount++;
				RefreshWordText();
				
				if (OnWordTextChanged != null)
				{
					OnWordTextChanged();
				}

				if (correctCharCount == wordChars.Count)
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
			int charIndex = UnityEngine.Random.Range(0, tempChars.Count);
			cell.Text = tempChars[charIndex].ToString();
			tempChars.RemoveAt(charIndex);
		}
	}
	
	public override void Init(Wave wave)
	{
		int totalTipCount = wave.tipNumber;
		if (totalTipCount <= 12)
		{
			fullBlockDisplayTime = totalTipCount * 0.6f / 12 + 0.4f;
		}
		else
		{
			fullBlockDisplayTime = 1;
		}
		wordLength = Mathf.Clamp(wave.tipNumber, 0, MAX_WORD_LENGTH);
		wordData = WordData.GetRandomWord(wordLength);
		wordChars.Clear();
		tempChars.Clear();
		wordChars.AddRange(wordData.word.ToCharArray());
		tempChars.AddRange(wordChars);

		correctCharCount = 0;
		RefreshWordText();

		if (OnWordTextChanged != null)
		{
			OnWordTextChanged();
		}
	}
	
	public override void HandleAllBlockFinded()
	{
		
	}

	private void RefreshWordText()
	{
		stringBuilder.Remove(0, stringBuilder.Length);
		stringBuilder.Append("[00FF00]");
		int index = 0;
		while(index < correctCharCount)
		{
			stringBuilder.Append(wordChars[index]);
			index++;
		}
		stringBuilder.Append("[-][333333]");
		while(index < wordChars.Count)
		{
			stringBuilder.Append(wordChars[index]);
			index++;
		}
		stringBuilder.Append("[-]");
		wordText = stringBuilder.ToString();
	}
	
	public override void Rescue ()
	{
		base.Rescue();
		this.Reset();
		if (OnWordTextChanged != null)
		{
			OnWordTextChanged();
		}
	}
}
