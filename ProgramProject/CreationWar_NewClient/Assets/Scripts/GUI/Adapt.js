var gui : GUITexture;
private var xx : float;
private var yy : float;
private var ww : float;
private var hh : float;
var left = false;
var padsmall : boolean = false;

function Awake(){
	gui = GetComponent( GUITexture );
	if(left){
	    //xx = gui.pixelInset.x * 0.0015625 * Screen.height + Screen.width * 0.05;
	    //yy = gui.pixelInset.y * 0.0015625 * Screen.height + Screen.height * 0.06;
	    xx = gui.pixelInset.x * 0.0015625 * Screen.height;
	    yy = gui.pixelInset.y * 0.0015625 * Screen.height;
	}
	else{
	xx = Screen.width -(960-gui.pixelInset.x)*0.0015625 * Screen.height;
	yy = gui.pixelInset.y * 0.0015625 * Screen.height;
	}
	ww = gui.pixelInset.width * 0.0015625 * Screen.height;
	hh = gui.pixelInset.height * 0.0015625 * Screen.height;
if(padsmall&& Screen.width*3==Screen.height*4){
	ww =ww*0.6;
	hh =hh*0.6;
	xx =xx*0.6;
    yy =yy*0.6;
	}	
	gui.pixelInset = Rect(xx,yy,ww,hh);

}
