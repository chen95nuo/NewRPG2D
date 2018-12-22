using UnityEngine;
using System.Collections;

public class UniteBtnManager : MonoBehaviour {
	
	public static UniteBtnManager mInstance ;
	
	public PVESceneControl pveControl;

    public UIInterfaceManager uiInterface;
//	public int uniteSkillId01;
//	public int uniteSkillId02;
//	public int uniteSkillId03;
	
	//合体技btn//
	public GameObject[] skillBtn;
	//每个合体技btn释放需要的怒气值//
	public UILabel[] btnLabel;
	
	public GameObject[] skillBlackBtn;
	public UIAtlas UniteSNFAtlas;
	
	
	private UnitSkillData[] skillDatas;
	
	private int uniteSkillNum;
	private Player curPlayer;
	private float preTime;
	Hashtable powerEF = new Hashtable();

    public UISprite automaticLabelFriend;

    public UISprite automaticLbaelOneself;
	//==是否已经使用了好友合体技==//
	private bool usedFriendSkill;
	//private string btnEffName = "";
	private string btnEffPath = "";
	private int btnEffId = 10006;

    bool isFrist;
	void Awake(){
		mInstance = this;
		
	}
	// Use this for initialization
	void Start () {
		
		UIEffectData ued = UIEffectData.getData(btnEffId);
		btnEffPath = ued.path + ued.name;
	}
	
