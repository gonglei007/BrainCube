using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUI : MonoBehaviour {
	
	public BlockConfirmMenu			blockConfirmMenu;
	public AutoConfirmMenu			confirmMenu;
	public CountDownMenu			countDownMenu;
	public DailyRewardMenu			dailyRewardMenu;
	public GiftMenu					giftMenu;
	public GameMenu					gameMenu;
	public MainMenu					mainMenu;
	public PauseMenu				pauseMenu;
	public RankMenu					rankMenu;
	public RecommendConfirmMenu		recommendConfirmMenu;
	public RescueConfirmMenu		rescueConfirmMenu;
	public ResultMenu				resultMenu;
	public ShopMenu					shopMenu;
	public VIPConfirmMenu			vipConfirmMenu;

	public Camera					mainCamera;
	public UITexture				bluredTexture;
	
	private RenderTexture			screenTexture;
	private HashSet<BaseMenu>		blurCallers = new HashSet<BaseMenu>();
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetBackgroundBlur(bool blur, BaseMenu caller)
	{
		if (screenTexture == null)
		{
			int height = Constant.SCREEN_HEIGHT;
			float deviceWHAspect = SystemUtility.GetDeviceWHAspect();
			int width = (int)(deviceWHAspect * height);
			screenTexture = new RenderTexture(width, height, 24);
			screenTexture.name = "ScreenTexture";

			if (deviceWHAspect > Constant.STANDARD_WH_ASPECT)
			{
				bluredTexture.height = height;
				bluredTexture.width = width;
			}
			else
			{
				bluredTexture.height = (int)(Constant.SCREEN_WIDTH / SystemUtility.GetDeviceWHAspect());
				bluredTexture.width = Constant.SCREEN_WIDTH;
			}
		}
		if (blur)
		{
			blurCallers.Add(caller);
			mainCamera.targetTexture = screenTexture;
			bluredTexture.mainTexture = screenTexture;
			bluredTexture.gameObject.SetActive(blur);
		}
		else
		{
			blurCallers.Remove(caller);
			if (blurCallers.Count <= 0)
			{
				mainCamera.targetTexture = null;
				bluredTexture.gameObject.SetActive(blur);
			}
		}
	}
}