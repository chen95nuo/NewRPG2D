using UnityEngine;
using System.Collections;

public class CardNameManager : MonoBehaviour {
	private Transform _myTransform;
	GameObject posObject;
	Camera mainCamera;
	Camera NGUICamera;

	// Use this for initialization
	void Start () {
		_myTransform = transform;
	}

	void LateUpdate () {
		if(posObject != null && NGUICamera!= null && mainCamera!= null)
		{
			Vector3 worldPos = new Vector3(posObject.transform.position.x, posObject.transform.position.y + 0.5f, posObject.transform.position.z);
			Vector2 screenPos =  mainCamera.WorldToScreenPoint(worldPos);
			Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
			_myTransform.position = curPos;
		}
	}
	
	public void SetData(GameObject pos, Camera main, Camera ngui)
	{
		posObject = pos;
		mainCamera = main;
		NGUICamera = ngui;
	}
}
