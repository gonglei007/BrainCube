using UnityEngine;
using System.Collections;

public class GameLogSystem {
	
	private static string	LogTagCreateBall = "crate_ball";
	private static string	LogTagDestroyBall = "destroy_ball";
	private static string	LogTagMoney = "money";
	private static string	LogTagTriggerSlotMachine = "trigger_slot_machine";

	public enum BallSource{
		Throw,
		Bonus,
		Item_DivisionBall,
	}
	
	public enum BallTarget{
		Destroy,
		Money,
	}
	
	static public void CreateBallLog(GameLogSystem.BallSource ballSource, int bet){
		LogSystem.GameLog(GameLogSystem.LogTagCreateBall, ballSource, bet);
	}
	
	static public void DestroyBallLog(int bet, BallTarget target){
		LogSystem.GameLog(GameLogSystem.LogTagDestroyBall, target, bet);
	}
	
	static public void MoneyLog(int money){
		LogSystem.GameLog(GameLogSystem.LogTagMoney, money);
	}
	
	static public void TriggerSlotMachineLog(){
		LogSystem.GameLog(GameLogSystem.LogTagTriggerSlotMachine);
	}
	
	static public void Flush(){
		LogSystem.FlushLog();
	}
}
