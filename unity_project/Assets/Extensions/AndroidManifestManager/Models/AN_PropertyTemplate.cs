using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class AN_PropertyTemplate : AN_BaseTemplate {
	public bool IsOpen = false;

	private string _tag = string.Empty;
	public AN_PropertyTemplate(string tag) : base() {
		_tag = tag;
	}

	public override void ToXmlElement (XmlDocument doc, XmlElement parent)
	{
		AddAttributesToXml (doc, parent, this);
		AddPropertiesToXml (doc, parent, this);
	}

	public string Tag {
		get {
			return _tag;
		}
	}


	public string Name {
		get {
			return GetValue("android:name");
		} 

		set {
			SetValue("android:name", value);
		}
	}


	public string Value {
		get {
			return GetValue("android:value");
		} 
		
		set {
			SetValue("android:value", value);
		}
	}


	public string Label {
		get {
			return GetValue("android:label");
		} 
		
		set {
			SetValue("android:label", value);
		}
	}
}
