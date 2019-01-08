#pragma strict
var left = false;
var down = false;
function Awake () {
ttt = this.transform;
temph = ttt.localPosition.x;
if(down)
temph = ttt.localPosition.y;
if(left)
aa = -1;
photonView = GetComponent(PhotonView);
}
private var height = 0.0;
private var ttt:Transform;
private var aa =1;
private var temph = 0.0;
private var photonView : PhotonView;

function OpenGate () {
photonView.RPC("OpenGates",PhotonTargets.AllBuffered);
}

@RPC
function OpenGates () {
		if(down){
			while(height<15){
				if(ttt)
					ttt.localPosition.y -= Time.deltaTime;
				height += Time.deltaTime;
				yield;
			}
		}else{
			while(height<15){
				if(ttt)
					ttt.localPosition.x += aa*Time.deltaTime;
				height += Time.deltaTime;
				yield;
			}
		}
}

function OpenGateNow(){
if(down)
ttt.localPosition.y = temph -15;
else
ttt.localPosition.x = temph +aa*15;
}

function ResteGate(){
if(down)
ttt.localPosition.y = temph;
else
ttt.localPosition.x = temph;

}