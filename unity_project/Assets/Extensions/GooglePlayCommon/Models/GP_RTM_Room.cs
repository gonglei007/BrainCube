using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GP_RTM_Room {

	public string id = string.Empty;
	public string creatorId = string.Empty;
	public GP_RTM_RoomStatus status = GP_RTM_RoomStatus.ROOM_VARIANT_DEFAULT;
	public long creationTimestamp = 0;

	public List<GP_Partisipant> partisipants = new List<GP_Partisipant>();


	public GP_RTM_Room() {
		partisipants = new List<GP_Partisipant>();
	}

	public void AddPartisipant(GP_Partisipant p) {
		partisipants.Add(p);
	}

	public GP_Partisipant GetPartisipantById(string id) {
		foreach(GP_Partisipant p in partisipants) {
			if(p.id.Equals(id)) {
				return p;
			}
		}

		return null;
	}
}
