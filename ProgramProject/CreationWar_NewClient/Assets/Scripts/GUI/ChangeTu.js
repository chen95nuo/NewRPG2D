#pragma strict
var SpriteButt : UISprite;
var A1 : UIAtlas;
var A2 : UIAtlas;

var ran1 : int = 0;
var ran2 : int = 0;
function Start () {
var ran =Random.Range(ran1,ran2);
switch (ran)
{				
 case 0:
SpriteButt.atlas = A1;
SpriteButt.spriteName = "Loding1024YX"; 
  break; 
  
   case 1:
SpriteButt.atlas = A1;
SpriteButt.spriteName = "map1024"; 
  break; 
  
   case 2:
SpriteButt.atlas = A2;
SpriteButt.spriteName = "Loding1024FS"; 
  break; 
  
   case 3:
SpriteButt.atlas = A2;
SpriteButt.spriteName = "Loding1024ZS"; 
  break; 
}
}

