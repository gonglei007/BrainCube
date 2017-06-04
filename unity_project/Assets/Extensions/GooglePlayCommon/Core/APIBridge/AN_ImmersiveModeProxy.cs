using UnityEngine;
using System.Collections;

public class AN_ImmersiveModeProxy {

	private const string CLASS_NAME = "com.androidnative.features.ImmersiveMode";


	private static void CallActivityFunction(string methodName, params object[] args) {
		AN_ProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}

	public static void enableImmersiveMode() {
		CallActivityFunction("enableImmersiveMode");
	}


	
	public static void setGravity(int gravity, int id) {

		CallActivityFunction("setGravity", gravity, id);
	}
	
	
	public static void setPosition(int x, int y, int id) {
		CallActivityFunction("setPosition", x, y, id);
	}
	
	
	public static void show(int id) {
		CallActivityFunction("show", id);
	}
	
	
	public static void hide(int id) {
		CallActivityFunction("hide", id);
	}
	
	
	public static void refresh(int id) {
		CallActivityFunction("refresh", id);
	}
}
