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


public class GPAchievement  {


	private string _id;
	private string _name;
	private string _description;

	private int _currentSteps;
	private int _totalSteps;

	private GPAchievementType _type;
	private GPAchievementState _state;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	public GPAchievement(string aId, string aName, string aDescr, string aCurentSteps, string aTotalSteps, string aState, string aType) {
		_id = aId;
		_name = aName;
		_description = aDescr;

		_currentSteps = System.Convert.ToInt32 (aCurentSteps);
		_totalSteps = System.Convert.ToInt32 (aTotalSteps);


		_type  = PlayServiceUtil.GetAchievementTypeById (System.Convert.ToInt32 (aType));
		_state = PlayServiceUtil.GetAchievementStateById (System.Convert.ToInt32 (aState));
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------




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

	public string description {
		get {
			return _description;
		}
	}

	public int currentSteps {
		get {
			return _currentSteps;
		}
	} 

	public int totalSteps {
		get {
			return _totalSteps;
		}
	} 
	
	public GPAchievementType type {
		get {
			return _type;
		}
	}

	public GPAchievementState state {
		get {
			return _state;
		}
	}

}
