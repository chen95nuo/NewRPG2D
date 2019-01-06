using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class UIDlgAttributeDetailTip : UIModule
{
	public UIScrollList scrollList;

	public GameObjectPool attributePool;
	public UIListItemContainer avatarDescContainer;
	public UIListItemContainer helpPromptContainer;
	public UIListItemContainer powerValueContainer;

	private const int C_Column_Count = 3;
	private Color C_Color_Brown = GameDefines.txColorBrown;
	private Color C_Color_White = GameDefines.txColorWhite;
	private int showlocationId;
	private int positionId;
	private KodGames.ClientClass.Player player;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		List<AttributeCalculator.Attribute> attributes = userDatas[0] as List<AttributeCalculator.Attribute>;
		KodGames.ClientClass.Avatar avatar = userDatas[1] as KodGames.ClientClass.Avatar;
		bool showGrowth = (bool)userDatas[2];

		if (userDatas.Length > 3)
			positionId = (int)userDatas[3];

		if (userDatas.Length > 4)
			showlocationId = (int)userDatas[4];

		if (userDatas.Length > 5)
			player = userDatas[5] as KodGames.ClientClass.Player;


		InitView(attributes, avatar, showGrowth);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		showlocationId = -1;
		positionId = -1;
		player = null;
		scrollList.ClearList(false);
	}

	private void InitView(List<AttributeCalculator.Attribute> attributes, KodGames.ClientClass.Avatar avatar, bool showGrowth)
	{
		UIElemAvatarAttributeItem attributeItem = attributePool.AllocateItem().GetComponent<UIElemAvatarAttributeItem>();
		attributeItem.InitView();

		int apType = _AvatarAttributeType.PAP;
		int apValue = 0;
		int speedValue = 0;
		int hpValue = 0;
		for (int i = attributes.Count - 1; i >= 0; i--)
		{
			switch (attributes[i].type)
			{
				case _AvatarAttributeType.PAP:
				case _AvatarAttributeType.MAP:
					apType = attributes[i].type;
					apValue = KodGames.Math.RoundToInt(attributes[i].value);
					attributes.RemoveAt(i);
					break;

				case _AvatarAttributeType.MaxHP:
					hpValue = KodGames.Math.RoundToInt(attributes[i].value);
					attributes.RemoveAt(i);
					break;

				case _AvatarAttributeType.Speed:
					speedValue = KodGames.Math.RoundToInt(attributes[i].value);
					attributes.RemoveAt(i);
					break;
			}
		}

		attributeItem.attributeLables[0].Text = GetAttributeString(_AvatarAttributeType.MaxHP, hpValue);
		attributeItem.attributeLables[1].Text = GetAttributeString(apType, apValue);
		attributeItem.attributeLables[2].Text = GetAttributeString(_AvatarAttributeType.Speed, speedValue);
		scrollList.AddItem(attributeItem.gameObject);

		if (showGrowth)
		{
			var growthAttributes = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).GetAvatarBreakthrough(avatar.BreakthoughtLevel).breakThrough.growthAttributes;
			attributeItem = attributePool.AllocateItem().GetComponent<UIElemAvatarAttributeItem>();
			attributeItem.InitView();

			for (int i = 0; i < growthAttributes.Count; i++)
			{
				string growthDesc = GameUtility.FormatUIString("UIDlgAttributeDetailTip_Growth", C_Color_Brown, C_Color_White, ((float)growthAttributes[i].deltaValue).ToString("F2"));
				if (growthAttributes[i].type == _AvatarAttributeType.MaxHP)
					attributeItem.attributeLables[0].Text = growthDesc;
				else if (growthAttributes[i].type == apType)
					attributeItem.attributeLables[1].Text = growthDesc;
				else if (growthAttributes[i].type == _AvatarAttributeType.Speed)
					attributeItem.attributeLables[2].Text = growthDesc;
			}

			scrollList.AddItem(attributeItem.gameObject);
		}

		// Add Empty Label.
		attributeItem = attributePool.AllocateItem().GetComponent<UIElemAvatarAttributeItem>();
		attributeItem.InitView();
		scrollList.AddItem(attributeItem.gameObject);

		// Other Attribute.
		int row = attributes.Count / C_Column_Count;
		int column = attributes.Count % C_Column_Count;

		row = (column == 0) ? row - 1 : row;

		for (int i = 0; i <= row; i++)
		{
			attributeItem = attributePool.AllocateItem().GetComponent<UIElemAvatarAttributeItem>();
			attributeItem.InitView();

			int j = 0;
			for (; j < C_Column_Count && (i * C_Column_Count + j) < attributes.Count; j++)
				attributeItem.attributeLables[j].Text = GetAttributeString(attributes[i * C_Column_Count + j].type, attributes[i * C_Column_Count + j].value);

			for (; j < C_Column_Count; j++)
				attributeItem.attributeLables[j].Text = string.Empty;

			scrollList.AddItem(attributeItem.gameObject);
		}

		//Set HelpPrompt
		helpPromptContainer.Text = GameUtility.FormatUIString("UIDlgAttributeDetailTip_HelpPrompt", GameDefines.txColorWhite);
		scrollList.AddItem(helpPromptContainer);

		// Add Empty Label.
		attributeItem = attributePool.AllocateItem().GetComponent<UIElemAvatarAttributeItem>();
		attributeItem.InitView();
		scrollList.AddItem(attributeItem.gameObject);

		//Add Power label

		int powerValue = 0;
		if (player != null)
		{
			Debug.Log("Player != null");
			var dans = PlayerDataUtility.GetLineUpDans(player, positionId, showlocationId);

			List<float> values = new List<float>();

			foreach (var dan in dans)
				values.Add(dan.DanPower);

			powerValue = (int)PlayerDataUtility.CalculateAvatarPower(avatar, positionId, showlocationId, player, values);
		}
		else
			powerValue = (int)PlayerDataUtility.CalculateAvatarPower(avatar);

		powerValueContainer.Text = GameUtility.FormatUIString("UIPnlAvatar_Power", GameDefines.txColorBrown, GameDefines.colorWhite, PlayerDataUtility.GetPowerString(powerValue));
		scrollList.AddItem(powerValueContainer);

		// Set Avatar Desc.
		string avatarDesc = GameUtility.FormatUIString("UIDlgAttributeDetailTip_Desc", GameDefines.txColorBrown, GameDefines.txColorWhite);
		avatarDesc += "\n" + ItemInfoUtility.GetAssetExtraDesc(avatar.ResourceId);
		avatarDescContainer.Text = avatarDesc;
		scrollList.AddItem(avatarDescContainer);

	}

	private string GetAttributeString(int type, double value)
	{
		return GameUtility.FormatUIString("UIDlgAttributeDetailTip_AttributeDetail", C_Color_Brown, _AvatarAttributeType.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg), C_Color_White, ItemInfoUtility.GetAttribDisplayString(type, value));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabClose(UIButton btn)
	{
		HideSelf();
	}
}
