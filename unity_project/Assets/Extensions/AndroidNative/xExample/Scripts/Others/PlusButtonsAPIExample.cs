using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PlusButtonsAPIExample : MonoBehaviour {

	private List<AN_PlusButton> Abuttons =  new List<AN_PlusButton>();

	private AN_PlusButton PlusButton = null;
	private string PlusUrl = "https://unionassets.com/";


	public void CreatePlusButtons() {

		if(Abuttons.Count != 0) {
			return;
		} 

		AN_PlusButton b =  new AN_PlusButton(PlusUrl, AN_PlusBtnSize.SIZE_TALL, AN_PlusBtnAnnotation.ANNOTATION_BUBBLE);
		b.SetGravity(TextAnchor.UpperLeft);

		Abuttons.Add(b);


		b =  new AN_PlusButton(PlusUrl, AN_PlusBtnSize.SIZE_SMALL, AN_PlusBtnAnnotation.ANNOTATION_INLINE);
		b.SetGravity(TextAnchor.UpperRight);
		Abuttons.Add(b);

		b =  new AN_PlusButton(PlusUrl, AN_PlusBtnSize.SIZE_MEDIUM, AN_PlusBtnAnnotation.ANNOTATION_INLINE);
		b.SetGravity(TextAnchor.UpperCenter);
		Abuttons.Add(b);

		b =  new AN_PlusButton(PlusUrl, AN_PlusBtnSize.SIZE_STANDARD, AN_PlusBtnAnnotation.ANNOTATION_INLINE);
		b.SetGravity(TextAnchor.MiddleLeft);

		Abuttons.Add(b);

		foreach(AN_PlusButton bb in Abuttons) {
			bb.ButtonClicked += ButtonClicked;
		}

	}


	public void HideButtons() {
		foreach(AN_PlusButton b in Abuttons) {
			b.Hide();
		}
	}

	public void ShoweButtons() {
		foreach(AN_PlusButton b in Abuttons) {
			b.Show();
		}
	}


	public void CreateRandomPostButton() {
		if(PlusButton == null) {
			PlusButton =  new AN_PlusButton(PlusUrl, AN_PlusBtnSize.SIZE_STANDARD, AN_PlusBtnAnnotation.ANNOTATION_BUBBLE);
			PlusButton.SetPosition(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
			PlusButton.ButtonClicked += ButtonClicked;
		}

	}


	public void ChangePosPostButton()  {
		if(PlusButton != null) {
			PlusButton.SetPosition(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
		}

	}

	void ButtonClicked () {
		AndroidMessage.Create("Click Detected", "Plus Button Click Detected");
	}

	void OnDestroy() {
		HideButtons();
		if(PlusButton != null) {
			PlusButton.Hide();
		}
	}
	
}
