//#define FX_POOL_METRICS
using UnityEngine;
using System.Collections.Generic;

public class FxPool
{
	private Transform root;
	private FXController template;
	private List<FXController> despawmFXs = new List<FXController>();
	private bool released = false;

#if FX_POOL_METRICS
	private int allocCount;
	public int AllocCount
	{
		get { return allocCount; }
	}

	private int spawmFromPoolCount;
	public int SpawmFromPoolCount
	{
		get { return spawmFromPoolCount; }
	}

	private int despawmToPoolCount;
	public int DespawmToPoolCount
	{
		get { return despawmToPoolCount; }
	}
#endif

	public FxPool(Transform root, GameObject template)
	{
		// Get FxController
		var fx = template.GetComponent<FXController>();
		if (fx == null)
		{
			fx = template.AddComponent<FXController>();
			fx.loop = true;
			fx.autoDestroy = true;
		}

		// Inactive template
		template.SetActive(false);

		// Attach template to root
		ObjectUtility.AttachToParentAndKeepLocalTrans(root, template.transform);

		this.root = root;
		this.template = fx;
		this.released = false;
	}

	public void Release()
	{
		FreePooledFx();

		if (template != null)
			Object.Destroy(template.gameObject);
		template = null;

		released = true;
	}

	public FXController Spawm()
	{
		if (released)
			return null;

		if (template == null)
			return null;

		FXController fx = null;
		if (despawmFXs.Count == 0 || despawmFXs[despawmFXs.Count - 1].GetNextPoolTime() > Time.time)
		{
#if FX_POOL_METRICS
			allocCount++;
#endif

			// Instantiate new FX
			var go = GameObject.Instantiate(template.gameObject) as GameObject;

			// Template is inactive, set new go active
			go.SetActive(true);

			// Get FXController
			fx = go.GetComponent<FXController>();

			// Add a root GO as default
			fx.CreateRoot();
			Object.DontDestroyOnLoad(fx.Root);

			// Set pool
			fx.FXPool = this;
		}
		else
		{
#if FX_POOL_METRICS
			spawmFromPoolCount++;
#endif

			// Get from pool
			fx = despawmFXs[despawmFXs.Count - 1];
			despawmFXs.RemoveAt(despawmFXs.Count - 1);

			// Attach scene root
			ObjectUtility.AttachToParentAndResetLocalTrans(null, fx.Root);

			// Pooled Fx is inactive, set active
			fx.Root.gameObject.SetActive(true);
		}

		// Start FX
		fx.Start();

		return fx;
	}

	public void Despawm(FXController fx)
	{
		if (released)
		{
			Object.Destroy(fx.Root.gameObject);
		}
		else
		{
#if FX_POOL_METRICS
			despawmToPoolCount++;
#endif

			// Reset data
			fx.ClearFinishCallback();
			fx.SetFreeToLastPoolTime(Time.time);

			Debug.Assert(fx.Root.parent != root);

			// Return to pool
			ObjectUtility.AttachToParentAndResetLocalTrans(root, fx.Root);
			fx.Root.gameObject.SetActive(false);
			despawmFXs.Insert(0, fx);
		}
	}

	public void FreePooledFx()
	{
		foreach (var fx in despawmFXs)
			Object.Destroy(fx.Root.gameObject);

		despawmFXs.Clear();

#if FX_POOL_METRICS
		allocCount = 0;
		spawmFromPoolCount = 0;
		despawmToPoolCount = 0;
#endif
	}
}

public class FXController : MonoBehaviour
{
	[System.Serializable]
	public abstract class FXData
	{
		public float beginTime;
		public float endTime;
		protected bool played = false;

		public abstract bool IsDead { get; }
		public abstract void Play();
		public abstract void Stop();
		public abstract void Reset(bool clearFX);
		public virtual void FadeOut(float remainTime) { }

		public virtual void Update(float playingTime)
		{
			if (playingTime >= beginTime)
			{
				if (playingTime <= endTime)
				{
					if (played == false)
						Play();
				}
				// End time smaller than zero means need not stop
				else if (endTime >= 0)
				{
					if (played)
						Stop();
				}
			}
		}

	}

