using UnityEngine;
using System.Collections;

public class InverseMode : BaseMode {
	private static InverseMode instance;
	
	public static InverseMode GetInstance()
	{
		if (instance == null)
		{
			instance = new InverseMode();
		}
		return instance;
	}
	
	protected InverseMode()
	{
	}
	
	public override bool IsRightCell(Cell cell)
	{
		return cell.Type == Cell.CellType.Empty;
	}
	
	public override bool IsRightCellNotFliped(Cell cell)
	{
		return IsRightCell(cell) && cell.State != Cell.CellState.Opened;
	}
}
