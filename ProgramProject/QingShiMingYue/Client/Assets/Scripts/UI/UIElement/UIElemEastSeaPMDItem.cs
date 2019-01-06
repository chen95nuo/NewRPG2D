//#define MONTHCARD_LOG
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;

public class UIElemEastSeaPMDItem : MonoBehaviour
{
	public SpriteText messageLable;

	public void SetData(string message)
	{
		//if (message.Length > 200)
		//	message = message.Substring(0, 180)+"...";
		messageLable.Text = message;
		
	}
	
}
