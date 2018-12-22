using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMessages
{
	private static BattleMessages instance;
	
	private List<string> messages;
	
	
	public static BattleMessages getInstance()
	{
		if(instance==null)
		{
			instance=new BattleMessages();
		}
		return instance;
	}
	
	private BattleMessages()
	{
		messages=new List<string>();
	}
	
	//增加战报//
	public void addBattleMessage()
	{
		//TODO
	}
	
	//战报发送给服务器//
	public void sendToServer()
	{
		//TODO
		
		
		messages.Clear();
	}
	
}
