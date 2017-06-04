using UnityEngine;
using System.Collections;


public class AndroidImagePickResult : AndroidActivityResult {


	private Texture2D _image = null;



	public AndroidImagePickResult(string codeString, string ImageData) : base("0", codeString) {



		if(ImageData.Length > 0) {
			byte[] decodedFromBase64 = System.Convert.FromBase64String(ImageData);
			_image = new Texture2D(1, 1, TextureFormat.DXT5, false);
			_image.LoadImage(decodedFromBase64);
		}
			


	}
	
	
	
	public Texture2D image {
		get {
			return _image;
		}
	}




}
