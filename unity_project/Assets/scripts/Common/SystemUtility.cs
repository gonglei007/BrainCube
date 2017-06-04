using UnityEngine;
using System.Collections;

public class SystemUtility {
	
	public enum BuildVersion {
		DEBUG,
		RELEASE
	}

	/// <summary>
	/// 实例化一个Prefab.
	/// </summary>
	/// <returns>The prefabe.</returns>
	/// <param name="prefabe">Prefabe.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="newPosition">New position.</param>
	static public GameObject InstantiatePrefabe(GameObject prefabe, GameObject parent, Vector3 newPosition)
	{
		GameObject newObject = (GameObject)Object.Instantiate(prefabe, Vector3.zero, Quaternion.identity);
		newObject.transform.parent = parent.transform;
		newObject.transform.localScale = prefabe.transform.localScale;
		newObject.transform.localPosition = newPosition;
		newObject.transform.localRotation = prefabe.transform.localRotation;
		return newObject;
	}

	/// <summary>
	/// 销毁gameObject的所有子物体，注意下一帧才会生效，所以在某些情况下需要立即再次获得子物体时会包括下一帧要销毁的物体，
	/// tempParent用来暂时性转移父子关系，从而避免上述问题
	/// </summary>
	/// <param name="gameobject">Gameobject.</param>
	/// <param name="TempParent">Temp parent.</param>
	public static void DestroyChildren(GameObject gameobject, Transform tempParent = null)
	{
		Transform[] transforms = gameobject.GetComponentsInChildren<Transform>(true);
		foreach(Transform transform in transforms)
		{
			if (transform != gameobject.transform)
			{
				if (tempParent != null)
				{
					transform.parent = tempParent;
				}
				GameObject.Destroy(transform.gameObject);
			}
		}
	}
	
	static public float GetDeviceWHAspect()
	{
		int screenWidth = Screen.width < Screen.height ? Screen.width : Screen.height;
		int screenHeight = Screen.width < Screen.height ? Screen.height : Screen.width;
		
		return screenWidth * 1.0f / screenHeight;
	}

	static public float GetFinalWHAspect(float standardWHAspect)
	{
		float finalAspect = GetDeviceWHAspect();
		if (finalAspect < standardWHAspect)
		{
			finalAspect = standardWHAspect;
		}
		return finalAspect;
	}
}
