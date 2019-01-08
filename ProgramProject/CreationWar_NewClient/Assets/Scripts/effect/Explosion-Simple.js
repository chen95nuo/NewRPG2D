var explosionRadius = 5.0;
var explosionPower = 10.0;
var explosionDamage = 100.0;
var enemy = false;


function Awake(){
//explosionDamage = LevelStatus.leve*16+Random.Range(0,8*LevelStatus.leve);
if(enemy)
explosionDamage *= 4;
}
function OnEnable(){
Bomb();
}
function Bomb () {

	var explosionPosition = transform.position;

	var colliders : Collider[] = Physics.OverlapSphere (explosionPosition, explosionRadius);
	for (var hit in colliders) {
		var closestPoint = hit.ClosestPointOnBounds(explosionPosition);
		var distance = Vector3.Distance(closestPoint, explosionPosition);
		var hitPoints = 1.0 - Mathf.Clamp01(distance / explosionRadius);
		hitPoints *= explosionDamage;
		if(!enemy&&hit.CompareTag ("Enemy"))
		hitPoints *= 5;
	     var settingsArray = new int[3];
			settingsArray[0]=10;
			settingsArray[1]=hitPoints;
			settingsArray[2]=6;
		hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			
		
	}


	colliders = Physics.OverlapSphere (explosionPosition, explosionRadius);
	for (var hit in colliders) {
		if (hit.rigidbody)
			hit.rigidbody.AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 3.0);
	}

}
