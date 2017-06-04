using UnityEngine;
using System.Collections;

public class SAOpenUrlOnClick : SAOnClickAction {

	public string url;

	protected override void OnClick() {
		Application.OpenURL(url);
	}
}
