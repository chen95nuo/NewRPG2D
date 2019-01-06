using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

[AddComponentMenu("Game/UI Element/Asset Icon")]
public class UIElemAssetIcon : MonoBehaviour, IIconBase
{
	public AutoSpriteControlBase border; // 图标或者空底板

	public UIElemAssetIconBreakThroughBtn breakBorder;
	public SpriteText leftLable; // 突破
	public SpriteText rightLable; // 等级和数量
	public SpriteText emptyLabel; // 当图标有空状态显示时，显示空状态字符串
	public SpriteText assetNameLabel; // 用于显示资源的名字，可以不设置

	public bool useSamllIcon = false;

	/*
	 * 层级关系：leftBorder,rightBorder为border的子layer，emptyLabel为border的SpriteText
	 * 注意：外界不再使用绑定控件的Data,使用this.Data
	 */

	public string Text
	{
		get
		{
			if (assetNameLabel != null)
				return assetNameLabel.Text;
			return "";
		}

		set
		{
			if (assetNameLabel != null)
				assetNameLabel.Text = value;
		}
	}

	public int AssetId { get { return assetId; } }
	private int assetId = IDSeg.InvalidId;

	private int breakLevel = 0;

	private object data;
	public object Data
	{
		get { return data; }
		set { data = value; }
	}

	public void Awake()
	{
#if UNITY_EDITOR
		Debug.Assert(border.gameObject == this.gameObject);
#endif
		border.Data = this;
	}

	public AutoSpriteControlBase GetControl()
	{
		return border;
	}

	public void OnDestroy()
	{
		ReleaseIcon();
	}

	public void SetTriggerMethod(MonoBehaviour scripWithMethod, string method)
	{
		if (border is UIButton)
		{
			UIButton borderBtn = border as UIButton;
			borderBtn.scriptWithMethodToInvoke = scripWithMethod;
			borderBtn.methodToInvoke = method;
		}
	}

	private void ReleaseIcon()
	{
		SysIconManger iconMgr = SysModuleManager.Instance.GetSysModule<SysIconManger>();
		if (iconMgr != null && GetControl() != null)
			iconMgr.ReleaseIcon(this);
	}

