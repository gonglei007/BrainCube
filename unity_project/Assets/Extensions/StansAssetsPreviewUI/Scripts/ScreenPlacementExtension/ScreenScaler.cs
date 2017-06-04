
using UnityEngine;
using System.Collections;

public class ScreenScaler : MonoBehaviour {
	
	public bool calulateStartOnly = true;

	
	
	public float persentsY = 100;
	private float _scaleFactorY = 1f;
	private float _xScaleDiff;
	
	 

	
	void Awake(){
		_scaleFactorY = transform.localScale.y;
		_xScaleDiff   = transform.localScale.x / transform.localScale.y;
		

		if(calulateStartOnly) {
			placementCalculation();
		}
	}
	
	void Update() {
		
		if(!calulateStartOnly) {
			placementCalculation();
		}
		
	
	}
	

	public void placementCalculation() {
		
		float desireSizeY = Screen.height / 100f * persentsY;
		
	
		Rect size = PreviewScreenUtil.getObjectBounds(gameObject);

		if(size.height < desireSizeY) {
			while(size.height < desireSizeY) {
				size  =  PreviewScreenUtil.getObjectBounds(gameObject);
				transform.localScale =  new Vector3(_scaleFactorY * _xScaleDiff, _scaleFactorY, 0);
				
				_scaleFactorY += 0.1f;
			}
		} else {
			while(size.height > desireSizeY) {
				size  =  PreviewScreenUtil.getObjectBounds(gameObject);
				transform.localScale =  new Vector3(transform.localScale.x - transform.localScale.x * 0.1f, transform.localScale.y - transform.localScale.y * 0.1f, transform.localScale.z);
				

			}
		}
	 

	}
}
