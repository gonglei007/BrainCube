using UnityEngine;
using System.Collections;

public class GallerySaveResult : AN_Result {

	private string _imagePath;

	public GallerySaveResult(string path, bool res):base(res) {
		_imagePath = path;
	}

	public string imagePath {
		get {
			return _imagePath;
		}
	}
}
