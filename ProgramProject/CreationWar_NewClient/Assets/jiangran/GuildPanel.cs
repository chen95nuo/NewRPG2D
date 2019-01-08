using UnityEngine;
using System.Collections;

public class GuildPanel : MonoBehaviour {
	private GameObject MyPanel;
	public GameObject OtherPanel;
	// Use this for initialization
	void Start(){
		MyPanel = this.gameObject;
		InvokeRepeating("ShowInformation",0,0.1f);
	}
	void OnEnable(){
		if(OtherPanel!=null){
			OtherPanel.SetActive(false);
		}
	}
	void OnDisable(){
		if(OtherPanel!=null){
			OtherPanel.SetActive(true);
		}
	}
	void ShowInformation(){
		if(OtherPanel.activeSelf==true){
			MyPanel.SetActive(false);
		}
	} 

	public void BtnClose(){
			MyPanel.SetActive(false);
	}
}
