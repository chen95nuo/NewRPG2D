#pragma strict
class MonsterUIControl extends Song{
	var LabelMonsterName : UILabel;
	var LabelMonsterLevel : UILabel;
	var MonsterS : MonsterStatus;
	private var playerS : PlayerStatus;
	var fsHP : UIFilledSprite;
	var fsNU : UIFilledSprite;
	var touxiang : UISprite;
	var tanhao : UISprite;
	private var float1 : float;
	private var float2 : float;
	private var float3 : float;
	private var float4 : float;
	var TransMonster : Transform;
	var SpriteBoss : UISprite;
	var LabelHP : UILabel;
	var LableNU : UILabel;
	function Awake(){
		AllManage.MonsterUICLStatic = this;
	}

	function Update () {
		
		if(	MonsterS != null	){
		
			if(!TransMonster.gameObject.active){
				TransMonster.gameObject.SetActiveRecursively(true);		
			}
			float1 = parseInt(MonsterS.Health);
			float2 = parseInt(MonsterS.Maxhealth);
			LabelMonsterName.text = MonsterS.Name.ToString();
			LabelMonsterLevel.text = MonsterS.Level.ToString();
			float3 = MonsterS.Mana;
			float4 = MonsterS.MaxMana;
			fsHP.fillAmount = float1 / float2;
			fsNU.fillAmount = float3 / float4;
//			touxiang.spriteName = MonsterS.gameObject.name.Substring(0,5);
			LabelHP.text =String.Format("{0}/{1}",MonsterS.Health, MonsterS.Maxhealth); 
			LableNU.text =String.Format("{0}/{1}",MonsterS.Mana, MonsterS.MaxMana); 
			getMonster(MonsterS);
		}else
		if(PlayerS != null){
			if(!TransMonster.gameObject.active){
				TransMonster.gameObject.SetActiveRecursively(true);		
			}
			float1 = parseInt(PlayerS.Health);
			float2 = parseInt(PlayerS.Maxhealth);
			LabelMonsterName.text = PlayerS.PlayerName.ToString();
			LabelMonsterLevel.text = "Lv." + PlayerS.Level.ToString();
			float3 = parseInt(PlayerS.Mana);
			float4 = parseInt(PlayerS.Maxmana);
			fsHP.fillAmount = float1 / float2;
			fsNU.fillAmount = float3 / float4;
			LabelHP.text = String.Format("{0}/{1}",PlayerS.Health, PlayerS.Maxhealth);
			LableNU.text = String.Format("{0}/{1}",PlayerS.Mana, PlayerS.Maxmana);
			getMonster(PlayerS);
		}else{
			if(TransMonster.gameObject.active){
				TransMonster.gameObject.SetActiveRecursively(false);		
			}
		}
	}

	var atlasMonster : UIAtlas;
	var atlasPlayer : UIAtlas;
	function getMonster(ms : MonsterStatus){
		MonsterS = ms;
		if(MonsterS.showtask==false){
		tanhao.enabled = false;
		}else{
		tanhao.enabled = true;
		}
		
		if(MonsterS){
			if(touxiang.atlas != atlasMonster){
				touxiang.atlas = atlasMonster;
			}
			if(MonsterS.monsterLevel == MonsterLEVEL.Monster || MonsterS.monsterLevel == MonsterLEVEL.PET){
				if(touxiang.spriteName != "targetbossM"){
					touxiang.spriteName = "targetbossM";
				}
			}
			if(MonsterS.monsterType == MonsterType.pro || MonsterS.monsterType == MonsterType.strong){
				fsNU.spriteName = "UIM_Anger Article";
			}else
			if(MonsterS.monsterType == MonsterType.rogue){
				fsNU.spriteName = "UIM_Charge_ Article";
			}else
			if(MonsterS.monsterType == MonsterType.magic){
				fsNU.spriteName = "UIM_Magic Article";
			}
			if(MonsterS.monsterLevel == MonsterLEVEL.RAID || MonsterS.monsterLevel == MonsterLEVEL.BIGBOSS || MonsterS.monsterLevel == MonsterLEVEL.BOSS || MonsterS.monsterLevel == MonsterLEVEL.Elite){
				if(MonsterS.monsterLevel == MonsterLEVEL.Elite){
					SpriteBoss.spriteName = "targetbossY";
				}else{
					SpriteBoss.spriteName = "targetbossG";				
				}
				SpriteBoss.enabled = true;
			}else{
				SpriteBoss.enabled = false;			
			}
		}
	}

	private var PlayerS : PlayerStatus;
	function getMonster(ms : PlayerStatus){
		PlayerS = ms;
		if(SpriteBoss.enabled){
			SpriteBoss.enabled = false;
		}
		if(PlayerS){
			if(touxiang.atlas != atlasPlayer){
				touxiang.atlas = atlasPlayer;
			}
			if(PlayerS.ProID == 1){
				switch(PlayerS.BranchID){
					case "0" :
						touxiang.spriteName = "head-zhanshi";
						break; 
					case "1" :
						touxiang.spriteName = "UIM_Anti-War_N";
						break; 
					case "2" :
						touxiang.spriteName = "UIM_Violent-War_N";
						break; 
					case "3" :
						touxiang.spriteName = "UIM_Violent-War_N";
						break; 
				}
//				touxiang.spriteName = "head-zhanshi";
				fsNU.spriteName = "UIM_Anger Article";
			}else
			if(PlayerS.ProID == 2){
				switch(PlayerS.BranchID){
					case "0" :
						touxiang.spriteName = "head-youxia";
						break; 
					case "1" :
						touxiang.spriteName = "UIM_Robber_O";
						break; 
					case "2" :
						touxiang.spriteName = "UIM_Ranger_N ";
						break; 
					case "3" :
						touxiang.spriteName = "UIM_Ranger_N ";
						break; 
				}
//				touxiang.spriteName = "head-youxia";
				fsNU.spriteName = "UIM_Charge_ Article";
			}else
			if(PlayerS.ProID == 3){
				switch(PlayerS.BranchID){
					case "0" :
						touxiang.spriteName = "head-fashi";
						break; 
					case "1" :
						touxiang.spriteName = "UIM_Master_N";
						break; 
					case "2" :
						touxiang.spriteName = "UIM_Necromancer_N ";
						break; 
					case "3" :
						touxiang.spriteName = "UIM_Master_N";
						break; 
				}
//				touxiang.spriteName = "head-fashi";
				fsNU.spriteName = "UIM_Magic Article";
			}
		}
	}
	function getMonster(){
		PlayerS = null;
		MonsterS = null;
	}
}