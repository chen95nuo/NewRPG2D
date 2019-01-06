#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgLeaveMsg : UIModule
{
	public UITextField messageInput;
	public SpriteText messageToLabel;
	public UIButton levelMsgBtn;
	
	//public override bool Show (params object[] userDatas)
	//{
	//    if(!base.Show(userDatas))
	//        return false;
		
	//    SetData(userDatas[0] as KodGames.ClientClass.PlayerRecord);
	//    return true;
	//}
	
	//public override void Hide ()
	//{
	//    levelMsgBtn.Data = null;
	//    base.Hide ();
	//}
	
	//public void ResetInput()
	//{
	//    messageInput.Text = "";
	//}
	
	//private void SetData(KodGames.ClientClass.PlayerRecord player)
	//{
	//    levelMsgBtn.Data = player;
		
	//    //Set player name.
	//    messageToLabel.Text = string.Format(GameUtility.GetUIString("UIDlgLeaveMsg_Label_LeaveMsgTo"),
	//        player.PlayerName);
		
	//    //Set input place holder.
	//    messageInput.placeHolder = GameUtility.GetUIString("UIDlgLeaveMsg_Label_InputPlaceHolder");
		
	//    messageInput.Text = "";
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnCancel(UIButton btn)
	//{
	//    this.Hide();
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnOK(UIButton btn)
	//{
	//    if(messageInput.Text == "" || messageInput.Text == null)
	//    {
	//        SysUIEnv.Instance.GetUIModule<UIPnlTipFlow>().Show(GameUtility.GetUIString("UIDlgLeaveMsg_Label_MessageEmpty"));
	//        return;
	//    }
		
	//    KodGames.ClientClass.PlayerRecord playerRecord = btn.Data as KodGames.ClientClass.PlayerRecord;
	//    if(playerRecord == null)
	//        return;
		
	//    KodGames.ClientClass.ChatMessage msg = new KodGames.ClientClass.ChatMessage();
	//    msg.MessageType = _ChatType.Private;
	//    msg.Content = messageInput.Text;	
	//    msg.Time = KodGames.TimeEx.DateTimeToInt64(System.DateTime.UtcNow);
	//    msg.SenderId = SysLocalDataBase.Inst.LocalPlayer.PlayerId;
	//    msg.SenderName = SysLocalDataBase.Inst.LocalPlayer.Name;
	//    msg.SenderVipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;
	//    msg.ReceiverId = playerRecord.PlayerId;
	//    msg.ReceiverName = playerRecord.PlayerName;
	//    msg.ReceiverVipLevel = playerRecord.VipLevel;
	//    SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().GetCurrentState<GameState_CentralCity>().Chat(msg);
	//}

	//[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	//private void OnClose(UIButton btn)
	//{
	//    this.Hide();
	//}
}

#endif
