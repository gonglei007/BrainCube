using UnityEngine;
using System.Collections;

public class SALevelLoader : SA_Singleton<SALevelLoader> {

	private Texture2D bg;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	public void LoadLevel(string name) {
		Application.LoadLevel(name);
	}

	public void Restart() {
		Application.LoadLevel(Application.loadedLevelName);
	}
}
