#pragma strict
private var photonView : PhotonView;
var Dontdis = false;
function Start () {
photonView = GetComponent(PhotonView);
}

function DoSomething(time : int){
////print("addbuff");
 photonView.RPC("allbuff", 6, Random.Range(8,33), Random.Range(30,250), Random.Range(3,12), "Player");
    TimeOut(time);

}


function TimeOut(time : int) {
    yield WaitForSeconds(time);
     if(PhotonNetwork.isMasterClient && !Dontdis)
      PhotonNetwork.Destroy(gameObject);  
}

@RPC
function allbuff(radius:int,buffID:int,buffValue:int,bufftime :int,tag:String){
 	var colliders : Collider[] = Physics.OverlapSphere ( transform.position,radius);
	     var settingsArray = new int[4];
			settingsArray[0]=0;
			settingsArray[1]=buffID;
			settingsArray[2]=buffValue;
			settingsArray[3]=bufftime;	
	for (var hit in colliders) { 
//		//print(hit.tag+" == "+tag);
		if(hit.CompareTag (tag)){
			hit.SendMessageUpwards("ApplyBuff",settingsArray,SendMessageOptions.DontRequireReceiver );	
//			//print("addbuff" +buffID + "=== "+buffValue + "=== "+bufftime  );		
		}
	}  
}