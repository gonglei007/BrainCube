using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankMenu : BaseMenu {
	public UILabel 		modeNameLabel;
	public UILabel		modeTypeNameLabel;
	public UIPanel		topRankPanel;
	public UIPanel		myRankPanel;
	public UITable 		topRankTable;
	public UITable		myRankTable;
	public UIToggle		topRankToggle;
	public GameObject	loginButton;
	public GameObject	recycle;
	public GameObject	loading;

	private List<RankItem> 	topRankItems = new List<RankItem>(10);
	private List<RankItem> 	myRankItems = new List<RankItem>(10);
	private Queue<RankItem>	recycledItems = new Queue<RankItem>(20);

	private GameObject rankItemPrefab;

	void Awake()
	{
		AdjustRankPanel();
		GameSystem.GetInstance().gameUI.rankMenu = this;
		this.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () 
	{
		LeaderboardManager.LeaderboardTarget = this;
		LeaderboardManager.LeaderboardCallback = this.RequestGameLeaderboardFinish;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void Show(bool active)
	{
		base.Show(active);
		if(active)
		{
			GameSystem.GetInstance().gameUI.mainMenu.Show(false);
			modeNameLabel.text = TextManager.GetText(string.Format("mode_name_{0}", (int)GameSystem.GetInstance().CurrentMode));
			modeTypeNameLabel.text = string.Format("({0})", TextManager.GetText(string.Format("mode_type_name_{0}", (int)GameSystem.GetInstance().CurrentModeType)));

			topRankToggle.value = true;
			ResetTopRankPanelClipOffset();

#if UNITY_EDITOR
			loading.SetActive(false);
			loginButton.SetActive(false);
			RecycleAllTopRankItem();
			RecycleAllMyRankItem();
			for(int i = 0 ; i < Constant.TOP_RANK_COUNT ; ++i)
			{
				RankItem rankItem = GetRankItem(topRankTable.gameObject);
				rankItem.name = string.Format("RankItem{0}", i);
				rankItem.Data = new RankData("username" + i, i+1, (100 * (Constant.TOP_RANK_COUNT - i - 1)).ToString());
				topRankItems.Add(rankItem);

				rankItem = GetRankItem(myRankTable.gameObject);
				rankItem.name = string.Format("RankItem{0}", i);
				rankItem.Data = new RankData("username" + i, i+1, (100 * (Constant.TOP_RANK_COUNT - i - 1)).ToString());
				myRankItems.Add(rankItem);
			}
			topRankTable.repositionNow = true;
			myRankTable.repositionNow = true;
#else
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				Debug.Log("Start Load Global Rank");
				loading.SetActive(true);
				LeaderboardManager.RequestGameLeaderboard(GameSystem.GetInstance().CurrentMode, GameSystem.GetInstance().CurrentModeType, this, RequestGameLeaderboardFinish);
			}
#endif
		}
	}

	public void LoginButtonOnClick()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			if (KTPlay.IsEnabled()) {
				KTAccountManager.ShowLoginView(true, this, OnKTPlayLoginFinish);
			}
		}
		GameSoundSystem.GetInstance().PlayRandomSound();
	}
	
	public void TopRankButtonOnClick()
	{
		ResetTopRankPanelClipOffset();
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	private void ResetTopRankPanelClipOffset()
	{
		topRankPanel.transform.localPosition = new Vector3(0, 220, 0);
		topRankPanel.clipOffset = Vector2.zero;
	}

	public void MyRankButtonOnClick()
	{
		ResetMyRankPanelClipOffset();
		GameSoundSystem.GetInstance().PlayRandomSound();
	}
	
	private void ResetMyRankPanelClipOffset()
	{
		topRankPanel.transform.localPosition = new Vector3(0, 220, 0);
		topRankPanel.clipOffset = Vector2.zero;
	}

	public void CloseButtonOnClick()
	{
		loading.SetActive(false);
		GameSystem.GetInstance().gameUI.mainMenu.Show(true);
		GameSystem.GetInstance().gameUI.mainMenu.ShowEnterAnimation(()=>{
			this.Show(false);
		});
		GameSoundSystem.GetInstance().PlayRandomSound();
	}

	RankItem GetRankItem(GameObject container)
	{
		RankItem rankItem = null;
		if (rankItemPrefab == null)
		{
			rankItemPrefab = Resources.Load("Prefabs/RankItem") as GameObject;
		}
		if (recycledItems.Count > 0)
		{
			rankItem = recycledItems.Dequeue();
			rankItem.transform.parent = container.transform;
		}
		else
		{
			GameObject rankItemObj = SystemUtility.InstantiatePrefabe(rankItemPrefab, container, new Vector3(0, -48, 0));
			rankItem = rankItemObj.GetComponent<RankItem>();
		}
		return rankItem;
	}

	void RecycleAllTopRankItem()
	{
		foreach(RankItem rankItem in topRankItems)
		{
			recycledItems.Enqueue(rankItem);
			rankItem.transform.parent = recycle.transform;
		}
		topRankItems.Clear();
	}

	void RecycleAllMyRankItem()
	{
		foreach(RankItem rankItem in myRankItems)
		{
			recycledItems.Enqueue(rankItem);
			rankItem.transform.parent = recycle.transform;
		}
		myRankItems.Clear();
	}

	void OnKTPlayLoginFinish(string param)
	{
		KTAccountManager.KTAccountManagerCallbackParams accountParam = new KTAccountManager.KTAccountManagerCallbackParams(param);
		switch(accountParam.accountManagerEventResult)
		{
		case KTAccountManager.KTAccountManagerCallbackParams.KTAccountManagerEvent.KTPlayAccountEventLoginViewLogin:
		{
			//操作成功
			this.Show(true);
			//user 信息
		}
			break;
		case KTAccountManager.KTAccountManagerCallbackParams.KTAccountManagerEvent.OnKTError:
		{
			//操作失败
			KTError error = (KTError) accountParam.playError;
			Debug.Log("KTPlayParams.OnKTPlayError:"+"   code:"+error.code+"    description:"+error.description);
		}
			break;
		}
	}
	
	void RequestMyLeaderboardFinish(string param)
	{
		loading.SetActive(false);

		KTPlayLeaderboard.KTLeaderboardCallbackParams leaderParams = new KTPlayLeaderboard.KTLeaderboardCallbackParams(param);
		switch(leaderParams.leaderboardEventResult)
		{
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.KTPlayLeaderboardEventFriendsLeaderboard:
		{
		}
			break;
			
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.KTPlayLeaderboardEventGlobalLeaderboard:
		{
			RecycleAllMyRankItem();
			
			ArrayList users = leaderParams.globalLeaderboardPaginator.items;
			
			Debug.Log("RequestMyLeaderboardFinish rank item count : " + users.Count.ToString());
			
			int index = 0;
			foreach(KTUser user in users)
			{
				RankItem rankItem = GetRankItem(myRankTable.gameObject);
				rankItem.name = string.Format("RankItem{0}", index);
				rankItem.Data = new RankData(user.nickname, (int)user.rank, user.score);
				
				bool isPlayer = (user.userId.Equals(KTAccountManager.CurrentAccount().userId));
				rankItem.IsPlayer = isPlayer;
				myRankItems.Add(rankItem);
				index++;
			}
			myRankTable.repositionNow = true;
		}
			break;
			
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.KTPlayLeaderboardEventReportScore:
		{
			Debug.Log("KTPlayParams.KTPlayLeaderboardEventReportScore");
			Debug.Log("[reportScore requestInfo] leaderboardId = " + leaderParams.leaderboardId + " score = " + leaderParams.score); 
		}
			break;
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.OnKTError:
		{	
			KTError error = (KTError) leaderParams.playError;
			Debug.Log("KTPlayParams.OnKTPlayError:"+"   code:"+error.code+"    description:"+error.description);
		}
			break;
		}
	}

	void RequestGameLeaderboardFinish(string param)
	{		
		loading.SetActive(false);
		KTPlayLeaderboard.KTLeaderboardCallbackParams leaderParams = new KTPlayLeaderboard.KTLeaderboardCallbackParams(param);
		switch(leaderParams.leaderboardEventResult)
		{
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.KTPlayLeaderboardEventFriendsLeaderboard:
		{
		}
			break;
			
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.KTPlayLeaderboardEventGlobalLeaderboard:
		{
			RecycleAllTopRankItem();
			
			ArrayList users = leaderParams.globalLeaderboardPaginator.items;
			Debug.Log("RequestGameLeaderboardFinish rank item count : " + users.Count.ToString());
			
			int index = 0;
			foreach(KTUser user in users)
			{
				RankItem rankItem = GetRankItem(topRankTable.gameObject);
				rankItem.name = string.Format("RankItem{0}", index);
				rankItem.Data = new RankData(user.nickname, (int)user.rank, user.score);
				topRankItems.Add(rankItem);
				index++;
			}
			topRankTable.repositionNow = true;

			if(KTAccountManager.IsLoggedIn())
			{
				Debug.Log("Start Load My Rank");
				loginButton.SetActive(false);
				int myRank = (int)KTAccountManager.CurrentAccount().rank;
				int startIndex = myRank - 4;
				if (startIndex < 0)
				{
					startIndex = 0;
				}
				loading.SetActive(true);
				LeaderboardManager.RequestGameLeaderboard(GameSystem.GetInstance().CurrentMode, GameSystem.GetInstance().CurrentModeType, this, RequestMyLeaderboardFinish, startIndex);
			}
			else
			{
				loginButton.SetActive(true);
				RecycleAllMyRankItem();
			}
		}
			break;
			
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.KTPlayLeaderboardEventReportScore:
		{
			Debug.Log("KTPlayParams.KTPlayLeaderboardEventReportScore");
			Debug.Log("[reportScore requestInfo] leaderboardId = " + leaderParams.leaderboardId + " score = " + leaderParams.score); 
		}
			break;
		case KTPlayLeaderboard.KTLeaderboardCallbackParams.KTLeaderboardEvent.OnKTError:
		{	
			KTError error = (KTError) leaderParams.playError;
			Debug.Log("KTPlayParams.OnKTPlayError:"+"   code:"+error.code+"    description:"+error.description);
		}
			break;
		}
	}
	
	private void AdjustRankPanel()
	{
		Vector4 clipRegion = topRankPanel.baseClipRegion;
		int heightExtend = (int)((Constant.SCREEN_WIDTH / SystemUtility.GetDeviceWHAspect() - Constant.SCREEN_HEIGHT) / 2);
		heightExtend = heightExtend > 0 ? heightExtend : 0;
		int newPanelHeight = (int)clipRegion.w + heightExtend;
		clipRegion.w = newPanelHeight;
		clipRegion.y = -newPanelHeight / 2;

		topRankPanel.baseClipRegion = clipRegion;
		myRankPanel.baseClipRegion = clipRegion;
	}
}
