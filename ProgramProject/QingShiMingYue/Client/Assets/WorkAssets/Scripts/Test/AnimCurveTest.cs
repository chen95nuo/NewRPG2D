#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimCurveTest : MonoBehaviour
{
	public KodGames.AnimCurve animCurve;

	// Use this for initialization
	void Start()
	{
		if (animCurve != null)
		{
			Dictionary<string, float> s = new Dictionary<string, float>();
			s["m_LocalPosition.z"] = 6.0f;
			s["m_LocalPosition.y"] = 2.0f;

			string crvName = "ThrowWeapon";
			if (animCurve.AddCrvClipToObj(gameObject, crvName, s, 1))
			{
				// Set speed.
				animation[crvName].speed = animation[crvName].length / 10.0f;

				animation.Play(crvName);
			}

//			AnimationState stFirst = null;
//
//			foreach (AnimationState st in animation)
//			{
//				stFirst = st;
//				break;
//
//			}
//
//			if (stFirst != null)
//				animation.Play(stFirst.name);
		}
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(transform.localPosition);
	}

	// Used for event callback.
	protected void EventCallback(string evnName)
	{
		if (evnName == "Destroy")
		{
			GameObject.Destroy(gameObject);
		}
	}
}
#endif