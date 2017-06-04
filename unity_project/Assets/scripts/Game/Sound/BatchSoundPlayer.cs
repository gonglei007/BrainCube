using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BatchSoundPlayer : MonoBehaviour{

	public int                  		batchNumber;
	
	protected Action<BatchSoundPlayer>	OnFinished;
	protected AudioSource[]				audioSources;
	protected Queue<AudioClip>			audioClipQueue = new Queue<AudioClip>(10);
	protected float						interval;
	protected float						timer;
	protected bool						isPlaying = false;
	protected bool						isOnFinishedTriggled = false;

	
	public virtual bool Mute {
		get {
			return audioSources[0].mute;
		}
		
		set {
			foreach(AudioSource audioSource in audioSources)
			{
				audioSource.mute = value;
			}
		}
	}
	
	public virtual float Volume {
		get {
			return audioSources[0].volume;
		}
		set {
			foreach(AudioSource audioSource in audioSources)
			{
				audioSource.volume = value;
			}
		}
	}
	
	protected virtual void Awake() {
		if (batchNumber <= 0)
		{
			batchNumber = 1;
		}
		if (audioSources == null) {
			audioSources = new AudioSource[batchNumber];
			for(int i = 0 ; i < batchNumber ; i++) {
				audioSources[i] = gameObject.AddComponent<AudioSource>();
				audioSources[i].playOnAwake = false;
			}
		}
	}

	protected virtual void Update()
	{
		timer += Time.deltaTime;
		if (audioClipQueue.Count > 0)
		{
			if (timer > interval)
			{
				timer = 0;
				PlaySoundInBatch(audioClipQueue.Dequeue(), false);
			}
		}

		bool finished = true;
		foreach(AudioSource audioSource in audioSources){
			if (audioSource.isPlaying == true && audioSource.loop == false)
			{
				finished = false;
			}
		}

		if (finished && isPlaying)
		{
			Debug.Log("BatchSoundPlayer finished");
			isPlaying = false;
			if (OnFinished != null)
			{
				OnFinished(this);
			}
		}
	}
	
	public virtual void PlaySoundInBatch(AudioClip audioClip, bool loop){
		foreach(AudioSource audioSource in audioSources){
			if(audioSource.isPlaying == false){
				isPlaying = !loop;
				audioSource.loop = loop;
				audioSource.clip = audioClip;
				audioSource.Play();
				break;
			}
		}
	}

	public virtual void PlaySoundsInBatch(List<AudioClip> audioClips, float _interval, Action<BatchSoundPlayer> endCallback) {
		if (audioClips != null)
		{
			interval = _interval;
			this.OnFinished = endCallback;
			foreach(AudioClip audioClip in audioClips)
			{
				audioClipQueue.Enqueue(audioClip);
			}
			if (isPlaying == false)
			{
				timer = 0;
				foreach(AudioSource audioSource in audioSources){
					if(audioSource.isPlaying == false){
						isPlaying = true;
						audioSource.loop = false;
						audioSource.clip = audioClipQueue.Dequeue();
						audioSource.Play();
						break;
					}
				}
			}
		}
	}
	
	public virtual void StopSound(AudioClip audioClip) {
		foreach(AudioSource audioSource in audioSources){
			if(audioSource.isPlaying == true && audioSource.clip == audioClip){
				audioSource.Stop();
			}
		}
	}

	public virtual void StopAllSound()
	{
		foreach(AudioSource audioSource in audioSources){
			audioSource.Stop();
		}
	}
}
