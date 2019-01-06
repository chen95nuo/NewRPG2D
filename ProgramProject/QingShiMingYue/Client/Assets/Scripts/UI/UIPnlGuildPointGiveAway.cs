using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlGuildPointGiveAway : UIModule
{
	public UIScrollList itemList;
	public GameObjectPool itemPool;
	public GameObjectPool morePool;
	public SpriteText guildNameLable;
	public SpriteText guildManifestoLable;
	public UIButton tabBtn;

	private int maxCount = 10;
	private int preNum = 1;
	private List<GuildMemberInfo> guildMemberInfos;
	private UIListItemContainer viewMoreBtnItem;
	private KodGames.ClientClass.GuildInfoSimple guildInfoSimple;
	private KodGames.ClientClass.StageInfo stageInfo;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;
		tabBtn.SetControlState(UIButton.CONTROL_STATE.DISABLED);
		if (userDatas != null && userDatas.Length > 0)
		{
			guildInfoSimple = userDatas[0] as KodGames.ClientClass.GuildInfoSimple;
			guildMemberInfos = guildInfoSimple.Members;
			stageInfo = userDatas[1] as KodGames.ClientClass.StageInfo;
			SetData();
		}

		return true;
	}

	private void SetData()
	{
		guildNameLable.Text = GameUtility.FormatUIString("UIPnlGuildPointGiveAway_Manifesto_Title", GameDefines.textColorTipsInBlack, guildInfoSimple.GuildName, GameDefines.txColorYellow4, guildInfoSimple.GuildLevel);
		guildManifestoLable.Text = GameUtility.FormatUIString("UIPnlGuildPointGiveAway_Manifesto", GameDefines.textColorTipsInBlack, GameDefines.txColorYellow4, guildInfoSimple.Declaration);
		List<GuildMemberInfo> infos = new List<GuildMemberInfo>();
		if (maxCount * preNum > guildMemberInfos.Count)
			infos = guildMemberInfos.GetRange(maxCount * (preNum - 1), guildMemberInfos.Count - maxCount * (preNum - 1));
		else
			infos = guildMemberInfos.GetRange(maxCount * (preNum - 1), maxCount);

		foreach (var guildMember in infos)
		{
			if (guildMember.PlayerId != SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			{
				UIListItemContainer container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemGuildPointGiveAway item = container.gameObject.GetComponent<UIElemGuildPointGiveAway>();
				item.SetData(guildMember);
				container.Data = item;
				itemList.AddItem(container);
			}
			
		}
		if (guildMemberInfos.Count > maxCount * preNum)
		{
			viewMoreBtnItem = morePool.AllocateItem().GetComponent<UIListItemContainer>();
			itemList.InsertItem(viewMoreBtnItem, itemList.Count);
		}
		else
		{
			if (viewMoreBtnItem != null)
			{
				itemList.RemoveItem(viewMoreBtnItem, false);
				viewMoreBtnItem = null;
			}
		}
		itemList.ScrollToItem(maxCount * (preNum - 1), 0);
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		itemList.ClearList(false);
		preNum = 1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRewardClick(UIButton btn)
	{
		GuildMemberInfo info = btn.Data as GuildMemberInfo;
		RequestMgr.Inst.Request(new GuildStageGiveBoxReq(info.PlayerId,() =>
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildPointGiveAway_GiveAwaySuccess"));
			HideSelf();
			return true;
		}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIEffectSchoolOpenBox>(stageInfo, true, true, false, true);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPrePageClick(UIButton btn)
	{
		preNum++;
		SetData();
	}

}
