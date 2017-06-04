using UnityEngine;
using System.Collections;
using System;

public class TimeUtility {

	public static string GetTimestamp(){
		return DateTime.Now.ToString("yyyyMMddHHmmssffff");
	}
	
	public static bool IsDifferentDay(DateTime formerTime, DateTime laterTime) {
		Debug.Log("IsDifferentDay --- " + formerTime.ToString() + " +++ " + laterTime.ToString());
		int yearDiff = laterTime.Year - formerTime.Year;
		Debug.Log("yearDiff " + yearDiff);
		if (yearDiff > 0) {
			return true;
		}
		else {
			int monthDiff = laterTime.Month - formerTime.Month;
			Debug.Log("monthDiff " + monthDiff);
			if (monthDiff > 0) {
				return true;
			}
			else {
				int dayDiff = laterTime.Day - formerTime.Day;
				Debug.Log("dayDiff " + dayDiff);
				if (dayDiff > 0) {
					return true;
				}
				else {
					return false;
				}
			}
		}
	}
	
	public static double GetHourDiff(DateTime formerTime, DateTime laterTime) {
		return (laterTime - formerTime).TotalHours;
	}
}
