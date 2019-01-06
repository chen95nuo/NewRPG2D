using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAvatarBottomItem : MonoBehaviour
{
	public UIElemAssetIcon assetIcon;
	public UIBox selectedLight;
	public UIListItemContainer container;
	public UIBox notifyIcon;
	public UIBox danIcon;

	public void Awake()
	{
		container.Data = this;
	}

	// Set Equipment or Skill.
	public void SetData(KodGames.ClientClass.Player player, KodGames.ClientClass.Location location, int type, int equipTypeOrSkillSoltIndex)
	{
		SetSelectedStat(false);

		if (string.IsNullOrEmpty(location.Guid))
		{
			switch (type)
			{
				case IDSeg._AssetType.Equipment:
					HideDan(true);
					string str = EquipmentConfig._Type.GetDisplayNameByType(equipTypeOrSkillSoltIndex, ConfigDatabase.DefaultCfg);
					AutoSpriteControlBase borderTemplate = UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty;
					assetIcon.SetEmpty(borderTemplate, str);
					break;
				case IDSeg._AssetType.CombatTurn:
					HideDan(true);
					assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
					break;
				case IDSeg._AssetType.Dan:
					HideDan(false);
					danIcon.SetToggleState(equipTypeOrSkillSoltIndex);
					assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
					break;
			}

		}
		else
		{
			HideDan(true);
			switch (type)
			{
				case IDSeg._AssetType.Equipment:
					assetIcon.SetData(player.SearchEquipment(location.Guid));
					break;
				case IDSeg._AssetType.CombatTurn:
					assetIcon.SetData(player.SearchSkill(location.Guid));
					break;
				case IDSeg._AssetType.Dan:
					if (player.SearchDan(location.Guid) == null)
					{
						HideDan(false);
						danIcon.SetToggleState(equipTypeOrSkillSoltIndex);
						assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
					}
					else
						assetIcon.SetData(player.SearchDan(location.Guid));
					break;
			}
		}

		List<object> datas = new List<object>();
		datas.Add(equipTypeOrSkillSoltIndex);
		datas.Add(location);

		assetIcon.Data = datas;

		container.ScanChildren();
	}

	public void HideDan(bool isHide)
	{
		if (danIcon != null)
			danIcon.Hide(isHide);
	}

	public void SetTriggerMethod(MonoBehaviour script, string methodName)
	{
		assetIcon.SetTriggerMethod(script, methodName);
	}

	public void SetSelectedStat(bool selected)
	{
		selectedLight.Hide(!selected);
	}

	public void SetNotify(bool show)
	{
		if (notifyIcon != null)
			notifyIcon.Hide(!show);
	}
}
