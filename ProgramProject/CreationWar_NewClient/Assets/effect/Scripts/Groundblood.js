private var xx = 0;
private var dd = 0.0;
private var aa = false;
private var offsetX = 0.0;
private var offsetY = 0.0;
private var singleTexSize;
function Start() {
	singleTexSize = Vector2(0.5, 0.5);
	renderer.material.mainTextureScale = singleTexSize;
}


function OnEnable(){
 xx = Random.Range(5,60);
var yy = Random.Range(0,360);
var zz = Random.Range(0,4);
transform.localEulerAngles = Vector3(0,yy,0);
aa = true;
 switch(zz){
   case 0 :
   offsetX = 0.0;
   offsetY = 0.0;
   break;
   
   case 1 :
   offsetX = 0.5;
   offsetY = 0.0;
   break;
   
   case 2 :
   offsetX = 0.0;
   offsetY = 0.5;
   break;
   
   case 3 :
   offsetX = 0.5;
   offsetY = 0.5;
   break;   
  }    
renderer.material.SetTextureOffset ("_MainTex", Vector2(offsetX, offsetY));
}

function Update(){
if(aa&&dd<=xx){
dd+=2;
transform.localScale = Vector3(dd,dd,dd);
}
}

