using UnityEngine;
using System.Collections;

public class Crystallize : MonoBehaviour {
	public int gridWidth = 110;
	public int gridHeight = 110;

	public int fillWidth = 800;
	public int fillHeight = 1140;

	public Color minColor = Color.black;
	public Color maxColor = Color.gray;

	public float randomColorRange = 0.1f;

	public int renderQueue = 3010;
	public ParticleSystem	backgroundFX;

	private GameObject	planePrefab;

	// Use this for initialization
	void Start () {
		if(backgroundFX != null){
			backgroundFX.gameObject.renderer.material.renderQueue = renderQueue;
		}
		planePrefab = Resources.Load("Prefabs/BlankPlane") as GameObject;

		int columnCount = (int)(fillWidth * 1.0f / gridWidth);
		if ((columnCount & 1) == 1)
		{
			columnCount++;
		}
		columnCount /= 2;

		int rowCount = (int)(fillHeight * 1.0f / gridHeight);
		if ((rowCount & 1) == 1)
		{
			rowCount++;
		}
		rowCount /= 2;

		for(int row = 0 ; row < rowCount ; row++)
		{
			for(int column = 0 ; column < columnCount ; column++)
			{
				int count = 4;
				while(count > 0)
				{
					GameObject plane = NGUITools.AddChild(this.gameObject, planePrefab);
					UISprite planeSprite = plane.GetComponent<UISprite>();
					planeSprite.width = gridWidth;
					planeSprite.height = gridHeight;
					
					Vector3 position = Vector3.zero;	

					float colorDelta = 1.0f / rowCount / 2;
					float[] minHSB = ColorUtility.RGB2HSB(minColor);
					float[] maxHSB = ColorUtility.RGB2HSB(maxColor);
					float[] hsbValues = new float[3];
					float basicLerp = 0;

					if (count == 4)
					{
						position.x = (gridWidth / 2.0f - 1 + (gridWidth - 1) * column);
						position.y = (gridHeight / 2.0f - 1 + (gridHeight - 1) * row);

						basicLerp = colorDelta * (rowCount - row - 1);
					}
					else if (count == 3)
					{
						position.x = -(gridWidth / 2.0f - 1 + (gridWidth - 1) * column);
						position.y = (gridHeight / 2.0f - 1 + (gridHeight - 1) * row);
						
						basicLerp = colorDelta * (rowCount - row - 1);
					}
					else if (count == 2)
					{
						position.x = (gridWidth / 2.0f - 1 + (gridWidth - 1) * column);
						position.y = -(gridHeight / 2.0f - 1 + (gridHeight - 1) * row);
						
						basicLerp = colorDelta * (row + 1) + 0.5f;
					}
					else if (count == 1)
					{
						position.x = -(gridWidth / 2.0f - 1 + (gridWidth - 1) * column);
						position.y = -(gridHeight / 2.0f - 1 + (gridHeight - 1) * row);
						
						basicLerp = colorDelta * (row + 1) + 0.5f;
					}

					plane.transform.localPosition = position;

					hsbValues[0] = Mathf.Lerp(minHSB[0], maxHSB[0], basicLerp);
					hsbValues[0] += hsbValues[0] * Random.Range(-randomColorRange, randomColorRange);
					hsbValues[1] = Mathf.Lerp(minHSB[1], maxHSB[1], basicLerp);
					hsbValues[1] += hsbValues[1] * Random.Range(-randomColorRange, randomColorRange);
					hsbValues[2] = Mathf.Lerp(minHSB[2], maxHSB[2], basicLerp);
					hsbValues[2] += hsbValues[2] * Random.Range(-randomColorRange, randomColorRange);

					Color color = ColorUtility.HSB2RGB(hsbValues);
					planeSprite.color = color;

					count--;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
