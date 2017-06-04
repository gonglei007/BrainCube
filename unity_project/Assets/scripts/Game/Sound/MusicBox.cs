using UnityEngine;
using System.Collections;

public class MusicBox{
	const float MAX_POWER = 30.0f;
	private float time = 0.0f;
	private int timeIndex = 0;
	private double timeLine = 0.0;
	private bool isPause = true;
	private float power = 0.0f;
	private MusicData		currentMusicData = null;
	private int				currentMusicIndex = -1;

	// Use this for initialization
	public void Start () {
		isPause = true;
	}
	
	// Update is called once per frame
	public void Update () {
		if (currentMusicData == null) {
			return;
		}
		if (isPause) {
			return;
		}
		if (power <= 0.0f) {
			time = (float)timeLine;
			return;
		}

		time += Time.deltaTime * (float)currentMusicData.speed;
		power -= Time.deltaTime * (float)currentMusicData.speed;
		if (time >= timeLine) {
			if(timeIndex >= currentMusicData.GetLength()){
				this.Reset();
				this.RandomMusic();
				this.Play();
				time += Time.deltaTime * (float)currentMusicData.speed;
			}
			timeLine += currentMusicData.timeLine[timeIndex];
			int noteIndex = currentMusicData.notes[timeIndex];
			AudioClip noteClip = null;
			if(noteIndex != 0){
				noteClip = Resources.Load("Sounds/game/sound_" + noteIndex.ToString()) as AudioClip;
				//Debug.Log(string.Format("timeIndex : {0} Note : {1}", timeIndex, noteIndex));
			}
			GameSoundSystem.GetInstance().commonSoundPlayer.PlayOneShot(noteClip);
			timeIndex++;
		}
	}

	public void RandomMusic(){
		int index = UnityEngine.Random.Range(0, GameSoundSystem.GetInstance().AvailableMusicDatas.Count);
		currentMusicIndex = index;
		currentMusicData = GameSoundSystem.GetInstance().AvailableMusicDatas[currentMusicIndex];
		Debug.Log(string.Format("New Music : {0}", TextManager.GetText(string.Format("game_music_name_{0}", currentMusicData.id))));
	}

	public void AddPower(){
		if (power >= MAX_POWER) {
			return;		
		}
		power += 5.0f;
		return;
		double targetTime = (double)(
			(((int)(timeLine) + 1) / currentMusicData.clap + 1) * currentMusicData.clap - 0.1
			);
//		Debug.Log ("time line:" + timeLine.ToString());
//		Debug.Log ("target time:" + targetTime.ToString());
		power = (float)(targetTime - time);
	}

	public void ResetPower(){
		power = 0.0f;
	}

	public float GetPowerPercent(){
		return (float)(power / MAX_POWER);
	}

	public void Play(){
		isPause = false;
	}

	public void Pause(){
		isPause = true;
	}

	public void Stop(){
		this.Reset ();
		isPause = true;
		power = 0.0f;
	}
	
	private void Reset(){
		time = 0.0f;
		timeIndex = 0;
		timeLine = 0.0;
	}
}
