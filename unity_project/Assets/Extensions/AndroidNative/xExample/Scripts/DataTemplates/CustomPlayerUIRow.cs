using UnityEngine;
using System.Collections;

public class CustomPlayerUIRow : MonoBehaviour {


	public TextMesh playerId;
	public TextMesh playerName;
	public GameObject avatar;
	public TextMesh hasIcon;
	public TextMesh hasImage;

	void Awake() {
		avatar.GetComponent<Renderer>().material =  new Material(avatar.GetComponent<Renderer>().material);
	}


	public void Disable() {
		hasIcon.text = "";
		hasImage.text = "";
		playerId.text = "";
		playerName.text = "";

		avatar.GetComponent<Renderer>().enabled = false;
	}
}
