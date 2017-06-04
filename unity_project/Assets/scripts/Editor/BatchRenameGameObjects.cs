using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BatchRenameGameObjects : ScriptableWizard
{
	public bool		isReplaceNameOrRename;
	public string	originalString = string.Empty;
	public string	replaceString = string.Empty;

	public string	renameString;
	public int	  	startNumber;
	public int		numberDigits;

    public List<GameObject> targetObjects = new List<GameObject>(); 

    [MenuItem("Custom/Batch Rename GameObjects")]
    static void CreateWizard()
    {
		BatchRenameGameObjects renameGameObjects = ScriptableWizard.DisplayWizard<BatchRenameGameObjects>("Batch Rename GameObjects", "Rename");
		renameGameObjects.targetObjects.AddRange(Selection.gameObjects);
		renameGameObjects.targetObjects.Sort(SortByName);
    } 

    void OnWizardCreate()
    {
		int index = startNumber;
		foreach (GameObject go in targetObjects)
        {
			if (isReplaceNameOrRename)
			{
				go.name = go.name.Replace(originalString, replaceString);
			}
			else
			{
				string formatString = "{0}{1:D" + numberDigits.ToString() + "}";
				go.name = string.Format(formatString, renameString, index);
				index++;
			}
		}
	}
	
	static public int SortByName(GameObject a, GameObject b)
	{
		return string.Compare(a.name, b.name);
	}
}