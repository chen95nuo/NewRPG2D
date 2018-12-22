using UnityEngine;
using System.Collections;

public class AchieveRewardJson : BasicJson {
	public int acId;//==成就Id,excel表里对应的id==//
	
	public AchieveRewardJson(int id)
	{
		acId=id;
	}
}
