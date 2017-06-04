
using UnityEngine;
using System.Collections;

public class ScreenPlacement : MonoBehaviour {
	
	public ScreenPosition position;
	public Vector2 pixelOffset;
	public bool persents = false;
	public bool calulateStartOnly = true;
	
	
	public Transform boundsTransform = null;
	
	private Vector2 actualOffset = new Vector2();

	private ScreenOrientation orinetation;
	
	void Start(){
		if(calulateStartOnly) {
			placementCalculation();
		}
	}
	
	void FixedUpdate() {
		if(!calulateStartOnly) {
			placementCalculation();
		}
	}
	
	
	public void placementCalculation() {

		transform.ScreenPlacement( position, pixelOffset );	
		
		Transform tf;
		if(boundsTransform == null) {
			tf = transform;
		} else {
			tf = boundsTransform;
		}
		
	
		
		// Get mesh origin and farthest extent (this works best with simple convex meshes)
		Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(tf.GetComponent<Renderer>().bounds.min.x, tf.GetComponent<Renderer>().bounds.max.y, 0f));
		Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(tf.GetComponent<Renderer>().bounds.max.x, tf.GetComponent<Renderer>().bounds.min.y, 0f));

	    // Create rect in screen space and return - does not account for camera perspective
	    Rect size =  new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
		
		
				
		
		float offsetX = 0;
		float offsetY = 0;
		
		if(persents) {
			offsetX = Screen.width  / 100 * pixelOffset.x;
			offsetY = Screen.height / 100 * pixelOffset.y;
		} else {
			offsetX = pixelOffset.x;
			offsetY = pixelOffset.y;
		}
		

		switch(position) {
			case ScreenPosition.Right:
				actualOffset.x = offsetX + size.width / 2;
				break;
			case ScreenPosition.UpperRight:
				actualOffset.x = offsetX + size.width / 2;
				actualOffset.y = offsetY + size.height / 2;
				break;
			
			case ScreenPosition.LowerRight:
				actualOffset.x = offsetX + size.width / 2;
				actualOffset.y = offsetY + size.height / 2;
				break;
			
			case ScreenPosition.Left:
				actualOffset.x = offsetX + size.width / 2;
				break;
			
			case ScreenPosition.LowerLeft:
				actualOffset.x = offsetX + size.width / 2;
				actualOffset.y = offsetY + size.height / 2;
				break;
			
			case ScreenPosition.UpperLeft:
				actualOffset.x = offsetX + size.width / 2;
				actualOffset.y = offsetY + size.height / 2;
				break;
			
			case ScreenPosition.UpperMiddle:
				//actualOffset.x = offsetX - size.width / 2;
				actualOffset.y = offsetY + size.height / 2;
				break;
			case ScreenPosition.LowerMiddle:
				actualOffset.y = offsetY + size.height / 2;
				break;
			
			
		}
		
		transform.ScreenPlacement( position, actualOffset);		
	}
}
