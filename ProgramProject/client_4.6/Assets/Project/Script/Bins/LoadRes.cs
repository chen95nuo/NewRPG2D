using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class LoadRes : MonoBehaviour {
	public static string RES_DIR  = "";
	public bool loadOver;
	
	public List<string> binNames;
	private List<Type> types;
	private List<int> loadWays;
	private int binsNum;
	public int loadedNum;
	private LoadingControl lc;
	
	void Awake(){
	}

	// Use this for initialization
	void Start ()
	{
		lc=gameObject.GetComponent<LoadingControl>();
		binNames=new List<string>();
		types=new List<Type>();
		loadWays=new List<int>();
		addBins();
		binsNum=binNames.Count;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(binsNum>0 && !loadOver)
		{
			if(loadedNum==binsNum)
			{
				loadOver=true;
				lc.loadBinsOver();
			}
		}
	}
	
	public void loadBins()
	{
		StartCoroutine(readAllData2());
	}
	
	private void addBins()
	{
		//============================第一种加载方式============================//
		binNames.Add("card.bin");
		types.Add(typeof(CardData));
		loadWays.Add(1);
		
		binNames.Add("skill.bin");
		types.Add(typeof(SkillData));
		loadWays.Add(1);
		
		binNames.Add("passiveskill.bin");
		types.Add(typeof(PassiveSkillData));
		loadWays.Add(1);
		
		binNames.Add("equip.bin");
		types.Add(typeof(EquipData));
		loadWays.Add(1);
		
		binNames.Add("talent.bin");
		types.Add(typeof(TalentData));
		loadWays.Add(1);
		
		binNames.Add("command.bin");
		types.Add(typeof(CommandData));
		loadWays.Add(1);
		
		binNames.Add("uniteskill.bin");
		types.Add(typeof(UnitSkillData));
		loadWays.Add(1);
		
		binNames.Add("energy.bin");
		types.Add(typeof(EnergyData));
		loadWays.Add(1);
		
		binNames.Add("cardproperty.bin");
		types.Add(typeof(CardPropertyData));
		loadWays.Add(1);
		
		binNames.Add("battle.bin");
		types.Add(typeof(BattleData));
		loadWays.Add(1);
		
		binNames.Add("camera.bin");
		types.Add(typeof(CameraData));
		loadWays.Add(1);
		
		binNames.Add("texts.bin");
		types.Add(typeof(TextsData));
		loadWays.Add(1);
		
		binNames.Add("items.bin");
		types.Add(typeof(ItemsData));
		loadWays.Add(1);
		
		binNames.Add("cardexp.bin");
		types.Add(typeof(CardExpData));
		loadWays.Add(1);
		
		binNames.Add("skillbasicexp.bin");
		types.Add(typeof(SkillBasicExpData));
		loadWays.Add(1);
		
		binNames.Add("player.bin");
		types.Add(typeof(PlayerData));
		loadWays.Add(1);
		
		binNames.Add("uieffect.bin");
		types.Add(typeof(UIEffectData));
		loadWays.Add(1);
		
		binNames.Add("equipupgrade.bin");
		types.Add(typeof(EquipUpgradeData));
		loadWays.Add(1);
		
		binNames.Add("rune.bin");
		types.Add(typeof(RuneData));
		loadWays.Add(1);
		
		binNames.Add("runetotal.bin");
		types.Add(typeof(RuneTotalData));
		loadWays.Add(1);
		
		binNames.Add("music.bin");
		types.Add(typeof(MusicData));
		loadWays.Add(1);
		
		binNames.Add("mapscene.bin");
		types.Add(typeof(MapSceneData));
		loadWays.Add(1);
		
		binNames.Add("passiveskillbasicexp.bin");
		types.Add(typeof(PassiveSkillBasicExpData));
		loadWays.Add(1);
		
		binNames.Add("unlock.bin");
		types.Add(typeof(UnlockData));
		loadWays.Add(1);
		
		binNames.Add("taskdata.bin");
		types.Add(typeof(TaskData));
		loadWays.Add(1);
			
		binNames.Add("allthreestar.bin");
		types.Add(typeof(AllThreeStarData));
		loadWays.Add(1);
		
		binNames.Add("skillexp.bin");
		types.Add(typeof(SkillExpData));
		loadWays.Add(1);
		
		binNames.Add("skillproperty.bin");
		types.Add(typeof(SkillPropertyData));
		loadWays.Add(1);
		
		binNames.Add("se.bin");
		types.Add(typeof(SeData));
		loadWays.Add(1);
		
		binNames.Add("power.bin");
		types.Add(typeof(PowerData));
		loadWays.Add(1);
		
		binNames.Add("dayly.bin");
		types.Add(typeof(DaylyData));
		loadWays.Add(1);
		
		binNames.Add("evolution.bin");
		types.Add(typeof(EvolutionData));
		loadWays.Add(1);
		
		binNames.Add("friendenergy.bin");
		types.Add(typeof(FriendenergyData));
		loadWays.Add(1);
		
		binNames.Add("energyup.bin");
		types.Add(typeof(EnergyupData));
		loadWays.Add(1);
		
		binNames.Add("changenamecost.bin");
		types.Add(typeof(ChangenamecostData));
		loadWays.Add(1);
		
		binNames.Add("name.bin");
		types.Add(typeof(NameData));
		loadWays.Add(1);
		
		binNames.Add("iconUnlock.bin");
		types.Add(typeof(IconUnlockData));
		loadWays.Add(1);
		
		binNames.Add("runelight.bin");
		types.Add(typeof(RuneLightData));
		loadWays.Add(1);
		
		binNames.Add("recharge.bin");
		types.Add(typeof(RechargeData));
		loadWays.Add(1);
		
        binNames.Add("cardtips.bin");
        types.Add(typeof(CardTipsData));
        loadWays.Add(1);

        binNames.Add("tips.bin");
        types.Add(typeof(TipsData));
        loadWays.Add(1);
		
		binNames.Add("vip.bin");
		types.Add(typeof(VipData));
		loadWays.Add(1);
		
		binNames.Add("vipgift.bin");
		types.Add(typeof(VipGiftData));
		loadWays.Add(1);
		
		binNames.Add("gift.bin");
		types.Add(typeof(GiftData));
		loadWays.Add(1);
		
		binNames.Add("friendcost.bin");
		types.Add(typeof(FriendCostData));
		loadWays.Add(1);
		
		binNames.Add("bagcost.bin");
		types.Add(typeof(BagCostData));
		loadWays.Add(1);
		
		binNames.Add("koaward.bin");
		types.Add(typeof(KOAwardData));
		loadWays.Add(1);
		
		binNames.Add("preload.bin");
		types.Add(typeof(PreloadData));
		loadWays.Add(1);

        binNames.Add("shop.bin");
        types.Add(typeof(ShopData));
        loadWays.Add(1);

        binNames.Add("blackmarket.bin");
        types.Add(typeof(BlackMarketData));
        loadWays.Add(1);

        binNames.Add("blackshopbox.bin");
        types.Add(typeof(BlackShopboxData));
        loadWays.Add(1);

        binNames.Add("blackrefresh.bin");
        types.Add(typeof(BlackRefreshData));
        loadWays.Add(1);
		
		binNames.Add("mazeskilldrop.bin");
		types.Add(typeof(MazeskilldropData));
		loadWays.Add(1);
		
		binNames.Add("usableitem.bin");
		types.Add(typeof(UsableItemData));
		loadWays.Add(1);
		
		
		binNames.Add("gamebox.bin");
		types.Add(typeof(GameBoxData));
		loadWays.Add(1);
		
		binNames.Add("vitality.bin");
		types.Add(typeof(VitalityData));
		loadWays.Add(1);
		
		binNames.Add("bloodbuff.bin");
		types.Add(typeof(BloodBuffData));
		loadWays.Add(1);
		
		binNames.Add("shoppvp.bin");
		types.Add(typeof(ShopPvpData));
		loadWays.Add(1);
		
		binNames.Add("cardku.bin");
		types.Add(typeof(CardKuData));
		loadWays.Add(1);

        
		
		//============================第二种加载方式============================//
		binNames.Add("break.bin");
		types.Add(typeof(BreakData));
		loadWays.Add(2);
		
		binNames.Add("equipproperty.bin");
		types.Add(typeof(EquippropertyData));
		loadWays.Add(2);
		
		binNames.Add("mission.bin");
		types.Add(typeof(MissionData));
		loadWays.Add(2);
		
		binNames.Add("viewpath.bin");
		types.Add(typeof(ViewPathData));
		loadWays.Add(2);
		
		binNames.Add("mapstruct.bin");
		types.Add(typeof(MapStructData));
		loadWays.Add(2);
		
		binNames.Add("compose.bin");
		types.Add(typeof(ComposeData));
		loadWays.Add(2);
		
		binNames.Add("cardget.bin");
		types.Add(typeof(CardGetData));
		loadWays.Add(2);
		
		binNames.Add("maze.bin");
		types.Add(typeof(MazeData));
		loadWays.Add(2);
		
		binNames.Add("imagination.bin");
		types.Add(typeof(ImaginationData));
		loadWays.Add(2);
		
		binNames.Add("imaginationcompose.bin");
		types.Add(typeof(ImaginationcomposeData));
		loadWays.Add(2);
		
		binNames.Add("event.bin");
		types.Add(typeof(EventData));
		loadWays.Add(2);
			
		binNames.Add("FBevent.bin");
		types.Add(typeof(FBeventData));
		loadWays.Add(2);
		
		binNames.Add("achievement.bin");
		types.Add(typeof(AchievementData));
		loadWays.Add(2);
		
		binNames.Add("racepower.bin");
		types.Add(typeof(RacePowerData));
		loadWays.Add(2);
		
		binNames.Add("mazeprobability.bin");
		types.Add(typeof(MazeprobabilityData));
		loadWays.Add(2);

        binNames.Add("sevendays.bin");
        types.Add(typeof(SevenDaysData));
        loadWays.Add(2);

        binNames.Add("levelgift.bin");
        types.Add(typeof(LevelGiftData));
        loadWays.Add(2);

        binNames.Add("cornucopia.bin");
        types.Add(typeof(CornucopiaData));
        loadWays.Add(2);
		
		binNames.Add("runner.bin");
        types.Add(typeof(RunnerData));
        loadWays.Add(2);
		
		binNames.Add("run.bin");
        types.Add(typeof(RunData));
        loadWays.Add(2);

        binNames.Add("meditation.bin");
        types.Add(typeof(MeditationData));
        loadWays.Add(2);
		
	}
	
	private IEnumerator readAllData2()
	{
		string msg="";
		try
		{
			for(int i=0;i<binNames.Count;i++)
			{
				string binName=binNames[i];
				msg=binName;
				Type type=types[i];
				int loadWay=loadWays[i];
				string path=LoadingControl.localPath+binName;
				if(loadWay==1)
				{
					BinReader2.readFile1(path, type);
				}
				else
				{
					BinReader2.readFile2(path, type);
				}
				loadedNum++;
			}
		}
		catch(Exception e)
		{
			Debug.Log(msg+" error");
			Debug.Log(e.Message);
			Debug.Log(e.StackTrace);
		}
		yield return null;
	}
}
