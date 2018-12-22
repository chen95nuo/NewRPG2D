using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImaginationcomposeData : PropertyReader {

	/**编号**/
	public int id;
	/**兑换类别:1,card 2,equipment 3,material 4,被动技能**/
	public int type;
	/**兑换物品**/
	public int composite;
	/**材料**/
	public int material;
	/**数量**/
	public int number;
	
	public List<string> material_num;
	
	private static Hashtable data =new Hashtable();
	
	public void addData() {
		data.Add(id, this);
	}
	
	public void parse(string[] ss) {
		int location =0;
		id =StringUtil.getInt(ss[location]);
		type =StringUtil.getInt(ss[location+1]);
		composite =StringUtil.getInt(ss[location+2]);
		material=StringUtil.getInt(ss[location+3]);
		number=StringUtil.getInt(ss[location+4]);
		
		addData();
		
	}
	
	public void resetData() {
		data.Clear();
	}
	
	public static int getCount(){
		return data.Count;
	}
	
	public static ImaginationcomposeData getData(int id)
	{
		return (ImaginationcomposeData)data[id];
	}
}
