#pragma strict 

@script RequireComponent(Camera)

public var cookShadersCover : Material;
private var cookShadersObject : GameObject;

function Awake () {	
#if UNITY_IPHONE || UNITY_ANDROID	
	if (!cookShadersCover.HasProperty ("_TintColor"))
		Debug.LogWarning ("Dualstick: the CookShadersCover material needs a _TintColor property to properly hide the cooking process", transform);
#endif
}

function BlackAll(){
	CreateCameraCoverPlane ();
	cookShadersCover.SetColor ("_TintColor", Color (0.0,0.0,0.0,1.0));
}

function CreateCameraCoverPlane () : GameObject {
	cookShadersObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
	cookShadersObject.renderer.material = cookShadersCover;	
	cookShadersObject.transform.parent = transform;
	cookShadersObject.transform.localPosition = Vector3.zero;
	cookShadersObject.transform.localPosition.z += 1.55;
	cookShadersObject.transform.localRotation = Quaternion.identity;
	cookShadersObject.transform.localEulerAngles.z += 180;
	cookShadersObject.transform.localScale = Vector3.one *1.5;	
	cookShadersObject.transform.localScale.x *= 1.6;	
	
	return cookShadersObject;		
}

function BlackOut () {
	CreateCameraCoverPlane ();
	cookShadersCover.SetColor ("_TintColor", Color (0.0,0.0,0.0,0.0));
		var c : Color = Color.black;
		c.a = 0.0;
		while (c.a<=1.0) {
			c.a += Time.deltaTime*0.5;
			cookShadersCover.SetColor ("_TintColor", c);
			yield;
		}
	  yield WaitForSeconds(3);
	DestroyCameraCoverPlane ();
}

function BlackIn () {	
	
//	cookShadersCover.SetColor ("_TintColor", Color (0.0,0.0,0.0,1.0));
while(!PlayerStatus.MainCharacter){
 yield;
}
yield;
		var c : Color = Color.black;
		c.a = 1.0;
		while (c.a>0.0) {
			c.a -= Time.deltaTime*0.5;
			cookShadersCover.SetColor ("_TintColor", c);
			yield;
		}
	DestroyCameraCoverPlane ();
}

function DestroyCameraCoverPlane () {
	if (cookShadersObject)
		DestroyImmediate (cookShadersObject);	
	    cookShadersObject = null;
}

function Start () {	


}

