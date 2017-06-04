////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GPLeaderBoard  {
	
	private string _id;
	private string _name;

	
	public GPScoreCollection SocsialCollection =  new GPScoreCollection();
	public GPScoreCollection GlobalCollection  =  new GPScoreCollection();

	public Dictionary<string, int> currentPlayerRank =  new Dictionary<string, int>();


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	

	public GPLeaderBoard(string lId, string lName) {
		_id = lId;
		_name = lName;
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public void UpdateName(string lName) {
		_name = lName;
	}


	public List<GPScore> GetScoresList(GPBoardTimeSpan timeSpan, GPCollectionType collection) {
		GPScoreCollection col = GlobalCollection;
		
		switch(collection) {
		case GPCollectionType.GLOBAL:
			col = GlobalCollection;
			break;
		case GPCollectionType.FRIENDS:
			col = SocsialCollection;
			break;
		}
		
		
		Dictionary<int, GPScore> scoreDict = col.AllTimeScores;
		
		switch(timeSpan) {
		case GPBoardTimeSpan.ALL_TIME:
			scoreDict = col.AllTimeScores;
			break;
		case GPBoardTimeSpan.TODAY:
			scoreDict = col.TodayScores;
			break;
		case GPBoardTimeSpan.WEEK:
			scoreDict = col.WeekScores;
			break;
		}

		List<GPScore> scores = new List<GPScore>();
		scores.AddRange(scoreDict.Values);


		return scores;
	}



	public GPScore GetScoreByPlayerId(string playerId, GPBoardTimeSpan timeSpan, GPCollectionType collection) {
		List<GPScore> scores = GetScoresList(timeSpan, collection);
		foreach(GPScore s in scores) {
			if(s.playerId.Equals(playerId)) {
				return s;
			}
		}

		return null;
	}


	public GPScore GetScore(int rank, GPBoardTimeSpan timeSpan, GPCollectionType collection) {
		
		GPScoreCollection col = GlobalCollection;
		
		switch(collection) {
		case GPCollectionType.GLOBAL:
			col = GlobalCollection;
			break;
		case GPCollectionType.FRIENDS:
			col = SocsialCollection;
			break;
		}
		

		Dictionary<int, GPScore> scoreDict = col.AllTimeScores;
		
		switch(timeSpan) {
		case GPBoardTimeSpan.ALL_TIME:
			scoreDict = col.AllTimeScores;
			break;
		case GPBoardTimeSpan.TODAY:
			scoreDict = col.TodayScores;
			break;
		case GPBoardTimeSpan.WEEK:
			scoreDict = col.WeekScores;
			break;
		}


		if(scoreDict.ContainsKey(rank)) {
			return scoreDict[rank];
		} else {
			return null;
		}
		
	}

	public GPScore GetCurrentPlayerScore(GPBoardTimeSpan timeSpan, GPCollectionType collection) {
		string key = timeSpan.ToString() + "_" + collection.ToString();
		if(currentPlayerRank.ContainsKey(key)) {
			int rank = currentPlayerRank[key];
			return GetScore(rank, timeSpan, collection);
		} else {
			return null;
		}

	}

	public void UpdateCurrentPlayerRank(int rank, GPBoardTimeSpan timeSpan, GPCollectionType collection) {
		string key = timeSpan.ToString() + "_" + collection.ToString();
		if(currentPlayerRank.ContainsKey(key)) {
			currentPlayerRank[key] = rank;
		} else {
			currentPlayerRank.Add(key, rank);
		}
	}
	
	public void UpdateScore(GPScore score) {
	
		GPScoreCollection col = GlobalCollection;
		
		switch(score.collection) {
		case GPCollectionType.GLOBAL:
			col = GlobalCollection;
			break;
		case GPCollectionType.FRIENDS:
			col = SocsialCollection;
			break;
		}

		
		Dictionary<int, GPScore> scoreDict = col.AllTimeScores;
		
		switch(score.timeSpan) {
		case GPBoardTimeSpan.ALL_TIME:
			scoreDict = col.AllTimeScores;
			break;
		case GPBoardTimeSpan.TODAY:
			scoreDict = col.TodayScores;
			break;
		case GPBoardTimeSpan.WEEK:
			scoreDict = col.WeekScores;
			break;
		}
	
		if(scoreDict.ContainsKey(score.rank)) {
			scoreDict[score.rank] = score;
		} else {
			scoreDict.Add(score.rank, score);
		}

	}


	//--------------------------------------
	// GET / SET
	//--------------------------------------


	public string id {
		get {
			return _id;
		}
	}



	public string name {
		get {
			return _name;
		}
	}






}
