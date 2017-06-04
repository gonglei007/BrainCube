using UnityEngine;
using System;
using System.Collections;

public class AndroidCamera : SA_Singleton<AndroidCamera>  {

	//Actions
	public Action<AndroidImagePickResult> OnImagePicked = delegate{};
	public Action<GallerySaveResult> OnImageSaved = delegate{};
	
	//Events
	public const string  IMAGE_PICKED = "image_picked";
	public const string  IMAGE_SAVED = "image_saved";



	private static string _lastImageName = string.Empty;

	void Awake() {
		DontDestroyOnLoad(gameObject);

		int mode = (int) AndroidNativeSettings.Instance.CameraCaptureMode;
		AndroidNative.InitCameraAPI(AndroidNativeSettings.Instance.GalleryFolderName, AndroidNativeSettings.Instance.MaxImageLoadSize, mode);
	}


	[Obsolete("SaveImageToGalalry is deprecated, please use SaveImageToGallery instead.")]
	public void SaveImageToGalalry(Texture2D image, String name = "Screenshot") {
		SaveImageToGallery(image, name);
	}

	public void SaveImageToGallery(Texture2D image, String name = "Screenshot") {
		if(image != null) {
			byte[] val = image.EncodeToPNG();
			string mdeia = System.Convert.ToBase64String (val);
			AndroidNative.SaveToGalalry(mdeia, name);
		}  else {
			Debug.LogWarning("AndroidCamera::SaveToGalalry:  image is null");
		}
	}




	public void SaveScreenshotToGallery(String name = "") {
		_lastImageName = name;
		SA_ScreenShotMaker.instance.OnScreenshotReady += OnScreenshotReady;
		SA_ScreenShotMaker.instance.GetScreenshot();
	}


	public void GetImageFromGallery() {
		AndroidNative.GetImageFromGallery();
	}
	
	
	
	public void GetImageFromCamera() {
		AndroidNative.GetImageFromCamera();
	}




	private void OnImagePickedEvent(string data) {

		Debug.Log("OnImagePickedEvent");
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		AndroidImagePickResult result =  new AndroidImagePickResult(storeData[0], storeData[1]);

		dispatch(IMAGE_PICKED, result);
		if(OnImagePicked != null) {
			OnImagePicked(result);
		}

	}

	private void OnImageSavedEvent(string data) {
		GallerySaveResult res =  new GallerySaveResult(data, true);

		OnImageSaved(res);
		dispatch(IMAGE_SAVED, res);
	}

	private void OnImageSaveFailedEvent(string data) {
		GallerySaveResult res =  new GallerySaveResult("", false);
		
		OnImageSaved(res);
		dispatch(IMAGE_SAVED, res);
	}



	private void OnScreenshotReady(Texture2D tex) {
		SA_ScreenShotMaker.instance.OnScreenshotReady -= OnScreenshotReady;
		SaveImageToGallery(tex, _lastImageName);

	}
}
