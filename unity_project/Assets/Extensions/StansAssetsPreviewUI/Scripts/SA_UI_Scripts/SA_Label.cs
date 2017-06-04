using UnityEngine;
using System.Collections;

public class SA_Label : MonoBehaviour {

	public string text {
		get {

			if(gameObject == null) {
				return string.Empty;;
			}

			TextMesh mesh  = gameObject.GetComponentInChildren<TextMesh>();
			if(mesh != null) {
				return mesh.text;
			} else {
				return string.Empty;
			}

		}
		
		set {
			if(gameObject == null) {
				return;
			}

			TextMesh[] meshes  = gameObject.GetComponentsInChildren<TextMesh>();

			foreach(TextMesh mesh in meshes) {
				if(mesh != null) {
					mesh.text = value;
				}

			}
		}
	}
}
