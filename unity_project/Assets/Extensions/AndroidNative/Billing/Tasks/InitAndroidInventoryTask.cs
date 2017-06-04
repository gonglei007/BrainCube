using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class InitAndroidInventoryTask : EventDispatcher {


	public static InitAndroidInventoryTask Create() {
		return new GameObject("InitAndroidInventoryTask").AddComponent<InitAndroidInventoryTask>();
	}

	public void Run() {

		Debug.Log("InitAndroidInventoryTask task started");
		if(AndroidInAppPurchaseManager.instance.IsConnectd) {
			OnBillingConnected(null);
		} else {
			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_BILLING_SETUP_FINISHED, OnBillingConnected);
			if(!AndroidInAppPurchaseManager.instance.IsConnectingToServiceInProcess) {
				AndroidInAppPurchaseManager.instance.loadStore();
			}
		}
	}



	private void OnBillingConnected(CEvent e) {
		Debug.Log("OnBillingConnected");
		if(e == null) {
			OnBillingConnectFinished();
			return;
		}

		BillingResult result = e.data as BillingResult;
		AndroidInAppPurchaseManager.instance.removeEventListener (AndroidInAppPurchaseManager.ON_BILLING_SETUP_FINISHED, OnBillingConnected);
		
		
		if(result.isSuccess) {
			OnBillingConnectFinished();
		}  else {
			Debug.Log("OnBillingConnected Failed");
			dispatch(BaseEvent.FAILED);
		}

	}

	private void OnBillingConnectFinished() {
		Debug.Log("OnBillingConnected COMPLETE");
		//Store connection is Successful. Next we loading product and customer purchasing details

		if(AndroidInAppPurchaseManager.instance.IsInventoryLoaded) {
			Debug.Log("IsInventoryLoaded COMPLETE");
			dispatch(BaseEvent.COMPLETE);
		} else {
			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_RETRIEVE_PRODUC_FINISHED, OnRetrieveProductsFinised);
			if(!AndroidInAppPurchaseManager.instance.IsProductRetrievingInProcess) {
				AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			}
		}

	}


	private void OnRetrieveProductsFinised(CEvent e) {
		Debug.Log("OnRetrieveProductsFinised");
		BillingResult result = e.data as BillingResult;
		AndroidInAppPurchaseManager.instance.removeEventListener (AndroidInAppPurchaseManager.ON_RETRIEVE_PRODUC_FINISHED, OnRetrieveProductsFinised);
		
		if(result.isSuccess) {
			Debug.Log("OnRetrieveProductsFinised COMPLETE");
			dispatch(BaseEvent.COMPLETE);
		} else {
			Debug.Log("OnRetrieveProductsFinised FAILED");
			dispatch(BaseEvent.FAILED);
		}
	}






}
