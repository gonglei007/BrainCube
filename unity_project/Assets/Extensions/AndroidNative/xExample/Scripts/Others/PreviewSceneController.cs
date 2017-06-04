using UnityEngine;
using System.Collections;

public class PreviewSceneController : MonoBehaviour {

	public SA_Label title;

	void Awake() {
		title.text = "Android Native Unity3d Plugin (" +  AndroidNativeSettings.VERSION_NUMBER + ")";
	}


	public void SendMail() {
		AndroidSocialGate.SendMail("Send Mail", "", "Android Native Plugin Question", "stans.assets@gmail.com");
	}

	public void SendBug() {
		AndroidSocialGate.SendMail("Send Mail", "", "Hey Stan, I found a bug", "stans.assets@gmail.com");
	}


	public void OpenDocs() {
		string url = "http://goo.gl/pTcIR8";
		Application.OpenURL(url);
	}

	public void OpenAssetStore() {
		string url = "http://goo.gl/g8LWlC";
		Application.OpenURL(url);
	}


	public void MorePlugins() {
		string url = "http://goo.gl/MgEirV";
		Application.OpenURL(url);
	}


}
