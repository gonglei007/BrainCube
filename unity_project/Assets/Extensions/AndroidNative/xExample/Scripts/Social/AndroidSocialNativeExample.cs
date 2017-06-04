using UnityEngine;
using System.Collections;

public class AndroidSocialNativeExample : MonoBehaviour {


	public Texture2D shareTexture;

	void Awake() {
		SA_StatusBar.text = "Social Sharing scene is loaded";
	}

	public void ShareText() {
		Debug.Log("ShareText");
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share");

	}

	public void ShareScreehshot() {
		StartCoroutine(PostScreenshot());
	}

	public void ShareImage() {
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "Sharing Hello wolrd image", shareTexture);
	}




	public void TwitterShare() {
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", shareTexture, "twi");
	}


	public void ShareMail() {
		AndroidSocialGate.SendMail("Hello Share Intent", "This is my text to share", "My E-mail Subject", "mail1@gmail.com, mail2@gmail.com", shareTexture);
	}


	public void InstaShare() {
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", shareTexture, "insta");
	}

	public void GoogleShare() {
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", shareTexture, "com.google.android.apps.plus");
	}


	public void ShareFB() {
		StartCoroutine(PostFBScreenshot());
	}



	private IEnumerator PostScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", tex);
		
		Destroy(tex);
		
	}

	private IEnumerator PostFBScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		AndroidSocialGate.StartShareIntent("Hello Share Intent", "This is my text to share", tex,  "facebook.katana");
		
		Destroy(tex);
		
	}
}
