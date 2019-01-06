using UnityEngine;
using System.Collections;

public class UIElemFriendItem : MonoBehaviour
{
	public SpriteText rankLabel;
	public UIButton itemBgBtn;
	public SpriteText playerNameLabel;
	public SpriteText isOnLine;
	public SpriteText leftTime;
	public UIButton communicateBtn;

	private KodGames.ClientClass.FriendInfo friendInfo;
	public KodGames.ClientClass.FriendInfo FriendInfo
	{
		get { return friendInfo; }		
	}

	public void SetData(KodGames.ClientClass.FriendInfo friendInfo)
	{
		this.friendInfo = friendInfo;

		if (friendInfo.IsOnLine)
		{
			isOnLine.Text = "";
			leftTime.Text = "";
			playerNameLabel.color.a= 1f;
		}			
		else
		{					
			isOnLine.Text = GameUtility.GetUIString("UIPnlFriendTab_Label_NotOnline");
			Debug.Log("Time : " + SysLocalDataBase.Inst.LoginInfo.NowTime.ToString());
			leftTime.Text = GameUtility.Time2SortString(SysLocalDataBase.Inst.LoginInfo.NowTime - friendInfo.LastLoginTime);
			playerNameLabel.color.a = 0.5f;
		}
		// Name		
		playerNameLabel.Text = friendInfo.Name;

		// Level		
		rankLabel.Text = string.Format(GameUtility.GetUIString("UIPnlFriendTab_Label_FirendLevel"), friendInfo.Level);

		// Save playerRecord in button data
		communicateBtn.Data = friendInfo;
		itemBgBtn.Data = friendInfo;
	}
}
