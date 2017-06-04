using UnityEngine;
using System;
using System.Collections;

public class BaseSoundPlayer : MonoBehaviour {
	public Action<BaseSoundPlayer>	OnFinished;

	protected AudioSource		audioSource;
	protected bool				isPlaying = false;
	
	public virtual bool Mute {
		get {
			return audioSource.mute;
		}
		
		set {
			if (audioSource != null)
			{
				audioSource.mute = value;
			}
		}
	}
	
	public virtual float Volume {
		get {
			return audioSource.volume;
		}
		set {
			if (audioSource != null)
			{
				audioSource.volume = value;
			}
		}
	}
	
	protected virtual void Awake(){
		if (audioSource == null) {
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (isPlaying)
		{
			if (audioSource.isPlaying == false)
			{
				isPlaying = false;
				if (OnFinished != null)
				{
					OnFinished(this);
				}
			}
		}
	}
	
	public virtual void PlayOneShot(AudioClip audioClip) {
		isPlaying = true;
		if (audioClip != null) {
			audioSource.PlayOneShot(audioClip);
		}
	}
	
	public virtual void PlayOneShot(AudioClip audioClip, float volumnScale) {
		isPlaying = true;
		audioSource.PlayOneShot(audioClip, volumnScale);
	}
	
	public virtual void PlaySound(AudioClip audioClip, bool loop, bool fadeIn = false){
		if (fadeIn) {
			VolumeChange(0);
			AudioFadeIn();
		}
		else {
			VolumeChange(1);
		}
		isPlaying = !loop;
		audioSource.loop = loop;
		audioSource.clip = audioClip;
		audioSource.Play();
	}
	
	public virtual void StopSound(bool fadeOut = false) {
		if (fadeOut) {
			AudioFadeOut();
		}
		else {
			audioSource.Stop();
		}
	}
	
	protected virtual void AudioFadeIn(){
		iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", 2f, "onupdate", "VolumeChange", "oncompletetarget", this.gameObject));
	}

	protected virtual void AudioFadeOut(){
		iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", 2f, "onupdate", "VolumeChange", "oncomplete", "OnFadeOutComplete", "oncompletetarget", this.gameObject));
	}

	protected virtual void VolumeChange(float volume) {
		audioSource.volume = volume;
	}
	
	protected virtual void OnFadeOutComplete() {
		audioSource.Stop();
	}
}