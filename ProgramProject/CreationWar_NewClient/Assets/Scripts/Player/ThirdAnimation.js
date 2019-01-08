#pragma strict
enum AniStates 
	{
		walk = 0,
		run,
		run2,
		die,
		jumpstart,
		jump,
		jumpend,
		jumprun,
		idle,
		idle2,
		hit,
		battle,
		attacka0,
		attacka1,
		attacka2,
		attacka3,
		attacka4,
		attackb0,
		attackb1,
		attackb2,
		attackb3,
		stun,
		down,
		standup,
		attackA,
		attackB
	}
var AniSpeedScale = 1.0;
var yaobone : Transform;
var anim_ : Animation;
var rideanim_:Animation;
private var photonView : PhotonView;
private var Status : PlayerStatus;

function	Awake ()
{							
	photonView	=	GetComponent(	PhotonView		);
	Status		=	GetComponent(	PlayerStatus	);
}

function Start ()
{
	anim_.wrapMode				=	WrapMode.Once;
	anim_["jumpstart"].layer	=	1;	
    anim_["jump"].wrapMode		=	WrapMode.Loop;      
	anim_["jumpend"].layer		=	1;
	anim_["jumprun"].layer		=	1;
	anim_["run"].wrapMode		=	WrapMode.Loop;
	anim_["run2"].wrapMode		=	WrapMode.Loop;
	anim_["walk"].wrapMode		=	WrapMode.Loop;		
	anim_["battle"].wrapMode	=	WrapMode.Loop;
	anim_["idle"].wrapMode		=	WrapMode.Loop;
	anim_["stun"].wrapMode		=	WrapMode.Loop;		
//	anim_["hit"].blendMode = AnimationBlendMode.Additive;
	anim_["hit"].layer			=	1;
	anim_["hit"].AddMixingTransform(yaobone);
//	anim_["fish"].AddMixingTransform(yaobone);
//	anim_["fishing"].AddMixingTransform(yaobone);
//	anim_["fishup"].AddMixingTransform(yaobone);
//	anim_["make"].AddMixingTransform(yaobone);			
    anim_["die"].wrapMode		=	WrapMode.ClampForever ;
	anim_["down"].wrapMode		=	WrapMode.ClampForever ;  
	anim_.Stop();
	anim_.CrossFade("idle");
	if(!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
	{
		GetAnimation();
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_GetAnimation, "");
//		photonView.RPC("GetAnimation");
	}
	yield	WaitForSeconds(0.5);
	if(	!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)	&&	currentAnimation	==	AniStates.idle	)
	{
		GetAnimation();
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_GetAnimation, "");
//		photonView.RPC(	"GetAnimation"	);
	}
}

@RPC
function	GetAnimation()
{
	if(	PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)	){
//		photonView.RPC("SyyAnimation",currentAnimation.ToString());
		SyyAnimation(currentAnimation.ToString());
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_SyyAnimation, currentAnimation.ToString());
	}
}

@RPC
function	SyyAnimation(name:String)
{
	if(	!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		anim_.CrossFade(name);
}

var	currentAnimation : AniStates  = AniStates.idle;
var lastAnimation : AniStates = AniStates.jump;
	
function playAnimation(animat: String)
{
	try
	{
		currentAnimation = AniStates.Parse(typeof(AniStates),animat);
		if (lastAnimation != currentAnimation)
		{
			lastAnimation = currentAnimation;
			CrossAnimation(animat);
		}
	}
	catch(e)
	{
	}
	
}


private var lastspeed = 1.0;
function	Update()
{
	if(	PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && Math.Abs(lastspeed-AniSpeedScale)>0.2)
	{
		lastspeed = AniSpeedScale;
		synAspeed(AniSpeedScale);
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_synAspeed, AniSpeedScale.ToString());
	}
}

@RPC
function synAspeed(Aspeed : float )
{
	if(	!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)	)
		AniSpeedScale	=	Aspeed;
	var aniname : String = currentAnimation.ToString();
	anim_[aniname].speed = AniSpeedScale;	   
}


