using UnityEngine;
using System.Collections;

// Overwrite UnityEngine.MonoBehaviour
public class MonoBehaviour : UnityEngine.MonoBehaviour
{
	private Animation cachedAnimation = null;
	public Animation CachedAnimation
	{
		get { return cachedAnimation != null ? cachedAnimation : cachedAnimation = this.animation; }
	}

	private AudioSource cachedAudio = null;
	public AudioSource CachedAudio 
	{
		get { return cachedAudio != null ? cachedAudio : cachedAudio = this.audio; }
	}

	private Camera cachedCamera = null;
	public Camera CachedCamera 
	{
		get { return cachedCamera != null ? cachedCamera : cachedCamera = this.camera; }
	}

	private Collider cachedCollider = null;
	public Collider CachedCollider 
	{
		get { return cachedCollider != null ? cachedCollider : cachedCollider = this.collider; }
	}

	private ConstantForce cachedConstantForce = null;
	public ConstantForce CachedConstantForce 
	{
		get { return cachedConstantForce != null ? cachedConstantForce : cachedConstantForce = this.constantForce; }
	}

	private GUIText cachedGuiText = null;
	public GUIText CachedGuiText
	{
		get { return cachedGuiText != null ? cachedGuiText : cachedGuiText = this.guiText; }
	}

	private GUITexture cachedGuiTexture = null;
	public GUITexture CachedGuiTexture 
	{
		get { return cachedGuiTexture != null ? cachedGuiTexture : cachedGuiTexture = this.guiTexture; }
	}

	private HingeJoint cachedHingeJoint = null;
	public HingeJoint CachedHingeJoint 
	{
		get { return cachedHingeJoint != null ? cachedHingeJoint : cachedHingeJoint = this.hingeJoint; }
	}

	private Light cachedLight = null;
	public Light CachedLight
	{
		get { return cachedLight != null ? cachedLight : cachedLight = this.light; }
	}

	private NetworkView cachedNetworkView = null;
	public NetworkView CachedNetworkView 
	{
		get { return cachedNetworkView != null ? cachedNetworkView : cachedNetworkView = this.networkView; }
	}

	private ParticleEmitter cachedParticleEmitter = null;
	public ParticleEmitter CachedParticleEmitter 
	{
		get { return cachedParticleEmitter != null ? cachedParticleEmitter : cachedParticleEmitter = this.particleEmitter; }
	}

	private ParticleSystem cachedParticleSystem = null;
	public ParticleSystem CachedParticleSystem 
	{
		get { return cachedParticleSystem != null ? cachedParticleSystem : cachedParticleSystem = this.particleSystem; }
	}

	private Renderer cachedRenderer = null;
	public Renderer CachedRenderer
	{
		get { return cachedRenderer != null ? cachedRenderer : cachedRenderer = this.renderer; }
	}

	private Rigidbody cachedRigidbody = null;
	public Rigidbody CachedRigidbody 
	{
		get { return cachedRigidbody != null ? cachedRigidbody : cachedRigidbody = this.rigidbody; }
	}

	private Transform cachedTransform = null;
	public Transform CachedTransform
	{
		get { return cachedTransform != null ? cachedTransform : cachedTransform = this.transform; }
	}

#if UNITY_EDITOR
	#region Obfusation Checkion
	public new void Invoke(string methodName, float time)
	{
		KodGames.ObfuscateUtility.CheckExcludeMethodName(this.GetType(), methodName);
		base.Invoke(methodName, time);
	}
	
	public new void InvokeRepeating(string methodName, float time, float repeatRate)
	{
		KodGames.ObfuscateUtility.CheckExcludeMethodName(this.GetType(), methodName);
		base.InvokeRepeating(methodName, time, repeatRate);
	}
	
	public new bool IsInvoking(string methodName)
	{
		KodGames.ObfuscateUtility.CheckExcludeMethodName(this.GetType(), methodName);
		return base.IsInvoking(methodName);
	}
	
	public new Coroutine StartCoroutine(string methodName)
	{
		KodGames.ObfuscateUtility.CheckExcludeMethodName(this.GetType(), methodName);
		return base.StartCoroutine(methodName);
	}
	
	public new Coroutine StartCoroutine(string methodName, object value)
	{
		KodGames.ObfuscateUtility.CheckExcludeMethodName(this.GetType(), methodName, value);
		return base.StartCoroutine(methodName, value);
	}
	#endregion
#endif
}

