using UnityEngine;
using System;

public class UIClose : MonoBehaviour {
	public delegate bool WillStartCallBack(GameObject gameObject);
	
	public Action<GameObject>	endCallBack;
	public WillStartCallBack	willStartCallBack;
		
	public void Close() {
		bool enableClose = true;
		if (willStartCallBack != null) {
			enableClose = willStartCallBack(gameObject);
		}
		
		if (enableClose == false) {
			return;
		}

		this.gameObject.SetActive(false);
		
		if (endCallBack != null) {
			endCallBack(gameObject);
		}
	}
}
