//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This script is mainly same with the NGUI UIRootF
/// We modify it to Fix a bug : On some device, the Mathf.Approximately in UIRoot return the wrong value
/// </summary>

[ExecuteInEditMode]
public class UIRootModify : MonoBehaviour
{
	public int manualHeight = 960;
	private int cachedManualHeight = 960;
	
	void Start ()
	{
		UIOrthoCamera oc = GetComponentInChildren<UIOrthoCamera>();
		
		if (oc != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", oc);
			Camera cam = oc.gameObject.GetComponent<Camera>();
			oc.enabled = false;
			if (cam != null) cam.orthographicSize = 1f;
		}
		
		cachedManualHeight = manualHeight;
		UpdtaeCameraTransform();
	}
	
	void Update() {
		if (manualHeight > 0) {
			if (manualHeight != cachedManualHeight) {
				UpdtaeCameraTransform();
				cachedManualHeight = manualHeight;
			}
		}
	}
	
	private void UpdtaeCameraTransform() {
		float size = 2f / manualHeight;
		transform.localScale = new Vector3(size, size, size);
	}
}