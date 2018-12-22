using UnityEngine;
using System.Collections;

public class MusicManager{
	
	public static AudioSource bgMusicSource;
	public static AudioSource[] effectMusicSources;
	public static GameObject bgMusicObj;
	public static GameObject effectMusicObj;
	public static string audioPrefabPath = "Prefabs/Sound/";
	public static string musicPath = "Sounds/";
	public static string bgPrefabName = "Music_Bg";
	public static string effectPrefabName = "Music_Effect";
	public static bool isBgMute = false;
	public static bool isEffectMute = false;

	public static bool isCanPlayEffect = true;
	
	static string curMusicName = "";
	
	static void initBgObj(){

		bgMusicObj = Object.Instantiate(Resources.Load(audioPrefabPath + bgPrefabName) as GameObject) as GameObject;
		
		Object.DontDestroyOnLoad(bgMusicObj);
		bgMusicSource = bgMusicObj.GetComponent<AudioSource>();
		bgMusicSource.mute = isBgMute;
	}
	
	static void initEftObj(){
		effectMusicObj = Object.Instantiate(Resources.Load(audioPrefabPath + effectPrefabName) as GameObject) as GameObject;
		Component[] audioSourceList = effectMusicObj.GetComponents<AudioSource>();
		int count = audioSourceList.Length;
		effectMusicSources = new AudioSource[count];
		for(int i=0; i<count; i++){
			effectMusicSources[i] = audioSourceList[i] as AudioSource;
			effectMusicSources[i].mute = isEffectMute;
		}
	}
	
	public static AudioClip getAudioClipByName(string name){
		return Resources.Load(musicPath + name) as AudioClip;
		//Debug.Log("");
		//Debug.Log("musicPath ======================== " + musicPath + name);
		//Debug.Log("");
		
	}
	
	public static void playBgMusic(string musicName){
		
		if(null == bgMusicObj){
			AudioSettings.speakerMode = AudioSpeakerMode.Raw;
			System.Threading.Thread.Sleep(0);
			AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
			initBgObj();
		}
		if(curMusicName != musicName){
			
			bgMusicSource.loop = true;
			clearClip(bgMusicSource);
			bgMusicSource.clip = getAudioClipByName(musicName);
			bgMusicSource.Play();
		}

		curMusicName = musicName;

		bgMusicSource.volume = PlayerInfo.getInstance().musicBgVolume;
//		Debug.Log("curMusicName ====== " + curMusicName);
	}
	
	public static void bgMusicFade(bool isIn,float time){
		if(null == bgMusicObj){
			return;
		}
		if(isIn){
			bgMusicSource.volume = 1f;
//			iTween.AudioFrom(bgMusicObj,0,1,time);
		}else{
//			iTween.AudioTo(bgMusicObj,0,1,time);
		}
	}
	
	public static void playEndMusic(string musicName){
		if(null == bgMusicObj){
			initBgObj();
		}
		clearClip(bgMusicSource);
		bgMusicSource.loop = false;
		bgMusicSource.clip = getAudioClipByName(musicName);
		bgMusicSource.Play();
	}
	
	public static void clearClip(AudioSource audio){
		if(null != audio.clip){
			audio.Stop();
			Resources.UnloadAsset(audio.clip);
		}
	}
	
	public static int playEffectMusic(string musicName,bool isLoop = false,bool changeSpeed = false){
		if(PlayerInfo.getInstance().soundEffVolume <= 0)
		{
			return -1;
		}
		if(isCanPlayEffect){
			int audioIndex = getAudioIndex();
			AudioSource tempAudio = effectMusicSources[audioIndex];
			clearClip(tempAudio);
			tempAudio.clip = getAudioClipByName(musicName);
			tempAudio.loop = isLoop;
			if(changeSpeed)
			{
				tempAudio.pitch = Time.timeScale;
			}
			else
			{
				tempAudio.pitch = 1;
			}
			tempAudio.Play();
			return audioIndex;
		}else{
			return -1;
		}
	}
	//播放音效表音效  //
	public static int playEffectById(int formId, bool isLoop = false)
	{
		SeData sd=SeData.getData(formId);
		if(sd!=null)
		{
			string effectName = sd.music;
			int index = playEffectMusic(effectName);
			return index;
		}
		return 0;
	}
	
