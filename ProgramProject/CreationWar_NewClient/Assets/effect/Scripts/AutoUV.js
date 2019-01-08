
private var offsetX = 0.0;
private var offsetY = 0.0;
private var singleTexSize;
var zz = 1;
function Awake(){
 SetUV(zz);

}

function SetUV(zz:int) {
	singleTexSize = Vector2(0.5, 0.5);
	renderer.material.mainTextureScale = singleTexSize;
	 switch(zz){
   case 3 :
   offsetX = 0.0;
   offsetY = 0.0;
   break;
   
   case 4 :
   offsetX = 0.5;
   offsetY = 0.0;
   break;
   
   case 1 :
   offsetX = 0.0;
   offsetY = 0.5;
   break;
   
   case 2 :
   offsetX = 0.5;
   offsetY = 0.5;
   break;   
  }    
renderer.material.SetTextureOffset ("_MainTex", Vector2(offsetX, offsetY));
}




