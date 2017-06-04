using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class AN_ManifestTemplate : AN_BaseTemplate {
	private AN_ApplicationTemplate _applicationTemplate = null;
	private List<AN_PropertyTemplate> _permissions = null; 

	public AN_ManifestTemplate() : base() {
		_applicationTemplate = new AN_ApplicationTemplate ();
		_permissions = new List<AN_PropertyTemplate> ();
	}

	public bool HasPermission(string name) {

		foreach(AN_PropertyTemplate permission in Permissions) {
			if(permission.Name.Equals(name)) {
				return true;
			}
		}

		return false;
	}


	public void RemovePermission(string name) {
		while(HasPermission(name)) {
			foreach(AN_PropertyTemplate permission in Permissions) {
				if(permission.Name.Equals(name)) {
					RemovePermission(permission);
					break;
				}
			}
		}
	}

	public void RemovePermission(AN_PropertyTemplate permission) {
		_permissions.Remove (permission);
	}


	public void AddPermission(string name) {
		if(!HasPermission(name)) {
			AN_PropertyTemplate uses_permission = new AN_PropertyTemplate("uses-permission");
			uses_permission.Name = name;
			AddPermission(uses_permission);
		}
	}
	

	public void AddPermission(AN_PropertyTemplate permission) {
		_permissions.Add (permission);
	}
	



	public override void ToXmlElement (XmlDocument doc, XmlElement parent) {
		AddAttributesToXml (doc, parent, this);
		AddPropertiesToXml (doc, parent, this);

		XmlElement app = doc.CreateElement ("application");
		_applicationTemplate.ToXmlElement (doc, app);
		parent.AppendChild (app);

		foreach (AN_PropertyTemplate permission in Permissions) {
			XmlElement p = doc.CreateElement("uses-permission");
			permission.ToXmlElement(doc, p);
			parent.AppendChild(p);
		}
	}

	public AN_ApplicationTemplate ApplicationTemplate {
		get {
			return _applicationTemplate;
		}
	}

	public List<AN_PropertyTemplate> Permissions {
		get {
			return _permissions;
		}
	}
}
