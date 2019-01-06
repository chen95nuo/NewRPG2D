using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIElemAvatarMeridianItem : MonoBehaviour
{
	[System.Serializable]
	public class AvatarMeridianItem
	{
		public UIElemAssetIcon meridianIcon;
		public SpriteText meridianAttributeLabel;
		public SpriteText meridianNameLabel;

		public MeridianConfig.Meridian meridianConfig;

		[UnityEngine.HideInInspector]
		public UnityEngine.GameObject itemParticleObj;

		public void SetIcon(AutoSpriteControlBase targetIcon)
		{
			SetIcon(targetIcon, 0f);
		}

		public void SetIcon(AutoSpriteControlBase targetIcon, float delayTime)
		{
			FadeSprite.Do(
				meridianIcon.border,
				EZAnimation.ANIM_MODE.FromTo,
				meridianIcon.border.Color,
				EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear),
				0f,
				delayTime,
				null,
				(data) =>
				{
					UIUtility.CopyIcon(meridianIcon.border, targetIcon);
				}
				);
		}
	}

	public List<AvatarMeridianItem> avatarMeridianItems;

	private KodGames.ClientClass.Avatar avatarData;

	public void InitData(KodGames.ClientClass.Avatar avatarData)
	{
		this.avatarData = avatarData;
	}

	public void ClearData()
	{
		avatarData = null;

		// Clear Item Data.
		foreach (var avatarItem in avatarMeridianItems)
		{
			avatarItem.meridianConfig = null;

			avatarItem.meridianIcon.Data = null;
		}
	}

	public void SetMeridianItem(int index, MeridianConfig.Meridian meridianConfig)
	{
		// Set Meridian Id.
		avatarMeridianItems[index].meridianConfig = meridianConfig;

		// Set UI.
		RefreshMeridianItem(avatarMeridianItems[index]);
	}

	public void RefreshMeridianItem(int meridianId, bool changMeridian)
	{
		RefreshMeridianItem(GetAvatarMeridianById(meridianId));

		if (!changMeridian)//有激活的穴位 就不从这里控制更换穴位的icon图标
			RefreshMeridianItem(GetAvatarMeridianByPreMeridianId(meridianId));
	}

	private void RefreshMeridianItem(AvatarMeridianItem item)
	{
		if (avatarData == null || item == null || item.meridianConfig == null)
			return;

		//前置穴位属性
		var preMeridianAttrData = avatarData.GetMeridianByID(item.meridianConfig.preMeridianId);

		//前置穴位已激活
		if (avatarData.LevelAttrib.Level >= item.meridianConfig.level && (item.meridianConfig.preMeridianId == IDSeg.InvalidId || (ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(item.meridianConfig.preMeridianId) != null && preMeridianAttrData != null && preMeridianAttrData.Modifiers.Count > 0)))
		{
			KodGames.ClientClass.MeridianData meridianAttrData = avatarData.GetMeridianByID(item.meridianConfig.id);

			//isOpen == true : 本穴位处于 解锁未激活状态   isOpen == false : 本穴位已激活
			bool isOpen = meridianAttrData == null || meridianAttrData.Modifiers.Count <= 0;

			item.SetIcon(isOpen ? UIElemTemplate.Inst.iconBorderTemplate.iconOpenMeridian : UIElemTemplate.Inst.iconBorderTemplate.iconActiveMeridian);
			item.meridianIcon.Data = item;

			// Set Particle.
			if (item.itemParticleObj != null)
			{
				UnityEngine.GameObject.Destroy(item.itemParticleObj);
			}

			if (!isOpen)
			{
				item.itemParticleObj = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.equipOpenParticle));
			}

			if (item.itemParticleObj != null)
				ObjectUtility.AttachToParentAndResetLocalPosAndRotation(item.meridianIcon.gameObject, item.itemParticleObj);

			// Set Attribute Label.
			item.meridianAttributeLabel.Text = string.Empty;
		}
		else//前置穴位未激活，本穴位处于未解锁状态
		{
			item.meridianIcon.Data = item;
			item.SetIcon(UIElemTemplate.Inst.iconBorderTemplate.iconLockMeridian);
			item.meridianAttributeLabel.Text = "";/* GameUtility.FormatUIString("UIAvatarMeridian_RequireLevel", item.meridianConfig.level);*/

			// Destroy Particle if has.
			if (item.itemParticleObj != null)
				UnityEngine.GameObject.Destroy(item.itemParticleObj);
		}

		// Set Name Label.
		item.meridianNameLabel.Text = item.meridianConfig.name;
	}

	private AvatarMeridianItem GetAvatarMeridianById(int meridianId)
	{
		foreach (var avatarMeridian in avatarMeridianItems)
		{
			if (avatarMeridian.meridianConfig != null && avatarMeridian.meridianConfig.id == meridianId)
				return avatarMeridian;
		}

		return null;
	}

	private AvatarMeridianItem GetAvatarMeridianByPreMeridianId(int preMeridianId)
	{
		foreach (var avatarMeridian in avatarMeridianItems)
		{
			if (avatarMeridian.meridianConfig != null && avatarMeridian.meridianConfig.preMeridianId == preMeridianId)
				return avatarMeridian;
		}

		return null;
	}

	public void AddActionEffect(int meridianId)
	{
		AvatarMeridianItem item = GetAvatarMeridianById(meridianId);
		item.itemParticleObj = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.equipActiveParticle));
		if (item.itemParticleObj != null)
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(item.meridianIcon.gameObject, item.itemParticleObj);
		AudioManager.Instance.PlaySound("SkillStart", 1.9f);
		var meridianAttrData = avatarData.GetMeridianByID(item.meridianConfig.id);
		item.SetIcon((meridianAttrData == null || meridianAttrData.Modifiers.Count <= 0) ? UIElemTemplate.Inst.iconBorderTemplate.iconOpenMeridian : UIElemTemplate.Inst.iconBorderTemplate.iconActiveMeridian, 0);
	}


}