using UnityEngine;
using System.Collections;

public class UIElemDailyRewardSpecialItem : MonoBehaviour
{
	public UIElemAssetIcon specialRewardItemIcon;


	public void SetData(KodGames.ClientClass.Attachment attachment)
	{
		specialRewardItemIcon.SetData(attachment.Id);
	}
}
