using UnityEngine;
using System.Collections;

public class SALoadedSceneOnClick : SAOnClickAction {

	public string levelName;

	protected override void OnClick() {
		SALevelLoader.instance.LoadLevel(levelName);
	}
}
