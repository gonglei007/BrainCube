////////////////////////////////////////////////////////////////////////////////
//  
// @module Events Pro
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////
 

using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnionAssets.FLE {

	public class EventDispatcherBase : IDispatcher {

		private Dictionary<int, List<EventHandlerFunction>> listners = new Dictionary<int, List<EventHandlerFunction>>();
		private Dictionary<int, List<DataEventHandlerFunction>> dataListners = new Dictionary<int, List<DataEventHandlerFunction>>();

		
		//--------------------------------------
		// ADD LISTENER'S
		//--------------------------------------


		public void addEventListener(string eventName, EventHandlerFunction handler) {
			addEventListener(eventName.GetHashCode(), handler, eventName);
		}

		public void addEventListener(int eventID, EventHandlerFunction handler) {
			addEventListener(eventID, handler, eventID.ToString());
		}

		private void addEventListener(int eventID, EventHandlerFunction handler, string eventGraphName) {

			if(listners.ContainsKey(eventID)) {
				listners[eventID].Add(handler);
			} else {
				List<EventHandlerFunction> handlers =  new  List<EventHandlerFunction>();
				handlers.Add(handler);
				listners.Add(eventID, handlers);
			}
		}



		public void addEventListener(string eventName, DataEventHandlerFunction handler) {
			addEventListener(eventName.GetHashCode(), handler, eventName);
		}

		public void addEventListener(int eventID, DataEventHandlerFunction handler) {
			addEventListener(eventID, handler, eventID.ToString());
		}

		private void addEventListener(int eventID, DataEventHandlerFunction handler,  string eventGraphName) {


			if(dataListners.ContainsKey(eventID)) {
				dataListners[eventID].Add(handler);
			} else {
				List<DataEventHandlerFunction> handlers =  new  List<DataEventHandlerFunction>();
				handlers.Add(handler);
				dataListners.Add(eventID, handlers);
			}
		}


		//--------------------------------------
		// REMOVE LISTENER'S
		//--------------------------------------
		
		public void removeEventListener(string eventName, EventHandlerFunction handler) {
			removeEventListener(eventName.GetHashCode(), handler, eventName);
		}

		public void removeEventListener(int eventID, EventHandlerFunction handler) {
			removeEventListener(eventID, handler, eventID.ToString());
		}

		public void removeEventListener(int eventID, EventHandlerFunction handler, string eventGraphName) {



			if(listners.ContainsKey(eventID)) {
				List<EventHandlerFunction> handlers =  listners[eventID];
				handlers.Remove(handler);

				if(handlers.Count == 0) {
					listners.Remove(eventID);
				}
			}
		}

		public void removeEventListener(string eventName, DataEventHandlerFunction handler)  {
			removeEventListener(eventName.GetHashCode(), handler, eventName);
		}

		public void removeEventListener(int eventID, DataEventHandlerFunction handler) {
			removeEventListener(eventID, handler, eventID.ToString());
		} 

		public void removeEventListener(int eventID, DataEventHandlerFunction handler, string eventGraphName) {


			if(dataListners.ContainsKey(eventID)) {
				List<DataEventHandlerFunction> handlers =  dataListners[eventID];
				handlers.Remove(handler);

				if(handlers.Count == 0) {
					dataListners.Remove(eventID);
				}
			}
		}


		//--------------------------------------
		// DISPATCH I1
		//--------------------------------------



		public void dispatchEvent(string eventName) {
			dispatch(eventName.GetHashCode(), null, eventName);
		}

		public void dispatchEvent(string eventName, object data) {
			dispatch(eventName.GetHashCode(), data, eventName);
		}

		public void dispatchEvent(int eventID) {
			dispatch(eventID, null, string.Empty);
		}

		public void dispatchEvent(int eventID, object data) {
			dispatch(eventID, data, string.Empty);
		}

		//--------------------------------------
		// DISPATCH I2
		//--------------------------------------


		public void dispatch(string eventName) {
			dispatch(eventName.GetHashCode(), null, eventName);
		}


		public void dispatch(string eventName, object data) {
			dispatch(eventName.GetHashCode(), data, eventName);
		}

		public void dispatch(int eventID) {
			dispatch(eventID, null, string.Empty);
		}

		public void dispatch(int eventID, object data) {
			dispatch(eventID, data, string.Empty);
		}

		//--------------------------------------
		// PRIVATE DISPATCH I2
		//--------------------------------------


		private void dispatch(int eventID, object data, string eventName) {

			CEvent e = new CEvent(eventID, eventName, data, this);


			if(dataListners.ContainsKey(eventID)) {
				List<DataEventHandlerFunction>  handlers = cloenArray(dataListners[eventID]);
				int len = handlers.Count;
				for(int i = 0; i < len; i++) {
					if(e.canBeDisptached(handlers[i].Target)) {
						handlers[i](e);


					}
				}
			}


			if(listners.ContainsKey(eventID)) {
				List<EventHandlerFunction>  handlers = cloenArray(listners[eventID]);
				int len = handlers.Count;
				for(int i = 0; i < len; i++) {
					if(e.canBeDisptached(handlers[i].Target)) {
						handlers[i]();
					}

				}
			}

		}

		//--------------------------------------
		// PUBLIC METHODS
		//--------------------------------------
		

		public void clearEvents() {
			listners.Clear();
			dataListners.Clear();
		}

		//--------------------------------------
		// GET / SET
		//--------------------------------------


		
		//--------------------------------------
		// PRIVATE METHODS
		//--------------------------------------
		
		private List<EventHandlerFunction> cloenArray(List<EventHandlerFunction> list) {
			List<EventHandlerFunction> nl =  new List<EventHandlerFunction>();
			int len = list.Count;
			for(int i = 0; i < len; i++) {
				nl.Add(list[i]);
			}
			
			return nl;
		}

		private List<DataEventHandlerFunction> cloenArray(List<DataEventHandlerFunction> list) {
			List<DataEventHandlerFunction> nl =  new List<DataEventHandlerFunction>();
			int len = list.Count;
			for(int i = 0; i < len; i++) {
				nl.Add(list[i]);
			}

			return nl;
		}
		
		//--------------------------------------
		// DestroyCEvent
		//--------------------------------------
		
		public virtual void OnDestroy() {
			clearEvents();
		}

	}

}

