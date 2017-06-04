using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class PreviewScreenUtil : EventDispatcher
{
	private static PreviewScreenUtil _instance 			= null;
	public const string SCREEN_RESSIZED 		= "screen_resized"; 
	
	private int W = 0;
	private int H = 0;
	
	
	public static bool isInScreenRect(Rect rect, Vector2 point) {
		point.y = Screen.height - point.y;
		if(rect.Contains(point)) {
			return true;
		}
		return false;
	}
	
	
	public static Rect getObjectBounds(GameObject obj) {
		if(obj.GetComponent<Renderer>() != null) {
			return getRendererBounds(obj.GetComponent<Renderer>());
		} else {
			return new Rect();
		}
		
	}
		
	public static Rect getRendererBounds(Renderer renderer) {
	 	Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(renderer.bounds.min.x, renderer.bounds.max.y, 0f));
	    Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(renderer.bounds.max.x, renderer.bounds.min.y, 0f));

	    // Create rect in screen space and return - does not account for camera perspective
	    Rect size =  new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
		return size;
	}
	
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	void Awake() {
		W = Screen.width;
		H = Screen.height;
	}
	
	void FixedUpdate() {
		if(W != Screen.width || H != Screen.height) {
			W = Screen.width;			
			H = Screen.height;
			dispatch(SCREEN_RESSIZED);
		}
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	
	public static PreviewScreenUtil instance {
		get {
			if(_instance == null){
				_instance = new GameObject("ScreenUtil").AddComponent<PreviewScreenUtil>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}		
	}	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------
}

