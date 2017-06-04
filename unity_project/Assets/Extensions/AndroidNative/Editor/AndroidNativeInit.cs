using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class AndroidNativeInit  {

	static AndroidNativeInit () {


		#if UNITY_ANDROID
		if(FileStaticAPI.IsFileExists("Extensions/UltimateMobile/Resources/UltimateMobileSettings.asset")) {
			return;
		}

		if(!AndroidNativeSettingsEditor.IsInstalled) {
			EditorApplication.update += OnEditorLoaded;
		} else {
			if(!AndroidNativeSettingsEditor.IsUpToDate) {
				EditorApplication.update += OnEditorLoaded;
			}
		}

		#endif
	}

	private static void OnEditorLoaded() {

		EditorApplication.update -= OnEditorLoaded;
		Debug.LogWarning("Android Native Plugin Install Required. Opening Plugin settings...");
		Selection.activeObject = AndroidNativeSettings.Instance;
	}

}
