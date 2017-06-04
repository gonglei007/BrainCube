////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

public class GooglePurchaseTemplate  {

	public string orderId;
	public string packageName;
	public string SKU;

	public string developerPayload;
	public string signature;
	public string token;
	public long time;
	public string originalJson;
	public GooglePurchaseState state;


	public void SetState(string code) {
		int c = System.Convert.ToInt32(code);
		switch(c) {
		case 0:
			state = GooglePurchaseState.PURCHASED;
			break;
		case 1:
			state = GooglePurchaseState.CANCELED;
			break;
		case 2:
			state = GooglePurchaseState.REFUNDED;
			break;
		}
	}

}
