using System.Collections.Generic;

public class ArrayTool {

	public static bool Constains<T>(T[] array, T target)
	{
		foreach(T element in array)
		{
			if (EqualityComparer<T>.Default.Equals(element, target))
			{
				return true;
			}
		}
		return false;
	}
}
