using UnityEngine;
using ClientServerCommon;
using System.Collections;
using System.Collections.Generic;

public class UIPnlLevelRewardTab : UIModule
{
	public UIScrollList lvRewardList;
	public GameObjectPool rewardObjPool;

	private List<KodGames.ClientClass.LevelReward> levelRewards;
	private int selectedRwd = -1;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// In case of that the show function is not called by UIPnlActivityTab
		//   set the selected item of UIPnlActivityTab
		SysUIEnv.Instance.GetUIModule<UIPnlActivityTab>().SetActiveButton(_UIType.UIPnlLevelRewardTab);

		// [selectedRwd]'s default value is -1
		selectedRwd = -1;

		if (ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards == null || ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards.Count == 0)
			RequestMgr.Inst.Request(new QueryLevelRewardReq());
		else
			Filldata();

		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void Filldata()
	{
		if (ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards == null)
			return;
		ClearList();
		StartCoroutine("FillLevelRewardList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillLevelRewardList()
	{
		yield return null;

		for (int index = 0; index < ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards.Count; index++)
		{
			UIListItemContainer container = rewardObjPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemLevelReward lvRewardTmp = container.gameObject.GetComponent<UIElemLevelReward>();
			lvRewardTmp.SetData(this, "OnGetRewardClick", ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards[index], index);
			container.Data = lvRewardTmp;
			lvRewardList.AddItem(container);
		}
	}

	private void ClearList()
	{
		StopCoroutine("FillLevelRewardList");
		lvRewardList.ClearList(false);
		lvRewardList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRewardClick(UIButton btn)
	{
		KeyValuePair<int, LevelRewardConfig.UpgradeReward> idx_lvl = (KeyValuePair<int, LevelRewardConfig.UpgradeReward>)btn.Data;
		if (idx_lvl.Key < 0 || idx_lvl.Value == null)
			return;

		selectedRwd = idx_lvl.Key;

		LevelRewardConfig.UpgradeReward lvReward = idx_lvl.Value;

		if (lvReward.level > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlLevelReward_LevelTooLow"));
		else
			//RequestMgr.Inst.Request(new LevelRewardGetRewardReq(ActivityManager.Instance.GetActivity<ActivityLevelReward>().ActivityInfo.LevelRewards[selectedRwd].Id));
			RequestMgr.Inst.Request(new LevelRewardGetRewardReq(lvReward.id));
	}

	public void OnReqRewardListSuccess()
	{
		Filldata();
	}

	public void OnReqLevelRewardGetRewardSuccess(KodGames.ClientClass.Reward reward)
	{
		if (selectedRwd < 0)
			return;

		ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards[selectedRwd].State = 0;
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(reward, true, true));
		((UIElemLevelReward)((UIListItemContainer)lvRewardList.GetItem(selectedRwd)).Data).RwdGot();

		// reset [selectedRwd] after [ReqLevelRewardGetReward] finished
		selectedRwd = -1;
	}

	//µã»÷Í¼±ê
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgLevelRewards), (int)assetIcon.Data);
	}
}
