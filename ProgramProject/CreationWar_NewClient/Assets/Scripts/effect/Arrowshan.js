#pragma strict
var Value : float=1.0;
var sharedMaterial: Material;
private var useFloat : float;
private var mm : boolean = false;
private var c : Color;
function Start () {
	    c  = sharedMaterial.GetColor ("_TintColor");
		c.a = 1.0;
while (true) {
		if(useFloat <= 0){
			mm = true;
		}else
		if(useFloat > 1){
			mm = false;
		}
		if(mm){
			useFloat += Value*2*Time.deltaTime;		
		}else{
			useFloat -= Value*2*Time.deltaTime;		
		}
	c.a = useFloat;
sharedMaterial.SetColor ("_TintColor", c);
yield;
yield;
}
}