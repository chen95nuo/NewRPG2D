#pragma strict

private var hittime=0.0;
function OnTriggerStay(col : Collider) {
 if((col.CompareTag ("Player")||col.CompareTag ("Enemy")) && hittime+2<Time.time ){
	     hittime=Time.time;             
         addbuff(col);
     }
}

function addbuff(col : Collider){
       	  	var setArray = new int[4];
            setArray[0]= -1;
            setArray[1]= 11;           
            setArray[2]= 300;
            setArray[3]= 3;            						
	   col.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver ); 
	   }