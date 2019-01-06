using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLevelReward : MonoBehaviour
{
	public UIButton operateBtn;
	public UIBox alreadyGot;

	public UIElemAssetIcon rewardAssetIcon;
	public SpriteText rewardNameLabel;
	public SpriteText rewardDescLabel;

	private const float ORIGIN_DESC_Y = -10f;
	private const float NEW_DESC_Y = 14f;

	public void SetData(MonoBehaviour script, string methodName, KodGames.ClientClass.LevelReward lvReward, int rwdIndex)
	{
		// Set Assistant Data.
		operateBtn.GetComponent<UIElemAssistantBase>().assistantData = lvReward.Id;

		var levelRewardCfg = ConfigDatabase.DefaultCfg.LevelRewardConfig.GetLevelRewardById(lvReward.Id);

		// Set LevelReward Icon.
		rewardAssetIcon.SetData(levelRewardCfg.iconId != IDSeg.InvalidId ? levelRewardCfg.iconId : levelRewardCfg.id);
		rewardAssetIcon.Data = lvReward.Id;

		operateBtn.Data = new KeyValuePair<int, LevelRewardConfig.UpgradeReward>(rwdIndex, ConfigDatabase.DefaultCfg.LevelRewardConfig.GetLevelRewardById(lvReward.Id));
		operateBtn.scriptWithMethodToInvoke = script;
		operateBtn.methodToInvoke = methodName;

		rewardDescLabel.transform.localPosition = new Vector3(rewardDescLabel.transform.localPosition.x, NEW_DESC_Y, rewardDescLabel.transform.localPosition.z);

		rewardNameLabel.Text = ItemInfoUtility.GetAssetName(levelRewardCfg.id);
		rewardDescLabel.Text = ItemInfoUtility.GetAssetDesc(levelRewardCfg.id);

		switch (lvReward.State)
		{
			case _LevelRewardStateType.AlreadyGot:
				operateBtn.Hide(true);
				alreadyGot.Hide(false);
				break;

			case _LevelRewardStateType.Available:
				operateBtn.Hide(false);
				alreadyGot.Hide(true);
				break;

			case _LevelRewardStateType.NoAccess:
				operateBtn.Hide(false);
				alreadyGot.Hide(true);
				break;
		}
	}

	public void RwdGot()
	{
		operateBtn.Hide(true);
		alreadyGot.Hide(false);
	}
}
