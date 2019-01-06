using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgAvatarCheerDetail : UIModule
{
	public UIScrollList cheerDetailList;
	public GameObjectPool cheerDetailObjPool;
	public SpriteText emptyLabel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		InitUI(userDatas[0] as KodGames.ClientClass.Player, (int)userDatas[1]);
		return true;
	}

	private void ClearList()
	{
		cheerDetailList.ClearList(false);
		cheerDetailList.ScrollListTo(0f);
	}

	private void InitUI(KodGames.ClientClass.Player player, int positionId)
	{
		ClearList();

		var parterAvatars = PlayerDataUtility.GetCheerCalculatorAvatars(player, positionId);

		parterAvatars.Sort(SortByPartnerId);

		for (int i = 0; i < parterAvatars.Count; i++)
		{
			var partnerCfg = ConfigDatabase.DefaultCfg.PartnerConfig.GetPartnerById(parterAvatars[i].partnerId);
			var benifit = new System.Text.StringBuilder();

			var attributes = PlayerDataUtility.GetAvatarAttributesForAssistant(parterAvatars[i]);

			for (int j = 0; j < attributes.Count; j++)
			{
				var attrib = attributes[j];

				benifit.AppendFormat(GameUtility.GetUIString("UIDlgMessage_Title_CheerPosBenifit"),
					PositionConfig._EmBattleType.GetDisplayNameByType(partnerCfg.AffectType, ConfigDatabase.DefaultCfg),
					ItemInfoUtility.GetAttributeStr(attrib.type),
					ItemInfoUtility.GetAttribDisplayString(attrib.type, attrib.value));

				if (j < partnerCfg.Modifiers.Count - 1)
					benifit.AppendLine();
			}

			UIListItemContainer itemContainer = cheerDetailObjPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemCheerDetail cheerDetailItem = itemContainer.gameObject.GetComponent<UIElemCheerDetail>();
			cheerDetailItem.SetData(ItemInfoUtility.GetAssetName(partnerCfg.PartnerId), ItemInfoUtility.GetAssetName(parterAvatars[i].id), benifit.ToString());
			cheerDetailList.AddItem(itemContainer);
		}

		if (parterAvatars.Count <= 0)
			emptyLabel.Text = GameUtility.GetUIString("UIDlgAvatarCheerDetail_Empty");
		else
			emptyLabel.Text = string.Empty;
	}

	private static int SortByPartnerId(AttributeCalculator.Avatar avatar1, AttributeCalculator.Avatar avatar2)
	{
		return avatar1.partnerId - avatar2.partnerId;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysUIEnv.Instance.HideUIModule(typeof(UIDlgAvatarCheerDetail));
	}
}
