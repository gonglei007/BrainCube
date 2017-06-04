using UnityEngine;
using System.Collections;

public class AN_NotificationProxy {

	private const string CLASS_NAME = "com.androidnative.features.notifications.LocalNotificationManager";
	
	private static void CallActivityFunction(string methodName, params object[] args) {
		AN_ProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}

	// --------------------------------------
	// Toast Notification
	// --------------------------------------

	public static void ShowToastNotification(string text, int duration) {
		CallActivityFunction("ShowToastNotification", text, duration.ToString());
	}

	// --------------------------------------
	// Local Notifications
	// --------------------------------------

	public static void HideAllNotifications() {
		CallActivityFunction ("HideAllNotifications");
	}
	
	public static void requestCurrentAppLaunchNotificationId() { 
		CallActivityFunction("requestCurrentAppLaunchNotificationId");
	}

	public static void ScheduleLocalNotification(AndroidNotificationBuilder builder) {
		CallActivityFunction("ScheduleLocalNotification",
		                     builder.Title,
		                     builder.Message,
		                     builder.Time.ToString(),
		                     builder.Id.ToString(),
		                     builder.Icon,
		                     builder.Sound,
		                     builder.Vibration.ToString(),
		                     builder.ShowIfAppForeground.ToString(),
		                     builder.LargeIcon);
	}
	
	public static void CanselLocalNotification(int id) {
		CallActivityFunction("canselLocalNotification", id.ToString());
	}

	// --------------------------------------
	// Google Cloud Message
	// --------------------------------------

	public static void InitPushNotifications(string icon, string sound, bool vibration) {
		CallActivityFunction ("InitPushNotifications", icon, sound, vibration.ToString());
	}

	public static void InitParsePushNotifications(string appId, string dotNetKey) {
		CallActivityFunction ("InitParsePushNotifications", appId, dotNetKey);
	}
	
	public static void GCMRgisterDevice(string senderId) {
		CallActivityFunction("GCMRgisterDevice", senderId);
	}
	
	public static void GCMLoadLastMessage() {
		CallActivityFunction("GCMLoadLastMessage");
	}
}
