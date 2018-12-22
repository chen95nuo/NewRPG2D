using UnityEngine;
using System.Collections.Generic;

public class MissionData : PropertyReader
{
	/**关卡ID**/
	public int id{get;set;}
	/**关卡名称**/
	public string name{get;set;}
	/**地图**/
	public int map{get;set;}
	/**区域**/
	public int zone{get;set;}
	/**关卡类型**/
	public int missiontype{get;set;}
	/**区域名称**/
	public string zonename{get;set;}
	/**场景名称**/
	public string scene{get;set;}
	/**1普通关卡,2BOSS关卡**/
	public int type{get;set;}
	/**出现条件**/
	public int unlockshowup{get;set;}
	/**进入等级**/
	public int unlocklevel{get;set;}
	
	public string bossicon{get;set;}
	/**条件说明**/
	public string description{get;set;}
	/**消耗体力**/
	public int cost{get;set;}
	/**进入次数**/
	public int times{get;set;}
	/**6个怪的数据**/
	public string[] monsters;
	/**BOSS技能和cd**/
	public string[] bossSkills;
	/**人物经验**/
	public int personexp;
	/**卡牌经验**/
	public int cardexp;
	/**掉落金币**/
	public int coins;
	/**掉落物品**/
	public List<string> drops;
	/**额外奖励**/
	public int specialtype1;
	public string specialaward1;
	public int specialtype2;
	public string specialaward2;
	/**每次点击增加的长度**/
	public int bonusGrow;
	/**BONUS减少**/
	public int bonusReduce;
	/**持续时间**/
	public int lastTime;
	//任务类型//
	public int addtasktype;
	// 显示类型 // 
	public int showtype;
	// 任务需求 //
	public int addtaskid;
	// KO积分 //
	public int kopoint;
	
	
	private static List<MissionData> data=new List<MissionData>();
	
	public void addData()
	{
		data.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
		int location=0;
		id=StringUtil.getInt(ss[location]);
		name=StringUtil.getString(ss[location+1]);
		map=StringUtil.getInt(ss[location+2]);
		zone=StringUtil.getInt(ss[location+3]);
		missiontype=StringUtil.getInt(ss[location+4]);
		zonename=StringUtil.getString(ss[location+5]);
		scene=StringUtil.getString(ss[location+6]);
		
		type=StringUtil.getInt(ss[location+7]);
		unlockshowup=StringUtil.getInt(ss[location+8]);
		unlocklevel=StringUtil.getInt(ss[location+9]);
		bossicon=StringUtil.getString(ss[location+10]);
		description=StringUtil.getString(ss[location+11]);
		cost=StringUtil.getInt(ss[location+12]);
		times=StringUtil.getInt(ss[location+13]);
		
		monsters=new string[6];
		for(int i=0;i<6;i++)
		{
			location=14+i*7;
			int monster=StringUtil.getInt(ss[location]);
			int level=StringUtil.getInt(ss[location+1]);
			int atk=StringUtil.getInt(ss[location+2]);
			int def=StringUtil.getInt(ss[location+3]);
			int hp=StringUtil.getInt(ss[location+4]);
			string skill=StringUtil.getString(ss[location+5]);
			int BOSS=StringUtil.getInt(ss[location+6]);

			string monsterInfo=monster+"-"+level+"-"+atk+"-"+def+"-"+hp+"-"+skill+"-"+BOSS;
			monsters[i]=monsterInfo;
		}
		bossSkills=new string[3];
		for(int i=0;i<3;i++)
		{
			location=14+7*6+i*2;
			int bossSkill=StringUtil.getInt(ss[location]);
			int cd=StringUtil.getInt(ss[location+1]);
			bossSkills[i]=bossSkill+"-"+cd;
		}
		location=14+7*6+3*2;
		personexp=StringUtil.getInt(ss[location]);
		cardexp=StringUtil.getInt(ss[location+1]);
		coins=StringUtil.getInt(ss[location+2]);
		drops=new List<string>();
		for(int i=0;i<6;i++)
		{
			location=14+7*6+3*2+3+i*3;
			int droptype=StringUtil.getInt(ss[location]);
			string dropitem=StringUtil.getString(ss[location+1]);
			int pro=StringUtil.getInt(ss[location+2]);
			if(dropitem!="" && pro>0)
			{
				drops.Add(droptype+"-"+dropitem+"-"+pro);
			}
		}
		location=14+7*6+3*2+3+6*3;
		specialtype1=StringUtil.getInt(ss[location]);
		specialaward1=StringUtil.getString(ss[location+1]);
		specialtype2=StringUtil.getInt(ss[location+2]);
		specialaward2=StringUtil.getString(ss[location+3]);
		bonusGrow=StringUtil.getInt(ss[location+4]);
		bonusReduce=StringUtil.getInt(ss[location+5]);
		lastTime=StringUtil.getInt(ss[location+6]);
		
		addtasktype=StringUtil.getInt(ss[location+7]);
		showtype=StringUtil.getInt(ss[location+8]);
		addtaskid=StringUtil.getInt(ss[location+9]);
		kopoint=StringUtil.getInt(ss[location+10]);
		
		addData();
	}
	
