using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorFullMode : BaseMode {
	private static ColorFullMode instance;

	public 	Action	OnColorChanged;

	private List<Color> rightColors = new List<Color>(5);
	private List<Color> wrongColors = new List<Color>(5);

	private int rightBlockCount;
	private int wrongBlockCount;

	public List<Color> RightColors
	{
		get
		{
			return rightColors;
		}
	}
	
	public List<Color> WrongColors
	{
		get
		{
			return wrongColors;
		}
	}
	
	public static ColorFullMode GetInstance()
	{
		if (instance == null)
		{
			instance = new ColorFullMode();
		}
		return instance;
	}
	
	protected ColorFullMode()
	{
	}
	
	public override bool IsRightCell(Cell cell)
	{
		return cell.Type == Cell.CellType.Block && rightColors.Contains(cell.CurrentColor);
	}
	
	public override bool IsRightCellNotFliped(Cell cell)
	{
		return IsRightCell(cell) && cell.State != Cell.CellState.Opened;
	}
	
	public override void InitCell(Cell cell)
	{
		if (cell.Type == Cell.CellType.Block)
		{
			if (UnityEngine.Random.Range(0,2) == 0)
			{
				if (rightBlockCount > 0)
				{
					rightBlockCount--;
					int rightColorIndex = UnityEngine.Random.Range(0, rightColors.Count);
					cell.CurrentColor = rightColors[rightColorIndex];
				}
				else if (wrongBlockCount > 0)
				{
					wrongBlockCount--;
					int wrongColorIndex = UnityEngine.Random.Range(0, wrongColors.Count);
					cell.CurrentColor = wrongColors[wrongColorIndex];
				}
			}
			else
			{
				if (wrongBlockCount > 0)
				{
					wrongBlockCount--;
					int wrongColorIndex = UnityEngine.Random.Range(0, wrongColors.Count);
					cell.CurrentColor = wrongColors[wrongColorIndex];
				}
				else if (rightBlockCount > 0)
				{
					rightBlockCount--;
					int rightColorIndex = UnityEngine.Random.Range(0, rightColors.Count);
					cell.CurrentColor = rightColors[rightColorIndex];
				}
			}
		}
	}
	
	public override void Init(Wave wave)
	{
		base.Init(wave);
		rightColors.Clear();
		wrongColors.Clear();

		int totalTipCount = wave.tipNumber;
		wrongBlockCount = Mathf.Clamp(totalTipCount / 3, 1, 99);
		rightBlockCount = totalTipCount - wrongBlockCount;

		int wrongColorKind = Mathf.Clamp(Mathf.RoundToInt(wrongBlockCount / 5f), 1, 5);
		int rightColorKind = Mathf.Clamp(Mathf.RoundToInt(rightBlockCount / 5f), 1, 5);
		int totalKind = wrongColorKind + rightColorKind;

		int h = UnityEngine.Random.Range(0, 360);

		int avgDegree = 360 / totalKind;
		for(int i = 0 ; i < totalKind ; i++)
		{
			int newH = (h + i * avgDegree) % 360;
			Color color = ColorUtility.HSB2RGB(newH, 0.4f, 1.0f);
			if(rightColors.Count < rightColorKind)
			{
				rightColors.Add(color);
			}
			else
			{
				wrongColors.Add(color);
			}
		}
		
		if (OnColorChanged != null)
		{
			OnColorChanged();
		}
	}
}
