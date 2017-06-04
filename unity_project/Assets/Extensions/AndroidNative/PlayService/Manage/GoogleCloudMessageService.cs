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

public class GoogleCloudMessageService : SA_Singleton<GoogleCloudMessageService> {


	//Events
	public const string CLOUD_MESSAGE_SERVICE_REGISTRATION_FAILED = "cloud_message_service_registration_failed";
	public const string CLOUD_MESSAGE_SERVICE_REGISTRATION_RECIVED = "cloud_message_service_registration_recived";
	public const string CLOUD_MESSAGE_LOADED = "cloud_message_loaded";

	//Actions

	public static Action<string> ActionCouldMessageLoaded 						 =  delegate {};
	public static Action<GP_GCM_RegistrationResult> ActionCMDRegistrationResult  =  delegate {};



	private string _lastMessage = string.Empty;
	private string _registrationId = string.Empty;


	
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public void InitPushNotifications() {
		AN_NotificationProxy.InitPushNotifications (
			AndroidNativeSettings.Instance.PushNotificationIcon == null ? string.Empty : AndroidNativeSettings.Instance.PushNotificationIcon.name,
		    AndroidNativeSettings.Instance.PushNotificationSound == null ? string.Empty : AndroidNativeSettings.Instance.PushNotificationSound.name,
		    AndroidNativeSettings.Instance.EnableVibrationPush);
	}

	public void InitPushNotifications(string icon, string sound, bool enableVibrationPush) {
		AN_NotificationProxy.InitPushNotifications (icon, sound,enableVibrationPush);
	}

	public void InitParsePushNotifications() {
		AN_NotificationProxy.InitParsePushNotifications (AndroidNativeSettings.Instance.ParseAppId, AndroidNativeSettings.Instance.DotNetKey);
	}

	public void RgisterDevice() {
		AN_NotificationProxy.GCMRgisterDevice(AndroidNativeSettings.Instance.GCM_SenderId);
	}

	public void LoadLastMessage() {
		AN_NotificationProxy.GCMLoadLastMessage();
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public string registrationId {
		get {
			return _registrationId;
		}
	}

	public string lastMessage {
		get {
			return _lastMessage;
		}
	}
	
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnLastMessageLoaded(string data) {
		_lastMessage = data;
		dispatch(CLOUD_MESSAGE_LOADED, lastMessage);

	}

	
	private void OnRegistrationReviced(string regId) {
		_registrationId = regId;

		ActionCMDRegistrationResult(new GP_GCM_RegistrationResult(_registrationId));
		dispatch(CLOUD_MESSAGE_SERVICE_REGISTRATION_RECIVED, regId);
	}
	
	private void OnRegistrationFailed() {
		ActionCMDRegistrationResult(new GP_GCM_RegistrationResult());
		dispatch(CLOUD_MESSAGE_SERVICE_REGISTRATION_FAILED);
	}
	
	
	
}
