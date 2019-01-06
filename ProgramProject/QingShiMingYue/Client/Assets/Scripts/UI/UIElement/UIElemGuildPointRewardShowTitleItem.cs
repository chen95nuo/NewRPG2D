using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemGuildPointRewardShowTitleItem : MonoBehaviour 
{
	public UIBox publicReward;
	public UIBox rankReward;
	
	public void SetData(bool isPublic)
	{
		publicReward.Hide(!isPublic);
		rankReward.Hide(isPublic);
	}	
}
