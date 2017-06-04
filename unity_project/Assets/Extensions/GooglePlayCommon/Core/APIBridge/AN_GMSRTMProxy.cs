using UnityEngine;
using System.Collections;

public class AN_GMSRTMProxy : MonoBehaviour {

	private const string CLASS_NAME = "com.androidnative.gms.core.GameClientBridge";
	
	private static void CallActivityFunction(string methodName, params object[] args) {
		AN_ProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}

	// --------------------------------------
	// RTM
	// --------------------------------------
	
	public static void RTMFindMatch(int minPlayers, int maxPlayers, int bitMask) {
		CallActivityFunction("RTMFindMatch", minPlayers.ToString(), maxPlayers.ToString(), bitMask.ToString());
	}
	
	public static void sendDataToAll(string data, int sendType) {
		CallActivityFunction("sendDataToAll", data, sendType.ToString());
	}
	
	public static void sendDataToPlayers(string data, string players, int sendType) {
		CallActivityFunction("sendDataToAll", data, players, sendType.ToString());
	}
	
	public static void ShowWaitingRoomIntent() {
		CallActivityFunction("showWaitingRoomIntent");
	}
	
	public static void InvitePlayers(int minPlayers, int maxPlayers) {
		CallActivityFunction("invitePlayers", minPlayers.ToString(), maxPlayers.ToString());
	}
}
