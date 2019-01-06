using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemRewardItem : MonoBehaviour 
{
	public UIElemAssetIcon assetIcon;
	
	public void SetData(object obj, int count)
	{
		if(obj == null)
			return;
		
		if(obj is KodGames.ClientClass.Consumable)
		{
			assetIcon.SetData(obj as KodGames.ClientClass.Consumable);
		}
		else if(obj is KodGames.ClientClass.Avatar)
		{
			assetIcon.SetData(obj as KodGames.ClientClass.Avatar);
		}
		else if(obj is KodGames.ClientClass.Skill)
		{
			assetIcon.SetData(obj as KodGames.ClientClass.Skill);
		}
		else if(obj is KodGames.ClientClass.Equipment)
		{
			assetIcon.SetData(obj as KodGames.ClientClass.Equipment);
		}
	}
}
