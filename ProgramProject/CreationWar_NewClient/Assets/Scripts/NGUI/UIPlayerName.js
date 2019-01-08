#pragma strict
private var mi : float;
private var ma : float;
private var screenPos : Vector3 ;
private var uiPos : Vector3;
private var gensui : boolean = false;
private var GSpos : Vector3;
private var gaodu : float;
var par : Transform;
var cameraToLookAt : Camera;
var xx : int;
private var thistransform : Transform;
function Awake(){
thistransform = this.transform;
}

//function Update(){
//	if(par == null){
//		shanchu();
//	}
//}


var ptime : float;
var SpriteTeam : UISprite;
var LabelName : UILabel;
var zipanel : UIPanel;
private var sc = 0.0;
private var sz = 0.0;
private var aa = true;
var SpriteTitle : UISprite[];
private var ps : PlayerStatus;
private var rideGao : int = 0;
function setx(Obj : classHP){

	gensui = true;
	gaodu = Obj.gao;
	LabelName.text = Obj.name;
	if(Obj.rankLevel != 0){
		SpriteTitle[0].enabled = true;
		SpriteTitle[1].enabled = true;
		SpriteTitle[0].spriteName = "name" + Obj.rankLevel;
		SpriteTitle[1].spriteName = "name" + Obj.rankLevel;
	}
	ps = Obj.ps;
//	LabelName.enabled = false;
//	yield;
//	LabelName.enabled = true;
	while(gensui && par != null  && camera.main != null){
		sz =camera.main.transform.InverseTransformPoint(par.position).z;
		if(transform.localScale.x<0||transform.localScale.y<0){
		transform.active = false;
		}else{
		transform.active = true;
		}
//		if(sz>0 && sz<32){
		if(true){
		    if(aa && zipanel){
			    zipanel.widgetsAreStatic = false;
			    zipanel.enabled = true;
			    yield;
			    zipanel.widgetsAreStatic = true;
			    aa = false;
		    }
		    try{
			    rideGao = 0;
			    if(ps != null){
			    	if(ps.ridemod){
			    		rideGao = 1;
			    	}
			    }
				screenPos = camera.main.WorldToScreenPoint (Vector3(par.position.x,par.position.y + 3.5 + rideGao,par.position.z));
				uiPos = AllManage.uicamStatic.ScreenToWorldPoint(screenPos);
				uiPos.z = sz;
				thistransform.position= uiPos;
				sc =  50/sz;
				thistransform.localScale= Vector3(sc,sc,4);		    
		    }catch(e){
		    
		    }
		}else 
		if(!aa && zipanel){
			zipanel.enabled = false;
			aa = true;
		}
		yield;
	}
	yield des();
}

function des(){
	if(par == null){
		shanchu();
	}
	return true;
}

function shanchu(){
	try{
		AllResources.FontpoolStatic.UnspawnEffect(xx,gameObject);
	}catch(e){
		
	}	
}
