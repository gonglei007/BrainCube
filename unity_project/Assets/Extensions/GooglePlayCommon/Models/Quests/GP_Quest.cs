using UnityEngine;
using System.Collections;

public class GP_Quest {

	public string Id;
	public string Name;
	public string Description;

	public string IconImageUrl;
	public string BannerImageUrl;

	public GP_QuestState state;

	public long LastUpdatedTimestamp;
	public long AcceptedTimestamp;
	public long EndTimestamp;

	private Texture2D _icon = null;
	private Texture2D _banner = null;


	public void LoadIcon() {
		if(icon != null) {
			return;
		}
		
		
		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.OnLoad += OnIconLoaded;
		loader.LoadTexture(IconImageUrl);
	}

	public void LoadBanner() {
		if(icon != null) {
			return;
		}
		
		
		WWWTextureLoader loader = WWWTextureLoader.Create();
		loader.OnLoad += OnBannerLoaded;
		loader.LoadTexture(BannerImageUrl);
	}
	
	
	
	public Texture2D icon {
		get {
			return _icon;
		}
	}
	

	
	public Texture2D banner {
		get {
			return _banner;
		}
	}
	
	

	private void OnBannerLoaded (Texture2D tex) {
		_banner = tex;
	}
	


	private void OnIconLoaded (Texture2D tex) {
		_icon = tex;
	}
}
