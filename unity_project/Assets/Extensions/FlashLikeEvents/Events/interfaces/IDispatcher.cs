////////////////////////////////////////////////////////////////////////////////
//  
// @module Events Pro
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////
 

using System;
using System.Collections.Generic;

namespace UnionAssets.FLE {


	public delegate void EventHandlerFunction();
	public delegate void DataEventHandlerFunction(CEvent e);

	public interface IDispatcher {
		

		//--------------------------------------
		// ADD LISTENER'S
		//--------------------------------------

		void addEventListener(string eventName, 	EventHandlerFunction handler);
		void addEventListener(int eventID, 			EventHandlerFunction handler);
		void addEventListener(string eventName, 	DataEventHandlerFunction handler);
		void addEventListener(int eventID, 			DataEventHandlerFunction handler);


		//--------------------------------------
		// REMOVE LISTENER'S
		//--------------------------------------

		void removeEventListener(string eventName, 	EventHandlerFunction handler);
		void removeEventListener(int eventID, 		EventHandlerFunction handler);
		void removeEventListener(string eventName,  DataEventHandlerFunction handler);
		void removeEventListener(int eventID, 		DataEventHandlerFunction handler);
		

		//--------------------------------------
		// DISPATCH I1
		//--------------------------------------

		void dispatchEvent(int eventID);
		void dispatchEvent(int eventID, object data);
		void dispatchEvent(string eventName);
		void dispatchEvent(string eventName, object data);
		

		//--------------------------------------
		// DISPATCH I2
		//--------------------------------------


		void dispatch(int eventID);
		void dispatch(int eventID, object data);
		void dispatch(string eventName);
		void dispatch(string eventName, object data);
		

		//--------------------------------------
		// METHODS
		//--------------------------------------

		void clearEvents();

	}
}
