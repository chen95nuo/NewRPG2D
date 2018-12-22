using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventData : PropertyReader {

	/**编号**/
	public int id;
	/**位置编号**/
	public int positionid;
	/**时间类型**/
	public int timestyle;
    /**名称类型**/
    public int nametype;
	/**活动名称**/
	public string name;
	/**活动描述**/
	public string content;
	/**活动图片**/
	public string image;
	/**突击图集**/
	public string atlas;
	/**活动种族**/
	public int raceimage;
	/**副本信息:fbid-number**/
	public List<int> fbids;

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id, this);
	}

	public void parse(string[] ss)
	{
		int location =0;
		id =StringUtil.getInt(ss[location]);
		positionid =StringUtil.getInt(ss[location+1]);
		timestyle =StringUtil.getInt(ss[location+2]);
        nametype = StringUtil.getInt(ss[location + 3]);
		name =StringUtil.getString(ss[location+4]);
		content =StringUtil.getString(ss[location+5]);
		image =StringUtil.getString(ss[location+6]);
		atlas = StringUtil.getString(ss[location+7]);
		raceimage=StringUtil.getInt(ss[location+8]);
		fbids =new List<int>();
		for(int i=0;i<5;i++)
		{
			location =8+i*1;
			int fbid =StringUtil.getInt(ss[location]);	
			if(fbid !=0)
			{
				fbids.Add(fbid);
			}
			else
			{
				continue;
			}
		}
		addData();
	}

	public void resetData()
	{
		data.Clear();
	}
	
	/**根据活动编号获取eventdata**/
	public static EventData getEventData(int index)
	{
		return (EventData)data[index];
	}
}
