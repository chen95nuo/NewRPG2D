using UnityEngine;
using System.Collections;

public class ShowEverydayAim : MonoBehaviour {
	public UISlider ShowAim;
	public UILabel Fen;
	// Use this for initialization
	void Start () {
		Invoke("ShowEveryAim",1);
	}


	void ShowEveryAim(){
		if(ShowAim!=null){
			float fen = ShowAim.value*100;
			Fen.text = fen.ToString();
		}
	}
}
