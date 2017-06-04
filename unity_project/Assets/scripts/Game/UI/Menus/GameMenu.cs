using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMenu : BaseMenu {

	public UILabel				scoreLabel;
	public UILabel				waveNumberLabel;
	public UILabel				modeLabel;
	public UILabel				modeTypeLabel;
	public UILabel				bestLabel;

	public GameObject[]			modeContents;

	public UISlider				hpSlider;
	public UILabel				hpLabel;

	public UISlider				lifeTimeSlider;
	public UILabel				lifeTimeLabel;

	public UISprite[]			rightColorSprites;
	public UIGrid				rightColorGrid;

	public UITable				cardTable;	
	public UISprite[]			cardArray;

	public UILabel				wordLabel;
	public UILabel				translationLabel;

	public TweenScale			leftSpriteOutlineTweener1;
	public TweenRotation		leftSpriteOutlineTweener2;
	public TweenScale			rightSpriteOutlineTweener1;
	public TweenRotation		rightSpriteOutlineTweener2;
	public UISlider				dualLeftSideSlider;
	public UILabel				dualLeftHPLabel;
	public UISlider				dualRightSideSlider;
	public UILabel				dualRightHPLabel;

	public UISlider				musicBoxPowerSlider;
	
	void Awake()
	{
		GameSystem.GetInstance().gameUI.gameMenu = this;
		this.gameObject.SetActive(false);
	}
	
	// Use this for initialization
	void Start () {
		GameSystem.GetInstance().OnWaveNumberChanged += HandleOnWaveNumberChanged;
		GameSystem.GetInstance().OnScoreChanged += HandleOnScoreChanged;
		SurvivalMode.GetInstance().OnHPChanged += HandleOnHPChanged;
		TimeRushMode.GetInstance().OnLifeTimeChanged += HandleOnLifeTimeChanged;
		ColorFullMode.GetInstance().OnColorChanged += HandleOnColorChanged;
		BlackJackMode.GetInstance().OnSelectCardChanged += HandleOnSelectCardChanged;
		DualMode.GetInstance().OnTurnChanged += HandleOnTurnChanged;
		DualMode.GetInstance().OnLeftHPChanged += HandleOnLeftHPChanged;
		DualMode.GetInstance().OnRightHPChanged += HandleOnRightHPChanged;
		WordMode.GetInstance().OnWordTextChanged += HandleOnWordTextChanged;
		translationLabel.gameObject.SetActive(LocalVersion.local == LocalVersion.Local.CN_ZH);

		dualLeftSideSlider.foregroundWidget.color = Constant.LEFT_COLOR;
		dualRightSideSlider.foregroundWidget.color = Constant.RIGHT_COLOR;
	}

	void Update()
	{
		musicBoxPowerSlider.value = GameSoundSystem.GetInstance ().MusicBox.GetPowerPercent ();
	}
	
	public override void Show(bool active)
	{
		base.Show(active);
		if (active)
		{
			waveNumberLabel.text 	= string.Format(TextManager.GetText("wave"), GameSystem.GetInstance().DisplayWaveNumber);
			scoreLabel.text 		= string.Format(TextManager.GetText("game_score"), GameSystem.GetInstance().Score);
			modeLabel.text			= TextManager.GetText(string.Format("mode_name_{0}", (int)GameSystem.GetInstance().CurrentMode));
			modeTypeLabel.text		= string.Format("({0})", TextManager.GetText(string.Format("mode_type_name_{0}", (int)GameSystem.GetInstance().CurrentModeType)));
			bestLabel.text			= string.Format(TextManager.GetText("best_score"), PlayerProfile.LoadBestRecord(GameSystem.GetInstance().CurrentMode, GameSystem.GetInstance().CurrentModeType));
			hpLabel.text 			= SurvivalMode.GetInstance().HP.ToString();
			lifeTimeLabel.text 		= string.Format("{0:F1}s", TimeRushMode.GetInstance().LifeTime);
			wordLabel.text 			= WordMode.GetInstance().WordText;
			foreach(UISprite cardSprite in cardArray)
			{
				cardSprite.gameObject.SetActive(false);
			}

			for(int i = 0; i < modeContents.Length ; i++)
			{
				modeContents[i].SetActive(i == (int)GameSystem.GetInstance().CurrentMode);
			}
		}
	}

	public void PauseButtonOnClick()
	{
		GameSystem.GetInstance().gameUI.pauseMenu.Show(true);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	public void HelpButtonOnClick()
	{
		string title = TextManager.GetText("help");
		string content = TextManager.GetText(string.Format("mode_help_{0}", (int)(GameSystem.GetInstance().CurrentMode)));
		GameSystem.GetInstance().gameUI.confirmMenu.SetContent(title, content, ConfirmStyle.OnlyYes);
		GameSystem.GetInstance().gameUI.confirmMenu.Show(true);
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	void HandleOnWaveNumberChanged(int waveNumber)
	{
		waveNumberLabel.text = waveNumber.ToString();
	}

	void HandleOnScoreChanged(int score)
	{
		scoreLabel.text = string.Format(TextManager.GetText("game_score"), score);
	}
	
	void HandleOnHPChanged(int hp)
	{
		hpSlider.value = hp * 1.0f/ Constant.HP_MAX;
		hpLabel.text = hp.ToString();
	}
	
	void HandleOnLifeTimeChanged(float lifeTime)
	{
		lifeTimeSlider.value = lifeTime / Constant.LIFE_TIME_MAX;
		lifeTimeLabel.text = string.Format("{0:F1}s", lifeTime);
	}

	void HandleOnColorChanged()
	{
		for(int i = 0 ; i < rightColorSprites.Length ; i++)
		{
			if (i < ColorFullMode.GetInstance().RightColors.Count)
			{
				rightColorSprites[i].color = ColorFullMode.GetInstance().RightColors[i];
				rightColorSprites[i].gameObject.SetActive(true);
			}
			else
			{
				rightColorSprites[i].gameObject.SetActive(false);
			}
		}
		rightColorGrid.Reposition();
	}

	void HandleOnSelectCardChanged()
	{
		int index = 0;
		foreach(UISprite cardSprite in cardArray)
		{
			if(index < BlackJackMode.GetInstance().SelectedCardNames.Count)
			{
				cardSprite.spriteName = BlackJackMode.GetInstance().SelectedCardNames[index];
				cardSprite.gameObject.SetActive(true);
			}
			else
			{
				cardSprite.gameObject.SetActive(false);
			}
			index++;
		}
		cardTable.Reposition();
	}

	void HandleOnTurnChanged(bool isLeftTurn)
	{
		leftSpriteOutlineTweener1.enabled = isLeftTurn;
		leftSpriteOutlineTweener2.enabled = isLeftTurn;
		
		rightSpriteOutlineTweener1.enabled = !isLeftTurn;
		rightSpriteOutlineTweener2.enabled = !isLeftTurn;
		if (isLeftTurn)
		{
			rightSpriteOutlineTweener1.ResetToBeginning();
			rightSpriteOutlineTweener2.ResetToBeginning();
			rightSpriteOutlineTweener1.transform.localScale = Vector3.one;
			rightSpriteOutlineTweener1.transform.localRotation = Quaternion.identity;
		}
		else
		{
			leftSpriteOutlineTweener1.ResetToBeginning();
			leftSpriteOutlineTweener2.ResetToBeginning();
			leftSpriteOutlineTweener1.transform.localScale = Vector3.one;
			leftSpriteOutlineTweener1.transform.localRotation = Quaternion.identity;
		}
	}

	void HandleOnLeftHPChanged(int leftHp)
	{
		dualLeftHPLabel.text = leftHp.ToString();
		dualLeftSideSlider.value = leftHp * 1.0f / Constant.DUAL_HP_MAX;
	}
	
	void HandleOnRightHPChanged(int rightHp)
	{
		dualRightHPLabel.text = rightHp.ToString();
		dualRightSideSlider.value = rightHp * 1.0f / Constant.DUAL_HP_MAX;
	}

	void HandleOnWordTextChanged()
	{
		wordLabel.text = WordMode.GetInstance().WordText;
		translationLabel.text = string.Format("({0})", WordMode.GetInstance().WordTranslation);
	}
}
