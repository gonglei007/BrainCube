using UnityEngine;
using System.Collections;

public class ResultMenu : BaseMenu {
	public UILabel			modeNameLabel;		//模式名称
	public UILabel			modeTypeNameLabel;	//模式类型
	public UILabel			scoreLabel;			//未破记录的总终分数
	public UILabel			achieveLabel;
	public UILabel			waveLabel;			//未破记录时的闯关结果
	public UILabel			bestRecordLabel;	//未破记录中的历史最佳
	public UILabel			newBestRecordLabel;	//破记录中的最终分数(在模式类型是闯关时显示为关卡进度)

	public GameObject		bonusAnimationObj;	//奖励白块的动画物体(根物体)
	public UILabel			bonusBlockLabel;	//奖励的白块数量
	public UILabel			totalBlockLabel;	//现有的白块总数量

	public GameObject		commonResultPanel;	//未破记录的内容
	public GameObject		passLevelElement;	//未破记录中闯关类型的内容
	public GameObject		nonePassLevelElement;//未破记录中非闯关类型的内容
	public GameObject		brokenRecordPanel;	//破记录的内容
	
	public GameObject[]		modeContents;

	public UISprite			dualWinnerSprite;

	private int 			popADScore 		= 60;
	private ADTrigger		adTrigger 		= new ADTrigger(1.0f, 30);

	void Awake()
	{
		GameSystem.GetInstance().gameUI.resultMenu = this;
		this.gameObject.SetActive(false);
	}

	public override void Show(bool active)
	{
		base.Show(active);
		if(active){
			int score = GameSystem.GetInstance().Score;
			int gainedCoin = GameSystem.GetInstance().CurrentModeLogic.ShouldGainBouns ? score / Constant.SCORE_TO_COIN : 0;
			if (GameSystem.GetInstance().IsVIP)
			{
				bonusBlockLabel.text = string.Format("+{0} x 2", gainedCoin);
			}
			else
			{
				bonusBlockLabel.text = string.Format("+{0}", gainedCoin);
			}
			totalBlockLabel.text = GameSystem.GetInstance().Coin.ToString();

			int bestRecord = PlayerProfile.LoadBestRecord(GameSystem.GetInstance().CurrentMode, GameSystem.GetInstance().CurrentModeType);
			modeNameLabel.text = TextManager.GetText(string.Format("mode_name_{0}", (int)GameSystem.GetInstance().CurrentMode));
			modeTypeNameLabel.text = string.Format("({0})", TextManager.GetText(string.Format("mode_type_name_{0}", (int)GameSystem.GetInstance().CurrentModeType)));

			if (GameSystem.GetInstance().CurrentModeType == GameSystem.ModeType.PassLevel)
			{
				scoreLabel.text = string.Format("{0}/{1}",  GameSystem.GetInstance().DisplayWaveNumber, Constant.MAX_PASSLEVEL);
				newBestRecordLabel.text = string.Format("{0}/{1}",  bestRecord, Constant.MAX_PASSLEVEL);
				//passLevelElement.SetActive(true);
				nonePassLevelElement.SetActive(false);
			}
			else
			{
				scoreLabel.text = score.ToString();
				bestRecordLabel.text = bestRecord.ToString();
				newBestRecordLabel.text = bestRecord.ToString();
				//passLevelElement.SetActive(false);
				nonePassLevelElement.SetActive(true);
			}

			if(GameSystem.GetInstance().BrokenRecord)
			{
				commonResultPanel.SetActive(false);
				brokenRecordPanel.SetActive(true);
			}
			else
			{
				commonResultPanel.SetActive(true);
				brokenRecordPanel.SetActive(false);
			}

			if(score >= popADScore)
			{
				adTrigger.ToShow (true);
			}

			if (GameSystem.GetInstance().IsActivated
			    && gainedCoin > 0 
			    && (GameSystem.GetInstance().BrokenRecord || GameSystem.GetInstance().CurrentModeType != GameSystem.ModeType.PassLevel))
			{
				PlayGetBonusAnimation();
			}
			else
			{
				bonusAnimationObj.SetActive(false);
			}
			
			for(int i = 0; i < modeContents.Length ; i++)
			{
				modeContents[i].SetActive(i == (int)GameSystem.GetInstance().CurrentMode);
			}
			
			if (GameSystem.GetInstance().CurrentMode == GameSystem.Mode.Dual)
			{
				dualWinnerSprite.color = DualMode.GetInstance().IsLeftWin ? Constant.LEFT_COLOR : Constant.RIGHT_COLOR;
			}
			int percent = AchieveData.computePercentByScore(GameSystem.GetInstance().CurrentMode, GameSystem.GetInstance().CurrentModeType, score);
			achieveLabel.text = string.Format(TextManager.GetText("achieve_desc"), percent);
		}
	}
	
	public void RestartButtonOnClick()
	{
		GameSystem.GetInstance().ChangeState(GameSystem.States.GameReady);
	}
	
	public void MainButtonOnClick()
	{
		StateGameResult.IS_EXIT_TO_MAIN = true;
		GameSystem.GetInstance().ChangeState(GameSystem.States.GameInit);
		GameSystem.GetInstance().gameUI.mainMenu.ShowEnterAnimation(()=>{
			this.Show(false);
		});
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void ShareButtonOnClick()
	{
		StartCoroutine(UMengManager.ShareScore(GameSystem.GetInstance().Score));
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	private void PlayGetBonusAnimation()
	{
		bonusAnimationObj.SetActive(true);
		bonusAnimationObj.transform.localPosition = new Vector3(0, 170, 0);
		bonusAnimationObj.transform.localScale = Vector3.one * 2;
		NGUIToolsExtend.SetAlphaRecursively(bonusAnimationObj, 1);

		TweenScale tweenScale = TweenScale.Begin(bonusAnimationObj, 0.3f, Vector3.one);
		tweenScale.method = UITweener.Method.EaseOutBack;
		EventDelegate.Add(tweenScale.onFinished, ()=>{
			TweenPosition.Begin(bonusAnimationObj, 1.5f, new Vector3(0, 300, 0)).method = UITweener.Method.Linear;
			TweenAlphaAdvance.Begin(bonusAnimationObj, 1.5f, 0).method = UITweener.Method.EaseIn;
		}, true);
	}
}
