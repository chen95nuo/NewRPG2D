using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixViewPoint : MonoBehaviour
{
	private float defalutFiled = 0f;

	public void Awake()
	{
		defalutFiled = this.gameObject.camera.fieldOfView;
	}

	public void Start()
	{
		FixCameraViewPoint();
	}

	private void FixCameraViewPoint()
	{
		float standardScreenProportion = GameDefines.uiDefaultScreenSize.x / GameDefines.uiDefaultScreenSize.y;
		float currentScreenProportion = Screen.width / (float)Screen.height;

		if (currentScreenProportion < standardScreenProportion)
			this.gameObject.camera.fieldOfView = standardScreenProportion * this.gameObject.camera.pixelHeight / this.gameObject.camera.pixelWidth * defalutFiled;
	}
}
