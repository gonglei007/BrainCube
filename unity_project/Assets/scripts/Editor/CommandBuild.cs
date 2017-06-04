/*
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -quit \
  -projectPath $PROJECT_PATH \
  -executeMethod CommandBuild.BuildiOS
*/
// Assets/Editor/CommandBuile.cs
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CommandBuild
{
	private static string   APP_FILE_NAME        	= "shikongchuanshuo-unity";
	private static string   XCODE_PROJECT_FOLDER 	= "iOS_Dev_Temp";
	
	private static string   STRING_WIN     			= "win";
	private static string   STRING_ANDROID 			= "android";
	private static string   STRING_IOS     			= "ios";
	private static string   STRING_EXE     			= "exe";
	private static string   STRING_APK     			= "apk";
	private static string   STRING_APP     			= "app";
	
	private static string   ANDROID_KEYSTORE 		= "gltop123";
	private static string   ANDROID_ALIAS    		= "gltop123";
	
	private static string[] scenes = new string []{
		"Assets/Scenes/MainScene.unity",
		"Assets/Scenes/BlockConfirmMenuScene.unity",
		"Assets/Scenes/ConfirmMenuScene.unity",
		"Assets/Scenes/CountDownMenuScene.unity",
		"Assets/Scenes/DailyRewardMenuScene.unity",
		"Assets/Scenes/GameMenuScene.unity",
		"Assets/Scenes/GiftMenuScene.unity",
		"Assets/Scenes/MainMenuScene.unity",
		"Assets/Scenes/PauseMenuScene.unity",
		"Assets/Scenes/RankMenuScene.unity",
		"Assets/Scenes/RecommendConfirmMenuScene.unity",
		"Assets/Scenes/RescueConfirmMenuScene.unity",
		"Assets/Scenes/ResultMenuScene.unity",
		"Assets/Scenes/ShopMenuScene.unity",
		"Assets/Scenes/VipConfirmMenuScene.unity",
	};
	
	[MenuItem("Build/Build Windows APP")]
	public static void BuildWinApp(){
		string winAppPath = CreateAppOutputDir(STRING_WIN, STRING_EXE);
		
		if (PreCompile.BuildVersion == SystemUtility.BuildVersion.DEBUG) {
			BuildPipeline.BuildPlayer(scenes, winAppPath, BuildTarget.StandaloneWindows, BuildOptions.ConnectWithProfiler | BuildOptions.Development | BuildOptions.AllowDebugging); 
		}
		else{
			BuildPipeline.BuildPlayer(scenes, winAppPath, BuildTarget.StandaloneWindows, BuildOptions.None); 
		}
	}
	
	[MenuItem("Build/Build Unity Android APP")]
	public static void BuildUnityAndroidApp(){
		string androidAppPath = CreateAppOutputDir(STRING_ANDROID, STRING_APK);
		
		PlayerSettings.keystorePass = ANDROID_KEYSTORE;
		PlayerSettings.keyaliasPass = ANDROID_ALIAS;
		if (PreCompile.BuildVersion == SystemUtility.BuildVersion.DEBUG) {
			BuildPipeline.BuildPlayer(scenes, androidAppPath, BuildTarget.Android, BuildOptions.ConnectWithProfiler | BuildOptions.Development | BuildOptions.AllowDebugging); 
		}
		else{
			BuildPipeline.BuildPlayer(scenes, androidAppPath, BuildTarget.Android, BuildOptions.None); 
		}
	}
	
	[MenuItem("Build/Build Google Android Project")]
	public static void BuildGoogleAndroidProject(){
		string androidProjectDir = CreateProjectOutputDir(STRING_ANDROID);
		
		PlayerSettings.keystorePass = ANDROID_KEYSTORE;
		PlayerSettings.keyaliasPass = ANDROID_ALIAS;
		if (PreCompile.BuildVersion == SystemUtility.BuildVersion.DEBUG) {
			BuildPipeline.BuildPlayer(scenes, androidProjectDir, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.Development | BuildOptions.AllowDebugging); 
		}
		else{
			BuildPipeline.BuildPlayer(scenes, androidProjectDir, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer); 
		}
	}
	
	[MenuItem("Build/Build iOS APP")]
	public static void BuildiOSApp()
	{
		string xCodeProjectDir = CreateProjectOutputDir(STRING_IOS);		
		
		if (PreCompile.BuildVersion == SystemUtility.BuildVersion.DEBUG) {
			BuildPipeline.BuildPlayer(scenes, xCodeProjectDir, BuildTarget.iPhone, BuildOptions.ConnectWithProfiler | BuildOptions.Development | BuildOptions.AllowDebugging); 
		}
		else{
			BuildPipeline.BuildPlayer(scenes, xCodeProjectDir, BuildTarget.iPhone, BuildOptions.None); 
		}
	}
	
	[MenuItem("Build/Extract Loc Key In Scene")]
	public static void ExtractLocTextInScene()
	{
		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		GameObject root = null;
		
		foreach(GameObject gameObject in allObjects)
		{
			if (gameObject.name.Equals("UI Root (2D)"))
			{
				root = gameObject;
				break;
			}
		}
		
		StringBuilder sb = new StringBuilder();
		List<string> keyList = new List<string>();
		
		if (root != null)
		{
			NGUITools.SetActive(root, true);
			UILabelLocalization[] labelLocScripts = root.GetComponentsInChildren<UILabelLocalization>();
			foreach(UILabelLocalization locScript in labelLocScripts)
			{
				if (!keyList.Contains(locScript.localizeKey))
				{
					keyList.Add(locScript.localizeKey);
					sb.AppendLine(locScript.localizeKey);
				}
			}
		}
		string allKeys = sb.ToString();
		string outputFile = Application.dataPath + "/../../output/localization/LocKeyInScene.txt";
		StreamWriter sw = new StreamWriter(outputFile);
		sw.Write(allKeys);
		sw.Flush();
		sw.Close();
		
		NGUITools.SetActive(root, false);
		foreach(GameObject go in allObjects)
		{
			go.SetActive(true);
		}
	}
	
	private static string CreateProjectOutputDir(string platform)
	{
		string outputFolder = Application.dataPath + string.Format("/../../output/{0}/", platform);
		if (platform.Equals(STRING_IOS))
		{
			outputFolder += XCODE_PROJECT_FOLDER;
		}
		try
		{
			string finalDir = string.Format("{0}/{1}", outputFolder, PlayerSettings.productName);
			if (Directory.Exists(finalDir))
			{
				Directory.Delete(finalDir, true);
			}
			if (Directory.Exists(outputFolder) == false)
			{
				Directory.CreateDirectory(outputFolder);
			}
		}
		catch(IOException e)
		{
			Debug.Log(e.Message);
		}
		return outputFolder;
	}
	
	private static string CreateAppOutputDir(string platform, string extension){
		string outputFolder = Application.dataPath + string.Format("/../../output/{0}/", platform);
		string outputAppFolder = outputFolder + STRING_APP + "/";
		string outputAppPath = outputAppFolder + string.Format("{0}.{1}", APP_FILE_NAME, extension);
		try
		{
			if (Directory.Exists(outputAppFolder))
			{
				Directory.Delete(outputAppFolder, true);
			}
			Directory.CreateDirectory(outputAppFolder);
		}
		catch(IOException e)
		{
			Debug.Log(e.Message);
		}
		return outputAppPath;
	}
}