
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class UIPnlAdventureScene : UIModule
{
	public List<UIButton> points;
	public List<UIBox> lines;

	public SpriteText playerNameText;
	public SpriteText timerText;
	public SpriteText energyNumText;
	public UIElemAssetIcon avatarIcon;
	public UIButton backBtn;

	public UIBox delaySign;

	private float lastUpdateDelta = 0f;

	private List<int> completedMarvellousScenarios;//已经完成的奇遇

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SetAvatarIcon();
		SetPlayerNameText();
		SetEnergyNumText();

		if (userDatas != null && userDatas.Length > 0)
		{
			var marvellousProto = userDatas[0] as com.kodgames.corgi.protocol.MarvellousProto;
			//currentEventId = marvellousProto.currentEventId;
			completedMarvellousScenarios = marvellousProto.completedMarvellousScenarios;
			MarvellousAdventureConfig.Marvellous marvellous = ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetMarvellousById(marvellousProto.marvellousId);
			int maxCount = 0;
			if (marvellous != null)
				maxCount = marvellous.MarvellousScenarios.Count;
			SetPoint(maxCount, completedMarvellousScenarios.Count);

			//显示地图触控点
			//AdventureSceneData.Instance.CreateMissionEvent(marvellousProto.visitZones);
		}
		else
		{
			if (AdventureSceneData.combatMarvellouseProto != null)
			{
				if ((AdventureSceneData.delayRewardsConst != null && AdventureSceneData.delayRewardsConst.Count > 0) ||
				(AdventureSceneData.fixRewardPackagePars != null && AdventureSceneData.fixRewardPackagePars.Count > 0) ||
				(AdventureSceneData.randRewardPackagePars != null && AdventureSceneData.randRewardPackagePars.Count > 0))
					SysUIEnv.Instance.ShowUIModule<UIPnlAdventureGetReward>(AdventureSceneData.combatMarvellouseProto, AdventureSceneData.delayRewardsConst, AdventureSceneData.fixRewardPackagePars, AdventureSceneData.randRewardPackagePars);
				AdventureSceneData.Instance.GetAdventureTypeByEventId(AdventureSceneData.combatMarvellouseProto);
				//显示地图触控点
				//AdventureSceneData.Instance.CreateMissionEvent(combatMarvellouseProto.visitZones);
				AdventureSceneData.combatMarvellouseProto = null;
				AdventureSceneData.delayRewardsConst = new List<com.kodgames.corgi.protocol.DelayReward>();
				AdventureSceneData.fixRewardPackagePars = null;
				AdventureSceneData.randRewardPackagePars = null;
			}

		}
		return true;
	}

	private void SetPoint(int maxCount, int currentCount)
	{
		ClosePoint();
		int centerPoint = points.Count / 2 - maxCount / 2;
		for (int i = 0; i < maxCount; i++)
		{
			int index = centerPoint + i;

			if (index < points.Count)
			{
				points[index].Hide(false);
				if (i < currentCount)
					points[index].SetControlState(UIButton.CONTROL_STATE.NORMAL);
				else
					points[index].SetControlState(UIButton.CONTROL_STATE.DISABLED);
			}
			if (index < lines.Count && i < maxCount - 1)
				lines[index].Hide(false);

		}
	}

	private void ClosePoint()
	{
		foreach (UIButton point in points)
			point.Hide(true);
		foreach (UIBox line in lines)
			line.Hide(true);
	}

	private void Update()
	{
		lastUpdateDelta += Time.deltaTime;
		if (lastUpdateDelta > 1f && SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value < SysLocalDataBase.Inst.LocalPlayer.Energy.MaxPoint)
		{
			lastUpdateDelta -= 1f;
			UpdateTimerTextView();
		}
		if (AdventureSceneData.Instance.HadDelayReward > 0)
			delaySign.Hide(false);
		else delaySign.Hide(true);
		backBtn.gameObject.SetActive(AdventureSceneData.isClick);
		SetEnergyNumText();
		
	}

	private void UpdateTimerTextView()
	{
		SysLocalDataBase localDataBase = SysLocalDataBase.Inst;

		//Stamina Point.
		float addEnergyTime = localDataBase.LocalPlayer.Energy.GetNextGenerationLeftTime(localDataBase.LoginInfo.NowTime);
		if (addEnergyTime >= 0)
		{
			if (!GameUtility.Time2String((long)addEnergyTime).Equals(timerText.Text))
			{
				timerText.Text = GameUtility.Time2String((long)addEnergyTime);
			}
		}
		
	}

	private void SetEnergyNumText()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value == SysLocalDataBase.Inst.LocalPlayer.Energy.MaxPoint)
			timerText.Text="";
		energyNumText.Text = SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value + "/" + SysLocalDataBase.Inst.LocalPlayer.Energy.MaxPoint;
	}

	private void SetPlayerNameText()
	{
		playerNameText.Text = SysLocalDataBase.Inst.LocalPlayer.Name;
	}

	private void SetAvatarIcon()
	{
		avatarIcon.SetData(ItemInfoUtility.GetAvatarFirstIconID());
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBtnDetailed(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIDlgAdventureExplain>();
	}
}