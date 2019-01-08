var jumpendsound : AudioClip;
//static var f=false;
private var playerController :ThirdPersonController;
private	var currentSpeed = 0.0;
private var Status:PlayerStatus; 
private var Animatc:ThirdAnimation; 
private var photonView : PhotonView;
function Start ()
{
	playerController = GetComponent(ThirdPersonController);	
	photonView = GetComponent(PhotonView);
	Animatc = GetComponent(ThirdAnimation);
	Status = GetComponent(PlayerStatus);
	playerController.yun = false;
	playerController.bing = false;
	playerController.down = false;
}

function Update ()
{  if(!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return; 
   if(Status.dead )	
     return; 
   else if(playerController.yun ){
        Animatc.RideAnimation("stun");
        return; 
      }
   else if(playerController.bing)
      {        
         Animatc.AniSpeedScale=0;
         return;
      } 
   else if(playerController.down)
      {  
         Animatc.AniSpeedScale=1; 
         return;
      }  
 
   else{	
    if ( !playerController.IsJumping ()){
        currentSpeed = playerController.GetSpeed();
		if (currentSpeed > 4 ){
			 Animatc.RideAnimation("run");
			 Animatc.AniSpeedScale = playerController.Movespeed;
			}

		else if (currentSpeed <= 4 && currentSpeed > 0.2){
			Animatc.RideAnimation("walk");
			Animatc.AniSpeedScale = playerController.Movespeed;
			}	 
		else{
		    if(Status.battlemod==true){
			Animatc.RideAnimation("battle");
		    Animatc.AniSpeedScale = 1;		
			}
		    else{
			Animatc.RideAnimation("idle"); 
			Animatc.AniSpeedScale = 1;
			}
	        }
      }
  
  }
}

private var aa = true;
private var speed : float = 0;

function DidLand () {
    aa = true;
if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    speed =currentSpeed;
else
    speed = playerController.useMovement.magnitude*10;
    
   if(speed >  0.1){     //pao)
        Animatc.RideAnimation("jumprun");
        Animatc.RideAnimation("run");

	}else{	            //zhan
        Animatc.RideAnimation("jumpend");
         if(Status.battlemod==true)
			Animatc.RideAnimation("battle");	
		 else
			Animatc.RideAnimation("idle"); 
    }
	if (jumpendsound)
		audio.PlayOneShot(jumpendsound);
}
