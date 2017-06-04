using UnityEngine;
using System.Collections;

public class CustomLeaderboardFiledsHolder : MonoBehaviour {

	public TextMesh rank;
	public TextMesh score;
	public TextMesh playerId;
	public TextMesh playerName;
	public GameObject avatar;

	void Awake() {
		avatar.GetComponent<Renderer>().material =  new Material(avatar.GetComponent<Renderer>().material);
	}


	public void Disable() {
		rank.text = "";
		score.text = "";
		playerId.text = "";
		playerName.text = "";

		avatar.GetComponent<Renderer>().enabled = false;
	}
}
