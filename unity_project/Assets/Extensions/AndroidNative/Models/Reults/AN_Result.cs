using UnityEngine;
using System.Collections;

public class AN_Result  {


	protected bool _IsSucceeded = true;

	
	public AN_Result(bool IsResultSucceeded) {
		_IsSucceeded = IsResultSucceeded;
	}
	
	public bool IsSucceeded {
		get {
			return _IsSucceeded;
		}
	}
	
	public bool IsFailed {
		get {
			return !_IsSucceeded;
		}
	}
}