	public static MissionData getData(int id)
	{
		foreach(MissionData md in data)
		{
			if(md.id==id)
			{
				return md;
			}
		}
		return null;
	}
	
	public static MissionData getUnlockData(int missionId)
	{
		foreach(MissionData md in data)
		{
			if(md.unlockshowup==missionId)
			{
				return md;
			}
		}
		return null;
	}
	
	/**missiontype:1普通关,2精英关**/
	public static List<MissionData> getData(int map,int zone,int missiontype)
	{
		List<MissionData> result=new List<MissionData>();
		foreach(MissionData md in data)
		{
			if(md.map==map && md.zone==zone && md.missiontype==missiontype)
			{
				result.Add(md);
			}
		}
		return result;
	}
	
	public static string getZoneName(int map,int zone,int missionType)
	{
		foreach(MissionData md in data)
		{
			if(md.map==map && md.zone==zone && md.missiontype==missionType)
			{
				return md.zonename;
			}
		}
		return null;
	}
	/**是否应该显示**/
	public bool canShowUp(int missionId)
	{
		if(missionId>=unlockshowup)
		{
			return true;
		}
		return false;
	}
	/**是否已完成**/
	public bool haveCompleted(int missionId)
	{
		if(missionId>=id)
		{
			return true;
		}
		return false;
	}
	
	public string getMissionInfoText()
	{
		string otherText="";
		if(cost>0)
		{
			if(!string.IsNullOrEmpty(otherText))
			{
				otherText+="\n";
			}
			otherText+=TextsData.getData(13).chinese+cost;
		}
		if(unlocklevel>0)
		{
			if(!string.IsNullOrEmpty(otherText))
			{
				otherText+="\n";
			}
			otherText+=TextsData.getData(14).chinese+unlocklevel;
		}
		MissionData comp=MissionData.getData(unlockshowup);
		if(comp!=null)
		{
			if(!string.IsNullOrEmpty(otherText))
			{
				otherText+="\n";
			}
			otherText+=TextsData.getData(15).chinese+comp.name;
		}
		return otherText;
	}
	
	public static int getOldScreenNum(int missionId)
	{
		int curScreenNum=0;
		MissionData md=getData(missionId);
		if(md!=null)
		{
			curScreenNum=md.map-1;
		}
		return curScreenNum;
	}
	
	public static int getNewScreenNum(int missionId)
	{
		int curScreenNum=7;
		MissionData md=getUnlockData(missionId);
		if(md!=null)
		{
			curScreenNum=md.map-1;
		}
		return curScreenNum;
	}
	
	public int getSequence()
	{
		int result=0;
		for(int i=0;i<data.Count;i++)
		{
			MissionData md=data[i];
			if(id/100000==md.id/100000 && id>=md.id)
			{
				result++;
			}
		}
		return result;
	}
	
	public bool isFirstZoneMission()
	{
		for(int i=0;i<data.Count;i++)
		{
			MissionData md=data[i];
			if(md.map==map && md.zone==zone && md.missiontype==missiontype)
			{
				if(md.id==id)
				{
					return true;
				}
				break;
			}
		}
		return false;
	}
	
	public bool isLastMissionInZone()
	{
		List<MissionData> mds=new List<MissionData>();
		for(int i=0;i<data.Count;i++)
		{
			MissionData md=data[i];
			if(md.map==map && md.zone==zone && md.missiontype==missiontype)
			{
				mds.Add(md);
			}
		}
		return mds[mds.Count-1].id==id;
	}
	
}
