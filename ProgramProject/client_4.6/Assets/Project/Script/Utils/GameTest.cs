using UnityEngine;
using System.Collections;

public class GameTest : MonoBehaviour 
{
	public bool isTest = false;
	public void Awake()
	{
		Application.targetFrameRate = 30;
	}
	void Update()
	{
		GameHelper.isTestCritCameraShow = isTest;
	}
}