	[System.Serializable]
	public class ParticleData : FXData
	{
		public ParticleEmitter emitter;

		public ParticleData Create(ParticleEmitter emitter)
		{
			this.emitter = emitter;
			this.beginTime = 0;
			this.endTime = -1;
			return this;
		}

		public override bool IsDead
		{
			get { return emitter.particleCount == 0; }
		}

		public override void Play()
		{
			played = true;
			emitter.emit = true;
		}

		public override void Stop()
		{
			emitter.emit = false;
		}

		public override void Reset(bool clearFX)
		{
			played = false;
			emitter.emit = false;
			if (clearFX)
				emitter.ClearParticles();
		}

		public override void FadeOut(float remainTime)
		{
			if (emitter.particleCount == 0)
				return;

			// Extract the particles, here we must create new particleSystem array to update the effect according to the unity notes.
			Particle[] particleDatas = emitter.particles;
			Particle[] newParticleDatas = new Particle[particleDatas.Length];

			for (int j = 0; j < particleDatas.Length; j++)
			{
				newParticleDatas[j] = new Particle();
				newParticleDatas[j] = particleDatas[j];

				if (newParticleDatas[j].energy > remainTime)
					newParticleDatas[j].energy = remainTime;
			}

			emitter.particles = newParticleDatas;
		}
	}

	[System.Serializable]
	public class ParticleSystemData : FXData
	{
		public ParticleSystem particleSystem;

		public ParticleSystemData Create(ParticleSystem particleSystem)
		{
			particleSystem.playOnAwake = false;

			this.particleSystem = particleSystem;
			this.beginTime = 0;
			this.endTime = -1;
			return this;
		}

		public override bool IsDead
		{
			get { return particleSystem.particleCount == 0; }
		}

		public override void Play()
		{
			played = true;
			particleSystem.playOnAwake = true;
			particleSystem.Play();
		}

		public override void Stop()
		{
			particleSystem.playOnAwake = false;
			particleSystem.Stop();
		}

		public override void Reset(bool clearFX)
		{
			played = false;
			particleSystem.Stop();
			particleSystem.playOnAwake = false;
		}

		public override void FadeOut(float remainTime)
		{
			if (particleSystem.particleSystem.particleCount == 0)
				return;

			// Extract the particleDatas, here we must create new particleSystem array to update the effect according to the unity notes.
			ParticleSystem.Particle[] particleDatas = new ParticleSystem.Particle[particleSystem.particleSystem.particleCount];
			particleSystem.particleSystem.GetParticles(particleDatas);
			ParticleSystem.Particle[] newParticleDatas = new ParticleSystem.Particle[particleDatas.Length];

			for (int j = 0; j < particleDatas.Length; j++)
			{
				newParticleDatas[j] = new ParticleSystem.Particle();
				newParticleDatas[j] = particleDatas[j];

				if (newParticleDatas[j].lifetime > remainTime)
					newParticleDatas[j].lifetime = remainTime;
			}

			particleSystem.particleSystem.SetParticles(newParticleDatas, newParticleDatas.Length);
		}
	}

	[System.Serializable]
	public class AnimationData : FXData
	{
		public Animation animation;

		//public override void Update(float playingTime)
		//{
		//    base.Update(playingTime);
		//    Debug.Log("playingTime " + playingTime + " speed " + animation["UI_S5SkillBG"].speed + " time " + animation["UI_S5SkillBG"].time +
		//        " length " + animation["UI_S5SkillBG"].length);
		//    //Time.timeScale = 0;
		//    animation["UI_S5SkillBG"].speed = 1.3f;
		//}

		public AnimationData Create(Animation animation)
		{
			this.animation = animation;
			this.beginTime = 0;
			this.endTime = -1;
			return this;
		}

		public override bool IsDead
		{
			get { return animation.isPlaying == false; }
		}

