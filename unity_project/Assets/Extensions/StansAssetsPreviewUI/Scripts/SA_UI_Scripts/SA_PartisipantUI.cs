using UnityEngine;
using System.Collections;

public class SA_PartisipantUI : MonoBehaviour {


	public GameObject avatar;
	public SA_Label id;
	public SA_Label status;
	public SA_Label playerId;
	public SA_Label playerName;

	private Texture defaulttexture;

	void Awake() {
		defaulttexture = avatar.GetComponent<Renderer>().material.mainTexture;
	}

	public void SetPartisipant(GP_Partisipant p) {

		id.text = "";
		playerId.text = "";
		playerName.text = "";
		status.text = GP_RTM_ParticipantStatus.STATUS_UNRESPONSIVE.ToString();

		avatar.GetComponent<Renderer>().material.mainTexture = defaulttexture;


		GooglePlayerTemplate player = GooglePlayManager.instance.GetPlayerById(p.playerId);
		if(player != null) {
			playerId.text = "Player Id: " + p.playerId;
			playerName.text = "Name: " + player.name;

			if(player.icon != null) {
				avatar.GetComponent<Renderer>().material.mainTexture = player.icon;
			}

		}
		id.text  = "ID: " +  p.id;
		status.text = "Status: " + p.status.ToString();





	}
}
