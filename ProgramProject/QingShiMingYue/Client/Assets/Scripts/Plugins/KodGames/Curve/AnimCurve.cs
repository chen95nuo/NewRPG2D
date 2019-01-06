using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KodGames
{
	/// <summary>
	/// Extract curve data from the curve animation resource made in editor.
	/// These curve data can be changed during running mode.
	/// </summary>

	[System.Serializable]
	public class AnimCurve : MonoBehaviour
	{
		// Event delegate.
		public delegate void EventDelegate(string eventName, object userData);
		public delegate void TranslateDelegate(object userData);

		// Curve data.
		[System.Serializable]
		public class Curve
		{
			public string path; // The path of the game object / bone being animated.
			public string type; // The type of the component / material being animated.
			public string propertyName; // The name of the property being animated.
			public AnimationCurve curve; // The actual animation curve.
			public float maxVal; // The curve maximum value.
			public float minVal; // The curve minimum value.
		}

		// Event data.
		[System.Serializable]
		public class Event
		{
			public SendMessageOptions messageOptions; // The event sending message.
			public float time; // The event trigger time.
			public string functionName; // The event callback functionName.
			public string stringParameter; // The event string data.
			public float floatParameter; // The event float data.
			public int intParameter; // The event int data.
			public Object objectReferenceParameter; // The event object data.

			public void FromAnimEvent(AnimationEvent e)
			{
				messageOptions = e.messageOptions;
				time = e.time;
				functionName = e.functionName;
				stringParameter = e.stringParameter;
				floatParameter = e.floatParameter;
				intParameter = e.intParameter;
				objectReferenceParameter = e.objectReferenceParameter;
			}

			public AnimationEvent ToAnimEvent()
			{
				AnimationEvent e = new AnimationEvent();

				e.stringParameter = stringParameter;
				e.floatParameter = floatParameter;
				e.intParameter = intParameter;
				e.objectReferenceParameter = objectReferenceParameter;
				e.functionName = functionName;
				e.time = time;
				e.messageOptions = messageOptions;

				return e;
			}
		}

		// Animation clip.
		[System.Serializable]
		public class AnimClip
		{
			public string name; // The clip name.
			public WrapMode wrapMode; // Clip wrap mode.
			public float frameRate; // Frame rate.
			public float length; // Clip length.
			public List<Curve> curves = new List<Curve>(); // Curves.
			public List<Event> events = new List<Event>(); // Events.
		}

		//	[HideInInspector]
		public List<AnimClip> animClips = new List<AnimClip>();

		private EventDelegate eventDelegate; // Event callback.
		private object eventUserData; // Event user data.
		private TranslateDelegate translateStartDelegate;
		private object translateStartUserData;
		private TranslateDelegate translateEndDelegate;
		private object translateEndUserData;

		public void SetEventDelegate(EventDelegate del, object userData)
		{
			this.eventDelegate = del;
			this.eventUserData = userData;
		}

		public void SetTranslateStartDelegate(TranslateDelegate del, object userData)
		{
			this.translateStartDelegate = del;
			this.translateStartUserData = userData;
		}

		public void SetTranslateEndDelegate(TranslateDelegate del, object userData)
		{
			this.translateEndDelegate = del;
			this.translateEndUserData = userData;
		}

		// Add animation clip.
		public void AddClip(string clipName, WrapMode wrapMode, float frameRate, float length)
		{
			if (FindClip(clipName) != null)
				return;

			// New animation clip and add to list.
			AnimClip clip = new AnimClip();

			clip.name = clipName;
			clip.wrapMode = wrapMode;
			clip.frameRate = frameRate;
			clip.length = length;

			animClips.Add(clip);
		}

		// Add curve to animation clip.
		public void AddCurve(string clipName, string path, string type, string propertyName, AnimationCurve curve)
		{
			// Find this clip.
			AnimClip clip = FindClip(clipName);

			if (clip == null)
				return;

			// New one curve.
			Curve c = new Curve();
			c.path = path;
			c.type = type;
			c.propertyName = propertyName;
			c.curve = curve;

			// Save key frames.
			Keyframe[] keys = c.curve.keys;

			if (keys.Length > 0)
			{
				c.maxVal = -Mathf.Infinity;
				c.minVal = Mathf.Infinity;

				// Find the maximum and minimum value.
				foreach (Keyframe f in keys)
				{
					if (f.value > c.maxVal)
						c.maxVal = f.value;

					if (f.value < c.minVal)
						c.minVal = f.value;
				}
			}
			else
			{
				c.maxVal = 0;
				c.minVal = 0;
			}

			// Add to list.
			clip.curves.Add(c);
		}

		// Add curve event.
		public void AddEvent(string clipName, AnimationEvent evn)
		{
			AnimClip clip = FindClip(clipName);

			if (clip == null)
				return;

			Event e = new Event();
			e.FromAnimEvent(evn);

			clip.events.Add(e);
		}

		public void Reset()
		{
			animClips.Clear();
		}

		// Get animation dstAnimClip length.
		public bool GetClipLength(string clipName, out float length)
		{
			length = 0;

			// Find animation dstAnimClip data.
			AnimClip anmClp = FindClip(clipName);

			if (anmClp == null)
				return false;

			length = anmClp.length;

			return true;
		}

		// Get curve value range.
		public bool GetClipCrvValRange(string clipName, string crvProperty, out float valRange)
		{
			valRange = 0;

			// Find animation dstAnimClip data.
			AnimClip anmClp = FindClip(clipName);

			if (anmClp == null)
				return false;

			// Find curve.
			foreach (Curve c in anmClp.curves)
			{
				if (crvProperty == c.propertyName)
				{
					valRange = c.maxVal - c.minVal;
					return true;
				}
			}

			return false;
		}

		// Add curve to one game object.
		public bool AddCrvClipToObj(GameObject obj, string clipName, Dictionary<string, float> valRange, float timeScale)
		{
			// Find animation dstAnimClip data.
			AnimClip srcAnimClip = FindClip(clipName);
			if (srcAnimClip == null)
				return false;

			// Get animation from the target object.
			Animation dstAnim = obj.GetComponent<Animation>();
			if (dstAnim == null)
				dstAnim = obj.AddComponent<Animation>();

			AnimCurve dstAnimCurve = obj.GetComponent<AnimCurve>();
			if (dstAnimCurve == null)
				dstAnimCurve = obj.AddComponent<AnimCurve>();

			// Create one animation dstAnimClip for this dstAnimClip data.
			AnimationClip dstAnimClip = new AnimationClip();
			dstAnimClip.name = srcAnimClip.name;

			// Add curve.
			foreach (Curve srcCurve in srcAnimClip.curves)
			{
				Keyframe[] keys = srcCurve.curve.keys;

				float startTime = -1;
				float endTime = -1;
				float startVal = srcCurve.minVal;
				float endVal = srcCurve.maxVal;

				// Get start/end value
				foreach (Event e in srcAnimClip.events)
				{
					if (e.functionName == "TranslateStart")
					{
						foreach (Keyframe key in keys)
						{
							if (Mathf.Approximately(key.time, e.time))
							{
								startTime = key.time;
								startVal = key.value;
								break;
							}
						}
					}
					else if (e.functionName == "TranslateEnd")
					{
						foreach (Keyframe key in keys)
						{
							if (Mathf.Approximately(key.time, e.time))
							{
								endTime = key.time;
								endVal = key.value;
								break;
							}
						}
					}
				}

				float curRange = endVal - startVal;

				if (curRange != 0)
				{
					float range = curRange;

					foreach (KeyValuePair<string, float> kvp in valRange)
					{
						if (kvp.Key == srcCurve.propertyName)
						{
							range = kvp.Value;
							break;
						}
					}

					// Scale value.
					if (range != curRange)
					{
						float valueScale = range / curRange;
						float lastValueDelta = 0;
						float lastTimeDelta = 0;

						for (int i = 0; i < keys.Length; i++)
						{
							if (startTime >= 0 && keys[i].time <= startTime)
								continue;

							if (endTime >= 0 && keys[i].time > endTime)
							{
								keys[i].value += lastValueDelta;
								keys[i].time += lastTimeDelta;
							}
							else
							{
								float _value = keys[i].value;
								keys[i].value *= valueScale;
								lastValueDelta = keys[i].value - _value;

								float _time = keys[i].time;
								keys[i].time = startTime + (keys[i].time - startTime) / timeScale;
								lastTimeDelta = keys[i].time - _time;
							}
						}
					}
				}
				else
				{
					continue;
					//Debug.LogWarning(string.Format("Can not scale curve {0} in {1}", srcCurve.propertyName, clipName));
				}

				dstAnimClip.SetCurve(srcCurve.path, System.Type.GetType(srcCurve.type), srcCurve.propertyName, new AnimationCurve(keys));
			}

			// Add events.
			foreach (Event e in srcAnimClip.events)
			{
				if (e.functionName == "TranslateStart" || e.functionName == "TranslateEnd")
					continue;
				
				AnimationEvent animationEvent = e.ToAnimEvent();
				
				animationEvent.time = e.time / timeScale;

				dstAnimClip.AddEvent(animationEvent);
			}

			// Add this dstAnimClip.
			if (dstAnim[dstAnimClip.name] != null)
				dstAnim.RemoveClip(dstAnimClip.name);

			dstAnim.AddClip(dstAnimClip, dstAnimClip.name);

			return true;
		}

		private AnimClip FindClip(string name)
		{
			foreach (AnimClip clip in animClips)
			{
				if (clip.name == name)
					return clip;
			}

			return null;
		}

		// Used for event callback.
		[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
		private void EventCallback(string evnName)
		{
			if (eventDelegate != null && evnName != "Destroy")
			{
				eventDelegate(evnName, eventUserData);
				return;
			}

			if (evnName == "Destroy")
			{
				var fx = GetComponent<FXController>();
				if (fx != null)
					fx.StopFX();
				else if (this.CachedTransform.parent != null)
					GameObject.Destroy(this.CachedTransform.parent.gameObject);
				else
					GameObject.Destroy(this.gameObject);
			}
		}

		// Method for "TranslateStart" event
		[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
		private void TranslateStart()
		{
			if (translateStartDelegate != null)
			{
				translateStartDelegate(translateStartUserData);
			}
		}

		// Method for "TranslateEnd" event
		[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
		private void TranslateEnd()
		{
			if (translateEndDelegate != null)
			{
				translateEndDelegate(translateEndUserData);
			}
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("Tools/Animation/Extract Curve")]
		static void ExtractCurve()
		{
			GameObject obj = UnityEditor.Selection.activeGameObject;

			if (obj == null)
				return;

			// Find old extractive data.
			AnimCurve crvDt = obj.GetComponent<AnimCurve>();

			// Add new data.
			if (crvDt == null)
				crvDt = obj.AddComponent<AnimCurve>();

			// Reset the data.
			crvDt.Reset();

			// Extract animation clips.
#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
			AnimationClip[] clips = UnityEditor.AnimationUtility.GetAnimationClips(obj);
#else
			AnimationClip[] clips = UnityEditor.AnimationUtility.GetAnimationClips(obj.animation);
#endif

			foreach (AnimationClip clip in clips)
			{
				UnityEditor.AnimationClipCurveData[] curves = UnityEditor.AnimationUtility.GetAllCurves(clip, true);

				// If has no curves, not extract the clip.
				if (curves == null)
					continue;

				// Add this clip.
				crvDt.AddClip(clip.name, clip.wrapMode, clip.frameRate, clip.length);

				// Add curves.
				foreach (UnityEditor.AnimationClipCurveData crv in curves)
				{
					crvDt.AddCurve(clip.name, crv.path, crv.type.AssemblyQualifiedName, crv.propertyName, crv.curve);
				}

				// Add events.
				foreach (AnimationEvent evn in UnityEditor.AnimationUtility.GetAnimationEvents(clip))
				{
					crvDt.AddEvent(clip.name, evn);
				}
			}
		}
#endif
	}
}