		public override void Play()
		{
			played = true;
			animation.Play();
		}

		public override void Stop()
		{
			animation.Stop();
		}

		public override void Reset(bool clearFX)
		{
			played = false;
			animation.playAutomatically = false;
		}
	}

	[System.Serializable]
	public class TrailRendererData : FXData
	{
		public TrailRenderer renderer;

		public TrailRendererData Create(TrailRenderer renderer)
		{
			this.renderer = renderer;
			this.beginTime = 0;
			this.endTime = -1;
			return this;
		}

		public override bool IsDead
		{
			get { return renderer.enabled; }
		}

		public override void Play()
		{
			played = true;
			renderer.enabled = true;
		}

		public override void Stop()
		{
			renderer.enabled = false;
		}

		public override void Reset(bool clearFX)
		{
			played = false;
			renderer.enabled = false;
		}

		//public override void FadeOut(float remainTime)
		//{
		//    if (renderer.enabled == false)
		//        return;

		//    if (remainTime <= 0)
		//        renderer.enabled = false;
		//}
	}

	public ParticleData[] particleDataArray;
	public ParticleSystemData[] particleSystemDataArray;
	public AnimationData[] animationDataArray;
	public TrailRendererData[] trailRendererDataArray;
	public float life = 1;
	public float fadeDelay = 0f; // Only use to set in editor.
	public bool loop = true;
	public bool autoDestroy = true;

	public enum _DESTROY_MODE
	{
		FADE,
		PARTICLE_DISAPPEAR,
	}

	public _DESTROY_MODE destroyMode = _DESTROY_MODE.FADE;

	private float playingTime;
	private float fadingRemainTime;
	private float lastFreeToPoolTime = 0;

	enum _STAGE
	{
		STOPPED,
		PLAYING,
		STOPPING,
	};

	private _STAGE playingStage = _STAGE.STOPPED;

	// End FX delegate	
	public delegate void OnFXFinishDelegate(object userData);

	private struct DelegateData
	{
		public OnFXFinishDelegate del;
		public object userData;

		public DelegateData(OnFXFinishDelegate del, object userData)
		{
			this.del = del;
			this.userData = userData;
		}
	}
	private List<DelegateData> onFXFinishDels;

	private Transform root;
	public Transform Root
	{
		get { return root != null ? root : this.CachedTransform; }
	}

	private FxPool fxPool;
	public FxPool FXPool
	{
		get { return fxPool; }
		set { fxPool = value; }
	}

	public void CreateRoot()
	{
		if (root == null)
		{
			root = new GameObject(this.gameObject.name).transform;
			ObjectUtility.AttachToParentAndKeepLocalTrans(root, this.CachedTransform);

			// Reset root transform
			root.localPosition = Vector3.zero;
			root.localRotation = Quaternion.identity;
			root.localScale = Vector3.one;
		}
	}

	[ContextMenu("Play FX")]
	public void PlayFX()
	{
		if (playingStage == _STAGE.PLAYING)
			return;

		playingStage = _STAGE.PLAYING;
		playingTime = 0;

		// Particles
		foreach (var data in particleDataArray)
		{
			data.Reset(true);
			if (data.beginTime == 0)
				data.Play();
		}

		// Animations
		foreach (var data in particleSystemDataArray)
		{
			data.Reset(true);
			if (data.beginTime == 0)
				data.Play();
		}

		// Animations
		foreach (var data in animationDataArray)
		{
			data.Reset(true);
			if (data.beginTime == 0)
				data.Play();
		}

		foreach (var data in trailRendererDataArray)
		{
			data.Reset(true);
			if (data.beginTime == 0)
				data.Play();
		}

		//Debug.Log( "Play FXController: name=" + gameObject.name );
	}

	[ContextMenu("Stop FX")]
	public void StopFX()
	{
		// Pfx is stopped.
		if (playingStage != _STAGE.PLAYING)
			return;

		playingStage = _STAGE.STOPPING;
		fadingRemainTime = fadeDelay;

		StopFXData();

		//Debug.Log( "Stop FXController: name=" + gameObject.name );
	}

