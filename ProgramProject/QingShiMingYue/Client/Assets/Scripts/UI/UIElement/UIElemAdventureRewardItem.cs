using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIElemAdventureRewardItem : MonoBehaviour
{
	public UIElemAssetIcon avatarIcon;
	public SpriteText rewardTypeMess;
	private int index;
	public int Index
	{
		get { return index; }
		set { index = value; }
	}



	public void SetData(int index, int id, int count, string mess)
	{
		avatarIcon.SetData(id, count);
		avatarIcon.Data = id;
		this.index = index;
		rewardTypeMess.Text = mess;
	}


}
