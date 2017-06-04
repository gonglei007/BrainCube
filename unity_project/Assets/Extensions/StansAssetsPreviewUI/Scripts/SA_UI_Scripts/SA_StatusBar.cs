using UnityEngine;
using System.Collections;

public class SA_StatusBar : MonoBehaviour {

	public TextMesh title;
	public TextMesh shadow;

	public void SetText(string text) {
		title.text = text;
		shadow.text = text;
	}



	public static string text {
		get {
			SA_StatusBar bar = GameObject.FindObjectOfType<SA_StatusBar>();
			if(bar == null) {
				return  "";
			}

			return bar.title.text;
		}

		set {
			SA_StatusBar bar = GameObject.FindObjectOfType<SA_StatusBar>();
			if(bar == null) {
				return;
			}

			bar.SetText(value);
		}
	}
}
