using UnityEngine;
using System.Collections;

public class OnEnableObject : MonoBehaviour {
	public GameObject MyObj;
	public UIToggle CkbCreat;
	public GameObject[] OtherObj;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		StartCoroutine(ShowMyObj());
	}
	IEnumerator ShowMyObj(){
		yield return new WaitForSeconds(0.5f);
		CkbCreat.value = true;
		if(MyObj!=null){
		MyObj.SetActiveRecursively(true);
			for(int i = 0; i<OtherObj.Length;i++){
		OtherObj[i].SetActiveRecursively(false);
			
			}
		}
	}
}
