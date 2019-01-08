
#pragma strict 
var reloadtime =90;
var PopPrefab : GameObject ;
var pp : boolean = true;
var currentObject : GameObject;
var photonView : PhotonView;

function Start () {
	move();
	yield;
	MakePop();
	InvokeRepeating("MakePop",0,reloadtime+1);
}

function move(){
var hit : RaycastHit;
if (Physics.Raycast (transform.position, -Vector3.up, hit, 200.0)) { 
     if(hit.collider.CompareTag ("Ground"))   
       transform.position = hit.point + Vector3(0,0.5,0) ;
    }
}

function MakePop () {
		if(PhotonNetwork.connected && PhotonNetwork.isMasterClient && pp && currentObject == null){
		    photonView.RPC("ef", 163);  
	    	currentObject = PhotonNetwork.InstantiateSceneObject(PopPrefab.name, transform.position, Quaternion.identity, 0 , null);
	        currentObject.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 	
        	photonView.RPC("TongBupp",PhotonTargets.AllBuffered, false);        	
			cooldown();	
		}
}

@RPC
function TongBupp( p:boolean){
pp=p;

}



function cooldown(){
yield WaitForSeconds(reloadtime);
photonView.RPC("TongBupp",PhotonTargets.AllBuffered, true); 
}

@RPC
function ef(i:int){
AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}