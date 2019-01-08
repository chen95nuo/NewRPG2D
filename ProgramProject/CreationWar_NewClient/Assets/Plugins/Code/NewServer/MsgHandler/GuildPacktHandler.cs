using UnityEngine;
using System.Collections;

public class GuildPacktHandler : ServerMonoBehaviour {

	public Warnings warnings;
	/// <summary>
	/// 设置公会公告
	/// </summary>
	public static void SetGuildNotice(string mNotice)
	{
		if(InRoom.GetInRoomInstantiate ().ServerConnected)
		{
			ZMNetData data=new ZMNetData((short)OpCode.SetGuildNotice);
			data.writeString(mNotice);
			ZealmConnector.sendRequest(data);
		}
	}

	/// <summary>
	/// 设置公会宣言
	/// </summary>
	/// <param name="mNotice">M notice.</param>
	public static void SetGuildDeclaration(string mDeclaration)
	{
		if(InRoom.GetInRoomInstantiate ().ServerConnected)
		{
			ZMNetData data=new ZMNetData((short)OpCode.SetGuildDeclaration);
			data.writeString(mDeclaration);
			ZealmConnector.sendRequest(data);
		}
	}

	/// <summary>
	///  获取公会公告
	/// </summary>
	public static void GetGuildNotice()
	{
		if(InRoom.GetInRoomInstantiate ().ServerConnected)
		{
			ZMNetData data=new ZMNetData((short)OpCode.GetGuildNotice);
			ZealmConnector.sendRequest(data);
		}
	}

	/// <summary>
	/// 获取公会宣言
	/// </summary>
	public static void GetGuildDeclaration()
	{
		if(InRoom.GetInRoomInstantiate ().ServerConnected)
		{
			ZMNetData data=new ZMNetData((short)OpCode.GetGuildDeclaration);
			ZealmConnector.sendRequest(data);
		}
	}

	/// <summary>
	/// 客户端消息接收
	/// </summary>
	/// <param name="mData">M data.</param>
	protected override void OnOperationResponse (ZMNetData mData)
	{
		short opcode=mData.type;
		switch((OpCode)opcode)
		{
			case OpCode.SetGuildDeclaration:
			{
				short returnCode=mData.readShort();//0为设置不成功，1为成功
			if(returnCode==0){
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info942"));
			}else{
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info941"));
			}
			}
			break;
			case OpCode.SetGuildNotice:
			{
				short returnCode=mData.readShort();//0为设置不成功，1为成功
			if(returnCode==0){
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info944"));
			}else{
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info943"));
			}
		}
			break;
			case OpCode.GetGuildDeclaration:
			{
				string declaration=mData.readString();//公会宣言
			}
			break;
			case OpCode.GetGuildNotice:
			{
				string notice=mData.readString();//公会公告
			}
			break;
		}
	}

}
