using UnityEngine;
using System.Collections;

public class SA_BackButton : DefaultPreviewButton {

	public static string firstLevel = string.Empty;

	void Start() {

		if(firstLevel != string.Empty) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		if(firstLevel == string.Empty) {
			firstLevel = Application.loadedLevelName;
		}
	}


	void FixedUpdate() {
		if(Application.loadedLevelName.Equals(firstLevel)) {
			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
		} else {
			GetComponent<Renderer>().enabled = true;
			GetComponent<Collider>().enabled = true;
		}
	}

	protected override void OnClick() {
		base.OnClick();
		GoBack();
		//Invoke("GoBack", 0.8f);

	}

	private void GoBack() {
		SALevelLoader.instance.LoadLevel(firstLevel);
	}
}