function RideAnimation(animat: String)  //zuo qi zhuan yong
{
	try{
		currentAnimation = AniStates.Parse(typeof(AniStates),animat);
		if (lastAnimation != currentAnimation)
		{
			lastAnimation = currentAnimation;
		if(Status.ridemod && (currentAnimation == AniStates.idle||currentAnimation == AniStates.battle||currentAnimation == AniStates.walk||currentAnimation == AniStates.run||currentAnimation == AniStates.jumpstart||currentAnimation == AniStates.jump||currentAnimation == AniStates.jumpend||currentAnimation == AniStates.jumprun )){
				if(rideanim_){
				    if (currentAnimation == AniStates.jumprun)
				        CrossAnimation2("jumpend");
				    else if (currentAnimation == AniStates.battle)
				        CrossAnimation2("idle");
				    else
				        CrossAnimation2(animat);	
				}			
				CrossAnimation("ride");
		}
		else
				CrossAnimation(animat);
		}
	}catch(e){
	}
}

function CrossAnimation(name : String )
{
	anim_.CrossFade(name , 0.1);
	anim_[name].speed = AniSpeedScale;

}


function CrossAnimation2(name : String )
{
	rideanim_.CrossFade(name);
	rideanim_[name].speed = AniSpeedScale;
}


function	InitlastAnimation()
{
	lastAnimation = AniStates.jump;
}

function	CallAnimation (bname : String )
{
	PlayAnimation(bname);
	ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayAnimation, bname);

}

function	CallrideAnimation(bname : String ) 
{
	ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_PlayrideAnimation, bname);
	PlayrideAnimation(bname);
//	photonView.RPC("PlayrideAnimation",bname); 
}

@RPC
function	PlayrideAnimation(bname : String )
{
    rideanim_[bname].layer = 3;
	rideanim_[bname].speed = AniSpeedScale;
	rideanim_.CrossFade(bname,0.2);
    yield WaitForSeconds (anim_[bname].length);
    try
    {
    	rideanim_[bname].layer = -1; 
    	rideanim_.Stop(bname);     
    }
    catch(e)
    {   
    }
}
//切换到某一动作播放一次后停止这一动画的播放//
@RPC
function	PlayAnimation(bname : String )
{
	anim_[bname].layer	=	3;
	anim_[bname].speed	=	AniSpeedScale;
	anim_.CrossFade(	bname,	0.1	);
	yield WaitForSeconds (anim_[bname].length);
    try
    {
    	anim_[bname].layer = -1; 
    	anim_.Stop(bname);     
    }
    catch(e)
    {   
    }
}
//切换到某一动作播放一次后停止这一动画的播放//
@RPC
function	PlayAnimation2(bname : String )
{
    anim_[bname].layer = 4;
	anim_[bname].speed = AniSpeedScale;
	anim_.CrossFade(bname,0.1);
    yield WaitForSeconds (anim_[bname].length+0.1);
    try
    {
    	anim_[bname].layer = -1; 
    	anim_.Stop(bname);     
    }
    catch(e)
    {   
    }   
}
//循环播放一个动画//
//@RPC
function	Callloop(aname:String)
{
    anim_[aname].wrapMode = WrapMode.Loop; 
    anim_[aname].layer = 2;
	anim_.CrossFade(aname,0.2);
}
//停止一个动画//
//@RPC
function	changelayer(	aname:String	)
{
    anim_[aname].layer		=	-1;
    anim_[aname].wrapMode	=	WrapMode.Once; 
    anim_.Stop(aname);
}

//动画播完之后播放下一动画//
//@RPC
function	PlayAnimat(	aname:String	)
{
	anim_[aname].layer	=	3;
	anim_[aname].speed	=	AniSpeedScale;
	anim_.CrossFadeQueued(	aname,	0.2	);
}

function	PlayAnimatAttack(	aname:String	)
{
	anim_[aname].layer	=	3;
	anim_[aname].speed	=	AniSpeedScale;
	anim_.PlayQueued(	aname , QueueMode.PlayNow);
}

function StopAnimat(	aname:String	){
	anim_.Stop(aname);
}

function SitDown(){
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_Callloop, "siting");
	Callloop("siting");
}

function StandUp(){
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_changelayer, "siting");
	changelayer("siting");
}

function YieldSitDown(){
	yield;
	yield;
	yield;
	if(!Status.ridemod){
		SitDown();
	}
}

function YieldStandUp(){
	yield;
	yield;
	yield;
	if(!Status.ridemod){
		StandUp();
	}
}
