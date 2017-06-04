using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CountDownMenu : BaseMenu {
	public UILabel				modeDescLabel;
	public UILabel				modeTypeDescLabel;
	public GameObject[]			countDownItems;
	public TweenAlphaAdvance[] 	countDown1AnimScripts;
	public TweenAlphaAdvance[] 	countDown2AnimScripts;
	public TweenAlphaAdvance[] 	countDown3AnimScripts;

	private List<TweenAlphaAdvance[]> countDownAnimScripts = new List<TweenAlphaAdvance[]>(3);

	private bool		animEnable				= false;
	private bool		isCountDownAnimFinished = false;
	private float		timer					= 0;
	private float		countDownInterval		= 1;
	private int			itemIndex				= -1;

	public bool IsCountDownAnimFinished
	{
		get
		{
			return isCountDownAnimFinished;
		}
		set
		{
			isCountDownAnimFinished = value;
		}
	}
	
	void Awake()
	{
		GameSystem.GetInstance().gameUI.countDownMenu = this;
		this.gameObject.SetActive(false);
		countDownAnimScripts.Add(countDown1AnimScripts);
		countDownAnimScripts.Add(countDown2AnimScripts);
		countDownAnimScripts.Add(countDown3AnimScripts);
	}
	
	// Update is called once per frame
	void Update () {
		if (animEnable)
		{
			timer += Time.deltaTime;
			if(timer > countDownInterval)
			{
				timer = 0;
				if (itemIndex == countDownItems.Length)
				{
					isCountDownAnimFinished = true;
					animEnable = false;
				}
				else
				{
					ShowCountDownSprite(itemIndex);
					itemIndex++;
				}
			}
		}
	}

	public void StarCountDownAnim()
	{
		modeDescLabel.text = TextManager.GetText(string.Format("mode_desc_{0}", (int)GameSystem.GetInstance().CurrentMode));
		modeTypeDescLabel.text = TextManager.GetText(string.Format("mode_type_desc_{0}", (int)GameSystem.GetInstance().CurrentModeType));
		timer = 0;
		itemIndex = 0;
		animEnable = true;
		isCountDownAnimFinished = false;
		ShowCountDownSprite(itemIndex);
		itemIndex++;
	}
	
	public void ShowCountDownSprite(int index){
		foreach(GameObject countDownItem in countDownItems){
			countDownItem.SetActive(false);
		}
		if(index >= 0 && index < countDownItems.Length){
			GameSoundSystem.GetInstance().PlayCountDownSound(index);
			countDownItems[index].SetActive(true);
			foreach(TweenAlphaAdvance animScript in countDownAnimScripts[index])
			{
				animScript.ResetToBeginning();
				animScript.PlayForward();
			}
		}
	}
}
