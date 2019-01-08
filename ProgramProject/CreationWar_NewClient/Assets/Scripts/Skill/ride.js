#pragma strict
var expTexture : Texture[];
var rideanim_:Animation;
var rideroot :Transform;
var TextureID : int = 0;

function Start () {
	rideanim_.wrapMode = WrapMode.Loop;
	rideanim_["jumpstart"].layer = 1;	
	rideanim_["jumpstart"].wrapMode = WrapMode.Once;    
	rideanim_["jumpend"].layer = 1;
	rideanim_["jumpend"].wrapMode = WrapMode.Once; 
	
//	rideanim_.Stop();
}

var rd : Renderer;
function ChangeTexture(id : int){
	TextureID = id;
	rd.material.mainTexture = expTexture[TextureID ];
}
