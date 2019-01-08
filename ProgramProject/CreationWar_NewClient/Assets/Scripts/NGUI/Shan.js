#pragma strict


var Value : float;
var SpriteQuan : UISprite;
var useFloat : float;
private var mm : boolean = false;
var oneTimes : boolean = false;
private var useInt : int = 0;
function Update () {
	if(!oneTimes){
		if(useFloat <= 0){
			mm = true;
		}else
		if(useFloat > 1){
			mm = false;
		}
		if(mm){
			useFloat += Value*2*Time.deltaTime;		
		}else{
			useFloat -= Value*Time.deltaTime;		
		}
		if(SpriteQuan.enabled){
			SpriteQuan.color.a = useFloat;	
		}
	}else{
		if(useInt < 2){
			if(useFloat <= 0){
				if(!mm)
					useInt += 1;
				mm = true;
			}else
			if(useFloat > 1){
				mm = false;
			}
			if(mm){
				useFloat += Value*2*Time.deltaTime;		
			}else{
				useFloat -= Value*Time.deltaTime;		
			}
			if(SpriteQuan.enabled){
				SpriteQuan.color.a = useFloat;	
			}		
		}
	}
}

function OnEnable(){
	useFloat = 0;
	if(useInt >= 2)
		useInt = 0;
	mm = false;
}
