using UnityEngine;
using System;
using System.Collections;

public class LeaderboardManager {
	private static string[][] LeaderboardIDs = new string[(int)GameSystem.Mode.Count][];
	public static MonoBehaviour	LeaderboardTarget = null;
	public static KTPlayLeaderboard.Callback	LeaderboardCallback = null;


	public static void Init(){
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Classic] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Classic][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_classic_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Classic][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_classic_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Classic][(int)GameSystem.ModeType.Hell] 		= "com_gltop_findblock_stable_classic_hell";
		
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Inverse] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Inverse][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_inverse_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Inverse][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_inverse_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Inverse][(int)GameSystem.ModeType.Hell] 		= "com_gltop_findblock_stable_inverse_hell";
		
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Survival] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Survival][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_survival_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Survival][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_survival_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Survival][(int)GameSystem.ModeType.Hell] 	 = "com_gltop_findblock_stable_survival_hell";
		
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.TimeRush] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.TimeRush][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_timer_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.TimeRush][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_timer_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.TimeRush][(int)GameSystem.ModeType.Hell] 	 = "com_gltop_findblock_stable_timer_hell";
		
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.ColorFull] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.ColorFull][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_colorful_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.ColorFull][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_colorful_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.ColorFull][(int)GameSystem.ModeType.Hell] 	  = "com_gltop_findblock_stable_colorful_hell";
		
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.BlackJack] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.BlackJack][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_blackjack_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.BlackJack][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_blackjack_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.BlackJack][(int)GameSystem.ModeType.Hell] 	  = "com_gltop_findblock_stable_blackjack_hell";

		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Word] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Word][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_word_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Word][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_word_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Word][(int)GameSystem.ModeType.Hell] 	 = "com_gltop_findblock_stable_word_hell";

		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Chimp] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Chimp][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_chimp_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Chimp][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_chimp_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Chimp][(int)GameSystem.ModeType.Hell] 	 = "com_gltop_findblock_stable_chimp_hell";
		
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Dual] = new string[(int)GameSystem.ModeType.Count];
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Dual][(int)GameSystem.ModeType.Challenge] = "com_gltop_findblock_stable_dual_challenge";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Dual][(int)GameSystem.ModeType.PassLevel] = "com_gltop_findblock_stable_dual_rush";
		LeaderboardManager.LeaderboardIDs[(int)GameSystem.Mode.Dual][(int)GameSystem.ModeType.Hell] 	 = "com_gltop_findblock_stable_dual_hell";
	}

	public static void ReportScore(GameSystem.Mode mode, GameSystem.ModeType modeType, int score){
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			if(KTAccountManager.IsLoggedIn())
			{
				string leaderboardId = LeaderboardIDs[(int)mode][(int)modeType];
				KTPlayLeaderboard.ReportScore(score, leaderboardId, LeaderboardTarget, LeaderboardCallback);
			}
		}
	}

	public static void RequestGameLeaderboard(GameSystem.Mode mode, GameSystem.ModeType modeType, MonoBehaviour target, KTPlayLeaderboard.Callback callback, int startIndex = 0){
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			string leaderboardId = LeaderboardIDs[(int)mode][(int)modeType];
			Debug.Log ("request leaderboard ID of game:" + leaderboardId);
			KTPlayLeaderboard.GlobalLeaderboard(leaderboardId, startIndex, Constant.TOP_RANK_COUNT, target, callback);
		}
	}
}
