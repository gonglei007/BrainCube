using UnityEngine;

/// <summary>
/// Screen positions for use with the ScreenPlacement transform and gameObject extension for the 9 positions around the screen edge.
/// </summary>
public enum ScreenPosition {UpperLeft, UpperMiddle, UpperRight, Left, Middle, Right, LowerLeft, LowerMiddle, LowerRight};

public static class ScreenPlacementExtension{
	
	//Add instructions
	
	//GameObject overrides:
	/// <summary>
	/// Places a GameObject at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="GameObject"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	public static void ScreenPlacement(this GameObject target, ScreenPosition position){
		DoScreenPlacement(target.transform, position, Vector2.zero, Camera.main);
	}
	
	/// <summary>
	/// Places a GameObject at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="GameObject"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	/// <param name="renderingCamera">
	/// A <see cref="Camera"/>
	/// </param>
	public static void ScreenPlacement(this GameObject target, ScreenPosition position, Camera renderingCamera){
		DoScreenPlacement(target.transform, position, Vector2.zero, renderingCamera);
	}
	
	/// <summary>
	/// Places a GameObject at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="GameObject"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	/// <param name="pixelsFromEdge">
	/// A <see cref="Vector2"/>
	/// </param>
	public static void ScreenPlacement(this GameObject target, ScreenPosition position, Vector2 pixelsFromEdge){
		DoScreenPlacement(target.transform, position, pixelsFromEdge, Camera.main);
	}
	
	/// <summary>
	/// Places a GameObject at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="GameObject"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	/// <param name="pixelsFromEdge">
	/// A <see cref="Vector2"/>
	/// </param>
	/// <param name="renderingCamera">
	/// A <see cref="Camera"/>
	/// </param>
	public static void ScreenPlacement(this GameObject target, ScreenPosition position, Vector2 pixelsFromEdge, Camera renderingCamera){
		DoScreenPlacement(target.transform, position, pixelsFromEdge, renderingCamera);
	}
	
	//Transform overrides:
	
	/// <summary>
	/// Places a Transform at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="Transform"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	public static void ScreenPlacement(this Transform target, ScreenPosition position){
		DoScreenPlacement(target.transform, position, Vector2.zero, Camera.main);
	}
	
	/// <summary>
	/// Places a Transform at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="Transform"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	/// <param name="renderingCamera">
	/// A <see cref="Camera"/>
	/// </param>
	public static void ScreenPlacement(this Transform target, ScreenPosition position, Camera renderingCamera){
		DoScreenPlacement(target.transform, position, Vector2.zero, renderingCamera);
	}
	
	/// <summary>
	/// Places a Transform at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="Transform"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	/// <param name="pixelsFromEdge">
	/// A <see cref="Vector2"/>
	/// </param>
	public static void ScreenPlacement(this Transform target, ScreenPosition position, Vector2 pixelsFromEdge){
		DoScreenPlacement(target.transform, position, pixelsFromEdge, Camera.main);
	}
	
	/// <summary>
	/// Places a Transform at one of the 9 positions around the screen edge.
	/// </summary>
	/// <param name="target">
	/// A <see cref="Transform"/>
	/// </param>
	/// <param name="position">
	/// A <see cref="ScreenPosition"/>
	/// </param>
	/// <param name="pixelsFromEdge">
	/// A <see cref="Vector2"/>
	/// </param>
	/// <param name="renderingCamera">
	/// A <see cref="Camera"/>
	/// </param>
	public static void ScreenPlacement(this Transform target, ScreenPosition position, Vector2 pixelsFromEdge, Camera renderingCamera){
		DoScreenPlacement(target.transform, position, pixelsFromEdge, renderingCamera);
	}	
	
	//Placement execution:
	private static void DoScreenPlacement(this Transform target, ScreenPosition position, Vector2 pixelsFromEdge, Camera renderingCamera){
		Vector3 screenPosition = Vector3.zero;
		float zPosition =  -renderingCamera.transform.position.z + target.position.z;
		
		switch (position) {		
		
		//uppers:
		case ScreenPosition.UpperLeft:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3(pixelsFromEdge.x, Screen.height-pixelsFromEdge.y, zPosition));
		break;
			
		case ScreenPosition.UpperMiddle:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3( (Screen.width/2)+pixelsFromEdge.x, Screen.height-pixelsFromEdge.y, zPosition));
		break;
			
		case ScreenPosition.UpperRight:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3(Screen.width-pixelsFromEdge.x, Screen.height-pixelsFromEdge.y, zPosition));
		break;	
		
		//mids:
		case ScreenPosition.Left:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3(pixelsFromEdge.x, (Screen.height/2) - pixelsFromEdge.y, zPosition));
		break;
				
		case ScreenPosition.Middle:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3((Screen.width/2) + pixelsFromEdge.x, (Screen.height/2) - pixelsFromEdge.y, zPosition));
		break;
			
		case ScreenPosition.Right:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3(Screen.width-pixelsFromEdge.x, (Screen.height/2)-pixelsFromEdge.y, zPosition));
		break;
		
		//lowers:
		case ScreenPosition.LowerLeft:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3(pixelsFromEdge.x, pixelsFromEdge.y, zPosition));
		break;
			
		case ScreenPosition.LowerMiddle:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3((Screen.width/2)+pixelsFromEdge.x, pixelsFromEdge.y, zPosition));
		break;
			
		case ScreenPosition.LowerRight:
			screenPosition = renderingCamera.ScreenToWorldPoint(new Vector3(Screen.width-pixelsFromEdge.x, pixelsFromEdge.y, zPosition));
		break;			
			
			
		}
		
		target.transform.position = screenPosition;
	}
}