	public void init()
	{
        isAutomaticRelease = PlayerInfo.getInstance().isAutomaticRelease;
        if (PlayerInfo.getInstance().battleType == 1)
        {
            if (isAutomaticRelease)
            {
                isFrist = true;
                automaticLbaelOneself.spriteName = "zidonghua2";
                automaticLabelFriend.spriteName = "zidonghua2";
            }
            else
            {
                automaticLbaelOneself.spriteName = "zidonghua1";
                automaticLabelFriend.spriteName = "zidonghua1";
                automaticLabelFriend.gameObject.gameObject.SetActive(false);
            }
        }
        else
        {
            automaticLabelFriend.gameObject.SetActive(false);
            automaticLbaelOneself.gameObject.SetActive(false);
        }
		//全部设置为不可用//
		if(skillBtn != null)
		{
			foreach(GameObject gb in skillBtn)
			{
				UISprite icon = gb.transform.FindChild("Icon").GetComponent<UISprite>();
				icon.gameObject.SetActive(false);
			}
		}
		if(btnLabel!=null)
		{
			foreach(UILabel bl in btnLabel)
			{
				bl.gameObject.SetActive(false);
			}
		}
		//需要的设置为可用//
		uniteSkillNum = pveControl.unitSkills.Length;
		skillDatas = new UnitSkillData[uniteSkillNum];
		for(int i= 0;i < uniteSkillNum; i ++)
		{
			skillDatas[i] = (UnitSkillData)pveControl.unitSkills[i]; 
			
			if(skillDatas[i]==null)
			{
				continue;
			}
			//显示btn//
			if(skillBtn != null)
			{
				skillBtn[i].SetActive(true);
				UISprite icon = skillBtn[i].transform.FindChild("Icon").GetComponent<UISprite>() ;
				icon.atlas = UniteSNFAtlas;
				icon.gameObject.SetActive(true);
				icon.spriteName = skillDatas[i].icon;
			}
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
			{
				btnLabel[0].gameObject.SetActive(true);
				btnLabel[0].text = pveControl.demoPlayerAUnitSkillEnergy.ToString();
			}
			else
			{
				if(btnLabel.Length > i)
				{
					NGUITools.SetActive(btnLabel[i].gameObject, true);
					btnLabel[i].text = skillDatas[i].cost + "";
				}
			}
			
		}
		
		curPlayer = pveControl.players[0];
		
		
		btnLabel[2].text="";
		skillBtn[1].SetActive(false);
		
		
		//==新手和推图才显示援护合体技==//
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			if(PlayerInfo.getInstance().brj.md < STATE.BATTEL_WITH_FRIEDN_ID && GuideManager.getInstance().getCurrentGuideID() <=(int)GuideManager.GuideType.E_Battle3_Friend )
			{
				skillBtn[2].SetActive(false);
			}
			else
			{
				skillBtn[2].SetActive(true);
			}
		}
		else
		{
			skillBtn[2].SetActive(false);
		}
	}
	
	// Update is called once per frame	
	void Update ()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			return;
		}
		if(skillDatas==null || curPlayer==null)
		{
			return;
		}
		//==pvp自动释放第一个技能==//
		if(skillDatas[0]!=null && PlayerInfo.getInstance().battleType==3 && curPlayer.getEnergy() >= skillDatas[0].cost)
		{
			UniteSkillBtn01();
		}
		if(!PVESceneControl.mInstance.GetBattleOver() && PlayerInfo.getInstance().battleType==1 &&!usedFriendSkill && pveControl.canUnitSkill[2])
		{
            if (skillDatas[2] != null && pveControl.getRound() >= FriendenergyData.getNumber(skillDatas[2].index))
            {
                CreatePowerEf3(btnEffPath, -1);
            }
		}
		if(skillDatas[2]!=null && skillBtn[2].activeSelf && pveControl.getRound()>=FriendenergyData.getNumber(skillDatas[2].index) && !usedFriendSkill&& pveControl.canUnitSkill[2])
		{
			skillBlackBtn[2].SetActive(false);
		}
        for (int i = 0; i < uniteSkillNum - 1; i++)
        {
            //检查是否满足合体技的消耗，以及当前按钮的特效是否播放//
            if (skillDatas[i] != null && PlayerInfo.getInstance().battleType == 1 && skillBlackBtn[i].activeSelf)
            {
                isAutomaticReleaseClick = true;
                if (!automaticLabelFriend.gameObject.activeSelf)
                {
                    if (isFrist)
                    {

                        if (automaticLbaelOneself.spriteName.Equals("zidonghua1"))
                        {
                            automaticLbaelOneself.gameObject.SetActive(false);
                        }
                        else
                            automaticLabelFriend.gameObject.SetActive(true);
                    }
                }
                if (!automaticLbaelOneself.gameObject.activeSelf)
                {
                        automaticLbaelOneself.gameObject.SetActive(true);
                }
                break;
            }
            else
            {
                isAutomaticReleaseClick = false;
                automaticLabelFriend.gameObject.SetActive(false);
                automaticLbaelOneself.gameObject.SetActive(false);

            }

        }
        if (isAutomaticRelease)
        {
            if (skillDatas[0] != null && curPlayer.getEnergy() >= skillDatas[0].cost)
            {
                UniteSkillBtn(uniteSkillId);
            }
            else
            {
                UniteSkillBtn03();
            }
        }
	}






    bool isAutomaticRelease;

    int uniteSkillId;

    bool isAutomaticReleaseClick;
	public void UniteSkillBtn(int id){
		//==pvp不能点==//
		if(PlayerInfo.getInstance().battleType==3)
		{
			return;
		}
		//==好友合体技不费怒气==//
		if(id==2)
		{
			UniteSkillBtn03();
			return;
		}
        if (skillDatas[0] != null && curPlayer.getEnergy() >= skillDatas[0].cost)
        {
            //==记录log==//
            pveControl.addBattleLog(skillDatas[0].index + "-" + skillDatas[0].cost + "-" + curPlayer.getMaxEnergy());
            //使用怒气释放合体技//
            curPlayer.removeEnergy(skillDatas[id].cost);
            pveControl.unitSkillQueue.Enqueue(new UnitSkill(0, skillDatas[id], pveControl, false));
            Debug.Log("队列加入合体技:" + skillDatas[id].name);
            //删除特效//
            RemovePowerEf();
            //改变士气槽//
            pveControl.energyManager.energyChange();
        }
        else
        {
            int unlockLevel = UnlockData.getData(33).method;
            if (PlayerInfo.getInstance().player.level < unlockLevel)
            {

                if (!uiInterface.AddSpeedTip.activeSelf)
                {

                    uiInterface.AddSpeedTip.SetActive(true);
                }
            }
            else
            {
                if (isAutomaticReleaseClick)
                {
                    if (isAutomaticRelease)
                    {
                        automaticLbaelOneself.spriteName = "zidonghua1";
                        automaticLabelFriend.spriteName = "zidonghua1";
                        automaticLabelFriend.gameObject.gameObject.SetActive(false);
                        isAutomaticRelease = false;
                        PlayerInfo.getInstance().isAutomaticRelease = isAutomaticRelease;
                    }
                    else
                    {
                        uniteSkillId = id;
                        isAutomaticRelease = true;
                        PlayerInfo.getInstance().isAutomaticRelease = isAutomaticRelease;
                        isFrist = true;
                        automaticLbaelOneself.spriteName = "zidonghua2";
                        automaticLabelFriend.spriteName = "zidonghua2";
                    }
                }
            }
        }
	}
	
	public void UniteSkillBtn01()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			if(curPlayer.getEnergy() >= pveControl.demoPlayerAUnitSkillEnergy)
			{
				//使用怒气释放合体技//
				curPlayer.removeEnergy(pveControl.demoPlayerAUnitSkillEnergy);
				pveControl.unitSkillQueue.Enqueue(new UnitSkill(0, skillDatas[0], pveControl,false));
				Debug.Log("队列加入合体技:"+skillDatas[0].name);
				//删除特效//
				RemovePowerEf();
				//改变士气槽//
				pveControl.energyManager.energyChange();
			}
		}
		else
		{
			if(curPlayer.getEnergy() >= skillDatas[0].cost)
			{
				//==记录log==//;
				pveControl.addBattleLog(skillDatas[0].index+"-"+skillDatas[0].cost+"-"+curPlayer.getMaxEnergy());
				//使用怒气释放合体技//
				curPlayer.removeEnergy(skillDatas[0].cost);
				pveControl.unitSkillQueue.Enqueue(new UnitSkill(0, skillDatas[0], pveControl,false));
				Debug.Log("队列加入合体技:"+skillDatas[0].name);
				//删除特效//
				RemovePowerEf();
				//改变士气槽//
				pveControl.energyManager.energyChange();
			}
		}
		
	}
	
	public void UniteSkillBtn02()
	{
		//Debug.Log("点击合体技02：" + skillDatas[1].name);
		//Debug.Log("点击合体技02是否可用：" + pveControl.canUnitSkill[1]);
		//if(curPlayer.getEnergy()>=skillDatas[1].cost && pveControl.canUnitSkill[1])
		if(curPlayer.getEnergy() >= skillDatas[1].cost)
		{
			curPlayer.removeEnergy(skillDatas[1].cost);
			pveControl.unitSkillQueue.Enqueue(new UnitSkill(0, skillDatas[1], pveControl,false));
			//Debug.Log("队列加入合体技:"+skillDatas[1].name);
			//删除特效//
			RemovePowerEf();
			//改变士气槽//
			pveControl.energyManager.energyChange();
		}
	}
	
	public void UniteSkillBtn03()
	{
		if(skillDatas[2] != null && pveControl.getRound()>=FriendenergyData.getNumber(skillDatas[2].index) && !usedFriendSkill)
		{
			usedFriendSkill=true;
			pveControl.unitSkillQueue.Enqueue(new UnitSkill(0, skillDatas[2], pveControl,true));
			//Debug.Log("队列加入合体技:"+skillDatas[2].name);
			if(powerEF != null && powerEF[2] != null)
			{
				GameObject obj = ((GameObject)powerEF[2]);
				Destroy(obj);
				powerEF.Remove(2);
			}
		}
	}
	
	//创建合体技按钮特效//
	public void CreatePowerEf(string effectName, int type = 1)
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			//UnitSkillData usd = skillDatas[0];	
			if(curPlayer.getEnergy()>=pveControl.demoPlayerAUnitSkillEnergy && pveControl.canUnitSkill[0] && skillBlackBtn[0].activeSelf)
			{
				GameObject obj = Instantiate(GameObjectUtil.LoadResourcesPrefabs(effectName,type))as GameObject;;
				powerEF.Add(0, obj);
				GameObjectUtil.setGameObjectLayer(obj,8);
				obj.transform.parent=skillBtn[0].transform;
				obj.transform.localPosition=new Vector3(0,-10,-10);
				obj.transform.localScale = new Vector3(350,350,350);
				skillBlackBtn[0].SetActive(false);
				pveControl.needShowUnitSkillDialog = true;
				pveControl.finishShowUnitSkillDialog = false;
			}
		}
		else
		{
			for(int i= 0;i < uniteSkillNum-1; i ++)
			{
				//检查是否满足合体技的消耗，以及当前按钮的特效是否播放//
                if (skillDatas[i] != null && curPlayer.getEnergy() >= skillDatas[i].cost && pveControl.canUnitSkill[i] && skillBlackBtn[i].activeSelf)
                {
                    GameObject obj = Instantiate(GameObjectUtil.LoadResourcesPrefabs(effectName, type)) as GameObject; ;
                    powerEF.Add(i, obj);
                    GameObjectUtil.setGameObjectLayer(obj, 8);
                    obj.transform.parent = skillBtn[i].transform;
                    obj.transform.localPosition = new Vector3(0, -10, -10);
                    obj.transform.localScale = new Vector3(350, 350, 350);
                    skillBlackBtn[i].SetActive(false);
                }
               
			}
		}
		
		
		
	}
	
	private void CreatePowerEf3(string effectName, int type = 1)
	{
//		return;
		//检查是否满足合体技的消耗，以及当前按钮的特效是否播放//
		if(skillDatas[2]!=null && powerEF[2] == null)
		{
//			GameObject obj =PVESceneControl.initPretab(effectName,1);
			GameObject obj = Instantiate(GameObjectUtil.LoadResourcesPrefabs(effectName,type))as GameObject;;
			powerEF.Add(2, obj);
			GameObjectUtil.setGameObjectLayer(obj,8);
			obj.transform.parent=skillBtn[2].transform;
			obj.transform.localPosition=new Vector3(0,0,-10);
			obj.transform.localScale = new Vector3(350,350,350);
			skillBlackBtn[2].SetActive(false);
		}
		if(skillDatas[2]!=null && skillBlackBtn[2].activeSelf)
		{
			skillBlackBtn[2].SetActive(false);
		}
	}
	
	//当怒气值不够时删除合体技按钮上的特效//
	public void RemovePowerEf()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			if(skillDatas[0]!=null && (curPlayer.getEnergy()<pveControl.demoPlayerAUnitSkillEnergy || !pveControl.canUnitSkill[0]))
			{
				skillBlackBtn[0].SetActive(true);
				if(powerEF != null && powerEF[0] != null)
				{
					GameObject obj = ((GameObject)powerEF[0]);
					Destroy(obj);
					powerEF.Remove(0);
				}
			}
		}
		else
		{
			for(int i= 0;i < uniteSkillNum; i ++)
			{
				if(skillDatas[i]!=null && (curPlayer.getEnergy()<skillDatas[i].cost || !pveControl.canUnitSkill[i]))
				{
					skillBlackBtn[i].SetActive(true);
					if(powerEF != null && powerEF[i] != null)
					{
						GameObject obj = ((GameObject)powerEF[i]);
						Destroy(obj);
						powerEF.Remove(i);
					}
				}
			}
		}
		
	}
	
	public void RemovAllEff()
	{
		if(powerEF != null)
		{
			
			for(int i = powerEF.Count - 1;i >=0 ;i--)
			{
				GameObject obj = ((GameObject)powerEF[i]);
				Destroy(obj);
				powerEF.Remove(i);
			}
			powerEF.Clear();
		}
	}
	
	public void hideFriendSkillBtn()
	{
		skillBtn[2].SetActive(false);
	}
	
	public void gc()
	{
		pveControl=null;
		skillDatas=null;
		curPlayer=null;
		if(powerEF!=null)
		{
			powerEF.Clear();
			powerEF=null;
		}
		
		mInstance = null;
		
	}
}