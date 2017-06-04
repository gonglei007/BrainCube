using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class AN_ActivityTemplate : AN_BaseTemplate {
	public bool IsOpen = false;

	private int _id = 0;
	private bool _isLauncher = false;
	private string _name = string.Empty;

	public AN_ActivityTemplate(bool isLauncher, string name) : base() {
		_isLauncher = isLauncher;
		_name = name;
		_id = GetHashCode ();

		_values = new Dictionary<string, string> ();
		_properties = new Dictionary<string, List<AN_PropertyTemplate>> ();
		SetValue("android:name", name);
	}

	public void SetName(string name) {
		_name = name;
		SetValue ("android:name", name);
	}

	public void SetAsLauncher(bool isLauncher) {
		_isLauncher = isLauncher;
	}

	public static AN_PropertyTemplate GetLauncherPropertyTemplate() {
		AN_PropertyTemplate launcher = new AN_PropertyTemplate ("intent-filter");

		AN_PropertyTemplate prop = new AN_PropertyTemplate ("action");
		prop.SetValue ("android:name", "android.intent.action.MAIN");
		launcher.AddProperty ("action", prop);

		prop = new AN_PropertyTemplate ("category");
		prop.SetValue ("android:name", "android.intent.category.LAUNCHER");
		launcher.AddProperty ("category", prop);

		return launcher;
	}

	public bool IsLauncherProperty(AN_PropertyTemplate property) {
		if(property.Tag.Equals("intent-filter")) {
			foreach (AN_PropertyTemplate p in property.Properties["category"]) {
				if (p.Values.ContainsKey("android:name")) {
					if (p.Values["android:name"].Equals("android.intent.category.LAUNCHER")) {
						return true;
					}
				}
			}
		}

		return false;
	}

	public override void ToXmlElement (XmlDocument doc, XmlElement parent)
	{
		AddAttributesToXml (doc, parent, this);

		AN_PropertyTemplate launcher = null;
		if (_isLauncher) {
			launcher = GetLauncherPropertyTemplate();
			AddProperty(launcher.Tag, launcher);
		}
		AddPropertiesToXml (doc, parent, this);
		if (_isLauncher) {
			_properties["intent-filter"].Remove(launcher);
		}
	}

	public bool IsLauncher {
		get {
			return _isLauncher;
		}
	}

	public string Name {
		get {
			return _name;
		}
	}

	public int Id {
		get {
			return _id;
		}
	}
}
