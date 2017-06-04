////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AndroidInAppPurchaseManager : SA_Singleton<AndroidInAppPurchaseManager> {

	//Events
	public const string ON_PRODUCT_PURCHASED   = "on_product_purchased";
	public const string ON_PRODUCT_CONSUMED    = "on_product_consumed";

	public const string ON_BILLING_SETUP_FINISHED   = "on_billing_setup_finished";
	public const string ON_RETRIEVE_PRODUC_FINISHED = "on_retrieve_produc_finished";

	//Actions
	public static Action<BillingResult>  ActionProductPurchased   = delegate {};
	public static Action<BillingResult>  ActionProductConsumed    = delegate {};
	
	public static Action<BillingResult>  ActionBillingSetupFinished   = delegate {};
	public static Action<BillingResult>  ActionRetrieveProducsFinished = delegate {};

	private List<string> _productsIds =  new List<string>();


	private string _processedSKU;
	private AndroidInventory _inventory;


	private bool _IsConnectingToServiceInProcess 	= false;
	private bool _IsProductRetrievingInProcess 		= false;

	private bool _IsConnectd = false;
	private bool _IsInventoryLoaded = false;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
		_inventory = new AndroidInventory ();
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------


	//Fill your products befor loading store
	public void addProduct(string SKU) {

		if(_productsIds.Contains(SKU)) {
			return;
		}


		_productsIds.Add(SKU);
	}

	public void retrieveProducDetails() {
		_IsProductRetrievingInProcess = true;
		AN_BillingProxy.RetrieveProducDetails();
	}




	public void purchase(string SKU) {
		purchase(SKU, "");
	}

	public void purchase(string SKU, string DeveloperPayload) {
		_processedSKU = SKU;
		AN_BillingProxy.Purchase (SKU, DeveloperPayload);
	}



	public void subscribe(string SKU) {
		subscribe(SKU, "");
	}


	public void subscribe(string SKU, string DeveloperPayload) {
		_processedSKU = SKU;
		AN_BillingProxy.Subscribe (SKU, DeveloperPayload);
	}

	public void consume(string SKU) {
		_processedSKU = SKU;
		AN_BillingProxy.Consume (SKU);
	}


	public void loadStore() {
		if(AndroidNativeSettings.Instance.IsBase64KeyWasReplaced) {
			loadStore(AndroidNativeSettings.Instance.base64EncodedPublicKey);
			_IsConnectingToServiceInProcess = true;
		} else {
			Debug.LogError("Replace base64EncodedPublicKey in Androdi Native Setting menu");
		}
	}


	public void loadStore(string base64EncodedPublicKey) {

		foreach(string pid in AndroidNativeSettings.Instance.InAppProducts) {
			addProduct(pid);
		}

		string ids = "";
		int len = _productsIds.Count;
		for(int i = 0; i < len; i++) {
			if(i != 0) {
				ids += ",";
			}

			ids += _productsIds[i];
		}

		AN_BillingProxy.Connect (ids, base64EncodedPublicKey);
	}



	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public AndroidInventory inventory {
		get {
			return _inventory;
		}
	}

	public bool IsConnectingToServiceInProcess {
		get {
			return _IsConnectingToServiceInProcess;
		}
	}

	public bool IsProductRetrievingInProcess {
		get {
			return _IsProductRetrievingInProcess;
		}
	}

	public bool IsConnectd {
		get {
			return _IsConnectd;
		}
	}

	public bool IsInventoryLoaded {
		get {
			return _IsInventoryLoaded;
		}
	}
	

	//--------------------------------------
	// EVENTS
	//--------------------------------------



	public void OnPurchaseFinishedCallback(string data) {
		Debug.Log(data);
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);
		GooglePurchaseTemplate purchase = new GooglePurchaseTemplate ();


		if(resp == BillingResponseCodes.BILLING_RESPONSE_RESULT_OK) {

			purchase.SKU 						= storeData[2];
			purchase.packageName 				= storeData[3];
			purchase.developerPayload 			= storeData[4];
			purchase.orderId 	       			= storeData[5];
			purchase.SetState(storeData[6]);
			purchase.token 	        			= storeData[7];
			purchase.signature 	        		= storeData[8];
			purchase.time						= System.Convert.ToInt64(storeData[9]);
			purchase.originalJson 				= storeData[10];

			if(_inventory != null) {
				_inventory.addPurchase (purchase);
			}

		} else {
			purchase.SKU 						= _processedSKU;
		}


		BillingResult result = new BillingResult (resp, storeData [1], purchase);

		ActionProductPurchased(result);
		dispatch (ON_PRODUCT_PURCHASED, result);
	}


	public void OnConsumeFinishedCallBack(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);
		GooglePurchaseTemplate purchase = null;


		if(resp == BillingResponseCodes.BILLING_RESPONSE_RESULT_OK) {
			purchase = new GooglePurchaseTemplate ();
			purchase.SKU 				= storeData[2];
			purchase.packageName 		= storeData[3];
			purchase.developerPayload 	= storeData[4];
			purchase.orderId 	        = storeData[5];
			purchase.SetState(storeData[6]);
			purchase.token 	        		= storeData[7];
			purchase.signature 	        	= storeData[8];
			purchase.originalJson 	        = storeData[9];

			if(_inventory != null) {
				_inventory.removePurchase (purchase);
			}

		}

		BillingResult result = new BillingResult (resp, storeData [1], purchase);

		ActionProductConsumed(result);
		dispatch (ON_PRODUCT_CONSUMED, result);
	}



	public void OnBillingSetupFinishedCallback(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);


		_IsConnectd = true;
		_IsConnectingToServiceInProcess = false;
		BillingResult result = new BillingResult (resp, storeData [1]);

		ActionBillingSetupFinished(result);
		dispatch (ON_BILLING_SETUP_FINISHED, result);
	}


	public void OnQueryInventoryFinishedCallBack(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);

		BillingResult result = new BillingResult (resp, storeData [1]);

		_IsInventoryLoaded = true;
		_IsProductRetrievingInProcess = false;

		ActionRetrieveProducsFinished(result);
		dispatch (ON_RETRIEVE_PRODUC_FINISHED, result);
	}



	public void OnPurchasesRecive(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("InAppPurchaseManager, no purchases avaiable");
			return;
		}

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);



		for(int i = 0; i < storeData.Length; i+=9) {
			GooglePurchaseTemplate tpl =  new GooglePurchaseTemplate();
			tpl.SKU 				= storeData[i];
			tpl.packageName 		= storeData[i + 1];
			tpl.developerPayload 	= storeData[i + 2];
			tpl.orderId 	        = storeData[i + 3];
			tpl.SetState(storeData[i + 4]);
			tpl.token 	        	= storeData[i + 5];
			tpl.signature 	        = storeData[i + 6];
			tpl.time 	        	= System.Convert.ToInt64(storeData[i + 7]); 
			tpl.originalJson 	    = storeData[i + 8];

			_inventory.addPurchase (tpl);
		}

		Debug.Log("InAppPurchaseManager, tottal purchases loaded: " + _inventory.purchases.Count);

	}


	public void OnProducttDetailsRecive(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("InAppPurchaseManager, no products avaiable");
			return;
		}

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);


		for(int i = 0; i < storeData.Length; i+=7) {
			GoogleProductTemplate tpl =  new GoogleProductTemplate();
			tpl.SKU 		  				= storeData[i];
			tpl.price 		  				= storeData[i + 1];
			tpl.title 	      				= storeData[i + 2];
			tpl.description   				= storeData[i + 3];
			tpl.priceAmountMicros 	      	= storeData[i + 4];
			tpl.priceCurrencyCode   		= storeData[i + 5];
			tpl.originalJson   				= storeData[i + 6];
			_inventory.addProduct (tpl);
		}

		Debug.Log("InAppPurchaseManager, tottal products loaded: " + _inventory.products.Count);
	}


}