	public void SetEmpty(AutoSpriteControlBase borderTemplate, string emptyText)
	{
		// Release current icon
		ReleaseIcon();

		// Hide border
		if (leftLable != null)
			leftLable.gameObject.SetActive(false);
		if (rightLable != null)
			rightLable.gameObject.SetActive(false);
		if (breakBorder != null)
			breakBorder.gameObject.SetActive(false);

		// Hide name label
		if (assetNameLabel != null)
			assetNameLabel.Text = "";

		// Set border
		UIUtility.CopyIcon(border, borderTemplate);

		// Set empty label
		if (emptyLabel != null)
		{
			emptyLabel.gameObject.SetActive(true);

			if (emptyText != null)
				emptyLabel.Text = emptyText;
		}
	}

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		SetData(avatar.ResourceId, avatar.BreakthoughtLevel, avatar.LevelAttrib.Level);
	}

	public void SetData(KodGames.ClientClass.Equipment equipment)
	{
		SetData(equipment.ResourceId, equipment.BreakthoughtLevel, equipment.LevelAttrib.Level);
	}

	public void SetData(KodGames.ClientClass.Skill skill)
	{
		SetData(skill.ResourceId, -1, skill.LevelAttrib.Level);
	}

	public void SetData(KodGames.ClientClass.Dan dan)
	{
		SetData(dan.ResourceId, dan.BreakthoughtLevel, dan.LevelAttrib.Level, false);
		//SetData(dan.ResourceId);
	}

	public void SetData(KodGames.ClientClass.Beast beast)
	{
		SetData(beast.ResourceId, beast.BreakthoughtLevel, beast.LevelAttrib.Level, true);
	}

	public void SetData(KodGames.ClientClass.Consumable consumable)
	{
		SetData(consumable.Id, consumable.Amount);
	}

	public void SetData(int assetId, int count)
	{
		SetData(assetId, count, false);
	}

	public void SetData(int assetId, int count, bool showCountOnName)
	{
		SetData(assetId);

		// Set count
		if (rightLable != null && showCountOnName == false)
		{
			if (IDSeg.ToAssetType(assetId) == IDSeg._AssetType.PveBuff)
				return;

			rightLable.gameObject.SetActive(true);
			if (count > 0)
				rightLable.Text = count.ToString();
			else if (count == -1)
				rightLable.Text = "";
			else
				rightLable.Text = "1";
			//rightLable.Text = count > 0 ? count.ToString() : "1";
		}

		if (showCountOnName && assetNameLabel != null && count > 1)
			assetNameLabel.Text = GameUtility.FormatUIString("UINameXCount", ItemInfoUtility.GetAssetName(assetId), count);
	}

	public void SetData(int assetId)
	{
		if (SysModuleManager.Instance.GetSysModule<SysIconManger>() == null)
		{
			return;
		}

		ReleaseIcon();

		// Set asset name
		this.assetId = ItemInfoUtility.GetRelativeId(assetId, this.breakLevel);

		if (assetNameLabel != null)
			assetNameLabel.Text = ItemInfoUtility.GetAssetName(assetId, breakLevel);

		if (this.assetId != IDSeg.InvalidId)
		{
			if (useSamllIcon)
			{
				UIElemTemplate elemTemplate = SysUIEnv.Instance.GetUIModule<UIElemTemplate>();
				if (elemTemplate.itemSmalIconTemplate.SetItemSamllIcon(border, this.assetId))
					return;
			}

			AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(this.assetId);
			if (assetDescCfg != null)
			{
				// Set icon
				SysModuleManager.Instance.GetSysModule<SysIconManger>().SetupControlIcon(this, assetDescCfg);
			}
			else
			{
				Debug.LogError("Invalid AssetId: " + assetId.ToString("X"));
			}

			// Hide empty text
			if (emptyLabel != null)
				emptyLabel.gameObject.SetActive(false);

			// Hide left border
			if (breakBorder != null)
				breakBorder.gameObject.SetActive(false);

			// Hide right border
			if (rightLable != null)
				rightLable.gameObject.SetActive(false);

			// Hide soul border
			if (leftLable != null)
				leftLable.gameObject.SetActive(false);

			// 如果是装备碎片或者角色魂魄 设置右下角角标。
			int itemType = ItemInfoUtility.GetItemType(assetId);
			switch (itemType)
			{
				case ItemConfig._Type.AvatarScorll:
					if (leftLable != null)
					{
						leftLable.gameObject.SetActive(true);
						leftLable.Text = GameUtility.GetUIString("AvatarScroll_ExtraNameDesc");
					}
					break;

				case ItemConfig._Type.BeastScroll:
				case ItemConfig._Type.BeastPart:
				case ItemConfig._Type.EquipScroll:
				case ItemConfig._Type.SkillScroll:
				case ItemConfig._Type.IllusionCostItem:
					if (leftLable != null)
					{
						leftLable.gameObject.SetActive(true);
						leftLable.Text = GameUtility.GetUIString("EquipScroll_ExtraNameDesc");
					}
					break;
			}
		}
		else
		{
			Debug.LogError("Invalid AssetId");
		}
	}

	public void SetData(int assetId, int breakthrough, int level, int count, bool showCountInName)
	{
		if (IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Avatar || IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Equipment || IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Dan || IDSeg.ToAssetType(assetId) == IDSeg._AssetType.CombatTurn)
			SetData(assetId, breakthrough, level);
		else
			SetData(assetId, count, showCountInName);
	}

	public void SetData(int assetId, int breakthrough, int level, int count)
	{
		SetData(assetId, breakthrough, level, count, false);
	}

	public void SetData(int assetId, int breakthrough, int level, bool isBreakBorder)
	{
		if(IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Beast)
		{
			this.breakLevel = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(level).Quality;
		}
		else			
			this.breakLevel = breakthrough;
		
		SetData(assetId);

		// Set breakthrough level
		if (breakBorder != null && isBreakBorder)
		{
			breakBorder.Value = breakthrough;
			breakBorder.gameObject.SetActive(true);
		}

		// Set level
		if (rightLable != null)
		{
			rightLable.gameObject.SetActive(true);
			rightLable.Text = GameUtility.FormatUIString("UILevelPrefix", level);
		}
	}

	public void SetData(int assetId, int breakthrough, int level)
	{
		this.breakLevel = breakthrough;
		SetData(assetId);

		// Set breakthrough level
		if (breakBorder != null)
		{
			breakBorder.Value = breakthrough;
			breakBorder.gameObject.SetActive(true);
		}

		// Set level
		if (rightLable != null)
		{
			rightLable.gameObject.SetActive(true);
			rightLable.Text = GameUtility.FormatUIString("UILevelPrefix", level);
		}
	}

	public void EnableButton(bool enable)
	{
		// Call start make layer setting available
		if (border.Started == false)
			border.Start();

		UIButton borderBtn = border as UIButton;
		if (borderBtn != null)
			borderBtn.controlIsEnabled = enable;
	}

	public bool IsHidden()
	{
		return border.IsHidden();
	}

	public virtual void Hide(bool tf)
	{
		border.Hide(tf);

		if (breakBorder != null)
			breakBorder.Hide(tf);

		if (leftLable != null)
			leftLable.Hide(tf);

		if (rightLable != null)
			rightLable.Hide(tf);

		if (emptyLabel != null)
			emptyLabel.Hide(tf);

		if (assetNameLabel != null)
			assetNameLabel.Hide(tf);
	}
}
