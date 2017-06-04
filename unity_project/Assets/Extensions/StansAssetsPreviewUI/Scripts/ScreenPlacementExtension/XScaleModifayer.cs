using UnityEngine;
using System.Collections;

public class XScaleModifayer : MonoBehaviour {

	public float XMaxSize = 10;
	public bool scaleDownOnly = false;
	public bool calulateStartOnly = false;

	
	void Awake () {

		Calculate();
	}


	void FixedUpdate() {
		if(!calulateStartOnly) {
			Calculate();
		}
	}


	public void Calculate() {
		Rect size = PreviewScreenUtil.getObjectBounds(gameObject);
		
		float desireSizeX = Screen.width / 100f * XMaxSize;
		
		if(size.width < desireSizeX) {
			if(scaleDownOnly) {
				return;
			}
		}
		
		float ScaleFactor = desireSizeX / size.width;
		
		transform.localScale = transform.localScale * ScaleFactor;
	}
	

}
