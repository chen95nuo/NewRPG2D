//#define ENABLE_AUDIO_MANAGER_TEST // 打开这个宏可以开启调试界面
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 音乐音效管理类, 内部使用对象管理池.
/// </summary>
public class AudioManager : SysModule, SysSceneManager.ISceneManagerListener
{
	public static AudioManager Instance { get { return SysModuleManager.Instance.GetSysModule<AudioManager>(); } }

	public delegate void PlayAudioDelegate(object userData);

	private string baseSoundPath = GameDefines.soundPath;
	private string baseMusicPath = GameDefines.musicPath;

	private bool soundMuted = false;
	/// <summary>
	/// 音效静音开关
	/// </summary>
	public bool SoundMuted
	{
		get { return soundMuted; }
		set
		{
			if (soundMuted != value)
			{
				soundMuted = value;
				UpdateAudioVolume(false);
			}
		}
	}

	private float soundVolume = 1.0f;
	/// <summary>
	/// 音效音量
	/// </summary>
	public float SoundVolume
	{
		get { return soundVolume; }
		set
		{
			if (soundVolume != value)
			{
				soundVolume = value;
				UpdateAudioVolume(false);
			}
		}
	}

	private float CurrentSoundVolume
	{
		get { return soundMuted ? 0 : soundVolume; }
	}

	private bool musicMuted = false;
	/// <summary>
	/// 音乐静音开关
	/// </summary>
	public bool MusicMuted
	{
		get { return musicMuted; }
		set
		{
			if (musicMuted != value)
			{
				musicMuted = value;
				UpdateAudioVolume(true);
			}
		}
	}

	private float musicVolume = 1.0f;
	/// <summary>
	/// 音乐音量
	/// </summary>
	public float MusicVolume
	{
		get { return musicVolume; }
		set
		{
			if (musicVolume != value)
			{
				musicVolume = value;
				UpdateAudioVolume(true);
			}
		}
	}

	private float CurrentMusicVolume
	{
		get { return musicMuted ? 0 : musicVolume; }
	}

	private enum PlayState
	{
		NotStart,
		Fadein,
		Normal,
		FadeOut,
		End,
	}

	private class AudioData
	{
		/// <summary>
		/// 是否是音乐类型
		/// </summary>
		public bool isMusic;

		/// <summary>
		/// 声音资源
		/// </summary>
		public AudioSource audioSource;

		/// <summary>
		/// 创建在第几帧
		/// </summary>
		public float createdFrameCount;

		/// <summary>
		/// 创建的时间, 不受TimeScale影响
		/// </summary>
		public float createdTime;

		/// <summary>
		/// 播放装
		/// </summary>
		public PlayState playState;

		/// <summary>
		/// 音量
		/// </summary>
		public float volume;

		/// <summary>
		/// 用于延迟计算的中间计数
		/// </summary>
		public float delay;

		/// <summary>
		/// 淡入淡出时间
		/// </summary>
		public float fadeTime;

		/// <summary>
		/// 淡入淡出开始时间
		/// </summary>
		public float fadeStartTime;

		/// <summary>
		/// 淡入淡出当前音效
		/// </summary>
		public float fadeVolume;

		/// <summary>
		/// 循环播放
		/// </summary>
		public bool loop;

		/// <summary>
		/// 播放回调
		/// </summary>
		public PlayAudioDelegate endDel;
		public object userData;

		public bool IsMusic
		{
			get { return isMusic; }
		}

		public float CurrentVolume
		{
			get { return playState == PlayState.Fadein || playState == PlayState.FadeOut ? fadeVolume : volume; }
		}

		public void Reset()
		{
			isMusic = false;
			audioSource.clip = null;
			audioSource.volume = 0;
			audioSource.ignoreListenerPause = false;
			audioSource.ignoreListenerVolume = false;
			audioSource.loop = false;
			createdFrameCount = 0;
			createdTime = 0;
			playState = PlayState.NotStart;
			volume = 0;
			delay = 0;
			fadeTime = 0;
			fadeStartTime = 0;
			fadeVolume = 0;
			endDel = null;
			userData = null;
		}

