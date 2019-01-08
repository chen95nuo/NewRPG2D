
var scrollSpeed = 5;
var countX : int = 4;
var countY : int = 4;

private var offsetX = 0.0;
private var offsetY = 0.0;
private var singleTexSize;
private var aa=true;
private var bb=true;
private var timef : float;

function Start() {
	singleTexSize = Vector2(1.0/countX, 1.0/countY);
	renderer.material.mainTextureScale = singleTexSize;
	while(true){
	flash();
	yield;
	}
}

function Update ()
{  if(aa){
	renderer.enabled=true;
	timef = Time.time - Mathf.Floor(Time.time);
var	frame = Mathf.Floor(timef*scrollSpeed);
	offsetX = frame/countX;
	offsetY = -(frame - frame%countX) /countY / countX;
	renderer.material.SetTextureOffset ("_MainTex", Vector2(offsetX, offsetY));

   }
   else
   renderer.enabled=false;
}
function flash(){
if(bb){
bb=false;
yield WaitForSeconds(0.1);
aa=false;
yield WaitForSeconds(Random.Range(0.1,0.3));
aa=true;
if(audio)
audio.Play();
yield WaitForSeconds(0.2);
aa=false;
yield WaitForSeconds(Random.Range(0.8,1.0));
aa=true;
yield WaitForSeconds(0.3);
aa=false;
yield WaitForSeconds(Random.Range(2.0,3.0));
aa=true;
bb=true;
}
}