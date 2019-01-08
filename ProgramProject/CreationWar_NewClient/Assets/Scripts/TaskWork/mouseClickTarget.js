#pragma strict
private var Player : GameObject;
private var agent : NavMeshAgent;
private var playerautoctrl : Autoctrl;
public var marker : Transform;

function Start () {
Player = FindPlayer ();
playerautoctrl = Player.GetComponent(Autoctrl);
agent = Player.GetComponent(NavMeshAgent);
}

function UdateAgentTargets(targetPosition : Vector3) {
		playerautoctrl.Wayfinding = true;
        agent.enabled = true;
		agent.destination = targetPosition;
}

//function Update () {
//  var button : int = 0;
//  if(Input.GetMouseButtonDown(button)) {
//    var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//    var hitInfo : RaycastHit;
//    if (Physics.Raycast(ray.origin, ray.direction, hitInfo)) {
//      var targetPosition : Vector3 = hitInfo.point;	     
//      UdateAgentTargets(targetPosition);
//		marker.position = targetPosition + Vector3(0,1,0);
//    }
//  }
//}

 function FindPlayer () : GameObject
 {
   var gos : GameObject[];
  gos = GameObject.FindGameObjectsWithTag("Player");
  var player1 : GameObject;
for (var go : GameObject in gos) {
    var controller = go.GetComponent(ThirdPersonController);
    if (controller && controller.enabled == true) {
       player1 = go;
       }
    }
return player1;
}