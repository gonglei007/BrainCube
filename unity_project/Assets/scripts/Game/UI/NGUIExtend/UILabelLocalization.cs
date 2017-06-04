using UnityEngine;
using System.Collections;

public class UILabelLocalization : MonoBehaviour {
	public string localizeKey;

	// Use this for initialization
	void Awake () {
		if (TextManager.LanguageLoaded == false)
		{
			TextManager.LoadLanguage("Localization/zh-Hans/loc-kit");
		}
		UILabel label = this.GetComponent<UILabel>();
		label.text = TextManager.GetText(localizeKey);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