	private void DestroyFX()
	{
		if (FXPool != null)
			FXPool.Despawm(this);
		else
			Object.Destroy(this.Root.gameObject);
	}

	private void StopFXData()
	{
		foreach (var data in particleDataArray)
			data.Stop();

		foreach (var data in particleSystemDataArray)
			data.Stop();

		foreach (var data in animationDataArray)
			data.Stop();

		foreach (var data in trailRendererDataArray)
			data.Stop();
	}

	private void ResetFX(bool clearFX)
	{
		foreach (var data in particleDataArray)
			data.Reset(clearFX);

		foreach (var data in particleSystemDataArray)
			data.Reset(clearFX);

		foreach (var data in animationDataArray)
			data.Reset(clearFX);

		foreach (var data in trailRendererDataArray)
			data.Reset(clearFX);
	}

	private void UpdateFX()
	{
		foreach (var data in particleDataArray)
			data.Update(playingTime);

		foreach (var data in particleSystemDataArray)
			data.Update(playingTime);

		foreach (var data in animationDataArray)
			data.Update(playingTime);

		foreach (var data in trailRendererDataArray)
			data.Update(playingTime);
	}

	[ContextMenu("Reset ParticleData Array")]
	public void ResetParticleArray()
	{
		// We can't place into Awake method. Maybe data has not been initialized.
		ParticleEmitter[] pfxEmits = gameObject.GetComponentsInChildren<ParticleEmitter>();
		particleDataArray = new ParticleData[pfxEmits.Length];

		for (int i = 0; i < pfxEmits.Length; i++)
		{
			particleDataArray[i] = new ParticleData().Create(pfxEmits[i]);

			// Turn off the auto destroy caused by the data.
			ParticleAnimator particleAnimator = (ParticleAnimator)pfxEmits[i].gameObject.GetComponentInChildren(typeof(ParticleAnimator));
			if (particleAnimator != null)
				particleAnimator.autodestruct = false;
		}
	}

	[ContextMenu("Reset ParticleData System Array")]
	public void ResetParticleSystemArray()
	{
		// We can't place into Awake method. Maybe data data has not been initialized.
		ParticleSystem[] pfxSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
		particleSystemDataArray = new ParticleSystemData[pfxSystems.Length];

		for (int i = 0; i < pfxSystems.Length; i++)
		{
			particleSystemDataArray[i] = new ParticleSystemData().Create(pfxSystems[i]);

			//// Turn off the auto destroy caused by the data.
			//ParticleAnimator particleAnimator = (ParticleAnimator)data.gameObject.GetComponentInChildren(typeof(ParticleAnimator));
			//if (particleAnimator != null)
			//    particleAnimator.autodestruct = false;
		}
	}

	[ContextMenu("Reset AnimationData Array")]
	public void ResetAnimationArray()
	{
		// Get animations data
		Animation[] animations = gameObject.GetComponentsInChildren<Animation>();
		animationDataArray = new AnimationData[animations.Length];

		for (int i = 0; i < animations.Length; ++i)
			animationDataArray[i] = new AnimationData().Create(animations[i]);
	}

	[ContextMenu("Reset TrailRenderer Array")]
	public void ResetTrailRendererArray()
	{
		// Get trailRenderer data
		TrailRenderer[] trailRenderer = gameObject.GetComponentsInChildren<TrailRenderer>();
		trailRendererDataArray = new TrailRendererData[trailRenderer.Length];

		for (int i = 0; i < trailRenderer.Length; ++i)
			trailRendererDataArray[i] = new TrailRendererData().Create(trailRenderer[i]);
	}

	[ContextMenu("Disable AutoPlay")]
	public void DisableAutoPlay()
	{
		ResetFX(true);
	}

	public void AddFinishCallback(OnFXFinishDelegate finishFun, object userData)
	{
		if (onFXFinishDels == null)
			onFXFinishDels = new List<DelegateData>();

		onFXFinishDels.Add(new DelegateData(finishFun, userData));
	}

