using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImaginationData : PropertyReader {

	/**冥想编号**/
	public int index;
	/**金币消耗**/
	public int spend;
	/**成功几率**/
	public int probability;
	/**物品类型-物品id-掉落几率**/
	public List<string> item_info;
	
	private static Hashtable data =new Hashtable();

	public void addData() {
		data.Add(index, this);
	}

	public void parse(string[] ss) {
		int location=0;
		index =StringUtil.getInt(ss[location]);
		spend =StringUtil.getInt(ss[location+1]);
		probability =StringUtil.getInt(ss[location+2]);
		item_info =new List<string>();
		for(int i=0;i<7;i++)
		{
			location =3+i*3;
			int type =StringUtil.getInt(ss[location]);
			//跟客户端无关，解析出来就行//
			StringUtil.getInt(ss[location+1]);
			int item =StringUtil.getInt(ss[location+2]);
			int pro =StringUtil.getInt(ss[location+3]);
			string itemsInfo =type+"-"+item+"-"+pro;
			item_info.Add(itemsInfo);
		}
		addData();
	}


	public void resetData() {
		data.Clear();
	}
	
	public static ImaginationData getData(int mxId){
		return (ImaginationData)data[mxId];
	}
}
