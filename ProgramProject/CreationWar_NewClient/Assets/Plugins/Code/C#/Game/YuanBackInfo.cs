using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class YuanBackInfo  {
	
	private string name;
	
	public string Name
	{
		get
		{
			return this.name;
		}
	}
	
	public OperationResponse opereationResponse;
	
	public bool isUpate;
	
	public YuanBackInfo(string mName)
	{
		name=mName;
		isUpate=false;
	}
	
	
	
}
