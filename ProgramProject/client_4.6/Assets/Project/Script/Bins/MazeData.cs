using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeData : PropertyReader {
	
	/**迷宫编号**/
	public int id;
	/**迷宫名称**/
	public string name;
	/**迷宫icon**/
	public string icon;
	/**迷宫地图**/
	public string map;
	/**迷宫场景**/
	public string scene;
	/**描述**/
	public string description;
	/**产出物品**/		
	public string output;
	/**解锁条件**/
	public int unlockCondition;
	/**出现条件**/
	public int condition;
	/**体力消耗**/
	public int energy;
	public int freeentry;
	public int payentry;
	public int expense;
	/**关卡步数**/
	public int step;
	/**事件及概率 格式：事件-概率**/
	public List<string> buff_pro;
	public List<string> gold_pro;
	public List<string> item_pro;
	
	private static Hashtable data=new Hashtable();
	
	public void addData() {
		data.Add(id, this);
	}

	public void parse(string[] ss) {
		int location =0;
		id =StringUtil.getInt(ss[location]);
		name =StringUtil.getString(ss[location+1]);
		icon =StringUtil.getString(ss[location+2]);
		map =StringUtil.getString(ss[location+3]);
		scene=StringUtil.getString(ss[location+4]);
		description=StringUtil.getString(ss[location+5]);	
		output=StringUtil.getString(ss[location+6]);
		unlockCondition=StringUtil.getInt(ss[location+7]);
		condition =StringUtil.getInt(ss[location+8]);
		step = StringUtil.getInt(ss[location+9]);
		energy =StringUtil.getInt(ss[location+10]);
		freeentry =StringUtil.getInt(ss[location+11]);
		payentry =StringUtil.getInt(ss[location+12]);
		expense =StringUtil.getInt(ss[location+13]);
		gold_pro =new List<string>();
		for(int i=0;i<3;i++)
		{
			location =14+i*2;
			int gold =StringUtil.getInt(ss[location]);
			int pro =StringUtil.getInt(ss[location+1]);
			if(gold ==0 && pro==0)
			{
				continue;
			}
			string goldpro =gold+"-"+pro;
			gold_pro.Add(goldpro);
		}
		item_pro =new List<string>();
		for(int i=0;i<1;i++)
		{
			location =20+i*2;
			int item =StringUtil.getInt(ss[location]);
			if(item ==0)
			{
				continue;
			}
			int pro =StringUtil.getInt(ss[location+1]);
			
			string itempro =item+"-"+pro;
			item_pro.Add(itempro);
		}
		buff_pro =new List<string>();
		for(int i=0;i<2;i++)
		{
			location =22+i*2;
			int exp =StringUtil.getInt(ss[location]);
			int pro =StringUtil.getInt(ss[location+1]);
			if(exp ==0 && pro==0)
			{
				continue;
			}
			string exppro =exp+"-"+pro;
			buff_pro.Add(exppro);
		}

		addData();
	}

	public void resetData() {
		data.Clear();
	}
	
	public static MazeData getData(int mazeId){
		return (MazeData)data[mazeId];
	}
	
}
