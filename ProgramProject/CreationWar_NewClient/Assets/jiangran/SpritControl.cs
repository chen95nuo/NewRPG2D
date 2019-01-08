using UnityEngine;
using System.Collections;

public class SpritControl : MonoBehaviour {
	public UISprite  Skill;
	public ParticleEmitter par;
	private bool thispar = true;
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		if(thispar){
		if(Skill!=null&&Skill.enabled==true&&par!=null){
			par.Emit();
				thispar = false;
		}
		}
	}
}
