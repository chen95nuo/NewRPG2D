using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemAvatarPageListItem : MonoBehaviour 
{
	public List<UIElemAvatarSelectToggleItem> avatarList;
	
	public void SetData(List<KodGames.ClientClass.Avatar> avatars)
	{
		for(int index = 0; index < avatarList.Count; index++)
		{
			if(avatars.Count > index)
			{
				avatarList[index].SetData(avatars[index]);
				avatarList[index].ResetToggleState(false);
			}
			else
			{
				avatarList[index].SetData(null); 
			}
		}
	}
}
