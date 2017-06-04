using UnityEngine;
using System.Collections;

public class GP_Event {


	public string Id;
	public string Description;

	public string IconImageUrl;
	public string FormattedValue;
	
	public long Value;
	
	private Texture2D _icon = null;




	public void LoadIcon() {
		if(icon != null) {
			return;
		}

		
		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.OnLoad += OnTextureLoaded;
		loader.LoadTexture(IconImageUrl);
	}



	public Texture2D icon {
		get {
			return _icon;
		}
	}



	private void OnTextureLoaded (Texture2D tex) {
		_icon = tex;
	}
}
