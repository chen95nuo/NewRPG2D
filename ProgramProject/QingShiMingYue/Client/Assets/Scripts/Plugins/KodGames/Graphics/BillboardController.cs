using UnityEngine;
using System;

public class BillboardController : MonoBehaviour
{
	public Transform faceTransform;
	public bool rotateXAxis = true;
	public bool rotateYAxis = true;
	public bool rotateZAxis = true;
	
	private Transform cachedTrans;

	public void SetFaceTransform(Transform trans)
	{
		faceTransform = trans;
	}

	private void Start()
	{
		cachedTrans = transform;
	}

	private void LateUpdate()
	{
		if (faceTransform == null && Camera.main != null)
			faceTransform = Camera.main.transform;

		if (faceTransform != null)
		{
			float rotationX = rotateXAxis ? faceTransform.eulerAngles.x : cachedTrans.eulerAngles.x;
			float rotationY = rotateYAxis ? faceTransform.eulerAngles.y : cachedTrans.eulerAngles.y;
			float rotationZ = rotateZAxis ? faceTransform.eulerAngles.z : cachedTrans.eulerAngles.z;
			cachedTrans.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
		}
	}
}