		public void UpdateVolume(float globalVolume)
		{
			if (audioSource == null)
				return;

			audioSource.volume = globalVolume * CurrentVolume;
		}
	}

	private AudioListener audioListener;
	private Dictionary<string, AudioData> persistentAudios = new Dictionary<string, AudioData>();
	private List<AudioData> spawnedAudio = new List<AudioData>();
	private List<AudioData> despawnedAudio = new List<AudioData>();
	private List<AudioData> audioTempList = new List<AudioData>();

	private class LoadingAudio
	{
		public WWW www;
		public string audioName;
		public bool isSound;
		public bool useStream;
		public AudioClip audioClip;
		public PlayAudioDelegate playAudioDelegate;
		public bool taskStarted;

		public LoadingAudio(WWW www, string audioName, bool isSound, PlayAudioDelegate playAudioDelegate)
			: this(www, audioName, true, isSound, playAudioDelegate)
		{

		}

		public LoadingAudio(WWW www, string audioName, bool useStream, bool isSound, PlayAudioDelegate playAudioDelegate)
		{
			this.www = www;
			this.audioName = audioName;
			this.useStream = useStream;
			this.isSound = isSound;
			this.audioClip = null;
			this.playAudioDelegate = playAudioDelegate;
		}
	}
	private List<LoadingAudio> loadingAudioList = new List<LoadingAudio>();

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		// Create audio listener
		audioListener = gameObject.GetComponent<AudioListener>();
		if (audioListener == null)
			audioListener = gameObject.AddComponent<AudioListener>();
		AudioListener.volume = 1;

		if (SysPrefs.Instance != null)
		{
			var sysPrefs = SysPrefs.Instance;

			sysPrefs.SetSndVlmCb(SoundVolumeChangedDel);
			sysPrefs.SetMscVlmCb(MusicVolumeChangedDel);

			sysPrefs.MusicOn = sysPrefs.MusicOn;
			sysPrefs.SoundOn = sysPrefs.SoundOn;
		}

