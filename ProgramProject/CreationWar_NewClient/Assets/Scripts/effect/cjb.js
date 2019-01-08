var scrollSpeed = 0.5;
var offset=0.4;
var t=0;
function Update() {

	if(Time.time-t>0.3 && offset>0)
offset-=Time.deltaTime * scrollSpeed;
renderer.material.mainTextureOffset = Vector2 (0,offset);
}


function OnEnable(){
	t=Time.time;
	offset=0.3;
}