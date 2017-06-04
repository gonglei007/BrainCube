using UnityEngine;
using System.Collections;

public class iOSDeviceInfo {
	public enum FormFactor {
		iPhone,
		iPhoneRetain,
		iPad,
		iPadRetain
	}
	
	public static FormFactor formFactor = Screen.currentResolution.height>480.0f ? 
																		(Screen.currentResolution.height>960.0f ?
																				(Screen.currentResolution.height>1024.0f ? FormFactor.iPadRetain : FormFactor.iPad)																						
																		: FormFactor.iPhoneRetain) :
																  FormFactor.iPhone;
}
