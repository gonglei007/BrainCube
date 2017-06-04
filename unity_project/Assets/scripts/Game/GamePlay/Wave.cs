using UnityEngine;
using System.Collections;


public class Wave{
	
	public int	rowNumber;
	public int	columnNumber;
	public int	tipNumber;
	
	public bool	locked;
	
	public Wave(int rowNumber, int columnNumber, int tipNumber){
		this.rowNumber = rowNumber;
		this.columnNumber = columnNumber;
		this.tipNumber = tipNumber;
		locked = true;
	}
};
