/// <summary>
/// 执行怪物的动作同步信息 -
/// </summary>
#pragma strict

	/// <summary>
	/// 怪物动作枚举 -
	/// </summary>
	enum MAniStates 
	{
		walk = 0,
		run,
		die,
		idle,
		hit,
		battle,
		stun,
		down,
		standup,
		attacka1,
		attacka2,
		attacka3,
		attacka4,
		attackb1,
		attackb2,
		attackb3,
		attackc1,
		attackc2,
		attackc3,
		attackd1,
		attackd2,
		attackd3,
		attacke1,
		attacke2,
		attackf1,
		attackf2,		
		spawn,
		ready,
		talk
	}
	
var AniSpeedScale = 1.0;
var anim_ : Animation;
function	Awake()
{
	anim_  = GetComponent.<Animation>();
}

function Start ()
{
	anim_.wrapMode = WrapMode.Once;
	if(	anim_["spawn"])
		anim_["spawn"].layer = 2;
	anim_["run"].wrapMode = WrapMode.Loop;
	anim_["run"].layer = -2;
	anim_["walk"].wrapMode = WrapMode.Loop;	
	anim_["walk"].layer = -2;
	anim_["battle"].wrapMode = WrapMode.Loop;
	anim_["battle"].layer = -2;
	anim_["idle"].wrapMode = WrapMode.Loop;
	anim_["idle"].layer = -2;
	if(anim_["stun"]!=null)
	{
		anim_["stun"].wrapMode = WrapMode.Loop;	
		anim_["stun"].layer = -2;	
	}
	anim_["hit"].layer = -1;
	anim_["hit"].blendMode = AnimationBlendMode.Additive;
	if(transform.Find("joint1/joint2"))
		anim_["hit"].AddMixingTransform(transform.Find("joint1/joint2"));
	anim_["die"].layer = 2;
	anim_["die"].wrapMode = WrapMode.ClampForever ; 
    if(anim_["down"]!=null)
    { 
	    anim_["down"].layer = -1; 
		anim_["down"].wrapMode = WrapMode.ClampForever ;
	}
	anim_.Stop();
	
}
//var	currentAnimation	:	MAniStates  = MAniStates.idle;
private var	aniStart : String;

//Used播放动作//
@RPC		
function	SyncAnimation(animat: String)
{         
//	aniStart	=	animat;
	if(	anim_[animat]	!=	null && aniStart	!=	animat){
		anim_.CrossFade(animat);
		aniStart	=	animat;
	}
}

@RPC		
function	StopAnimation(animat: String)
{         
//	aniStart	=	animat;
	if(	anim_[animat]	!=	null ){
		anim_.Stop(animat);
	}
}

/// <summary>
/// 同步角色动作速度 -
/// </summary>
@RPC
function	synanispeed(anispeed:float)
{
	AniSpeedScale = anispeed;	
}

function	Update()
{	
	if(	anim_[aniStart]	!=	null	)
		anim_[aniStart].speed	=	AniSpeedScale;
}


