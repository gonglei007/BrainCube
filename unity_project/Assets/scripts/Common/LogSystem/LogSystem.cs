using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class LogSystem : MonoBehaviour {
	
	private const int				logBufferSize = 1024*20;
	private static StringBuilder	logDataBuffer = new StringBuilder(logBufferSize);

	public static void GameLog(string tag){
		string	logString = string.Format("{0}\t{1}\n", TimeUtility.GetTimestamp(), tag);
		LogOut(logString);
	}
	
	public static void GameLog<T>(string tag, T t){
		string	logString = string.Format("{0}\t{1}\t{2}\n", TimeUtility.GetTimestamp(), tag, t);
		LogOut(logString);
	}
	
	public static void GameLog<T, U>(string tag, T t, U u){
		string	logString = string.Format("{0}\t{1}\t{2}\t{3}\n", TimeUtility.GetTimestamp(), tag, t, u);
		LogOut(logString);
	}
	
	public static void GameLog<T, U, V>(string tag, T t, U u, V v){
		string	logString = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", TimeUtility.GetTimestamp(), tag, t, u, v);
		LogOut(logString);
	}
	
	public static void GameLog<T, U, V, W>(string tag, T t, U u, V v, W w){
		string	logString = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", TimeUtility.GetTimestamp(), tag, t, u, v, w);
		LogOut(logString);
	}
	
	public static void FlushLog(){
		if(logDataBuffer.Length == 0){
			return;
		}
		
		string	logDir = Application.persistentDataPath + "/log";
		if(!Directory.Exists(logDir)){
			Directory.CreateDirectory(logDir);
		}

		string	logFile = string.Format("{0}/gamelog_{1}_{2}.log", logDir, NetworkUtility.GetMacAddress(), DateTime.Now.ToString("yyyyMMddHH"));
		if(!File.Exists(logFile)){
			File.Create(logFile);
		}
		using (FileStream	stream = new FileStream(logFile, FileMode.Append)) {
			if(stream != null){
				byte[]	logData = System.Text.Encoding.Default.GetBytes(logDataBuffer.ToString());
				stream.Write(logData, 0, logData.Length);
				stream.Flush();
				logDataBuffer.Length = 0;
				logDataBuffer.Capacity = logBufferSize;
			}
			stream.Close();
		}
	}
	
	private static void LogOut(string logString){
		logDataBuffer.Append(logString);
		if(logDataBuffer.Length >= logBufferSize - 1){
			FlushLog();
		}
	}
	
}
