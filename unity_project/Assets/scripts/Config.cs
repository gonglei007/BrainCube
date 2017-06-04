using UnityEngine;
using System.Collections;

public class Config{
	public static bool	isStoreActive = true;
	public static bool	isUseCenterActive = false;
	public static bool	isMoreGameActive = false;
	public static bool	isSkipPurchase = false;		// if it's not android or iphone, force setting true in IAPManager awake.
	public static bool	enableRescue = true;
	public static bool	enableHardActive = false;
	public static bool	enableRestore = true;
}
