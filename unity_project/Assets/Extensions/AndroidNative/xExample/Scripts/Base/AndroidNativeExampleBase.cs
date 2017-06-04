using UnityEngine;
using System.Collections;

public class AndroidNativeExampleBase : MonoBehaviour {

	public virtual void Awake() {
		if(Application.platform != RuntimePlatform.Android) {
			Debug.LogWarning("The Android Native Example Scene will only work on Real Android Device");
		}
	}
}
