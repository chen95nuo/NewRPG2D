using UnityEngine;
using System.Collections;

public class AuctionContorl : MonoBehaviour {
	public UIToggle MyAll;
	public UIToggle[] otherTog;
	// Use this for initialization
	void Start () {
		StartCoroutine(AllActivity());
	}

	IEnumerator AllActivity(){
		yield return new WaitForSeconds(0.01f);
		MyAll.value = true;
	}
	
	// Update is called once per frame

   public void OtherTog(){
		if(MyAll&&MyAll.value==true){
			foreach(UIToggle ot in otherTog){
				ot.value = true;
			}
		}

		}
	
	public void MyAllActivity(){
		MyAll.value = false;
			}
}
