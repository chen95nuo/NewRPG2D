using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FBeventData : PropertyReader {

	/**编号**/
	public int id;
	/**关卡名称**/
	public string name;
	/**时间类型**/
	public int timestyle;
	/**场景**/
	public string scene;
	/**类型**/
	public int type;
    /**关卡类型**/
    public int leveltype;
	/* 头像图集*/
	public string atlas;
	/**boss头像**/
	public string bossicon;
	/**进入次数**/
	public int entry;
	/**进入等级**/
	public int unlocklevel;
	/**消耗体力**/
	public int cost;
	/**怪的数据:monster-level-atk-def-hp-skill-boss**/
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
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id, this);
	}

	public void parse(string[] ss)
	{
		int location =0;
		id =StringUtil.getInt(ss[location]);
		name =StringUtil.getString(ss[location+1]);
		timestyle=StringUtil.getInt(ss[location+2]);
		scene =StringUtil.getString(ss[location+3]);
		type =StringUtil.getInt(ss[location+4]);
        leveltype = StringUtil.getInt(ss[location + 5]);
		atlas = StringUtil.getString(ss[location+6]);
		bossicon =StringUtil.getString(ss[location+7]);
		
		entry=StringUtil.getInt(ss[location+8]);
		unlocklevel =StringUtil.getInt(ss[location+9]);
		cost =StringUtil.getInt(ss[location+10]);
		monsters =new string [6];
		for(int i=0;i<6;i++)
		{
			location=11+i*6;
			int monster =StringUtil.getInt(ss[location]);
			int level =StringUtil.getInt(ss[location+1]);
			int atk =StringUtil.getInt(ss[location+2]);
			int def =StringUtil.getInt(ss[location+3]);
			int hp =StringUtil.getInt(ss[location+4]);
			int skill =StringUtil.getInt(ss[location+5]);
			int boss =StringUtil.getInt(ss[location+6]);
			string monter_info =monster+"-"+level+"-"+atk+"-"+def+"-"+hp+"-"+skill+"-"+boss;
			monsters[i] =monter_info;
		}
		bossSkills =new string[3];
		for(int i=0;i<3;i++)
		{
			location=11+7*6+i*2;
			int bossSkill=StringUtil.getInt(ss[location]);
			int cd=StringUtil.getInt(ss[location+1]);
			bossSkills[i]=bossSkill+"-"+cd;
		}
		location =11+7*6+2*3;
		personexp =StringUtil.getInt(ss[location]);
		cardexp =StringUtil.getInt(ss[location+1]);
		coins =StringUtil.getInt(ss[location+2]);
		drops=new List<string>();
		for(int i=0;i<6;i++)
		{
			location=11+7*6+3*2+3+i*3;
			int drop=StringUtil.getInt(ss[location]);
			string pro=StringUtil.getString(ss[location+1]);
			int ability = StringUtil.getInt(ss[location+2]);
			if(!"".Equals(pro))
			{
				drops.Add(drop+"-"+pro+"-"+ability);
			}
		}
		addData();
	}

	public void resetData()
	{
		data.Clear();
	}
	
	public static FBeventData getData(int index)
	{
		return (FBeventData)data[index];
	}
}
