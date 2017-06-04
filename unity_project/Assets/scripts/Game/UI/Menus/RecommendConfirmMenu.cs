using UnityEngine;
using System.Collections;

public class RecommendConfirmMenu : AutoConfirmMenu {

	public GameObject[] recommendContents;

	private GameSystem.Mode recommendMode;
	
	void Awake()
	{
		GameSystem.GetInstance().gameUI.recommendConfirmMenu = this;
	}

	public override void Show (bool active)
	{
		if (active)
		{
			GameSystem.GetInstance().gameUI.confirmMenu.Show(false);
		}
		base.Show (active);
	}

	public void SetRecommendMode(GameSystem.Mode mode)
	{
		recommendMode = mode;
	}
	
	protected override int CustomContentHeight ()
	{
		return 340;
	}
	
	protected override void CustomContent ()
	{
		foreach(GameObject modeContent in recommendContents)
		{
			modeContent.SetActive(false);
		}
		
		recommendContents[(int)recommendMode].SetActive(true);
	}
}
