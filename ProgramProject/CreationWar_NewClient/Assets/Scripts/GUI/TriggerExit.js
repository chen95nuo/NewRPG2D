#pragma strict
private var gan : Joystick;

function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("Player")){
	#if UNITY_IPHONE || UNITY_ANDROID
	   gan = FindObjectOfType(Joystick);
	   if(gan)
	   gan.ResetJoystick();
    #endif
		other.SendMessage("returnLevel",SendMessageOptions.DontRequireReceiver);
	}
}
