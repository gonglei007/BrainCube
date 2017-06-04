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

public class GooglePlayManager : SA_Singleton<GooglePlayManager> {


	//Events
	public const string SCORE_SUBMITED            = "score_submited";
	public const string SCORE_UPDATED             = "score_updated";
	public const string LEADERBOARDS_LOADED      = "leaderboards_loaded";
	public const string FRIENDS_LOADED            = "players_loaded";
	public const string ACHIEVEMENT_UPDATED       = "achievement_updated";
	public const string ACHIEVEMENTS_LOADED       = "achievements_loaded";
	public const string SCORE_REQUEST_RECEIVED    = "score_request_received";


	public const string SEND_GIFT_RESULT_RECEIVED  = "send_gift_result_received";
	public const string REQUESTS_INBOX_DIALOG_DISMISSED  = "requests_inbox_dialog_dismissed";

	public const string PENDING_GAME_REQUESTS_DETECTED  = "pending_game_requests_detected";
	public const string GAME_REQUESTS_ACCEPTED  		= "game_requests_accepted";


	public const string AVAILABLE_DEVICE_ACCOUNTS_LOADED  = "availablee_device_accounts_loaded";
	public const string OAUTH_TOKEN_LOADED  			 = "oauth_token_loaded";


	//Actions
	public static Action<GP_GamesResult> ActionSoreSubmited 							= delegate {};
	public static Action<GP_GamesResult> ActionPlayerScoreUpdated						= delegate {};
	public static Action<GooglePlayResult> ActionLeaderboardsLoaded 					= delegate {};
	public static Action<GooglePlayResult> ActionFriendsListLoaded 						= delegate {};
	public static Action<GP_GamesResult> ActionAchievementUpdated 						= delegate {};
	public static Action<GooglePlayResult> ActionAchievementsLoaded 					= delegate {};
	public static Action<GooglePlayResult> ActionScoreRequestReceived 					= delegate {};

	public static Action<GooglePlayGiftRequestResult> ActionSendGiftResultReceived 		= delegate {};
	public static Action ActionRequestsInboxDialogDismissed 							= delegate {};
	public static Action<List<GPGameRequest>> ActionPendingGameRequestsDetected 		= delegate {};
	public static Action<List<GPGameRequest>> ActionGameRequestsAccepted 				= delegate {};

	public static Action<List<string>> ActionAvailableDeviceAccountsLoaded 				= delegate {};
	public static Action<string> ActionOAuthTokenLoaded 								= delegate {};



	private GooglePlayerTemplate _player = null ;
	

	private Dictionary<string, GPLeaderBoard> _leaderBoards =  new Dictionary<string, GPLeaderBoard>();
	private Dictionary<string, GPAchievement> _achievements = new Dictionary<string, GPAchievement>();
	private Dictionary<string, GooglePlayerTemplate> _players = new Dictionary<string, GooglePlayerTemplate>();



	private List<string> _friendsList 		  				=  new List<string>();
	private List<string> _deviceGoogleAccountList 		 	=  new List<string>();
	private List<GPGameRequest> _gameRequests 				=  new List<GPGameRequest>();


	private string _loadedAuthToken = "";
	private string _currentAccount = "";

	private static bool _IsLeaderboardsDataLoaded = false;
	


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public void Create() {
		Debug.Log ("GooglePlayManager was created");

		//Creating sub managers
		GooglePlayQuests.instance.Init();
	}


	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------

	public void RetrieveDeviceGoogleAccounts() {
		AN_GMSGeneralProxy.loadGoogleAccountNames();
	}

	public void LoadToken(string accountName,  string scopes) {
		AN_GMSGeneralProxy.loadToken(accountName, scopes);

	}

	public void LoadToken() {
		AN_GMSGeneralProxy.loadToken();
		
	}

	public void InvalidateToken(string token) {
		AN_GMSGeneralProxy.invalidateToken(token);
	}



	[Obsolete("showAchievementsUI is deprecated, please use ShowAchievementsUI instead.")]
	public void showAchievementsUI() {
		ShowAchievementsUI();
	}

	public void ShowAchievementsUI() {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.showAchievementsUI ();
	}

