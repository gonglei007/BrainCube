using UnityEngine;
using System.Collections;

public class GP_AdvertisingIdLoadResult : AN_Result {

	public string id = string.Empty;
	public bool isLimitAdTrackingEnabled = false;


	public GP_AdvertisingIdLoadResult(bool IsResultSucceeded):base(IsResultSucceeded) {

	}

}
