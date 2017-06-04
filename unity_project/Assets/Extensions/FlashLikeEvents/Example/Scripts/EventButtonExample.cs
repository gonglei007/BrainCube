////////////////////////////////////////////////////////////////////////////////
//  
// @module Events Pro
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////
 

using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class EventButtonExample : EventDispatcher {

	public static EventButtonExample instance = null;
	
	public float w = 150;
	public float h = 50;
	
	void Awake() {
		instance = this;
	}
	
	void OnGUI() {
		Rect buttonRect =  new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
		if(GUI.Button(buttonRect, "click me")) {
			dispatch(BaseEvent.CLICK, "hello");
		}
	}
	
}