	[Obsolete("showLeaderBoardsUI is deprecated, please use showLeaderBoardsUI instead.")]
	public void showLeaderBoardsUI() {
		ShowLeaderBoardsUI();
	}

	public void ShowLeaderBoardsUI() {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.showLeaderBoardsUI ();
	}

	[Obsolete("showLeaderBoard is deprecated, please use ShowLeaderBoard instead.")]
	public void showLeaderBoard(string leaderboardName) {
		ShowLeaderBoard(leaderboardName);
	}
	

	public void ShowLeaderBoard(string leaderboardName) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.showLeaderBoard (leaderboardName);
	}

	[Obsolete("showLeaderBoardById is deprecated, please use ShowLeaderBoardById instead.")]
	public void showLeaderBoardById(string leaderboardId) {
		ShowLeaderBoardById(leaderboardId);
	}

	public void ShowLeaderBoardById(string leaderboardId) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.showLeaderBoardById (leaderboardId);
	}


	[Obsolete("submitScore is deprecated, please use SubmitScore instead.")]
	public void submitScore(string leaderboardName, long score) {
		SubmitScore(leaderboardName, score);
	}
	
	public void SubmitScore(string leaderboardName, long score) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.submitScore (leaderboardName, score);
	}

	[Obsolete("submitScoreById is deprecated, please use SubmitScoreById instead.")]
	public void submitScoreById(string leaderboardId, long score) {
		SubmitScoreById(leaderboardId, score);
	}

	public void SubmitScoreById(string leaderboardId, long score) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.submitScoreById (leaderboardId, score);
	}


	[Obsolete("loadLeaderBoards is deprecated, please use LoadLeaderBoards instead.")]
	public void loadLeaderBoards() {
		LoadLeaderBoards();
	}

	public void LoadLeaderBoards() {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.loadLeaderBoards ();
	}



	public void UpdatePlayerScore(string leaderboardId, GPBoardTimeSpan span, GPCollectionType collection) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.UpdatePlayerScore(leaderboardId, (int) span, (int) collection);
	}

	[Obsolete("loadPlayerCenteredScores is deprecated, please use LoadPlayerCenteredScores instead.")]
	public void loadPlayerCenteredScores(string leaderboardId, GPBoardTimeSpan span, GPCollectionType collection, int maxResults) {
		LoadPlayerCenteredScores(leaderboardId, span, collection, maxResults);
	}

	public void LoadPlayerCenteredScores(string leaderboardId, GPBoardTimeSpan span, GPCollectionType collection, int maxResults) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.loadPlayerCenteredScores(leaderboardId, (int) span, (int) collection, maxResults);
	}

	[Obsolete("loadTopScores is deprecated, please use LoadTopScores instead.")]
	public void loadTopScores(string leaderboardId, GPBoardTimeSpan span, GPCollectionType collection, int maxResults) {
		LoadTopScores(leaderboardId, span, collection, maxResults);
	}

	public void LoadTopScores(string leaderboardId, GPBoardTimeSpan span, GPCollectionType collection, int maxResults) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.loadTopScores(leaderboardId, (int) span, (int) collection, maxResults);
	}





	public void UnlockAchievement(string achievementName) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.reportAchievement (achievementName);
	}
	
	public void UnlockAchievementById(string achievementId) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.reportAchievementById (achievementId);
	}


	[Obsolete("reportAchievement is deprecated, please use unlockAchievement instead.")]
	public void reportAchievement(string achievementName) {
		UnlockAchievement(achievementName);
	}

	[Obsolete("reportAchievementById is deprecated, please use unlockAchievementById instead.")]
	public void reportAchievementById(string achievementId) {
		UnlockAchievementById(achievementId);
	}


	[Obsolete("revealAchievement is deprecated, please use RevealAchievement instead.")]
	public void revealAchievement(string achievementName)  {
		RevealAchievement(achievementName);
	}

	public void RevealAchievement(string achievementName) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.revealAchievement (achievementName);
	}


	[Obsolete("revealAchievementById is deprecated, please use RevealAchievementById instead.")]
	public void revealAchievementById(string achievementId) {
		RevealAchievementById(achievementId);
	}

	public void RevealAchievementById(string achievementId) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.revealAchievementById (achievementId);
	}

	[Obsolete("incrementAchievement is deprecated, please use IncrementAchievement instead.")]
	public void incrementAchievement(string achievementName, int numsteps) {
		IncrementAchievement(achievementName, numsteps);
	}


	public void IncrementAchievement(string achievementName, int numsteps) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.incrementAchievement (achievementName, numsteps.ToString());
	}

	[Obsolete("incrementAchievementById is deprecated, please use IncrementAchievementById instead.")]
	public void incrementAchievementById(string achievementId, int numsteps) {
		IncrementAchievementById(achievementId, numsteps);
	}

	public void IncrementAchievementById(string achievementId, int numsteps) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.incrementAchievementById (achievementId, numsteps.ToString());
	}

	[Obsolete("loadAchievements is deprecated, please use LoadAchievements instead.")]
	public void loadAchievements() {
		LoadAchievements();
	}

	public void LoadAchievements() {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.loadAchievements ();
	}

	[Obsolete("resetAchievement is deprecated, please use ResetAchievement instead.")]
	public void resetAchievement(string achievementId) {
		ResetAchievement(achievementId);
	}

	public void ResetAchievement(string achievementId) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.resetAchievement(achievementId);

	}

	public void ResetAllAchievements() {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.ResetAllAchievements();
		
	}


	[Obsolete("resetLeaderBoard is deprecated, please use ResetLeaderBoard instead.")]
	public void resetLeaderBoard(string leaderboardId) {
		ResetLeaderBoard(leaderboardId);
	}

	public void ResetLeaderBoard(string leaderboardId) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.resetLeaderBoard(leaderboardId);

		if(leaderBoards.ContainsKey(leaderboardId)) {
			leaderBoards.Remove(leaderboardId);
		}

	}

	[Obsolete("loadConnectedPlayers is deprecated, please use LoadConnectedPlayers instead.")]
	public void loadConnectedPlayers() {
		LoadConnectedPlayers();
	}

	public void LoadConnectedPlayers() {
		if (!GooglePlayConnection.CheckState ()) { return; }
		AN_GMSGeneralProxy.loadConnectedPlayers ();
	}


	//--------------------------------------
	// GIFTS
	//--------------------------------------
	
	public void SendGiftRequest(GPGameRequestType type, int requestLifetimeDays, Texture2D icon, string description, string playload = "") {
		if (!GooglePlayConnection.CheckState ()) { return; }

		byte[] val = icon.EncodeToPNG();
		string bytesString = System.Convert.ToBase64String (val);

		AN_GMSGiftsProxy.sendGiftRequest((int) type, playload, requestLifetimeDays, bytesString, description);

	}

	public string currentAccount {
		get {
			return _currentAccount;
		}
	}
	
	public void ShowRequestsAccepDialog() {
		if (!GooglePlayConnection.CheckState ()) { return; }

		AN_GMSGiftsProxy.showRequestAccepDialog();
	}

	public void AcceptRequests(params string[] ids) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		
		if(ids.Length == 0) {
			return;
		}
		
		
		AN_GMSGiftsProxy.acceptRequests(string.Join(AndroidNative.DATA_SPLITTER, ids));
	}


	public void DismissRequest(params string[] ids) {
		if (!GooglePlayConnection.CheckState ()) { return; }
		
		if(ids.Length == 0) {
			return;
		}
		
		
		AN_GMSGiftsProxy.dismissRequest(string.Join(AndroidNative.DATA_SPLITTER, ids));
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public GPLeaderBoard GetLeaderBoard(string leaderboardId) {
		if(_leaderBoards.ContainsKey(leaderboardId)) {
			return _leaderBoards[leaderboardId];
		} else {
			return null;
		}
	}
	

	public GPAchievement GetAchievement(string achievementId) {
		if(_achievements.ContainsKey(achievementId)) {
			return _achievements[achievementId];
		} else {
			return null;
		}
	}


	public GooglePlayerTemplate GetPlayerById(string playerId) {
		if(players.ContainsKey(playerId)) {
			return players[playerId];
		} else {
			return null;
		}
	}

	public GPGameRequest GetGameRequestById(string id) {
		foreach(GPGameRequest r in _gameRequests) {
			if(r.id.Equals(id)) {
				return r;
			} 
		}

		return null;
	} 


	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public GooglePlayerTemplate player {
		get {
			return _player;
		}
	}

	public Dictionary<string, GooglePlayerTemplate> players {
		get {
			return _players;
		}
	}
	

	public Dictionary<string, GPLeaderBoard> leaderBoards {
		get {
			return _leaderBoards;
		}
	}

	public Dictionary<string, GPAchievement> achievements {
		get {
			return _achievements;
		}
	}

	public List<string> friendsList {
		get {
			return _friendsList;
		}
	}


	public List<GPGameRequest> gameRequests {
		get {
			return _gameRequests;
		}
	}

	public List<string> deviceGoogleAccountList {
		get {
			return _deviceGoogleAccountList;
		}
	}

	public string loadedAuthToken {
		get {
			return _loadedAuthToken;
		}
	}

	public static bool IsLeaderboardsDataLoaded {
		get {
			return _IsLeaderboardsDataLoaded;
		}
	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnGiftSendResult(string data) {

		Debug.Log("OnGiftSendResult");

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		GooglePlayGiftRequestResult result =  new GooglePlayGiftRequestResult(storeData [0]);

		ActionSendGiftResultReceived(result);
		dispatch(SEND_GIFT_RESULT_RECEIVED, result);
	}

	private void OnRequestsInboxDialogDismissed(string data) {
		ActionRequestsInboxDialogDismissed();
		dispatch(REQUESTS_INBOX_DIALOG_DISMISSED);
	}


	private void OnAchievementsLoaded(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		GooglePlayResult result = new GooglePlayResult (storeData [0]);
		if(result.isSuccess) {

			_achievements.Clear ();

			for(int i = 1; i < storeData.Length; i+=7) {
				if(storeData[i] == AndroidNative.DATA_EOF) {
					break;
				}

				GPAchievement ach = new GPAchievement (storeData[i], 
				                                       storeData[i + 1],
				                                       storeData[i + 2],
				                                       storeData[i + 3],
				                                       storeData[i + 4],
				                                       storeData[i + 5],
				                                       storeData[i + 6]
				                                       );

				Debug.Log (ach.name);
				Debug.Log (ach.type);


				_achievements.Add (ach.id, ach);

			}

			Debug.Log ("Loaded: " + _achievements.Count + " Achievements");
		}

		ActionAchievementsLoaded(result);
		dispatch (ACHIEVEMENTS_LOADED, result);

	}

	private void OnAchievementUpdated(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		GP_GamesResult result = new GP_GamesResult (storeData [0]);
		result.achievementId = storeData [1];

		ActionAchievementUpdated(result);
		dispatch (ACHIEVEMENT_UPDATED, result);

	}

	private void OnScoreDataRecevied(string data) {
		Debug.Log("OnScoreDataRecevide");
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		GooglePlayResult result = new GooglePlayResult (storeData [0]);
		if(result.isSuccess) {

			GPBoardTimeSpan 	timeSpan 		= (GPBoardTimeSpan)  System.Convert.ToInt32(storeData[1]);
			GPCollectionType 	collection  	= (GPCollectionType) System.Convert.ToInt32(storeData[2]);
			string leaderboardId 				= storeData[3];
			string leaderboardName 				= storeData[4];


			GPLeaderBoard lb;
			if(_leaderBoards.ContainsKey(leaderboardId)) {
				lb = _leaderBoards[leaderboardId];
			} else {
				lb = new GPLeaderBoard (leaderboardId, leaderboardName);
				Debug.Log("Added: "  + leaderboardId);
				_leaderBoards.Add(leaderboardId, lb);
			}

			lb.UpdateName(leaderboardName);

			for(int i = 5; i < storeData.Length; i+=8) {
				if(storeData[i] == AndroidNative.DATA_EOF) {
					break;
				}


				
			 	long score = System.Convert.ToInt64(storeData[i]);
				int rank = System.Convert.ToInt32(storeData[i + 1]);


				string playerId = storeData[i + 2];
				if(!players.ContainsKey(playerId)) {
					GooglePlayerTemplate p = new GooglePlayerTemplate (playerId, storeData[i + 3], storeData[i + 4], storeData[i + 5], storeData[i + 6], storeData[i + 7]);
					AddPlayer(p);
				}

				GPScore s =  new GPScore(score, rank, timeSpan, collection, lb.id, playerId);
				lb.UpdateScore(s);

				if(playerId.Equals(player.playerId)) {
					lb.UpdateCurrentPlayerRank(rank, timeSpan, collection);
				}
			}
		}


		ActionScoreRequestReceived(result);
		dispatch (SCORE_REQUEST_RECEIVED, result);

	}

	private void OnLeaderboardDataLoaded(string data) {
		Debug.Log("OnLeaderboardDataLoaded");
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);


		GooglePlayResult result = new GooglePlayResult (storeData [0]);
		if(result.isSuccess) {

			for(int i = 1; i < storeData.Length; i+=26) {
				if(storeData[i] == AndroidNative.DATA_EOF) {
					break;
				}

				string leaderboardId = storeData[i];
				string leaderboardName = storeData [i + 1];

			
				GPLeaderBoard lb;
				if(_leaderBoards.ContainsKey(leaderboardId)) {
					lb = _leaderBoards[leaderboardId];
				} else {
					lb = new GPLeaderBoard (leaderboardId, leaderboardName);
					_leaderBoards.Add(leaderboardId, lb);
				}

				lb.UpdateName(leaderboardName);


				int start = i + 2;
				for(int j = 0; j < 6; j++) {

					long score = System.Convert.ToInt64(storeData[start]);
					int rank = System.Convert.ToInt32(storeData[start + 1]);

					GPBoardTimeSpan 	timeSpan 		= (GPBoardTimeSpan)  System.Convert.ToInt32(storeData[start + 2]);
					GPCollectionType 	collection  	= (GPCollectionType) System.Convert.ToInt32(storeData[start + 3]);

					//Debug.Log("timeSpan: " + timeSpan +   " collection: " + collection + " score:" + score + " rank:" + rank);

					GPScore variant =  new GPScore(score, rank, timeSpan, collection, lb.id, player.playerId);
					start = start + 4;
					lb.UpdateScore(variant);
					lb.UpdateCurrentPlayerRank(rank, timeSpan, collection);

				}


			}

			Debug.Log ("Loaded: " + _leaderBoards.Count + " Leaderboards");
		}

		_IsLeaderboardsDataLoaded = true;

		ActionLeaderboardsLoaded(result);
		dispatch (LEADERBOARDS_LOADED, result);

	}


	private void OnPlayerScoreUpdated(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("GooglePlayManager OnPlayerScoreUpdated, no data avaiable");
			return;
		}


		Debug.Log("OnPlayerScoreUpdated");


		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		GP_GamesResult result = new GP_GamesResult (storeData [0]);

		if(result.isSuccess) {

			GPBoardTimeSpan 	timeSpan 		= (GPBoardTimeSpan)  System.Convert.ToInt32(storeData[1]);
			GPCollectionType 	collection  	= (GPCollectionType) System.Convert.ToInt32(storeData[2]);

			string leaderboardId = storeData[3];

			long score = System.Convert.ToInt64(storeData[4]);
			int rank = System.Convert.ToInt32(storeData[5]);

			GPLeaderBoard lb;
			if(_leaderBoards.ContainsKey(leaderboardId)) {
				lb = _leaderBoards[leaderboardId];
			} else {
				lb = new GPLeaderBoard (leaderboardId, "");
				_leaderBoards.Add(leaderboardId, lb);
			}

			GPScore variant =  new GPScore(score, rank, timeSpan, collection, lb.id, player.playerId);
			lb.UpdateScore(variant);
			lb.UpdateCurrentPlayerRank(rank, timeSpan, collection);

		}

		ActionPlayerScoreUpdated(result);
		dispatch (SCORE_UPDATED, result);
	}

	private void OnScoreSubmitted(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("GooglePlayManager OnScoreSubmitted, no data avaiable");
			return;
		}

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		GP_GamesResult result = new GP_GamesResult (storeData [0]);
		result.leaderboardId = storeData [1];

		ActionSoreSubmited(result);
		dispatch (SCORE_SUBMITED, result);

	}

	private void OnPlayerDataLoaded(string data) {

		Debug.Log("OnPlayerDataLoaded");
		if(data.Equals(string.Empty)) {
			Debug.Log("GooglePlayManager OnPlayerLoaded, no data avaiable");
			return;
		}

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		_player = new GooglePlayerTemplate (storeData [0], storeData [1], storeData [2], storeData [3], storeData [4], storeData [5]);
		AddPlayer(_player);

		_currentAccount = storeData [6];
	}

	private void OnPlayersLoaded(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		
		GooglePlayResult result = new GooglePlayResult (storeData [0]);
		if(result.isSuccess) {

			for(int i = 1; i < storeData.Length; i+=6) {
				if(storeData[i] == AndroidNative.DATA_EOF) {
					break;
				}
				

				GooglePlayerTemplate p = new GooglePlayerTemplate (storeData[i], storeData[i + 1], storeData[i + 2], storeData[i + 3], storeData[i + 4], storeData[i + 5]);
				AddPlayer(p);
				if(!_friendsList.Contains(p.playerId)) {
					_friendsList.Add(p.playerId);
				}

			}
		}
		
		
		
		Debug.Log ("OnPlayersLoaded, total:" + players.Count.ToString());
		ActionFriendsListLoaded(result);
		dispatch (FRIENDS_LOADED, result);
	}

	private void OnGameRequestsLoaded(string data) {
		_gameRequests = new List<GPGameRequest>();
		if(data.Length == 0) {
			return;
		}


		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		for(int i = 0; i < storeData.Length; i+=6) {
			if(storeData[i] == AndroidNative.DATA_EOF) {
				break;
			}

			GPGameRequest r = new GPGameRequest();
			r.id = storeData[i];
			r.playload = storeData[i +1];

			r.expirationTimestamp 	 = System.Convert.ToInt64(storeData[i + 2]);
			r.creationTimestamp		 = System.Convert.ToInt64(storeData[i + 3]);

			r.sender = storeData[i +4];
			r.type = (GPGameRequestType) System.Convert.ToInt32(storeData[i + 5]);
			_gameRequests.Add(r);


		}

		ActionPendingGameRequestsDetected(_gameRequests);
		dispatch(PENDING_GAME_REQUESTS_DETECTED, _gameRequests);

	}

	private void OnGameRequestsAccepted(string data) {
		List<GPGameRequest> acceptedList =  new List<GPGameRequest>();

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		for(int i = 0; i < storeData.Length; i+=6) {
			if(storeData[i] == AndroidNative.DATA_EOF) {
				break;
			}
			
			GPGameRequest r = new GPGameRequest();
			r.id = storeData[i];
			r.playload = storeData[i +1];
			
			r.expirationTimestamp 	 = System.Convert.ToInt64(storeData[i + 2]);
			r.creationTimestamp		 = System.Convert.ToInt64(storeData[i + 3]);
			
			r.sender = storeData[i + 4];
			r.type = (GPGameRequestType) System.Convert.ToInt32(storeData[i + 5]);

			acceptedList.Add(r);
			
		}

		ActionGameRequestsAccepted(acceptedList);
		dispatch(GAME_REQUESTS_ACCEPTED, acceptedList);
	}

	private void OnAccountsLoaded(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		_deviceGoogleAccountList.Clear();

		foreach(string acc in storeData) {
			if(acc != AndroidNative.DATA_EOF) {
				_deviceGoogleAccountList.Add(acc);
			}
		}

		ActionAvailableDeviceAccountsLoaded(_deviceGoogleAccountList);
		dispatch(AVAILABLE_DEVICE_ACCOUNTS_LOADED, _deviceGoogleAccountList);
	}

	private void OnTokenLoaded(string token) {
		_loadedAuthToken = token;

		ActionOAuthTokenLoaded(_loadedAuthToken);
		dispatch(OAUTH_TOKEN_LOADED, _loadedAuthToken);
	}


	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------

	private void AddPlayer(GooglePlayerTemplate p) {
		if(!_players.ContainsKey(p.playerId)) {
			_players.Add(p.playerId, p);
		}
	}

}
