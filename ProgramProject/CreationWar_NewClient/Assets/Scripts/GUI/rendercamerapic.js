	#pragma strict
class rendercamerapic extends Song{
	private var  target1 : GameObject;	
	private var  varlineOfSightMask : LayerMask ;
	var MainCS =false ;
	var Pet = false;
	private var tt1 : Texture2D;
	private var camerat : GameObject;
	var pos : Transform;
	var camera2D : Camera;
	private var aa = true;
	private var bb = false;
	var Trans : Transform;
	var Transpositiony : float;
	function Awake(){
		camera2D = AllManage.uicamStatic;
	}

	function Update(){
	if(aa && MainCS &&this.gameObject.active == true && this.transform.localPosition.y==0 && Trans.localPosition.y == Transpositiony){
	aa = false;
	RenderM();
	}

	if(aa && Pet &&this.gameObject.active == true && this.transform.localPosition.y==0){
	aa = false;
	RenderSc(target1);
	}

	if(!aa && target1 && ( this.transform.localPosition.y!=0 || (MainCS && Trans.localPosition.y != Transpositiony) ) ){
	aa = true;
	cancelrenderCS(target1);
	}
	}
	//25.72021
	function OnDisable(){
	//if(!aa && target1){
	aa = true;
	cancelrenderCS(target1);
	//}
	}


	function RenderM() {
	 aa = false;
	if(!target1 && PlayerStatus.MainCharacter)
	target1 = PlayerStatus.MainCharacter.gameObject;
	////print(gameObject + " == target1 == "  + target1);
	if(target1){
	var viewPos : Vector3 = camera2D.WorldToViewportPoint (pos.position);
	 renderCS(target1,viewPos);
	 bb = true;
	 }
	}

	function RenderC(target: GameObject) {
	 aa = false;
	 bb = true;
	target1 = target;
	var viewPos : Vector3 = camera2D.WorldToViewportPoint (pos.position);
	 renderCS(target1,viewPos);
	}

	function RenderSc(target: GameObject) {
	target1 = target;
	aa = false;
	if(this.gameObject.active == true && this.transform.localPosition.y==0){
	var viewPos : Vector3 = camera2D.WorldToViewportPoint (pos.position);
	  bb = true;
	 renderCS(target1,viewPos); 

	 }
	}

	//function renderImage(target: GameObject)
	//{
	//  camerat= new GameObject("tempcamera", Camera);
	//  camerat.transform.position = target.transform.position + target.transform.forward*8+target.transform.up*1.8;
	//  camerat.transform.eulerAngles = target.transform.eulerAngles + Vector3(0, 180, 0);
	//  varlineOfSightMask = target.layer;
	//  ChangeLayer(target,12);
	//  camerat.camera.cullingMask = 1<< 12;
	//  camerat.camera.fieldOfView = 30;
	//  camerat.camera.clearFlags = CameraClearFlags.SolidColor;
	//  camerat.camera.backgroundColor = new Color(0.3, 0.2, 0, 0);
	//var screenSize = 256;
	//var rt = new RenderTexture (screenSize, screenSize, 24);
	//camerat.camera.targetTexture = rt;
	//var screenShot = new Texture2D (screenSize, screenSize, TextureFormat.ARGB32, false);
	//camerat.camera.Render();
	//RenderTexture.active = rt;
	//screenShot.ReadPixels (Rect (0, 0, screenSize, screenSize), 0, 0);
	//screenShot.Apply (true,true);
	//RenderTexture.active = null;
	//DestroyImmediate (rt);
	//ChangeLayer(target,varlineOfSightMask);
	//Destroy(camerat);
	// Minimap.mainTexture =screenShot;
	//} 

	function renderCS(target:GameObject,viewPos:Vector3){

	if(!target)
	return;
	  varlineOfSightMask = target.layer;
	if(!bb)
	return;
	  camerat = new GameObject("tempcamera", Camera);
	    if(Pet) 
	  camerat.transform.position = target.transform.position + target.transform.forward*5+target.transform.up*1.6;
	  else
	  camerat.transform.position = target.transform.position + target.transform.forward*9+target.transform.up*2;
	  camerat.transform.eulerAngles = target.transform.eulerAngles + Vector3(0, 180, 0);
	  camerat.transform.parent = target.transform;
	 // varlineOfSightMask = target.layer;
	  ChangeLayer(target,13);
	  camerat.camera.cullingMask = 1<< 13;
	  camerat.camera.fieldOfView = 55;
	  camerat.camera.rect = Rect (viewPos.x-(0.48*Screen.height/Screen.width),0.05, 1.0*Screen.height/Screen.width, 1);
	  camerat.camera.farClipPlane = 20.0;
	  camerat.camera.depth = 8;
	  camerat.camera.clearFlags = CameraClearFlags.Depth;
	}

	function cancelrenderCS(target: GameObject){
	if(target)
	ChangeLayer(target,varlineOfSightMask);
	if(camerat)
	Destroy(camerat);
	} 
	function cancelrenderCSNow(){
	var viewPos1 : Vector3 = camera2D.WorldToViewportPoint (pos.position);
	renderCS(target1,viewPos1); 
	}

	function delcamera(){
	yield;
	var camer = GameObject.Find("tempcamera");
	if(camer)
	Destroy(camer);
	}

	function ChangeLayer (src : GameObject,Layer : int)
	{
		src.layer = Layer;	
		for (var child : Transform in src.transform)
		{
			var curSrc = src.transform.Find(child.name);
			if (curSrc)
				ChangeLayer(curSrc.gameObject, Layer);
		}
	}

}