		return true;
	}

	public override void Dispose()
	{
		Destroy(audioListener);

		foreach (var item in spawnedAudio)
			Destroy(item.audioSource.gameObject);
		spawnedAudio.Clear();

		foreach (var item in despawnedAudio)
			Destroy(item.audioSource.gameObject);
		despawnedAudio.Clear();
	}

	public void FreeMemory()
	{
		foreach (var item in despawnedAudio)
			Destroy(item.audioSource.gameObject);
		despawnedAudio.Clear();
	}

	//public AudioSource GetPersistentSound(string audioName, float volume, bool ignoreListenerPause, bool ignoreListenerVolume)
	//{
	//    if (persistentAudios.ContainsKey(audioName))
	//        return persistentAudios[audioName].audioSource;

	//    //var audioClip = ResourceManager.Instance.LoadAsset<AudioClip>(KodGames.PathUtility.Combine(baseSoundPath, audioName));
	//    audioName = System.IO.Path.Combine(baseSoundPath, audioName);
	//    var audioClip = ResourceManager.Instance.LoadStreamingAudio(audioName).audioClip;
	//    if (audioClip != null)
	//        audioClip.name = audioName;
	//    else
	//        return null;

	//    var audioData = CreateAudioData();
	//    audioData.audioSource.clip = audioClip;
	//    audioData.audioSource.ignoreListenerPause = ignoreListenerPause;
	//    audioData.audioSource.ignoreListenerVolume = ignoreListenerVolume;
	//    audioData.audioSource.loop = false;
	//    audioData.volume = 1;

	//    audioData.UpdateVolume(CurrentSoundVolume);

	//    persistentAudios.Add(audioName, audioData);

	//    return audioData.audioSource;
	//}

	//public AudioSource GetPersistentSound(string audioName)
	//{
	//    return GetPersistentSound(audioName, 1, false, false);
	//}

	public bool IsSoundPlaying(string soundName)
	{
		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var item = spawnedAudio[i];
			if (item.IsMusic == false
				&& item.audioSource.clip.name.Equals(soundName, System.StringComparison.CurrentCultureIgnoreCase))
				return item.playState != PlayState.FadeOut && item.playState != PlayState.End;
		}

		for (int i = 0; i < loadingAudioList.Count; ++i)
		{
			var loadingAudio = loadingAudioList[i];
			if (loadingAudio != null && loadingAudio.isSound && loadingAudio.audioName.Equals(soundName))
				return true;
		}

		return false;
	}

	public void PlaySound(AudioClip audioClip, float volume, bool ignoreListenerPause, bool ignoreListenerVolume,
						  float delay, PlayAudioDelegate endDel, object userData)
	{
		if (audioClip == null)
			return;

		// Combine the sound played in the same frame
		var batchingSound = GetBatchingSound(audioClip, delay, Time.frameCount, userData);
		if (batchingSound != null)
		{
			batchingSound.volume = Mathf.Max(volume, batchingSound.volume);
			batchingSound.endDel += endDel;

			batchingSound.UpdateVolume(CurrentSoundVolume);
		}
		else
		{
			var audioData = SpawnAudio();
			audioData.isMusic = false;
			audioData.audioSource.clip = audioClip;
			audioData.audioSource.ignoreListenerPause = ignoreListenerPause;
			audioData.audioSource.ignoreListenerVolume = ignoreListenerVolume;
			audioData.loop = false;
			audioData.createdFrameCount = Time.frameCount;
			audioData.createdTime = Time.time;
			audioData.volume = volume;
			audioData.delay = delay;
			audioData.endDel += endDel;
			audioData.userData = userData;

			audioData.UpdateVolume(CurrentSoundVolume);

			if (delay == 0)
				audioData.audioSource.Play();
		}
	}

	public void PlaySound(AudioClip audioClip, float volume, float delay, PlayAudioDelegate endDel, object userData)
	{
		PlaySound(audioClip, volume, false, false, delay, endDel, userData);
	}

	public void PlaySound(AudioClip audioClip, float volume, float delay)
	{
		PlaySound(audioClip, volume, delay, null, null);
	}

	public void PlaySound(AudioClip audioClip, float delay)
	{
		PlaySound(audioClip, 1, delay, null, null);
	}

	public void PlaySound(string audioName, float volume, bool ignoreListenerPause, bool ignoreListenerVolume,
						  float delay, PlayAudioDelegate endDel, object userData)
	{
		// Load sound clip
		var audioClip = ResourceManager.Instance.LoadAsset<AudioClip>(KodGames.PathUtility.Combine(baseSoundPath, audioName));
		if (audioClip != null)
			audioClip.name = audioName;

		PlaySound(audioClip, volume, ignoreListenerPause, ignoreListenerVolume, delay, endDel, userData);
	}

	public void PlaySound(string soundName, float volume, float delay, PlayAudioDelegate endDel, object userData)
	{
		PlaySound(soundName, volume, false, false, delay, endDel, userData);
	}

	public void PlaySound(string soundName, float volume, float delay)
	{
		PlaySound(soundName, volume, delay, null, null);
	}

	public void PlaySound(string soundName, float delay)
	{
		PlaySound(soundName, 1, delay, null, null);
	}

	public void PlayStreamSound(string soundName, float volume, float delay)
	{
		for (int i = 0; i < loadingAudioList.Count; i++)
		{
			if (loadingAudioList[i].audioClip != null && loadingAudioList[i].isSound && loadingAudioList[i].audioName.Equals(soundName))
			{
				PlaySound(loadingAudioList[i].audioClip, volume, delay);
				return;
			}
		}

		string name = KodGames.PathUtility.Combine(false, baseSoundPath, soundName);
		var www = ResourceManager.Instance.LoadStreamingAudio(name);
		loadingAudioList.Add(new LoadingAudio(
			www,
			soundName,
			true,
			new PlayAudioDelegate((data) =>
			{
				PlaySound(data as AudioClip, volume, false, false, delay, null, null);
			})));
	}

	public void PlayStreamSound(string soundName, float delay)
	{
		PlayStreamSound(soundName, 1, delay);
	}

	public void StopSound(string soundName)
	{
		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var audioData = spawnedAudio[i];
			if (audioData.IsMusic == false
				&& audioData.audioSource != null
				&& audioData.audioSource.clip != null
				&& audioData.audioSource.clip.name.Equals(soundName))
				StopAudio(audioData);
		}

		// Destroy end music
		DespawnEndAudios();

		for (int i = loadingAudioList.Count - 1; i >= 0; i--)
		{
			var loadingAudio = loadingAudioList[i];
			if (loadingAudio != null && loadingAudio.isSound == true && loadingAudio.audioName.Equals(soundName))
				loadingAudioList.RemoveAt(i);
		}
	}

	public bool IsMusicPlaying(string soundName)
	{
		for (int i = 0; i < loadingAudioList.Count; ++i)
		{
			var loadingAudio = loadingAudioList[i];
			if (loadingAudio != null && loadingAudio.isSound == false && loadingAudio.audioName.Equals(soundName))
				return true;
		}

		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var item = spawnedAudio[i];
			if (item.IsMusic
				&& item.playState != PlayState.FadeOut
				&& item.playState != PlayState.End
				&& item.audioSource.clip.name.Equals(soundName, System.StringComparison.CurrentCultureIgnoreCase))
				return true;
		}

		return false;
	}

	public void PlayMusic(AudioClip audioClip, bool loop, float volume, bool ignoreListenerPause, bool ignoreListenerVolume, float delay, float fadeTime)
	{
		PlayMusic(audioClip, loop, volume, ignoreListenerPause, ignoreListenerVolume, delay, fadeTime, true);
	}

	public void PlayMusic(AudioClip audioClip, bool loop, float volume, bool ignoreListenerPause, bool ignoreListenerVolume, float delay, float fadeTime, bool checkIsPlaying)
	{
		if (audioClip == null)
			return;

		if (checkIsPlaying && IsMusicPlaying(audioClip.name))
			return;

		fadeTime = Mathf.Max(0, fadeTime);

		var audioData = SpawnAudio();
		audioData.isMusic = true;
		audioData.audioSource.clip = audioClip;
		audioData.audioSource.ignoreListenerPause = ignoreListenerPause;
		audioData.audioSource.ignoreListenerVolume = ignoreListenerVolume;
		audioData.loop = loop;
		audioData.createdFrameCount = Time.frameCount;
		audioData.createdTime = Time.time;
		audioData.volume = volume;
		audioData.delay = delay;
		audioData.fadeTime = fadeTime;

		audioData.UpdateVolume(CurrentMusicVolume);

		if (delay == 0)
			audioData.audioSource.Play();
	}

	public void PlayMusic(AudioClip audioClip, bool loop, float delay, float fadeTime)
	{
		PlayMusic(audioClip, loop, 1, false, false, delay, fadeTime);
	}

	public void PlayMusic(string audioName, bool loop, float volume, bool ignoreListenerPause, bool ignoreListenerVolume, float delay, float fadeTime)
	{
		// Load sound clip
		string name = KodGames.PathUtility.Combine(false, baseMusicPath, audioName);
		var www = ResourceManager.Instance.LoadStreamingAudio(name);
		loadingAudioList.Add(new LoadingAudio(
			www,
			audioName,
			false,
			new PlayAudioDelegate((data) =>
			{
				PlayMusic(data as AudioClip, loop, volume, ignoreListenerPause, ignoreListenerVolume, delay, fadeTime, false);
			})));
	}

	public void PlayMusic(string audioName, bool loop, float delay, float fadeTime)
	{
		PlayMusic(audioName, loop, 1, false, false, delay, fadeTime);
	}

	public void PlayMusic(string audioName, bool loop)
	{
		PlayMusic(audioName, loop, 1, false, false, 0, 0);
	}

	public void StopMusic(float fadeTime)
	{
		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var audioData = spawnedAudio[i];
			if (audioData.IsMusic)
				StopMusic(audioData, fadeTime);
		}

		// Destroy end music
		DespawnEndAudios();

		for (int i = loadingAudioList.Count - 1; i >= 0; i--)
		{
			var loadingAudio = loadingAudioList[i];
			if (loadingAudio != null && loadingAudio.isSound == false)
				loadingAudioList.RemoveAt(i);
		}
	}

	public void CrossFadeMusic(string audioName, bool loop, float fadeTime)
	{
		StopMusic(fadeTime);
		PlayMusic(audioName, loop, 0, fadeTime);
	}

	public void StopMusic()
	{
		StopMusic(0);
	}

	private void StopMusic(AudioData audioData, float fadeTime)
	{
		if (audioData.audioSource == null || audioData.audioSource.clip == null)
			return;

		// Not stated, destroy directly
		if (audioData.playState == PlayState.NotStart)
			StopAudio(audioData);

		if (audioData.playState == PlayState.End)
			return;

		// Skip fading out music
		if (audioData.playState == PlayState.FadeOut)
			return;

		// Playing
		if (fadeTime != 0)
		{
			audioData.volume = audioData.CurrentVolume;
			audioData.playState = PlayState.FadeOut;
			audioData.fadeTime = fadeTime;
			audioData.fadeStartTime = Time.time;
			audioData.fadeVolume = audioData.volume;
		}
		else
		{
			// Stop directly
			StopAudio(audioData);
		}
	}

	public void StepMusicToNormalState(string audioName)
	{
		foreach (var item in spawnedAudio)
			Debug.Log(item.audioSource.clip.name);

		AudioData playingMusic = null;
		foreach (var item in spawnedAudio)
			if (item.IsMusic
				&& item.playState != PlayState.FadeOut
				&& item.playState != PlayState.End
				&& item.audioSource.clip.name.Equals(audioName, System.StringComparison.CurrentCultureIgnoreCase))
			{
				playingMusic = item;
				break;
			}

		if (playingMusic == null || playingMusic.playState == PlayState.Normal)
			return;

		// Calculate time
		float stepTime = playingMusic.createdTime + playingMusic.delay + playingMusic.fadeTime - Time.time;
		Debug.Assert(stepTime >= 0);
		if (stepTime < 0)
			return;

		// Revert music
		foreach (var item in spawnedAudio)
			if (item.IsMusic)
			{
				item.createdTime -= stepTime;
				if (item.fadeStartTime != 0)
					item.fadeStartTime -= stepTime;
			}
	}

	#region Pool
	private AudioData CreateAudioData()
	{
		var audioData = new AudioData();
		audioData.audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
		audioData.audioSource.playOnAwake = false;
		ObjectUtility.AttachToParentAndResetLocalTrans(this.gameObject, audioData.audioSource.gameObject);

		DontDestroyOnLoad(audioData.audioSource.gameObject);

		return audioData;
	}

	private AudioData SpawnAudio()
	{
		AudioData item = null;
		if (despawnedAudio.Count != 0)
		{
			// Use old
			item = despawnedAudio[despawnedAudio.Count - 1];
			despawnedAudio.RemoveAt(despawnedAudio.Count - 1);
		}
		else
		{
			// No cached, create new one
			item = CreateAudioData();
		}

		// Add to spawned list
		spawnedAudio.Add(item);

		// Prepare to play
		item.Reset();

		return item;
	}

	private void DespawnAudio(AudioData audioData)
	{
		// Reset data
		audioData.audioSource.Stop();
		audioData.audioSource.clip = null;
		audioData.playState = PlayState.NotStart;
		audioData.endDel = null;
		audioData.userData = null;

		// Return to pool
		spawnedAudio.Remove(audioData);
		despawnedAudio.Add(audioData);
	}

	private void DespawnEndAudios()
	{
		audioTempList.Clear();

		// Destroy end music
		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var audioData = spawnedAudio[i];
			if (audioData.playState == PlayState.End)
				audioTempList.Add(audioData);
		}

		for (int i = 0; i < audioTempList.Count; ++i)
			DespawnAudio(audioTempList[i]);

		audioTempList.Clear();
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayAudio(LoadingAudio loadingAudio)
	{
		loadingAudio.taskStarted = true;

		if (loadingAudio.www == null)
		{
			yield break;
		}

		yield return loadingAudio.www;

		// Set Task Status.
		if (string.IsNullOrEmpty(loadingAudio.www.error) == false)
		{
			yield break;
		}

		bool useStream = loadingAudio.useStream;
		if (loadingAudio.www.GetAudioClip(false, useStream) == null)
		{
			yield break;
		}

		loadingAudio.audioClip = loadingAudio.www.GetAudioClip(false, useStream);
		loadingAudio.audioClip.name = loadingAudio.audioName;

		while (loadingAudio.www.GetAudioClip(false, useStream).isReadyToPlay == false)
			yield return null;

		loadingAudio.playAudioDelegate(loadingAudio.audioClip);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		// Update 
		for (int i = 0; i < loadingAudioList.Count; ++i)
		{
			var loadingAudio = loadingAudioList[i];
			if (loadingAudio == null)
				continue;

			if (loadingAudio.taskStarted == false)
				StartCoroutine("PlayAudio", loadingAudio);
		}

		// Update
		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var audioData = spawnedAudio[i];

			if (audioData.audioSource == null)
				continue;

			if (audioData.IsMusic)
				UpdateMusic(audioData);
			else
				UpdateSound(audioData);
		}

		// Destroy end music
		DespawnEndAudios();
	}

	private void UpdateSound(AudioData audioData)
	{
		// Not started, check to start
		if (audioData.playState == PlayState.NotStart)
		{
			// Update delay
			if (audioData.audioSource.isPlaying == false && Time.time - audioData.createdTime >= audioData.delay)
				audioData.audioSource.Play();

			if (audioData.audioSource.isPlaying)
				audioData.playState = PlayState.Normal;
		}

		// Playing, check end
		if (audioData.playState == PlayState.Normal && audioData.audioSource.isPlaying == false)
			StopAudio(audioData);
	}

	private void UpdateMusic(AudioData audioData)
	{
		// Not started, check to start fade-in
		if (audioData.playState == PlayState.NotStart)
		{
			// Update delay
			if (audioData.audioSource.isPlaying == false && Time.time - audioData.createdTime >= audioData.delay)
				audioData.audioSource.Play();

			if (audioData.audioSource.isPlaying)
			{
				if (audioData.fadeTime != 0)
				{
					// Start fade in
					audioData.playState = PlayState.Fadein;
					audioData.fadeStartTime = audioData.createdTime + audioData.delay;
				}
				else
				{
					audioData.playState = PlayState.Normal;
				}
			}
		}

		// Update fade-in
		if (audioData.playState == PlayState.Fadein)
		{
			// Update volume
			if (Time.time - audioData.fadeStartTime < audioData.fadeTime)
			{
				audioData.fadeVolume = Mathf.Lerp(0, audioData.volume, (Time.time - audioData.fadeStartTime) / audioData.fadeTime);
			}
			else
			{
				// Fade-in finished
				audioData.fadeTime = 0;
				audioData.fadeVolume = audioData.volume;
				audioData.playState = PlayState.Normal;
			}

			// Update volume according to music volume
			audioData.UpdateVolume(CurrentMusicVolume);
		}

		// Update fade-out
		if (audioData.playState == PlayState.FadeOut)
		{
			// Update volume
			if (Time.time - audioData.fadeStartTime < audioData.fadeTime)
			{
				audioData.fadeVolume = Mathf.Lerp(audioData.volume, 0, (Time.time - audioData.fadeStartTime) / audioData.fadeTime);

				// Update volume according to music volume
				audioData.UpdateVolume(CurrentMusicVolume);
			}
			else
			{
				// Fade-in finished
				StopAudio(audioData);
			}
		}

		// Loop Audio.
		if (audioData.playState == PlayState.Normal)
		{
			if (audioData.loop && !audioData.audioSource.isPlaying)
				audioData.playState = PlayState.NotStart;
		}
	}

	private void StopAudio(AudioData audioData)
	{
		if (audioData.audioSource != null)
			audioData.audioSource.Stop();

		audioData.playState = PlayState.End; // Mark as end and delete later
		if (audioData.endDel != null)
			audioData.endDel(audioData.userData);
	}

	private void UpdateAudioVolume(bool music)
	{
		foreach (var item in spawnedAudio)
			if (item.IsMusic == music)
				item.UpdateVolume(music ? CurrentMusicVolume : CurrentSoundVolume);

		if (music == false)
			foreach (var kvp in persistentAudios)
				kvp.Value.UpdateVolume(CurrentSoundVolume);
	}

	private AudioData GetBatchingSound(AudioClip audioClip, float delay, int createdframeCount, object userData)
	{
		foreach (var item in spawnedAudio)
			if (item.IsMusic == false
				&& item.audioSource.clip == audioClip
				&& Mathf.Approximately(item.delay, delay)
				&& item.createdFrameCount == createdframeCount
				&& item.userData == userData)
				return item;

		return null;
	}

	private void SoundVolumeChangedDel(float volume)
	{
		SoundVolume = volume;
	}

	private void MusicVolumeChangedDel(float volume)
	{
		MusicVolume = volume;
	}

	public void OnSceneWillChange(SysSceneManager manager, string currentScene, string newScene)
	{
		StopCoroutine("PlayAudio");

		for (int i = 0; i < spawnedAudio.Count; ++i)
		{
			var audioData = spawnedAudio[i];

			if (audioData.IsMusic)
				StopMusic(audioData, 0);
			else
				StopAudio(audioData);

			DespawnAudio(audioData);
		}

		// Clear LoadingAudioList.
		foreach (var audio in loadingAudioList)
		{
			audio.taskStarted = true;
			audio.audioClip = null;
			audio.www.Dispose();
			audio.www = null;
		}
		loadingAudioList.Clear();
	}

	public void OnSceneChanged(SysSceneManager manager, string oldScene, string currentScene)
	{
	}

