using UnityEngine;
using System.Collections;

public class UIWidgetFlash : MonoBehaviour {	
	public enum FlashType
	{
		Immediately,
		Tween
	}
	
	public enum EndAction
	{
		None,
		Replay,
		Destroy,
		Hide
	}
	
	public	delegate void EndCallBack();
	
	public	EndCallBack				endCallBack			= null;
	
	public	FlashType				flashType			= FlashType.Tween;
	public	iTween.EaseType			easeType			= iTween.EaseType.linear;
	
	public	bool					flashEnabled		= true;
	public							float flashInterval = 0.5f;
	public	int						flashTimes			= 2;
	public	float					InitialAlpha		= 1;
	public	float					maxAlpha			= 1f;
	public	bool					useRealTime			= false;
	public	EndAction				endAction			= EndAction.Replay;
	
	private	bool					flashDisabled		= true;
	private float					flashTimer			= 0;
	private int						flashTimesCounter	= 0;
	private float					currentAlpha		= 0;
	
	void Start () {
		if (InitialAlpha > maxAlpha)
		{
			InitialAlpha = maxAlpha;
		}
		Initialize();
	}
	
	void Update () {
		if (flashEnabled)
		{
			flashDisabled = false;
			
			float timeDelta;
			if (useRealTime)
			{
				timeDelta = RealTime.deltaTime;
				//Pause Game too long will cause real time delta very large, Reduce it!
				while(timeDelta >= flashInterval)
				{
					timeDelta -= flashInterval;
				}
			}
			else
			{
				timeDelta = Time.deltaTime;
			}
			flashTimer += timeDelta;
			if (flashTimer >= flashInterval)
			{
				flashTimesCounter++;
				if (flashTimesCounter > flashTimes)
				{
					Initialize();
					
					if (endCallBack != null)
					{
						endCallBack();
					}
					
					switch(endAction)
					{
					case EndAction.None:
						flashEnabled = false;
						break;
					case EndAction.Replay:
						break;
					case EndAction.Destroy:
						Destroy(this.gameObject);
						break;
					case EndAction.Hide:
						iTween.Stop(this.gameObject);
						NGUITools.SetActive(this.gameObject, false);
						break;
					default:
						break;
					}
				}
				else
				{
					if (flashType == FlashType.Immediately)
					{
						flashTimer = 0;
						currentAlpha = maxAlpha - currentAlpha;
						SetAlphaRecursively(this.gameObject, currentAlpha);
					}
					else
					{
						flashTimer = 0;
						StartTweenWidgetFlash();
					}
				}
			}
		}
		else
		{
			if (flashDisabled == false)
			{
				iTween.Stop(this.gameObject);
				Initialize();
				flashDisabled = true;
			}
		}
	}
	
	public void Initialize()
	{
		currentAlpha = InitialAlpha;
		flashTimer = flashInterval;
		flashTimesCounter = 0;
		SetAlphaRecursively(this.gameObject, currentAlpha);
	}
	
	private void StartTweenWidgetFlash()
	{
		iTween.Stop(this.gameObject);
		iTween.ValueTo(this.gameObject, iTween.Hash("from", currentAlpha,
													"to", maxAlpha - currentAlpha, 
													"time", flashInterval,
													"easetype", easeType,
													"onupdatetarget", this.gameObject, 
													"onupdate", "UIWidgetFlashTweenUpdate",
													"onupdateparams", currentAlpha));
		currentAlpha = maxAlpha - currentAlpha;
	}
	
	private void SetAlphaRecursively(GameObject gameObject, float alpha) {
		UIWidget[] widgets  = gameObject.GetComponentsInChildren<UIWidget>();
		foreach( UIWidget widget in widgets) {
			widget.alpha = alpha;
		}
	}
	
	private void UIWidgetFlashTweenUpdate(float deltaValue)
	{
		SetAlphaRecursively(this.gameObject, deltaValue);
	}
}
