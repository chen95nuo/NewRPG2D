#pragma strict
var  target : GameObject;	
private var  varlineOfSightMask : LayerMask ;
private var  camerat :GameObject;
//var cube : MeshRenderer;
function Start () {

//cube.material.mainTexture = renderImage(target);
}

function renderCS(target:GameObject){
if(!target)
return;
  camerat = new GameObject("tempcamera", Camera);
  camerat.transform.position = target.transform.position + target.transform.forward*10+target.transform.up*2;
  camerat.transform.eulerAngles = target.transform.eulerAngles + Vector3(0, 180, 0);
  varlineOfSightMask = target.layer;
  ChangeLayer(target,13);
  camerat.camera.cullingMask = 1<< 13;
  camerat.camera.rect = Rect (0, 0.1, 0.8*Screen.height/Screen.width, 0.8);
  camerat.camera.farClipPlane = 100.0;
  camerat.camera.depth = 8;
  camerat.camera.clearFlags = CameraClearFlags.Depth;
}

function cancelrenderCS(target:GameObject){
ChangeLayer(target,varlineOfSightMask);
if(camerat)
Destroy(camerat);
}    

static function ChangeLayer (src : GameObject,Layer : int)
{
	src.layer = Layer;	
	for (var child : Transform in src.transform)
	{
		var curSrc = src.transform.Find(child.name);
		if (curSrc)
			ChangeLayer(curSrc.gameObject, Layer);
	}
}
