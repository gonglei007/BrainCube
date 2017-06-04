using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class TwitterPostingTask : AsyncTask {

	private string 		_status = "";
	private Texture2D 	_texture = null;

	private TwitterManagerInterface _controller;

	public static TwitterPostingTask Cretae() {
		return	new GameObject("TwitterPositngTask").AddComponent<TwitterPostingTask>();
	}


	public void Post(string status, Texture2D texture, TwitterManagerInterface controller) {
		_status = status;
		_texture = texture;
		_controller = controller;


		if(_controller.IsInited) {
			OnTWInited();
		} else {
			_controller.addEventListener(TwitterEvents.TWITTER_INITED, OnTWInited);
			_controller.Init();
		}

	}



	private void OnTWInited() {
		_controller.removeEventListener(TwitterEvents.TWITTER_INITED, OnTWInited);

		if(_controller.IsAuthed) {
			OnTWAuth();
		} else {
			_controller.addEventListener(TwitterEvents.AUTHENTICATION_FAILED, OnTWAuthFailed);
			_controller.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED, OnTWAuth);
			_controller.AuthenticateUser();
		}
	}
	
	
	private void OnTWAuth() {
		_controller.removeEventListener(TwitterEvents.AUTHENTICATION_FAILED, OnTWAuthFailed);
		_controller.removeEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED, OnTWAuth);


		_controller.addEventListener(TwitterEvents.POST_FAILED, 	OnPostFailed);
		_controller.addEventListener(TwitterEvents.POST_SUCCEEDED, 	OnPost);

		if(_texture != null) {
			_controller.Post(_status, _texture);
		} else  {
			_controller.Post(_status);
		}

	}


	private void OnTWAuthFailed() {
		_controller.removeEventListener(TwitterEvents.AUTHENTICATION_FAILED, OnTWAuthFailed);
		_controller.removeEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED, OnTWAuth);
		
		TWResult res =  new TWResult(false, "Auth failed");
		dispatch(BaseEvent.COMPLETE, res);
	}
	
	
	private void OnPost(CEvent e) {
		TWResult res = e.data as TWResult;
		dispatch(BaseEvent.COMPLETE, res);
	}
	
	private void OnPostFailed(CEvent e) {
		TWResult res = e.data as TWResult;
		dispatch(BaseEvent.COMPLETE, res);
	}



}
