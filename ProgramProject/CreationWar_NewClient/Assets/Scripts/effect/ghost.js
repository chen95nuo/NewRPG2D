private var target:Transform ;
private var ttr = 0.0;
private var aa = false ;
var soulnum = 1;
function Start ()
{
target = FindClosestEnemy().transform;
}
function OnEnable(){
Timeout();
}

function Timeout(){
yield WaitForSeconds(0.5);
ttr= Time.time;
aa= true;
}

function Update () {
if (aa)
	transform.position = Vector3.Lerp (transform.position+transform.up*0.6, target.position+target.transform.up*2,(Time.time-ttr)* 0.3);
}

function OnTriggerEnter (col : Collider) {
	if(col.CompareTag ("Player") ){
	col.SendMessage("AddSoul",soulnum);
yield WaitForSeconds(0.3);
aa=false;
}
}



 function FindClosestEnemy () : GameObject
 {
   var gos : GameObject[];
  gos = GameObject.FindGameObjectsWithTag("Player");
  var closest : GameObject;
  var distance = Mathf.Infinity;
  var position = transform.position;
for (var go : GameObject in gos) {
    var diff = (go.transform.position - position);
    var curDistance = diff.sqrMagnitude;
    if (curDistance < distance) {
       closest = go;
       distance = curDistance;
       }
    }
return closest;
}
