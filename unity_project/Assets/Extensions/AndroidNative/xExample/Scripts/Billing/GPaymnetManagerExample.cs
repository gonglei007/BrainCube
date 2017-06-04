////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class GPaymnetManagerExample : MonoBehaviour {

	private static bool _isInited = false;

	//--------------------------------------
	//  INITIALIZE
	//--------------------------------------
	

	public const string ANDROID_TEST_PURCHASED = "android.test.purchased";

	public const string ANDROID_TEST_CANCELED = "android.test.canceled";
	public const string ANDROID_TEST_REFUNDED = "android.test.refunded";
	public const string ANDROID_TEST_ITEM_UNAVAILABLE = "android.test.item_unavailable";



	public static void init() {


		//Filling product list

		//When you will add your own proucts you can skip this code section of you already have added
		//your products ids under the editor setings menu
		AndroidInAppPurchaseManager.instance.addProduct(ANDROID_TEST_PURCHASED);
		AndroidInAppPurchaseManager.instance.addProduct(ANDROID_TEST_CANCELED);
		AndroidInAppPurchaseManager.instance.addProduct(ANDROID_TEST_REFUNDED);
		AndroidInAppPurchaseManager.instance.addProduct(ANDROID_TEST_ITEM_UNAVAILABLE);


		//listening for purchase and consume events
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;


		//listening for store initilaizing finish
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
	

		//you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
		AndroidInAppPurchaseManager.instance.loadStore();

		//or You can pass base64EncodedPublicKey using scirption:
		//AndroidInAppPurchaseManager.instance.loadStore(YOU_BASE_64_KEY_HERE);



	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------


	public static void purchase(string SKU) {
		AndroidInAppPurchaseManager.instance.purchase (SKU);
	}

	public static void consume(string SKU) {
		AndroidInAppPurchaseManager.instance.consume (SKU);
	}

	//--------------------------------------
	//  GET / SET
	//--------------------------------------

	public static bool isInited {
		get {
			return _isInited;
		}
	}


	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase) {
		//some stuff for processing product purchse. Add coins, unlock track, etc
	}

	private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase) {
		//some stuff for processing product consume. Reduse tip anount, reduse gold token, etc
	}

	private static void OnProductPurchased(BillingResult result) {


		if(result.isSuccess) {
			AndroidMessage.Create ("Product Purchased", result.purchase.SKU+ "\n Full Response: " + result.purchase.originalJson);
			OnProcessingPurchasedProduct (result.purchase);
		} else {
			AndroidMessage.Create("Product Purchase Failed", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Purchased Responce: " + result.response.ToString() + " " + result.message);
		Debug.Log (result.purchase.originalJson);
	}


	private static void OnProductConsumed(BillingResult result) {

		if(result.isSuccess) {
			AndroidMessage.Create ("Product Consumed", result.purchase.SKU + "\n Full Response: " + result.purchase.originalJson);
			OnProcessingConsumeProduct (result.purchase);
		} else {
			AndroidMessage.Create("Product Cousume Failed", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Cousume Responce: " + result.response.ToString() + " " + result.message);
	}
	

	private static void OnBillingConnected(BillingResult result) {
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;


		if(result.isSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
		} 

		AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
	}




	private static void OnRetrieveProductsFinised(BillingResult result) {
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;


		if(result.isSuccess) {
			_isInited = true;
			AndroidMessage.Create("Success", "Billing init complete inventory contains: " + AndroidInAppPurchaseManager.instance.inventory.purchases.Count + " products");

			Debug.Log("Loaded products names");
			foreach(GoogleProductTemplate tpl in AndroidInAppPurchaseManager.instance.inventory.products) {
				Debug.Log(tpl.title);
				Debug.Log(tpl.originalJson);
			}
		} else {
			 AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);

	}






















}
