using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddressBookExample : MonoBehaviour {

	public SA_Label _name;
	public SA_Label _phone;
	public SA_Label _note;
	public SA_Label _email;
	public SA_Label _chat;
	public SA_Label _address;

	private List<AndroidContactInfo> all_contacts = new List<AndroidContactInfo>();

	private void LoadAdressBook() {
		AddressBookController.instance.LoadContacts ();
		AddressBookController.instance.OnContactsLoadedAction += OnContactsLoaded;		
	}

	
	void OnContactsLoaded () {
		AddressBookController.instance.OnContactsLoadedAction -= OnContactsLoaded;
		all_contacts = AddressBookController.instance.contacts;

		AN_PoupsProxy.showMessage("On Contacts Loaded" , "Andress book has " + all_contacts.Count + " Contacts");

		foreach(AndroidContactInfo info in all_contacts) {
			_name.text = "Name " + info.name;
			_phone.text = "Phone " + info.phone;
			_note.text = "Note " + info.note;
			_email.text = "Email " + info.email.email;
			_chat.text = "Chat.name " + info.chat.name;
			_address.text = "Country " + info.address.country;
							
			break;
		}
	}
}
