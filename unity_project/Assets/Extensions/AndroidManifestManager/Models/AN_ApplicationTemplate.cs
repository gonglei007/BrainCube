using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class AN_ApplicationTemplate : AN_BaseTemplate {
	private Dictionary<int, AN_ActivityTemplate> _activities = null;

	public AN_ApplicationTemplate() : base(){
		_activities = new Dictionary<int, AN_ActivityTemplate> ();
	}
	
	public void AddActivity(AN_ActivityTemplate activity) {
		_activities.Add (activity.Id, activity);	
	}

	public void RemoveActivity(AN_ActivityTemplate activity) {
		_activities.Remove (activity.Id);
	}


	public AN_ActivityTemplate GetOrCreateActivityWithName(string name) {
		AN_ActivityTemplate activity = GetActivityWithName(name);
		if(activity == null) {
			activity =  new AN_ActivityTemplate(false, name);
			AddActivity(activity);
		}

		return activity;

	}

	public AN_ActivityTemplate GetActivityWithName(string name)  {


		foreach(KeyValuePair<int, AN_ActivityTemplate> entry in Activities) {
			if(entry.Value.Name.Equals(name)) {
				return entry.Value;
			}
		}

		return null;
	}

	public AN_ActivityTemplate GetLauncherActivity() {
		foreach(KeyValuePair<int, AN_ActivityTemplate> entry in Activities) {
			if(entry.Value.IsLauncher) {
				return entry.Value;
			}
		} 
		
		return null;
	}

	public override void ToXmlElement (XmlDocument doc, XmlElement parent)
	{
		AddAttributesToXml (doc, parent, this);
		AddPropertiesToXml (doc, parent, this);

		foreach (int id in _activities.Keys) {
			XmlElement activity = doc.CreateElement ("activity");
			_activities[id].ToXmlElement(doc, activity);
			parent.AppendChild (activity);
		}
	}

	public Dictionary<int, AN_ActivityTemplate> Activities {
		get {
			return _activities;
		}
	}



}
