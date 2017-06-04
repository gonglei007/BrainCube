using UnityEngine;
using System.Collections;

public class GP_Partisipant : MonoBehaviour {

	private string _id;
	private string _playerid;
	private string _HiResImageUrl;
	private string _IconImageUrl;
	private string _DisplayName;



	private GP_RTM_ParticipantStatus _status = GP_RTM_ParticipantStatus.STATUS_UNRESPONSIVE;


	public GP_Partisipant(string uid, string playerUid, string stat, string hiResImg, string IconImg, string Name) {
		_id = uid;
		_playerid = playerUid;
		_status = (GP_RTM_ParticipantStatus) System.Convert.ToInt32(stat);
		_HiResImageUrl = hiResImg;
		_IconImageUrl = IconImg;
		_DisplayName = Name;
	}

	public string id {
		get {
			return _id;
		}
	}

	public string playerId {
		get {
			return _playerid;
		}
	}

	public string HiResImageUrl {
		get {
			return _HiResImageUrl;
		}
	}

	public string IconImageUrl {
		get {
			return _IconImageUrl;
		}
	}

	public string DisplayName {
		get {
			return _DisplayName;
		}
	}

	public GP_RTM_ParticipantStatus status {
		get {
			return _status;
		}
	}
}
