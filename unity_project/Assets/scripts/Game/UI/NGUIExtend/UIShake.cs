using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour {
	public int shakeTimes = 3;
	public int shakeInterval = 1;
	public int shakeAngle = 5;
	public bool autoStart = false;

	private int 	shakeTimeCounter = 0;
	private float	shakeIntervalTimer = 0;
	private bool	startShake = false;
	private bool	startShakeWait = false;

	// Use this for initialization
	void Start () {
		if (autoStart)
		{
			StartShake();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (startShakeWait)
		{
			shakeIntervalTimer += Time.deltaTime;
			if (shakeIntervalTimer >= shakeInterval)
			{
				shakeIntervalTimer = 0;
				startShakeWait = false;
				DoStartShake();
			}
		}
	}

	public void StartShake()
	{
		startShake = true;
		shakeTimeCounter = 0;
		DoStartShake();
	}

	public void StopShake()
	{
		startShake = false;
		Destroy(this.GetComponent<TweenRotation>());
		this.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	void DoStartShake()
	{
		if (startShake)
		{
			Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, shakeAngle));
			TweenRotation tweenRotation = TweenRotation.Begin(this.gameObject, 0.03f, rotation);
			tweenRotation.from = new Vector3(0, 0, 0);
			tweenRotation.eventReceiver = this.gameObject;
			tweenRotation.callWhenFinished = "ShakeAnim1Finish";
		}
	}
	
	void ShakeAnim1Finish()
	{
		if (startShake)
		{
			Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			TweenRotation tweenRotation = TweenRotation.Begin(this.gameObject, 0.03f, rotation);
			tweenRotation.eventReceiver = this.gameObject;
			tweenRotation.callWhenFinished = "ShakeAnim2Finish";
		}
	}
	
	void ShakeAnim2Finish()
	{
		if (startShake)
		{
			Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 360-shakeAngle));
			TweenRotation tweenRotation = TweenRotation.Begin(this.gameObject, 0.03f, rotation);
			tweenRotation.from = new Vector3(0, 0, 360);
			tweenRotation.eventReceiver = this.gameObject;
			tweenRotation.callWhenFinished = "ShakeAnim3Finish";
		}
	}
	
	void ShakeAnim3Finish()
	{
		if (startShake)
		{
			Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 360));
			TweenRotation tweenRotation = TweenRotation.Begin(this.gameObject, 0.03f, rotation);
			tweenRotation.from = new Vector3(0, 0, 360-shakeAngle);
			tweenRotation.to = new Vector3(0, 0, 360);
			tweenRotation.eventReceiver = this.gameObject;
			tweenRotation.callWhenFinished = "ShakeAnim4Finish";
		}
	}

	void ShakeAnim4Finish()
	{
		if (startShake)
		{
			if (shakeTimeCounter < shakeTimes)
			{
				shakeTimeCounter++;
				DoStartShake();
			}
			else
			{
				shakeTimeCounter = 0;
				startShakeWait = true;
				shakeIntervalTimer = 0;
			}
		}
    }
}
