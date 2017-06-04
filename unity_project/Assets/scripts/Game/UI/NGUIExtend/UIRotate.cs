using UnityEngine;
using System.Collections;

public class UIRotate : MonoBehaviour {
	public float rotateSpeed = 120;
	public bool  ignoreTimeScale = true;
	
	private float rotateAngle = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ignoreTimeScale)
		{
			rotateAngle += RealTime.deltaTime * rotateSpeed;
		}
		else
		{
			rotateAngle += Time.deltaTime * rotateSpeed;
		}
		if (rotateAngle >= 360)
		{
			rotateAngle -= 360;
		}
		else if (rotateAngle <= -360)
		{
			rotateAngle += 360;
		}
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotateAngle));	
	}

	public void Reset(Quaternion quaternion)
	{
		this.transform.localRotation = quaternion;
	}
}
