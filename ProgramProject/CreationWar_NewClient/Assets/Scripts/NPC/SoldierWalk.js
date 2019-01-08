var speed = 4.0;
var rotationSpeed = 5.0;
var pickNextWaypointDistance = 4.0;
var CanSee =false;
function Start () {
	animation.wrapMode = WrapMode.Loop;
//	animation["talk"].wrapMode = WrapMode.Once;
//	animation["talk"].layer = 2;
	Patrol();
}

function Patrol () {
	var curWayPoint = AutoWayPoint.FindClosest(transform.position);
	while (true) {
		var waypointPosition = curWayPoint.transform.position;

		if (Vector3.Distance(waypointPosition, transform.position) < pickNextWaypointDistance)
			curWayPoint = PickNextWaypoint (curWayPoint);

//		if (CanSee)
//			yield StartCoroutine("TalktoPlayer");
		
		MoveTowards(waypointPosition);
		
		yield;
	}
}

function MoveTowards (position : Vector3) {
	var direction = position - transform.position;
	direction.y = 0;
	if (direction.magnitude < 0.5) {
		SendMessage("SetSpeed", 0.0);
		return;
	}
	transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
	transform.eulerAngles = Vector3(0, transform.eulerAngles.y, 0);
	var forward = transform.TransformDirection(Vector3.forward);
	var speedModifier = Vector3.Dot(forward, direction.normalized);
	speedModifier = Mathf.Clamp01(speedModifier);

	direction = forward * speed * speedModifier;
	GetComponent (CharacterController).SimpleMove(direction);
	
	SendMessage("SetSpeed", speed * speedModifier, SendMessageOptions.DontRequireReceiver);
}

function PickNextWaypoint (currentWaypoint : AutoWayPoint) {

	var forward = transform.TransformDirection(Vector3.forward);
	var best = currentWaypoint;
	var bestDot = -10.0;
	for (var cur : AutoWayPoint in currentWaypoint.connected) {
		var direction = Vector3.Normalize(cur.transform.position - transform.position);
		var dot = Vector3.Dot(direction, forward);
		if (dot > bestDot && cur != currentWaypoint) {
			bestDot = dot;
			best = cur;
		}
	}
	
	return best;
}

function TalktoPlayer () {

}

function SetSpeed (speed : float) {
 if (speed > 5){
	  	animation["run"].speed = 0.1*speed;
		    animation.CrossFade("run",0.5);
		    }
  else if (speed > 1){
	  	animation["walk"].speed = 0.2*speed;
		    animation.CrossFade("walk",0.5);
		    }
	else
	    animation.CrossFade("idle");
}