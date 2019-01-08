class	ThirdNetworkInit	extends	Photon.MonoBehaviour
{
	function	SetRemote()
	{
		if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	)	)
		{
			if(	PlayerStatus.MainCharacter == null || PlayerStatus.MainCharacter == transform	)
				Camera.main.SendMessage(	"SetTargetfast",	transform	);
			if(	GetComponent(qiuai)	)	
				GetComponent(qiuai).enabled = true;
			if(	GetComponent(alljoy)	)	
				GetComponent(alljoy).enabled = true;
			if(	GetComponent(Autoctrl)	)	
				GetComponent(Autoctrl).enabled = true;
	//		if(GetComponent(ThirdPersonPlayerAnimation))	
	//		GetComponent(ThirdPersonPlayerAnimation).enabled = true;
			if(	GetComponent(NavMeshAgent)	)	
				GetComponent(NavMeshAgent).enabled = true;
			if(	GetComponent(agentLocomotion)	)	
				GetComponent(agentLocomotion).enabled = true;		
			if(	GetComponent(Attack_simple)	)	
				GetComponent(Attack_simple).enabled =true;
			if(	GetComponent(ActiveSkill)	)	
				GetComponent(ActiveSkill).enabled =true;	
			if(	GetComponent(PassiveSkill)	)	
				GetComponent(PassiveSkill).enabled =true;		
			if(	GetComponent(hitsss)	)
				GetComponent(hitsss).enabled =true;
			if(	GetComponent(ThirdPersonWeapon)	)	
				GetComponent(ThirdPersonWeapon).enabled =true;						
			if(	GetComponent(MainPersonStatus)	)
				GetComponent(MainPersonStatus).enabled =true;
			if(	GetComponent(PlayerCollisionControl)	)
				GetComponent(PlayerCollisionControl).enabled =true;
			if(	GetComponent(SoulPet)	)
				GetComponent(SoulPet).enabled =true;
			transform.tag	=	"Player";
			if(UIControl.mapType == MapType.zhucheng){
				if(	GetComponent(PlayerApplyBuff)	)
					GetComponent(PlayerApplyBuff).enabled = false;		
				if(	GetComponent(PassiveSkill)	)
					GetComponent(PassiveSkill).enabled = false;		
//				if(	GetComponent(Attack_simple)	)
//					GetComponent(Attack_simple).enabled = false;		
			}
		}else{
			if(UIControl.mapType == MapType.zhucheng){
//				if(	GetComponent(ActiveSkill)	)
//					GetComponent(ActiveSkill).enabled = false;		
				if(	GetComponent(PlayerApplyBuff)	)
					GetComponent(PlayerApplyBuff).enabled = false;		
				if(	GetComponent(PassiveSkill)	)
					GetComponent(PassiveSkill).enabled = false;		
			}
		}
	}
	
	@RPC
	function	mover2(	position	:	Vector3	)
	{
		if(	!PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	)	)
			this.transform.position	=	position;
	} 

}