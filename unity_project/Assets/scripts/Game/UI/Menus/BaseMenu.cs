using UnityEngine;
using System.Collections;

public class BaseMenu : MonoBehaviour {

	protected bool isActive = false;

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Show(bool active)
	{
		if (active)
		{
			isActive = true;
			Vector3 showPosition = Vector3.zero;
			showPosition.z = this.gameObject.transform.localPosition.z;
			this.gameObject.transform.localPosition = showPosition;
		}
		else
		{
			isActive = false;
			this.gameObject.transform.localPosition = new Vector3(0,2000,this.gameObject.transform.localPosition.z);
		}
		this.gameObject.SetActive(active);
	}

	public virtual void ToggleShow()
	{
		if (isActive)
		{
			isActive = false;
			this.gameObject.transform.localPosition = new Vector3(0,2000,this.gameObject.transform.localPosition.z);
		}
		else
		{
			isActive = true;
			Vector3 showPosition = Vector3.zero;
			showPosition.z = this.gameObject.transform.localPosition.z;
			this.gameObject.transform.localPosition = showPosition;
		}
		this.gameObject.SetActive(isActive);
	}

	public virtual void Close()
	{		
		UIClose close = this.GetComponent<UIClose>();
		if (close != null)
		{
			close.Close();
		}
		else
		{
			Show(false);
		}
	}
}