#if ENABLE_AUDIO_MANAGER_TEST
	void Start()
	{
		Initialize();
		SysModuleManager.Instance.Initialize(gameObject);
		SysModuleManager.Instance.AddSysModule<ResourceManager>(true);
	}

	void Update()
	{
		OnUpdate();
	}

	void OnGUI()
	{
		if (GUILayout.Button("Play sound"))
			PlaySound("ATK_Fire", 0);

		if (GUILayout.Button("Play sound delay"))
			PlaySound("ATK_Light", 1);

		if (GUILayout.Button("Play music"))
			PlayMusic("BGM_MainCity", false, 0, 0);

		if (GUILayout.Button("Play music delay"))
			PlayMusic("BGM_Login", false, 1, 0);

		if (GUILayout.Button("Play music fade"))
			PlayMusic("BGM_Login", false, 0, 10);

		if (GUILayout.Button("Play music delay fade"))
			PlayMusic("BGM_Login", false, 3, 5);

		if (GUILayout.Button("stop music"))
			StopMusic(0);

		if (GUILayout.Button("stop music fade"))
			StopMusic(5);

		if (GUILayout.Button("StepMusicToNormalState"))
			StepMusicToNormalState("BGM_Login");

		SoundMuted = GUILayout.Toggle(SoundMuted, "SoundMuted");
		SoundVolume = GUILayout.HorizontalSlider(SoundVolume, 0, 1);
		MusicMuted = GUILayout.Toggle(MusicMuted, "MusicMuted");
		MusicVolume = GUILayout.HorizontalSlider(MusicVolume, 0, 1);
	}
#endif
}