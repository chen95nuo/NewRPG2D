using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementData : PropertyReader {
	
	/**编号**/
	public int id;
	/**前置成就**/
	public int exachieve;
	/**成就名称**/
	public string name;
	/**成就类型**/
	public int type;
	/**成就要求**/
	public string request;
	/**描述**/
	public string description;
	/**成就图标**/
	public string icon;
	/**奖励类别-奖励内容**/
	public List<string> reward;
	
	/* 不再生效 */
	public int disable;

	private static Hashtable data=new Hashtable();
	private static List<AchievementData> dataList=new List<AchievementData>();
	
	public void addData()
	{
		data.Add(id, this);
		dataList.Add(this);
	}
	
	public void resetData(){}
	
	public void parse(string[] ss)
	{
		int location=0;
		id=StringUtil.getInt(ss[location++]);
		exachieve=StringUtil.getInt(ss[location++]);
		name = StringUtil.getString(ss[location++]);
		type=StringUtil.getInt(ss[location++]);
		request=StringUtil.getString(ss[location++]);
		description=StringUtil.getString(ss[location++]);
		icon=StringUtil.getString(ss[location++]);
		
		int length=(ss.Length-location)/2;
		reward=new List<string>();
		for(int i=0;i<length;i++)
		{
			int rewardType=StringUtil.getInt(ss[location++]);
			string rewardDetail=StringUtil.getString(ss[location++]);
			if(rewardType!=0)
			{
				reward.Add(rewardType+"-"+rewardDetail);
			}
		}
		disable = StringUtil.getInt(ss[location++]);
		addData();
	}
	
	public static AchievementData getData(int id)
	{
		return (AchievementData)data[id];
	}
	
	public static List<AchievementData> getNextAchieveMent(int id)
	{
		List<AchievementData> result=new List<AchievementData>();
		foreach(AchievementData ad in dataList)
		{
			if(ad.exachieve==id)
			{
				result.Add(ad);
			}
		}
		return result;
	}
	
	public static List<int> getPreAchieve(int id)
	{
		List<int> result=new List<int>();
		AchievementData ad=AchievementData.getData(id);
		if(ad!=null && ad.exachieve>0)
		{
			ad=AchievementData.getData(ad.exachieve);
			while(ad!=null)
			{
				result.Insert(0,ad.id);
				if(ad.exachieve>0)
				{
					ad=AchievementData.getData(ad.exachieve);
				}
				else
				{
					break;
				}
			}
		}
		return result;
	}

}
