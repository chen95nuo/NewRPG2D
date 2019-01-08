#pragma strict
enum MoveType{from,to}

function Start () {

}
var SongCursor : UICursor;
var SongSprite : UIAtlas;
function MoveStart(str : String){
	isMove = true;
	SongCursor.Set(SongSprite,str);
}

function Update(){
	if (Input.GetButtonUp ("Fire1")) {  
		reMove();
	}
}
 
private var isMove : boolean = false;
function reMove(){ 
	yield; 
	if(isMove){		
		isMove = false;
		MoveClear();
	}
} 

function MoveClear(){
	SongCursor.Clear();
}
