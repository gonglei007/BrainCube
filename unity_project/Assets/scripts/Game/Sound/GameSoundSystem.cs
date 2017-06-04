using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSoundSystem : MonoBehaviour {
	private static GameSoundSystem instance;
	private static int[]	PAINO_CLIP_INDEXES = new int[]{2,3,4,5,6,7,
														11,12,13,14,15,16,17,
														21,22,23,24,25,26};

	public BatchSoundPlayer bgmSoundPlayer;
	public BaseSoundPlayer	commonSoundPlayer;
	public BatchSoundPlayer batchSoundPlayer;

	public AudioClip[]		countDownAudioClip;
	public AudioClip		flipWrongAudioClip;
	public AudioClip		cheerAudioClip;

	public AudioClip[]		bgmClips;
	
	private MusicBox		musicBox = new MusicBox();

	private List<MusicData> availableMusicDatas = new List<MusicData>(10);

	public static GameSoundSystem GetInstance()
	{
		return instance;
	}

	public bool Mute
	{
		get
		{
			return commonSoundPlayer.Mute;
		}
		set
		{
			commonSoundPlayer.Mute = value;
			batchSoundPlayer.Mute = value;
			bgmSoundPlayer.Mute = value;
		}
	}

	public List<MusicData> AvailableMusicDatas
	{
		get
		{
			return availableMusicDatas;
		}
	}

	public MusicBox MusicBox
	{
		get
		{
			return musicBox;
		}
	}

	void Awake()
	{
		instance = this;
		availableMusicDatas.AddRange(MusicData.gameMusicData);
	}

	void Start(){
		musicBox.Start ();
	}

	void Update(){
		musicBox.Update ();
	}

	public void ChooseMusic(){
		musicBox.Stop();
		musicBox.RandomMusic ();
		musicBox.Play ();
	}
	
	public void PlayCountDownSound(int index){
		commonSoundPlayer.PlayOneShot(countDownAudioClip[index]);
	}

	public void PlayCheerSound()
	{
		commonSoundPlayer.PlayOneShot(cheerAudioClip);
	}
		
	public void PlayFlipRightSound(){
		musicBox.AddPower ();
	}

	public void StopFlipRightSound(){
		musicBox.Stop();
		commonSoundPlayer.StopSound();
	}

	public void PlayFlipWrongSound(){
		commonSoundPlayer.PlayOneShot(flipWrongAudioClip);
		musicBox.ResetPower ();
	}

	public void PlayRandomSound()
	{
		int clipIndex = Random.Range(0, PAINO_CLIP_INDEXES.Length);
		AudioClip noteClip = Resources.Load("Sounds/game/sound_" + PAINO_CLIP_INDEXES[clipIndex].ToString()) as AudioClip;
		commonSoundPlayer.PlayOneShot(noteClip);
	}

	public void PlayBgmSound(int index)
	{
		bgmSoundPlayer.PlaySoundInBatch(bgmClips[index], true);
	}

	public void StopBgmSound(int index)
	{
		bgmSoundPlayer.StopSound(bgmClips[index]);
	}

	public void EnableGameMusic(int index)
	{
		MusicData musicData = MusicData.gameMusicData[index];
		if(availableMusicDatas.Contains(musicData) == false)
		{
			availableMusicDatas.Add(musicData);
		}
	}

	public void DisableGameMusic(int index)
	{
		MusicData musicData = MusicData.gameMusicData[index];
		if(availableMusicDatas.Contains(musicData))
		{
			availableMusicDatas.Remove(musicData);
		}
	}
}
