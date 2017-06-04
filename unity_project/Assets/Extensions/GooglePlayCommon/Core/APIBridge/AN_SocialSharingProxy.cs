using UnityEngine;
using System.Collections;

public class AN_SocialSharingProxy {

	private const string CLASS_NAME = "com.androidnative.features.social.common.SocialGate";
	
	private static void CallActivityFunction(string methodName, params object[] args) {
		AN_ProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}

	// --------------------------------------
	// Social
	// --------------------------------------
	
	
	public static void StartShareIntent(string caption, string message,  string subject, string filters) {
		CallActivityFunction("StartShareIntent", caption, message, subject, filters);
	}
	
	public static void StartShareIntent(string caption, string message, string subject, string media, string filters) {
		CallActivityFunction("StartShareIntentMedia", caption, message, subject, media, filters);
	}
	
	public static void SendMailWithImage(string caption, string message,  string subject, string email, string media) {
		CallActivityFunction("SendMailWithImage", caption, message, subject, email, media);
	}
	
	
	public static void SendMail(string caption, string message,  string subject, string email) {
		CallActivityFunction("SendMail", caption, message, subject, email);
	}


	public static void InstagramPostImage(string data, string cpation) {
		CallActivityFunction("InstagramPostImage", data, cpation);
	}






}
