////////////////////////////////////////////////////////////////////////////////
//  
// @module Events Pro
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////
 

namespace UnionAssets.FLE {

	public class BaseEvent  {
		
		
		//--------------------------------------
		// STATES
		//--------------------------------------
		
		public const string READY    		= "ready";
		public const string LOADED  	 	= "loaded";
		public const string FAILED    		= "failed";
		public const string CHANGED   		= "changed";
		public const string COMPLETE 		= "complete";	
		public const string INITIALIZED		= "initialized";
		
		
		
		//--------------------------------------
		// SIMPLE
		//--------------------------------------
		
		
		public const string NO  	 	= "no";
		public const string YES    		= "yes";
		
		public const string OPEN  	 	= "open";
		public const string CLOSED    	= "close";
		
		
		//--------------------------------------
		// MOUSE
		//--------------------------------------
		
		
		public const string CLICK    		= "click";
		public const string DOUBLE_CLICK    = "double_click";
		
		
		
		//--------------------------------------
		// ANIMATION
		//--------------------------------------
		
		
		public const string ANIMATION_STARTED 	= "animation_started";
		public const string ANIMATION_COMPLETE 	= "animation_complete";
		
		
	}
}
