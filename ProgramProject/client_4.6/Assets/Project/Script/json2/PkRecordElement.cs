using UnityEngine;
using System.Collections;

public class PkRecordElement {
	public int type;//0,挑战别人,1,别人来挑战//
	public string name;//pk玩家的名字//
	public int rank;//0,排名不变,x提升或者下降至x名//
	public int time;//记录时间,一小时为单位//
	public int r;//（1胜利,2失败）//
}
