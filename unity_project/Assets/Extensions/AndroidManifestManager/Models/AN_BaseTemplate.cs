using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public abstract class AN_BaseTemplate {
	protected Dictionary<string, List<AN_PropertyTemplate>> _properties = null;
	protected Dictionary<string, string> _values = null;



	public AN_PropertyTemplate GetOrCreateIntentFilterWithName(string name) {
		AN_PropertyTemplate filter = GetIntentFilterWithName(name);
		if(filter == null) {
			filter =  new AN_PropertyTemplate("intent-filter");
			AN_PropertyTemplate action = new AN_PropertyTemplate("action");
			action.SetValue("android:name", name);
			filter.AddProperty(action);
			AddProperty(filter);
		}

		return filter;
	}


	public AN_PropertyTemplate GetIntentFilterWithName(string name) {
		string tag = "intent-filter";
		List<AN_PropertyTemplate> filters =  GetPropertiesWithTag(tag);
		foreach(AN_PropertyTemplate intent_filter in filters) {
			string filter_name = GetIntentFilterName(intent_filter);
			if(filter_name.Equals(name)) {
				return intent_filter;
			}
		}

		return null;

	}


	public string GetIntentFilterName(AN_PropertyTemplate intent) {

		List<AN_PropertyTemplate> actions = intent.GetPropertiesWithTag("action");
		if(actions.Count > 0) {
			return actions[0].GetValue("android:name");
		} else {
			return string.Empty;
		}

	}

	public AN_PropertyTemplate GetOrCreatePropertyWithName(string tag, string name) {
		AN_PropertyTemplate p =  GetPropertyWithName(tag, name);
		if(p == null) {
			p = new AN_PropertyTemplate(tag);
			p.SetValue("android:name", name);
			AddProperty(p);
		}

		return p;
	}


	public AN_PropertyTemplate GetPropertyWithName(string tag, string name) {

		List<AN_PropertyTemplate> tags = GetPropertiesWithTag(tag);
		foreach(AN_PropertyTemplate prop in tags) {
			if(prop.Values.ContainsKey("android:name")) {
				if(prop.Values["android:name"] == name) {
					return prop;
				}
			}
		}


		return null;
	}


	public AN_PropertyTemplate GetOrCreatePropertyWithTag(string tag) {
		AN_PropertyTemplate p = GetPropertyWithTag(tag);
		if(p == null) {
			p = new AN_PropertyTemplate(tag);
			AddProperty(p);
		}

		return p;
	}


	public AN_PropertyTemplate GetPropertyWithTag(string tag) {
		List<AN_PropertyTemplate> props = GetPropertiesWithTag(tag);
		if(props.Count > 0) {
			return props[0];
		} else {
			return null;
		}
	}


	public List<AN_PropertyTemplate> GetPropertiesWithTag(string tag) {
		if(Properties.ContainsKey(tag)) {
			return Properties[tag];
		} else {
			return new List<AN_PropertyTemplate>();
		}

	} 
	
	public abstract void ToXmlElement(XmlDocument doc, XmlElement parent);

	public AN_BaseTemplate(){
		_values = new Dictionary<string, string> ();
		_properties = new Dictionary<string, List<AN_PropertyTemplate>> ();
	}
	

	public void AddProperty(AN_PropertyTemplate property) {
		AddProperty(property.Tag, property);
	}

	public void AddProperty(string tag, AN_PropertyTemplate property) {
		if (!_properties.ContainsKey(tag)) {
			List<AN_PropertyTemplate> list = new List<AN_PropertyTemplate>();
			_properties.Add(tag, list);
		}
		_properties [tag].Add (property);
	}
	
	public void SetValue(string key, string value) {
		if(_values.ContainsKey(key)) {
			_values[key] = value;
		} else {
			_values.Add (key, value);
		}
	}

	public string GetValue(string key) {
		if(_values.ContainsKey(key)) {
			return _values[key];
		} else {
			return string.Empty;
		}
	}

	public void RemoveProperty(AN_PropertyTemplate property) {
		_properties [property.Tag].Remove (property);
	}
	
	public void RemoveValue(string key) {
		_values.Remove (key);
	}

	public void AddPropertiesToXml(XmlDocument doc, XmlElement parent, AN_BaseTemplate template) {
		foreach (string key in template.Properties.Keys) {
			foreach (AN_PropertyTemplate p in template.Properties[key]) {
				XmlElement n = doc.CreateElement(key);
				AddAttributesToXml(doc, n, p);
				AddPropertiesToXml(doc, n, p);
				parent.AppendChild(n);
			}
		}
	}
	
	public void AddAttributesToXml(XmlDocument doc, XmlElement parent, AN_BaseTemplate template) {
		foreach (string key in template.Values.Keys) {

			string k = key;
			if (key.Contains("android:")) {
				k = key.Replace("android:", "android___");
			}
			XmlAttribute attr = doc.CreateAttribute (k);
			attr.Value = template.Values[key];

			parent.Attributes.Append(attr);
		}
	}

	public Dictionary<string, string> Values {
		get {
			return _values;
		}
	}
	
	public Dictionary<string, List<AN_PropertyTemplate>> Properties {
		get {
			return _properties;
		}
	}
}
