using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour {

	public GameObject		cellsContainer;
	public GameObject		cellMask;

	private float[]			gameRegionSizes 				= { 300.0f, 300.0f, 400.0f, 500.0f, 500.0f, 500.0f, 500.0f };
	private GameObject		cellPrefab;
	private Cell[,]			cells;
	private List<Cell> 		cellList						= new List<Cell>(36);
	private Queue<Cell>		cellPool 						= new Queue<Cell>(36);

	private bool			isLevelWavePassed 				= false;
	private bool			isWaveEnterAnimFinished 		= false;
	private bool			isWaveExitAnimFinished 			= false;
	private bool			isWaveFailAnimFinished 			= false;
	private int				showedCatCellCount 				= 0;
	private int				hidedCellCount 					= 0;
	private int				totalCatCellCount 				= 0;

	private Wave			wave;
	private int				row = 2;
	private int				column = 2;

	public bool IsLevelWavePassed{
		get{
			return isLevelWavePassed;
		}
		set{
			isLevelWavePassed = value;
		}
	}

	public bool IsWaveEnterAnimFinished{
		get{
			return isWaveEnterAnimFinished;
		}
		set{
			isWaveEnterAnimFinished = value;
		}
	}

	public bool IsWaveExitAnimFinished{
		get{
			return isWaveExitAnimFinished;
		}
		set{
			isWaveExitAnimFinished = value;
		}
	}

	public bool IsWaveFailAnimFinished{
		get{
			return isWaveFailAnimFinished;
		}
		set{
			isWaveFailAnimFinished = value;
		}
	}

	void Awake()
	{
		GameSystem.GetInstance().gameCore = this;
	}

	// Use this for initialization
	void Start () {
		this.cellPrefab = Resources.Load("Prefabs/Cell") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Init(Wave wave)
	{		
		this.wave = wave;
		this.row = wave.rowNumber;
		this.column = wave.columnNumber;
		this.cellMask.GetComponent<UISprite>().alpha = 1;
		GameSystem.GetInstance().CurrentModeLogic.Init(wave);
		DestroyAllCells();
		InitCells();
	}
	
	public void SetAllCellState(Cell.CellState state)
	{
		foreach(Cell cell in cells)
		{
			cell.State = state;
		}
	}

	/// <summary>
	/// 开始入场动画，cellMask的alpha从1渐变到0
	/// </summary>
	public void StarWaveEnterAnim(){
		isWaveEnterAnimFinished = false;

		foreach(Cell cell in cells)
		{
			cell.BackgroundAlpha = 1;
			cell.ForegroundAlpha = 0;
		}
		
		cellMask.SetActive(true);
		TweenAlpha tweenAlpha = TweenAlpha.Begin(cellMask, 0.5f, 0);
		tweenAlpha.from = 1;
		EventDelegate.Add(tweenAlpha.onFinished, CellMaskAlphaToZeroFirstTimeFinish, true);
	}

	public void StarWaveExitAnim()
	{
		isWaveExitAnimFinished = false;
		hidedCellCount = 0;

		int index = 0;
		float delayDelta = 0.05f;
		if (cells.Length > 10)
		{
			delayDelta = 1.0f / cells.Length;
		}
		foreach (Cell cell in cells)
		{
			cell.AnimateAlpha(1, 0, 0.05f, DoHideCellFinish, UITweener.Method.Linear, index * delayDelta);
			index++;
		}
	}

	public void StarWaveFailAnim()
	{
		cellMask.SetActive(true);
		isWaveFailAnimFinished = false;		
		
		FlipAllBlockCells();

		StartCoroutine(DoWaveFailAnim());
	}

	IEnumerator DoWaveFailAnim()
	{
		yield return new WaitForSeconds(0.5f);
		
		UIWidgetFlash flashScript = cellMask.GetComponent<UIWidgetFlash>();
		flashScript.Initialize();
		flashScript.flashEnabled = true;
		flashScript.endCallBack = WaveFailAnimFinish;
	}

	void WaveFailAnimFinish()
	{
		isWaveFailAnimFinished = true;
	}

	/// <summary>
	/// 入场动画续：cellMask的alpha变为0后,要记住的格子依次出现
	/// </summary>
	/// <param name="tweener">Tweener.</param>
	void CellMaskAlphaToZeroFirstTimeFinish()
	{
		showedCatCellCount = 0;
		totalCatCellCount = 0;
		float delayDelta = 0.2f;
		if (wave.tipNumber > 5)
		{
			delayDelta = 1.0f / wave.tipNumber;
		}
		foreach (Cell cell in cells)
		{
			if(cell.Type == Cell.CellType.Block)
			{
				cell.State = Cell.CellState.Showing;
				cell.ForegroundAlpha = 0;
				cell.AnimateForegroundAlpha(0, 1, 0.3f, DoShowCellFinish, UITweener.Method.Linear, delayDelta * totalCatCellCount);
				totalCatCellCount++;
			}
		}
	}
	
	void DoHideCellFinish()
	{
		hidedCellCount++;
		if (hidedCellCount >= cells.Length)
		{
			isWaveExitAnimFinished = true;
			hidedCellCount = 0;
		}
	}

	void DoShowCellFinish()
	{
		showedCatCellCount++;
		if (showedCatCellCount >= totalCatCellCount)
		{
			showedCatCellCount = 0;
			foreach(Cell cell in cells)
			{
				if (cell.Type == Cell.CellType.Block)
				{
					if (showedCatCellCount == totalCatCellCount - 1)
					{
						cell.AnimateForegroundAlpha(1, 1, 0.4f, EndShowCellFinish, UITweener.Method.Linear, GameSystem.GetInstance().CurrentModeLogic.FullBlockDisplayTime);
					}
					else
					{
						cell.AnimateForegroundAlpha(1, 1, 0.4f, null, UITweener.Method.Linear, GameSystem.GetInstance().CurrentModeLogic.FullBlockDisplayTime);
					}
					showedCatCellCount++;
				}
			}
		}
	}

	void EndShowCellFinish()
	{
		showedCatCellCount = 0;
		SetAllCellState(Cell.CellState.Closed);
		foreach(Cell cell in cells)
		{
			cell.ForegroundAlpha = 1;
		}
		isWaveEnterAnimFinished = true;
	}

	void DestroyAllCells()
	{
		if (cells != null)
		{
			foreach(Cell cell in cells)
			{
				cellPool.Enqueue(cell);
				cell.gameObject.SetActive(false);
			}
		}
	}
	
	public bool IsAllBlockFinded()
	{
		foreach(Cell cell in cells)
		{
			if(GameSystem.GetInstance().CurrentModeLogic.IsRightCellNotFliped(cell))
			{
				return false;
			}
		}
		return true;
	}

	void InitCells(){
		if(row != 0 && column != 0){
			int cellNum = row;
			if(column > row){
				cellNum = column;
			}
			float cellSize = gameRegionSizes[cellNum - 1] / (float)cellNum;
			UISprite cellMaskSprite = cellMask.GetComponent<UISprite>();
			cellMaskSprite.width = (int)gameRegionSizes[cellNum - 1] + 16;
			cellMaskSprite.height = cellMaskSprite.width;
			cells = new Cell[row, column];

			//目前使用在所有格子中随机的算法
//			if (true)
//			{
				cellList.Clear();
				for(int i = 0; i < row; ++i){
					for(int j = 0; j < column; ++j){
						Cell cell = this.CreateCell(i, j, cellSize, Cell.CellType.Empty);
						cells[i, j] = cell;
						cellList.Add(cell);
					}
				}

				//Random block now
				int blockCount = 0;
				while(blockCount < wave.tipNumber)
				{
					int randomIndex = Random.Range(0, cellList.Count);
					cellList[randomIndex].Type = Cell.CellType.Block;
					cellList.RemoveAt(randomIndex);
					blockCount++;
				}
				cellList.Clear();
//			}
//			else
//			{
//				for(int i=0; i<row; ++i){
//					Cell.CellType[]	cellTypes = new Cell.CellType[column];
//					for(int cellTypeIndex = 0; cellTypeIndex < column; ++cellTypeIndex){
//						cellTypes[cellTypeIndex] = Cell.CellType.Empty;
//					}
//					int	tipIndex = 0;
//					while(tipIndex < this.wave.tipNumber && tipIndex < this.column){
//						int holeIndex = Random.Range(0, column);
//						Cell.CellType	currentCellType = cellTypes[holeIndex];
//						if(currentCellType == Cell.CellType.Empty){
//							cellTypes[holeIndex] = Cell.CellType.Block;
//							tipIndex++;
//						}
//					}
//					for(int j=0; j<column; ++j){
//						cells[i, j] = this.CreateCell(i, j, cellSize, cellTypes[j]);
//					}
//				}
//			}

			foreach(Cell cell in cells)
			{				
				GameSystem.GetInstance().CurrentModeLogic.InitCell(cell);
			}
		}
	}

	Cell CreateCell(int rowIndex, int columnIndex, float cellSize, Cell.CellType cellType){
		Cell cell = null;
		if (cellPool.Count > 0)
		{
			cell = cellPool.Dequeue();
			ClearCell(cell);
		}
		else
		{
			GameObject	cellGameObject = GameObject.Instantiate(cellPrefab) as GameObject;		
			cellGameObject.transform.parent = cellsContainer.transform;
			cellGameObject.transform.localScale = Vector3.one;
			cell = cellGameObject.GetComponent<Cell>();
		}
		cell.gameObject.name = string.Format("Cell_{0}_{1}", columnIndex, rowIndex);	
		cell.gameObject.SetActive(true);

		Vector3 position = Vector3.zero;
		position.x += columnIndex * cellSize - ((cellSize * this.column) * 0.5f) + cellSize * 0.5f;
		position.y += ((cellSize * this.row) * 0.5f) - rowIndex * cellSize - cellSize * 0.5f;
		position.z += 1;
		cell.transform.localPosition = position;

		Vector3 colliderSize = Vector3.one;
		BoxCollider boxCollider = cell.GetComponent<BoxCollider>();
		colliderSize.x = cellSize;
		colliderSize.y = cellSize;
		boxCollider.size = colliderSize;

		UIButtonMessage cellMessage = cell.GetComponent<UIButtonMessage>();
		cellMessage.target = this.gameObject;
		cellMessage.functionName = "CellOnPress";

		cell.Init((int)(cellSize * 0.95f), cellType, rowIndex, columnIndex);
		return cell;
	}

	void CellOnPress(GameObject sender)
	{
		if (GameSystem.GetInstance().CurrentState == GameSystem.States.GamePlay)
		{
			Cell cell = sender.GetComponent<Cell>();

			if(cell.State == Cell.CellState.Closed)
			{
				bool isRight = GameSystem.GetInstance().CurrentModeLogic.IsRightCell(cell);

				FlipCell(cell, isRight, true);

				if (isRight)
				{
					if(IsAllBlockFinded())
					{
						GameSystem.GetInstance().CurrentModeLogic.HandleAllBlockFinded();
					}
				}

				GameSystem.GetInstance().CurrentModeLogic.HandlePlaySound(cell, isRight);
			}
		}
	}

	Cell GetCell(int row, int column){
		if(row < 0 || column < 0 || row >= this.row || column >= this.column || cells == null){
			return null;
		}
		return cells[row, column];
	}
	
	void FlipCell(Cell cell, bool isRight, bool recordScore){
		if (recordScore)
		{
			GameSystem.GetInstance().CurrentModeLogic.FlipCell(cell, isRight);
			cell.State = Cell.CellState.Opened;
			GameSystem.GetInstance().CurrentModeLogic.AfterFlipCell(cell, isRight);
			cell.PlayGlowBorderAnimation();
		}
		else
		{
			cell.State = Cell.CellState.Opened;
		}
	}

	void FlipAllBlockCells()
	{
		foreach(Cell cell in cells)
		{
			if (cell.Type == Cell.CellType.Block && cell.State == Cell.CellState.Closed)
			{
				FlipCell(cell, true, false);
			}
		}
	}

	void ClearCell(Cell cell)
	{
		Destroy(cell.GetComponentInChildren<TweenAlpha>());
		Destroy(cell.GetComponentInChildren<TweenScale>());
		cell.BackgroundAlpha = 1;
		cell.ForegroundAlpha = 1;
		cell.TextureName = string.Empty;
		cell.transform.localScale = Vector3.one;
	}
}
