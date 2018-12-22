using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardEffectControl : MonoBehaviour 
{

	public List<GameObject> effectList;
	// Use this for initialization
	void Start ()
	{
#if UNITY_IPHONE

		if(iPhone.generation == iPhoneGeneration.iPhone4)
		{
			hideEffect();
		}
#endif
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void showEffect()
	{
		#if UNITY_IPHONE
		if(iPhone.generation == iPhoneGeneration.iPhone4)
		{
			return;
		}
		#endif
		
		for(int i = 0; i < effectList.Count;++i)
		{
			GameObject obj = effectList[i];
			if(obj != null)
			{
				obj.SetActive(true);
			}
		}
	}
	
	public void hideEffect()
	{
		for(int i = 0; i < effectList.Count;++i)
		{
			GameObject obj = effectList[i];
			if(obj != null)
			{
				obj.SetActive(false);
			}
		}
	}
}