	public void ClearFinishCallback()
	{
		if (onFXFinishDels != null)
			onFXFinishDels.Clear();
	}

	// Use this for initialization
	public void Start()
	{
		if (fadeDelay <= 0)
			fadeDelay = 0;

		// If no FX data, get this from game object.
		if ((particleDataArray == null || particleDataArray.Length == 0) &&
			(particleSystemDataArray == null || particleSystemDataArray.Length == 0) &&
			(animationDataArray == null || animationDataArray.Length == 0) &&
			(trailRendererDataArray == null || trailRendererDataArray.Length == 0))
		{
			if ((particleDataArray == null || particleDataArray.Length == 0))
				ResetParticleArray();

			if ((particleSystemDataArray == null || particleSystemDataArray.Length == 0))
				ResetParticleSystemArray();

			if ((animationDataArray == null || animationDataArray.Length == 0))
				ResetAnimationArray();

			if ((trailRendererDataArray == null || trailRendererDataArray.Length == 0))
				ResetTrailRendererArray();
		}

		PlayFX();
	}

	public void SetFreeToLastPoolTime(float time)
	{
		this.lastFreeToPoolTime = time;
	}

	public float GetNextPoolTime()
	{
		return lastFreeToPoolTime + GetDelayPoolTime();
	}

	private float GetDelayPoolTime()
	{
		float time = 0;
		foreach (var data in trailRendererDataArray)
			time = Mathf.Max(time, data.renderer.time);

		return time;
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		// Pfx is stopped.
		if (playingStage == _STAGE.STOPPED)
			return;

		// Update FX stage.
		if (playingStage == _STAGE.PLAYING)
		{
			// Update FX played time.
			playingTime += Time.deltaTime;

			UpdateFX();

			if (playingTime > life)
			{
				if (loop)
				{
					// life > 0 means this FX has recycle duration
					if (life > 0)
					{
						StopFXData();
						ResetFX(false);
						playingTime -= life;
					}
				}
				else
				{
					StopFX();
				}
			}
		}
		else if (playingStage == _STAGE.STOPPING)
		{
			// Update fading time.
			fadingRemainTime -= Time.deltaTime;

			// Life is end. 
			if (destroyMode == _DESTROY_MODE.FADE)
			{
				FadeOut(fadingRemainTime);

				if (fadingRemainTime <= 0)
				{
					// Mark flag.
					playingStage = _STAGE.STOPPED;
				}
			}
			else if (destroyMode == _DESTROY_MODE.PARTICLE_DISAPPEAR)
			{
				if (IsDead())
				{
					// Mark flag.
					playingStage = _STAGE.STOPPED;
				}
			}
			else // Error process.
			{
				// Mark flag.
				playingStage = _STAGE.STOPPED;
			}

			// Process stopped stage.
			if (playingStage == _STAGE.STOPPED)
			{
				playingTime = 0;

				// Call back.
				if (onFXFinishDels != null)
					foreach (var delData in onFXFinishDels)
						if (delData.del != null)
							delData.del(delData.userData);

				// Destroy self.
				if (autoDestroy)
					DestroyFX();
			}
		}
	}

	private bool IsDead()
	{
		foreach (var data in particleDataArray)
			if (data.IsDead == false)
				return false;

		foreach (var data in particleSystemDataArray)
			if (data.IsDead == false)
				return false;

		foreach (var data in animationDataArray)
			if (data.IsDead == false)
				return false;

		foreach (var data in trailRendererDataArray)
			if (data.IsDead == false)
				return false;

		return true;
	}

	private void FadeOut(float remainTime)
	{
		foreach (var data in particleDataArray)
			data.FadeOut(remainTime);

		foreach (var data in particleSystemDataArray)
			data.FadeOut(remainTime);

		foreach (var data in animationDataArray)
			data.FadeOut(remainTime);

		foreach (var data in trailRendererDataArray)
			data.FadeOut(remainTime);
	}
}
