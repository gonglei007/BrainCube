using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GooglePlayRTM : SA_Singleton<GooglePlayRTM>  {




	//Events
	public const string DATA_RECIEVED		      	= "data_recieved";
	public const string ROOM_UPDATED            	= "room_updated";


	public const string ON_CONNECTED_TO_ROOM        = "OnConnectedToRoom";
	public const string ON_DISCONNECTED_FROM_ROOM 	= "OnDisconnectedFromRoom";
	public const string ON_P2P_CONNECTED			= "OnP2PConnected";
	public const string ON_P2P_DISCONNECTED 		= "OnP2PDisconnected";
	public const string ON_PEER_DECLINED 			= "OnPeerDeclined";
	public const string ON_PEER_INVITED_TO_ROOM 	= "OnPeerInvitedToRoom";
	public const string ON_PEER_JOINED 				= "OnPeerJoined";
	public const string ON_PEER_LEFT 				= "OnPeerLeft";
	public const string ON_PEERS_CONNECTED 			= "OnPeersConnected";
	public const string ON_PEERS_DISCONNECTED 		= "OnPeersDisconnected";
	public const string ON_ROOM_AUTOMATCHING 		= "OnRoomAutoMatching";
	public const string ON_ROOM_CONNECTING 			= "OnRoomConnecting";
	public const string ON_JOINED_ROOM 				= "OnJoinedRoom";
	public const string ON_LEFT_ROOM 				= "OnLeftRoom";
	public const string ON_ROOM_CONNECTED 			= "OnRoomConnected";
	public const string ON_ROOM_CREATED 			= "OnRoomCreated";

	public const string ON_INVITATION_BOX_UI_CLOSED = "onInvitationBoxUiClosed";
	public const string ON_WATING_ROOM_INTENT_CLOSED = "OnWatingRoomIntentClosed";


	public const string ON_INVITATION_RECEIVED = "on_invitation_received";
	public const string ON_INVITATION_REMOVED = "on_invitation_removed";


	//Actions
	public static  Action<GP_RTM_Network_Package> ActionDataRecieved		      	=  delegate{};
	public static  Action<GP_RTM_Room> ActionRoomUpdated            	=  delegate{};
	
	
	public static Action ActionConnectedToRoom        =  delegate{};
	public static Action ActionDisconnectedFromRoom 	=  delegate{};

	//contains participant id
	public static Action<string>  ActionP2PConnected			=  delegate{};
	public static Action<string>  ActionP2PDisconnected 		=  delegate{};

	//contains participants ids
	public static Action<string[]> ActionPeerDeclined 			=  delegate{};
	public static Action<string[]> ActionPeerInvitedToRoom 	=  delegate{};
	public static Action<string[]> ActionPeerJoined 				=  delegate{};
	public static Action<string[]> ActionPeerLeft 				=  delegate{};
	public static Action<string[]> ActionPeersConnected 			=  delegate{};
	public static Action<string[]> ActionPeersDisconnected 		=  delegate{};
	public static Action ActionRoomAutomatching 		=  delegate{};
	public static Action ActionRoomConnecting 			=  delegate{};
	public static Action<GP_GamesStatusCodes> ActionJoinedRoom 				=  delegate{};
	public static Action<GP_RTM_Result> ActionLeftRoom 				=  delegate{};
	public static Action<GP_GamesStatusCodes> ActionRoomConnected 			=  delegate{};
	public static Action<GP_GamesStatusCodes> ActionRoomCreated 			=  delegate{};
	
	public static Action<AndroidActivityResult> ActionInvitationBoxUIClosed =  delegate{};
	public static Action<AndroidActivityResult> ActionWatingRoomIntentClosed =  delegate{};
	
	//contains invitation id
	public static Action<string> ActionInvitationReceived =  delegate{};
	public static Action<string> ActionInvitationRemoved =  delegate{};





	private const int BYTE_LIMIT = 256;
	private GP_RTM_Room _currentRoom = new GP_RTM_Room();
	private List<GP_RTM_Invite> _invitations =  new List<GP_RTM_Invite>();

	
	//--------------------------------------
	// INITIALIZATION
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
		_currentRoom = new GP_RTM_Room();

		Debug.Log("GooglePlayRTM Created");

	}

	//--------------------------------------
	// API METHODS
	//--------------------------------------

	public void FindMatch(int minPlayers, int maxPlayers, int bitMask = 0) {
		AN_GMSRTMProxy.RTMFindMatch(minPlayers, maxPlayers, bitMask);
	}

	public void SendDataToAll(byte[] data, GP_RTM_PackageType sendType) {
		string dataString = ConvertByteDataToString(data);
		AN_GMSRTMProxy.sendDataToAll(dataString, (int) sendType);
	}
	
	public void sendDataToPlayers(byte[] data, GP_RTM_PackageType sendType, params string[] players) {
		string dataString = ConvertByteDataToString(data);
		string playersString = string.Join(AndroidNative.DATA_SPLITTER, players);
		AN_GMSRTMProxy.sendDataToPlayers(dataString, playersString, (int) sendType);
	}

	public void ShowWaitingRoomIntent() {
		AN_GMSRTMProxy.ShowWaitingRoomIntent();
	}

	public void OpenInvitationBoxUI(int minPlayers, int maxPlayers) {
		AN_GMSRTMProxy.InvitePlayers(minPlayers, maxPlayers);
	}

	public void LeaveRoom() {
		AN_GMSGiftsProxy.leaveRoom();
	}


	public void AcceptInviteToRoom(string intitationId) {
		AN_GMSGiftsProxy.acceptInviteToRoom(intitationId);
	}
	
	public void OpenInvitationInBoxUI()  {
		AN_GMSGiftsProxy.showInvitationBox();
	}



	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public GP_RTM_Room currentRoom {
		get {
			return _currentRoom;
		}
	}

	public List<GP_RTM_Invite> invitations {
		get {
			return _invitations;
		}
	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnWatingRoomIntentClosed(string data) {

		string[] storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		AndroidActivityResult result =  new AndroidActivityResult(storeData[0], storeData[1]);

		ActionWatingRoomIntentClosed(result);
		dispatch(ON_WATING_ROOM_INTENT_CLOSED,  result);
	}

	private void OnRoomUpdate(string data) {


		string[] storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		_currentRoom =  new GP_RTM_Room();
		_currentRoom.id = storeData[0];
		_currentRoom.creatorId = storeData[1];

		string[] ParticipantsInfo = storeData[2].Split(","[0]);

		for(int i = 0; i < ParticipantsInfo.Length; i += 6) {
			if(ParticipantsInfo[i] == AndroidNative.DATA_EOF) {
				break;
			}

			GP_Partisipant p =  new GP_Partisipant(ParticipantsInfo[i], ParticipantsInfo[i + 1], ParticipantsInfo[i + 2], ParticipantsInfo[i + 3], ParticipantsInfo[i + 4], ParticipantsInfo[i + 5]);
			_currentRoom.AddPartisipant(p);
		}




		_currentRoom.status =  (GP_RTM_RoomStatus) System.Convert.ToInt32(storeData[3]);
		_currentRoom.creationTimestamp = System.Convert.ToInt64(storeData[4]);

		Debug.Log("GooglePlayRTM OnRoomUpdate Room State: " + _currentRoom.status.ToString());

		ActionRoomUpdated(_currentRoom);
		dispatch(ROOM_UPDATED, _currentRoom);

	}


	private void OnMatchDataRecieved(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("OnMatchDataRecieved, no data avaiable");
			return;
		}
		
		string[] storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		GP_RTM_Network_Package package = new GP_RTM_Network_Package (storeData[0], storeData [1]);


		ActionDataRecieved(package);
		dispatch (DATA_RECIEVED, package);
		Debug.Log ("GooglePlayManager -> DATA_RECEIVED");
	}


	private void OnWatingRoomIntentClosed() {

	}
	
	private void OnConnectedToRoom(string data) {
		ActionConnectedToRoom();
		dispatch (ON_CONNECTED_TO_ROOM);
	}
	
	private void OnDisconnectedFromRoom(string data) {
		ActionDisconnectedFromRoom();
		dispatch (ON_DISCONNECTED_FROM_ROOM);
	}
	
	private void OnP2PConnected(string participantId) {
		ActionP2PConnected(participantId);
		dispatch (ON_P2P_CONNECTED, participantId);
	}
	
	private void OnP2PDisconnected(string participantId) {
		ActionP2PDisconnected(participantId);
		dispatch (ON_P2P_DISCONNECTED, participantId);
	}

	private void OnPeerDeclined(string data) {
		string[] participantsids = data.Split(","[0]);
		ActionPeerDeclined(participantsids);
		dispatch (ON_PEER_DECLINED, participantsids);
	}
	
	private void OnPeerInvitedToRoom(string data) {
		string[] participantsids = data.Split(","[0]);
		ActionPeerInvitedToRoom(participantsids);
		dispatch (ON_PEER_INVITED_TO_ROOM, participantsids);

	}
	
	private void OnPeerJoined(string data) {
		string[] participantsids = data.Split(","[0]);
		ActionPeerJoined(participantsids);
		dispatch (ON_PEER_JOINED, participantsids);

	}
	
	private void OnPeerLeft(string data) {
		string[] participantsids = data.Split(","[0]);
		ActionPeerLeft(participantsids);
		dispatch (ON_PEER_LEFT, participantsids);

	}
	
	private void OnPeersConnected(string data) {
		string[] participantsids = data.Split(","[0]);
		ActionPeersConnected(participantsids);
		dispatch (ON_PEERS_CONNECTED, participantsids);

	}
	
	private void OnPeersDisconnected(string data) {
		string[] participantsids = data.Split(","[0]);
		ActionPeersDisconnected(participantsids);
		dispatch (ON_PEERS_DISCONNECTED, participantsids);

	}
	
	private void OnRoomAutoMatching(string data) {
		ActionRoomAutomatching();
		dispatch (ON_ROOM_AUTOMATCHING);
	}
	
	private void OnRoomConnecting(string data) {
		ActionRoomConnecting();
		dispatch (ON_ROOM_CONNECTING);
	}
		
	private void OnJoinedRoom(string data) {
		GP_GamesStatusCodes code = (GP_GamesStatusCodes)Convert.ToInt32(data);
		ActionJoinedRoom(code);
		dispatch (ON_JOINED_ROOM, code);
	}
	
	private void OnLeftRoom(string data) {
		Debug.Log("OnLeftRoom Created OnRoomUpdate");
		string[] storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		GP_RTM_Result package = new GP_RTM_Result (storeData[0], storeData [1]);


		_currentRoom =  new GP_RTM_Room();
		ActionRoomUpdated(_currentRoom);
		dispatch(ROOM_UPDATED, _currentRoom);

		ActionLeftRoom(package);
		dispatch (ON_LEFT_ROOM, package);
	}
	
	private void OnRoomConnected(string data) {
		GP_GamesStatusCodes code = (GP_GamesStatusCodes)Convert.ToInt32(data);
		ActionRoomConnected(code);
		dispatch (ON_ROOM_CONNECTED, code);
	}
	
	private void OnRoomCreated(string data) {
		GP_GamesStatusCodes code = (GP_GamesStatusCodes)Convert.ToInt32(data);
		ActionRoomCreated(code);
		dispatch (ON_ROOM_CREATED, code);
	}

	private void OnInvitationBoxUiClosed(string data) {

		string[] storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		AndroidActivityResult result =  new AndroidActivityResult(storeData[0], storeData[1]);
	
		ActionInvitationBoxUIClosed(result);
		dispatch(ON_INVITATION_BOX_UI_CLOSED,  result);
	}

	private void OnInvitationReceived(string invitationId) {
	

		GP_RTM_Invite inv =  new GP_RTM_Invite();
		inv.id = invitationId;
		_invitations.Add(inv);
		ActionInvitationReceived(invitationId);
		dispatch(ON_INVITATION_RECEIVED, invitationId);

	}

	private void OnInvitationRemoved(string invitationId) {
		foreach(GP_RTM_Invite inv in _invitations) {
			if(inv.id.Equals(invitationId)) {
				_invitations.Remove(inv);
				return;
			}
		}
		ActionInvitationRemoved(invitationId);
		dispatch(ON_INVITATION_REMOVED, invitationId);
	}

	//--------------------------------------
	// STATIC
	//--------------------------------------

	public static byte[] ConvertStringToByteData(string data) {

	#if UNITY_ANDROID
		if(data == null) {
			return null;
		}
		
		data = data.Replace(AndroidNative.DATA_EOF, string.Empty);
		if(data.Equals(string.Empty)) {
			return null;
		}
		
		string[] array = data.Split("," [0]);
		
		List<byte> listOfBytes = new List<byte> ();
		foreach(string str in array) {
			int param = System.Convert.ToInt32(str);
			int temp_param = param < 0 ? BYTE_LIMIT + param : param;
			listOfBytes.Add (System.Convert.ToByte(temp_param));
		}
		
		return listOfBytes.ToArray ();


	#else
		return new byte[]{};
	#endif

	}

	public static string ConvertByteDataToString(byte[] data) {
		
		string b = "";
		for(int i = 0; i < data.Length; i++) {
			if(i != 0) {
				b += ",";
			}
			
			b += data[i].ToString();
		}

		return b;
		
	}





}
