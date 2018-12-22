using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RotateCard : MonoBehaviour 
{
	public GameObject card3DObj;
	public float speed = 1;
	
	public void setCard3DObj(GameObject obj)
	{
		card3DObj = obj;
	}

	void OnDrag(Vector2 delta)
	{
		if(card3DObj == null)
			return;
		float lastY = card3DObj.transform.localEulerAngles.y;
		if(delta.x < 0)
		{
			card3DObj.transform.localEulerAngles = new Vector3(0,lastY+Math.Abs(delta.x)*speed ,0);
		}
		else if(delta.x > 0)
		{
			card3DObj.transform.localEulerAngles = new Vector3(0, lastY+(-1) * Math.Abs(delta.x)*speed ,0);
		}
	}
	
	public void gc()
	{
		card3DObj = null;
		GC.Collect();
	}
}
