using UnityEngine;
using System.Collections;

public class GameBoxJson : BasicJson {
	
	//要打开的item的id//
	public int boxId;
	//是用次数//
	public int usetimes;
	
	public GameBoxJson(int id, int times)
	{
		this.boxId = id;
		this.usetimes = times;
	}
	
}
