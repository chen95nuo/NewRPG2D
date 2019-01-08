using UnityEngine;
using System.Collections;

public class ControlCenter : MonoBehaviour {
	public GameObject obj;
	public GameObject obj1;
	// Use this for initialization
	void OnEnable () {
		ShowbtnCenter ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public  void SDKCenter(){
#if UNITY_IOS
#if SDK_JY
#elif SDK_I4
		ASSDKControl.ASSDKCenter ();
#elif SDK_KUAIYONG
		KYSdkControl.KYSDKCenter();
#elif SDK_PP
		PPSdkControl.PPSdkCenter ();
#endif
#endif
	}

	void ShowbtnCenter(){
		Vector3 locPost = obj1.transform.localPosition;
#if UNITY_IOS
		obj.SetActive (false);
		obj1.transform.localPosition = new Vector3(10,locPost.y,locPost.z);
#if SDK_I4 
		obj.SetActive (true);
		obj1.transform.localPosition = new Vector3(152,locPost.y,locPost.z);
#elif SDK_KUAIYONG
		obj.SetActive (true);
		obj1.transform.localPosition = new Vector3(152,locPost.y,locPost.z);
#elif SDK_PP
		obj.SetActive (true);
		obj1.transform.localPosition = new Vector3(152,locPost.y,locPost.z);
#else
		obj.SetActive (false);
		obj1.transform.localPosition = new Vector3(10,locPost.y,locPost.z);
#endif
#else
		obj.SetActive (false);
		obj1.transform.localPosition = new Vector3(10,locPost.y,locPost.z);
#endif
	}
	
}
