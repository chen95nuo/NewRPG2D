using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

//TODO 代码优化
public class SelectPlayerSceneData : MonoBehaviour
{
	private static SelectPlayerSceneData instance = null;
	public static SelectPlayerSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(SelectPlayerSceneData)) as SelectPlayerSceneData;

			return instance;
		}
	}

	private UnityEngine.Camera mainCamera;
	public UnityEngine.Camera MainCamera
	{
		get
		{
			if (mainCamera == null)
				mainCamera = KodGames.Camera.main;

			return mainCamera;
		}
	}
	public GameObject cameraRoot;
	public List<GameObject> marks;
	public float dragDelta = 0.2f;
}