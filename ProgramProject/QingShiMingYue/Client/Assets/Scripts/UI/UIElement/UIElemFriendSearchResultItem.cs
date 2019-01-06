using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemFriendSearchResultItem : MonoBehaviour
{
	public SpriteText rankLabel;
	public SpriteText playerNameLabel;
	public UIButton inviteBtn;
	public UIButton addFriendBtn;
	public UIButton chatBtn;
	public UIBox alreadyAdd;

	private int playerId;
	public int PlayerId { get { return playerId; } }

	public void SetData(KodGames.ClientClass.FriendInfo friendInfo)
	{
		this.playerId = friendInfo.PlayerId;
		playerNameLabel.Text = friendInfo.Name;		
		rankLabel.Text  = string.Format(GameUtility.GetUIString("UIPnlFriendTab_Label_FirendLevel"),friendInfo.Level);

		chatBtn.Data = friendInfo;
		addFriendBtn.Data = friendInfo;
		inviteBtn.Data = friendInfo;		

		SetSearchState(friendInfo.Status);
	}
	
	public void SetSearchState(int state)
	{
		//Init Button
		addFriendBtn.Hide(true);
		chatBtn.Hide(true);
		alreadyAdd.Hide(true);

		switch(state)
		{
			// 状态：未邀请
			case ClientServerCommon._FriendStatus.NotFriend: addFriendBtn.Hide(false); break;
			// 已邀请
			case ClientServerCommon._FriendStatus.Invited: alreadyAdd.Hide(false); break;
			//已添加
			case ClientServerCommon._FriendStatus.IsFriend: chatBtn.Hide(false); break;
			default: Debug.Log("State : " + state.ToString()); break;
		}
	}
}
