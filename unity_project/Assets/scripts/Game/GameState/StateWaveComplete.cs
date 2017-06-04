using UnityEngine;
using System.Collections;

//All states should use the Serializable attribute if you want them to be visible in the inspector
[System.Serializable]
public class StateWaveComplete : FSMState<GameSystem, GameSystem.States>
{
	private float			timer = 0.0f;
	private float			actionDelay = 0.6f;
	private bool			isWaveExitAnimPlayed = false;
	private bool			isWaveFailAnimPlayed = false;

	private int				rescuePrice = 0;
	
	public override GameSystem.States StateID {
		get {
			return GameSystem.States.WaveComplete;
		}
	}

	public override void Enter ()
	{
		timer = 0.0f;
		isWaveExitAnimPlayed = false;
		isWaveFailAnimPlayed = false;
		entity.fullScreenBlock.enabled = true;
	}

	public override void Execute ()
	{
		timer += Time.deltaTime;

		if (timer > actionDelay)
		{
			if(entity.gameCore.IsLevelWavePassed){
				if (isWaveExitAnimPlayed == false){
					entity.gameCore.StarWaveExitAnim();
					isWaveExitAnimPlayed = true;
				}
			}
			else{
				if (isWaveFailAnimPlayed == false){
					rescuePrice = (int)(Mathf.Pow(Constant.RESCUE_COST_COEFF, GameSystem.GetInstance().RescuedTimes) * Constant.RESCUE_BASIC_PRICE);
					if (Config.enableRescue && entity.CurrentModeLogic.EnableRescue)
					{
						StartRescue();
					}
					else
					{
						entity.gameCore.StarWaveFailAnim();
					}
					isWaveFailAnimPlayed = true;
				}
			}
		}
		if(entity.gameCore.IsWaveExitAnimFinished){
			entity.gameCore.IsWaveExitAnimFinished = false;			
			entity.StartNextWave();
		}

		if(entity.gameCore.IsWaveFailAnimFinished)
		{
			entity.gameCore.IsWaveFailAnimFinished = false;
			entity.ChangeState(GameSystem.States.GameResult);
		}
	}

	public override void Exit ()
	{
		entity.gameUI.gameMenu.Show(false);
	}
	
	private void StartRescue()
	{
		rescuePrice = Constant.RESCUE_BASIC_PRICE;
#if UNITY_IOS
		if (GameSystem.GetInstance().Coin < rescuePrice)
		{
			GameSystem.GetInstance().gameUI.confirmMenu.SetConfirmCallback((result)=>{
				GameSystem.GetInstance().JumpToGameEnd();
			});
			string title = TextManager.GetText("not_enough_coin_title");
			string content = string.Format(TextManager.GetText("not_enough_coin_for_rescue"), Constant.RESCUE_BASIC_PRICE);
			GameSystem.GetInstance().gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.OnlyYes);
			GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
		}
		else
#endif
		{
			string rescueTitleText = TextManager.GetText("rescue_title");
			string rescueContentText = string.Format(TextManager.GetText("cost_coin"), rescuePrice);
			GameSystem.GetInstance().gameUI.rescueConfirmMenu.AutoCloseTime = Constant.RESCUE_TIME;
			GameSystem.GetInstance().gameUI.rescueConfirmMenu.SetConfirmCallback(ConfirmRescueCallback);
			GameSystem.GetInstance().gameUI.rescueConfirmMenu.SetContent(rescueTitleText, rescueContentText);
			GameSystem.GetInstance().gameUI.rescueConfirmMenu.Show(true);
		}
	}
	
	private void ConfirmRescueCallback(bool result)
	{
		if (result)
		{
			if (GameSystem.GetInstance().CheckIsActivated(ConfirmActiveGame, ActivateGameSucceed))
			{
				if (GameSystem.GetInstance().Coin >= rescuePrice)
				{
					GameSystem.GetInstance().Coin -= rescuePrice;
					GameSystem.GetInstance().CurrentModeLogic.Rescue();
					GameSystem.GetInstance().ChangeState(GameSystem.States.GamePreview);
					PlayerProfile.SaveCoin(GameSystem.GetInstance().Coin);

					UMengManager.Buy(UMengManager.Item_IAP[(int)IAPManager.IAPProduct.Rescue], 1, rescuePrice);
				}
				else
				{
					IAPManager.GetInstance().Pay(IAPManager.IAPProduct.Rescue);
				}
			}
		}
		else
		{
			entity.gameCore.StarWaveFailAnim();
		}
	}

	private void ConfirmActiveGame(bool result)
	{
		if (result == false)
		{
			entity.gameCore.StarWaveFailAnim();
		}
	}

	private void ActivateGameSucceed()
	{
		GameSystem.GetInstance().CurrentModeLogic.Rescue();
		GameSystem.GetInstance().ChangeState(GameSystem.States.GamePreview);
	}
}


