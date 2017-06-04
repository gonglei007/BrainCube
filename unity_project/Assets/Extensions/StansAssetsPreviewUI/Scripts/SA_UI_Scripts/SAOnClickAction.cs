using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public abstract class SAOnClickAction : MonoBehaviour {

	void Awake() {
		DefaultPreviewButton btn = GetComponent<DefaultPreviewButton>();
		if(btn != null) {
			btn.addEventListener(BaseEvent.CLICK, OnClick);
		}
	}
	
	protected abstract void OnClick();
}

