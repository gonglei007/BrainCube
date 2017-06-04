using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class AsyncTask : EventDispatcher {


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

}