	//播放模型音效， state 0 动作音效， 1 死亡音效//
	public static int playCardSoundEffect(int formId, int state, bool isLoop = false)
	{
		string effectName = "";
		CardData cd = CardData.getData(formId);
		if(cd != null)
		{
			switch(state)
			{
			case 0:			//动作音效//
				effectName = CardData.getData(formId).chargeVoice;
				break;
			case 1:			//死亡音效//
				effectName = CardData.getData(formId).hurtVoice;
				
				break;
			}
			
			int index = playEffectMusic(effectName);
			return index;
		}
		else 
		{
			return -1;
		}
	}
	
	//播放技能音效， state 0 蓄力音效， 1 释放音效， 2 受伤音效//
	public static int playSkillSoundEffect(int formId, int state, bool isLoop = false)
	{
		string effectName = "";
		bool changeSpeed = false;
		switch(state)
		{
		case 0:			//蓄力音效//
			effectName = SkillData.getData(formId).chargemusic;
			break;
		case 1:			//释放音效//
			effectName = SkillData.getData(formId).music;
			changeSpeed = true;
			break;
			
		case 2:			//受伤特效//
			
			effectName = SkillData.getData(formId).hurtMusic;
			break;
		}
		
		int index = playEffectMusic(effectName,false,changeSpeed);
		return index;
	}
	
	//播放合体技能音效， state 0 读取muisc数据， 1 读取actionMuisc对应的数据//
	public static int playUniteSkillSoundEffect(int formId, int state, bool isLoop = false)
	{
		string effectName = "";
		if(state == 0)
		{
			effectName = UnitSkillData.getData(formId).music;
		}
		else if(state == 1)
		{
			effectName = UnitSkillData.getData(formId).actionMusic;
		}
		
		int index = playEffectMusic(effectName,false,true);
		return index;
	}
	
	public static int playUniteSkillSoundEffByName(string name, int state, bool isLoop = false)
	{
		int index = playEffectMusic(name);
		return index;
	}
	
	
	public static void cancleLoop(int audioIndex){
		if(isCanPlayEffect){
			AudioSource tempAudio = effectMusicSources[audioIndex];
			tempAudio.loop = false;
			tempAudio.clip = null;
		}
	}

	public static bool isPlayByIndex(int audioIndex){
		AudioSource tempAudio = effectMusicSources[audioIndex];
		return tempAudio.isPlaying;
	}
	
	public static int getAudioIndex(){
		if(null == effectMusicObj){
			initEftObj();
		}
		int count = effectMusicSources.Length;
		for(int i=0; i<count; i++){
			AudioSource tempAudio = effectMusicSources[i];
			if(!tempAudio.isPlaying){
				return i;
			}
		}
		return 0;
	}
	
	public static void muteAll(){
		muteBgMusic();
		muteEffectMusic();
	}
	
	public static void pauseBgMusic(){
		if(null != bgMusicObj){
			bgMusicSource.Pause();
		}
	}
	
	public static void muteBgMusic(){
		if(null == bgMusicObj){
			initBgObj();
		}
		bgMusicSource.mute = true;
		isBgMute = true;
	}
	
	public static void muteEffectMusic(){
		if(null == effectMusicObj){
			initEftObj();
		}
		int count = effectMusicSources.Length;
		for(int i=0; i<count; i++){
			effectMusicSources[i].mute = true;
		}
		isEffectMute = true;
	}
	
	public static void cancelMuteAll(){
		cancelMuteBgMusic();
		cancelMuteEffectMusic();
	}
	
	public static void cancelMuteBgMusic(){
		if(null == bgMusicObj){
			initBgObj();
		}
		bgMusicSource.mute = false;
		isBgMute = false;
	} 
	
	public static void cancelMuteEffectMusic(){
		if(null == effectMusicObj){
			initEftObj();
		}
		int count = effectMusicSources.Length;
		for(int i=0; i<count; i++){
			effectMusicSources[i].mute = false;
		}
		isEffectMute = false;
	}
	
	public static void bgVolume(float vol){
		if(null == bgMusicObj){
			initBgObj();
		}
		bgMusicSource.volume = vol;
	}
	
	public static void effectVolume(float vol){
		if(null == effectMusicObj){
			initEftObj();
		}
		int count = effectMusicSources.Length;
		for(int i=0; i<count; i++){
			effectMusicSources[i].volume = vol;
		}
	}
	
	public static void resetSource(){
		bgMusicSource.clip = null;
		int count = effectMusicSources.Length;
		for(int i=0; i<count; i++){
			effectMusicSources[i].clip = null;
		}
	}
	
	public static void clearUnuseAudio(){
		int count = effectMusicSources.Length;
		for(int i=0; i<count; i++){
			if(null != effectMusicSources[i].clip && !effectMusicSources[i].isPlaying){
				effectMusicSources[i].clip = null;
			}
		}
	}
}
