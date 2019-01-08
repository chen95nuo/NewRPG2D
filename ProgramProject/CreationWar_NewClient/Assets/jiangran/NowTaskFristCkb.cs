using UnityEngine;
using System.Collections;

public class NowTaskFristCkb : MonoBehaviour {
	public UIToggle Mytog;
	public GameObject[] OtherOpen;
	public UIButtonMessage IsFrist;
	// Use this for initialization
	void Start () {
		StartCoroutine(ActivityFrist());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		StartCoroutine(FristCkb());
		StartCoroutine(ActivityFrist());

	}
	IEnumerator  ActivityFrist(){
		yield return new WaitForSeconds(0.1f);
		if(OtherOpen!=null){
			foreach(GameObject other in OtherOpen){
				other.SetActive(false);
			}
		}
	}

	IEnumerator FristCkb(){
		yield return new WaitForSeconds(0.1f);
		if(Mytog!=null){
			Mytog.value = true;
		}
		if(IsFrist!=null){
			IsFrist.OnClick();
		}
	}
}
