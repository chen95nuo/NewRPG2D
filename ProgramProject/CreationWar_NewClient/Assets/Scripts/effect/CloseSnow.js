#pragma strict

function Start () {
	#if UNITY_ANDROID
		var pars : ParticleSystem[];
		pars = FindObjectsOfType(ParticleSystem);
		for(var i=0; i<pars.length; i++){
			if(pars[i] && pars[i].gameObject.name == "Snow"){
				pars[i].active = false;
			}
		}
	#endif
}

function Update () {

}