using UnityEngine;
using System.Collections;

public class SA_Texture : MonoBehaviour {


	void Awake () {
		GetComponent<Renderer>().material =new Material(GetComponent<Renderer>().material);
	}
	
	public Texture  texture {
		get {
			return GetComponent<Renderer>().material.mainTexture;
		}

		set {
			GetComponent<Renderer>().material.mainTexture = value;
		}
	}
}
