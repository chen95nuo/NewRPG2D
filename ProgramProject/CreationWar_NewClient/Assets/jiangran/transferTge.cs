using UnityEngine;
using System.Collections;

public class transferTge : MonoBehaviour {
	private GameObject my;
	public AudioSource music;
	void Awake(){
		my = this.gameObject;
		music = this.gameObject.audio;
	}
	void OnTriggerEnter(Collider collider){
		if(collider.name=="SpriteCollider"){
			UIButtonMessage uim = my.transform.GetComponent<UIButtonMessage>();
					uim.OnClick();
					music.Play();
		}
	}
}
