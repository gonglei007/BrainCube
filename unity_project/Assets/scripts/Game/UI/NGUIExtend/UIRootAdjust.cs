using UnityEngine;
using System.Collections;

public class UIRootAdjust : MonoBehaviour {
	public UIRootModify uiRoot;
	public int standardWidth = 640;
	public int standardHeight = 960;
	
	void Awake() {
		
//		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor){
//			return;
//		}

		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		
		bool isMoreWide = ((screenWidth * 1.0f / screenHeight - standardWidth * 1.0f / standardHeight) > 0.001f);
		
		if (isMoreWide) {
			//Do nothing, Unity will do everything
			//This will lead the device screen has two black lines on left and right
		}
		else {
			//This will lead the device screen has two black lines on top and bottom
			uiRoot.manualHeight = (int)(screenHeight * (standardWidth * 1.0f / screenWidth));
		}
	}
}
