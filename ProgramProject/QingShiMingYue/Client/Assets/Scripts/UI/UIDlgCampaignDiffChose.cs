using System;
using System.Collections;
using ClientServerCommon;

public class UIDlgCampaignDiffChose : UIModule
{
	public UIButton normalDiffButton;
	public UIButton hardDiffButton;
	public UIButton nightmareButton;

	public UIBox normalLock;
	public UIBox hardLock;
	public UIBox nightmareLock;

	public delegate void OnClickChangeDiff(int diffType);
	private OnClickChangeDiff onClickChangeDiff;
	private int zoneId;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// Set the Button Data.
		normalDiffButton.Data = _DungeonDifficulity.Common;
		hardDiffButton.Data = _DungeonDifficulity.Hard;
		nightmareButton.Data = _DungeonDifficulity.Nightmare;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		// Set ZoneId.
		this.zoneId = (int)userDatas[0];

		SetDiffButtonUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		this.onClickChangeDiff = null;
		this.zoneId = IDSeg.InvalidId;
	}

	public void SetClickChangeDel(OnClickChangeDiff del)
	{
		onClickChangeDiff = del;
	}

	private void SetDiffButtonUI()
	{
		// Set the Button show.
		CampaignConfig.Zone zoneConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId);
		normalDiffButton.gameObject.SetActive(zoneConfig.GetDungeonDifficultyByDifficulty(_DungeonDifficulity.Common) != null);
		hardDiffButton.gameObject.SetActive(zoneConfig.GetDungeonDifficultyByDifficulty(_DungeonDifficulity.Hard) != null);
		nightmareButton.gameObject.SetActive(zoneConfig.GetDungeonDifficultyByDifficulty(_DungeonDifficulity.Nightmare) != null);

		normalLock.Hide(true);
		HideLockUI(hardLock, zoneConfig, _DungeonDifficulity.Hard);
		HideLockUI(nightmareLock, zoneConfig, _DungeonDifficulity.Nightmare);
	}

	private void HideLockUI(AutoSpriteControlBase lockBase, CampaignConfig.Zone zoneCfg, int diffType)
	{
		lockBase.Hide(zoneCfg.GetDungeonDifficultyByDifficulty(diffType) == null ||
					  (zoneCfg.GetDungeonDifficultyByDifficulty(diffType).levelLimit <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level &&
					   CampaignData.IsDiffcultComplement(zoneCfg.zoneId, diffType - 1)));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChoseDiff(UIButton btn)
	{
		int diffType = (int)btn.Data;
		bool isLocked = false;

		switch (diffType)
		{
			case _DungeonDifficulity.Common:
				isLocked = !normalLock.IsHidden();
				break;
			case _DungeonDifficulity.Hard:
				isLocked = !hardLock.IsHidden();
				break;
			case _DungeonDifficulity.Nightmare:
				isLocked = !nightmareLock.IsHidden();
				break;
		}

		if (isLocked)
		{
			if (!CampaignData.IsDiffcultComplement(zoneId, diffType - 1))
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow),
								  GameUtility.FormatUIString("UICampaign_SwitchDiffWaring",
											 _DungeonDifficulity.GetDisplayNameByType(diffType - 1, ConfigDatabase.DefaultCfg),
											 _DungeonDifficulity.GetDisplayNameByType(diffType, ConfigDatabase.DefaultCfg)));
			else
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow),
								  GameUtility.FormatUIString("UICampaign_ActivityCampaign_EnterLevel",ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId).GetDungeonDifficultyByDifficulty(diffType).levelLimit));
		}
		else
		{
			if (onClickChangeDiff != null)
				onClickChangeDiff((int)btn.Data);

			HideSelf();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}