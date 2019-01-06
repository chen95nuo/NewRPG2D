using UnityEngine;
using System.Collections;

public class UITipIndicator : UIModule 
{
	public Transform busyBtn;
	public float roateAngle = 360f;
	public float maxElapse = 0.1f;

	public void Update()
	{
		float time = Mathf.Min(Time.deltaTime, maxElapse);
		busyBtn.localRotation *= Quaternion.AngleAxis(roateAngle * time, Vector3.back);
	}
}
