using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

	public GameObject[] jumpElements;

	// Use this for initialization
	void Start () {
		int index = 0;
		foreach(GameObject jumpElement in jumpElements)
		{
			Vector3 pos = jumpElement.transform.localPosition;
			pos.y += 40;
			TweenPosition tweenPosition = TweenPosition.Begin(jumpElement, 0.5f, pos);
			tweenPosition.delay = index * 0.1f;
			tweenPosition.method = UITweener.Method.EaseOut;
			tweenPosition.style = UITweener.Style.PingPong;
			index++;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
