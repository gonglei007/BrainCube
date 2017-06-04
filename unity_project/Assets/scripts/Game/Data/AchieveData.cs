using UnityEngine;
using System.Collections;

public class AchieveData{
	public static int[][] HighScores = new int[(int)GameSystem.Mode.Count][];

	public static void init(){
		HighScores[(int)GameSystem.Mode.Classic] = new int[]{100, 500, 30};
		HighScores[(int)GameSystem.Mode.Inverse] = new int[]{100, 500, 20};
		HighScores[(int)GameSystem.Mode.Survival] = new int[]{120, 500, 30};
		HighScores[(int)GameSystem.Mode.TimeRush] = new int[]{100, 500, 20};
		HighScores[(int)GameSystem.Mode.ColorFull] = new int[]{80, 500, 20};
		HighScores[(int)GameSystem.Mode.BlackJack] = new int[]{60, 500, 20};
		HighScores[(int)GameSystem.Mode.Word] = new int[]{60, 500, 20};
		HighScores[(int)GameSystem.Mode.Chimp] = new int[]{80, 500, 10};
		HighScores[(int)GameSystem.Mode.Dual] = new int[]{50, 500, 20};
	}

	/**
	 * (ATAN((POWER(A2;1.4)- I2/2) / (I2/4)) + 1.57) / 3.14 * 110 - 10
	 */
	public static int computePercentByScore(GameSystem.Mode mode, GameSystem.ModeType modeType, int score){
		Debug.Log ("HighScore:" + HighScores.ToString());
		float param = (float)HighScores[(int)mode][(int)modeType];
		Debug.Log ("Param:" + param.ToString());
		int percent = (int)((Mathf.Atan((Mathf.Pow((float)score, 1.1f) - (param/2.0f)) / (param / 4.0f)) + Mathf.PI / 2.0f) / Mathf.PI * 110.0f - 10.0f);
		return percent;
	}
}
