using UnityEngine;
using System.Collections;

public class ControlUpdate : MonoBehaviour {
	public UIScrollView upDateScrollView;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
//		Debug.Log("--------------------");
		StartCoroutine(ConUpdate());
	}
	IEnumerator ConUpdate(){
		yield return new WaitForSeconds(0.1f);
		upDateScrollView.movement = UIScrollView.Movement.Vertical;
	}
}
