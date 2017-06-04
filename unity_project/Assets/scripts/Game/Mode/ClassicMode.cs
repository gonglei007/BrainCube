using UnityEngine;
using System.Collections;

public class ClassicMode : BaseMode {
	private static ClassicMode instance;
	
	public static ClassicMode GetInstance()
	{
		if (instance == null)
		{
			instance = new ClassicMode();
		}
		return instance;
	}
	
	protected ClassicMode()
	{
	}
	
	public override bool IsRightCell(Cell cell)
	{
		return cell.Type == Cell.CellType.Block;
	}
	
	public override bool IsRightCellNotFliped(Cell cell)
	{
		return IsRightCell(cell) && cell.State != Cell.CellState.Opened;
	}
}
