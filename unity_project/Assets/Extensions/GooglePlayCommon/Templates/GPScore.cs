////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;

public class GPScore  {


	private int _rank = 0;
	private long _score = 0;

	private string _playerId;
	private string _leaderboardId;

	private GPCollectionType _collection;
	private GPBoardTimeSpan _timeSpan;

	

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	public GPScore(long vScore, int vRank, GPBoardTimeSpan vTimeSpan, GPCollectionType sCollection, string lid, string pid) {
		_score = vScore;
		_rank = vRank;

		_playerId = pid;
		_leaderboardId = lid;
	

		_timeSpan  = vTimeSpan;
		_collection = sCollection;

	}


	public void UpdateScore(long vScore) {
		_score = vScore;
	}



	//--------------------------------------
	// GET / SET
	//--------------------------------------


	public int rank {
		get {
			return _rank;
		}
	}


	public long score {
		get {
			return _score;
		}
	}

	public string playerId {
		get {
			return _playerId;
		}
	}

	public string leaderboardId {
		get {
			return _leaderboardId;
		}
	}
	

	public GPCollectionType collection {
		get {
			return _collection;
		}
	}


	public GPBoardTimeSpan timeSpan {
		get {
			return _timeSpan;
		}
	}

}
