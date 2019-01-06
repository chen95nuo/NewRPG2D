using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAssistantItem : MonoBehaviour
{
	public UIElemAssetIcon itemIcon;
	public SpriteText itemDesc;
	public UIButton operatorBtn;

	private com.kodgames.corgi.protocol.Task task;
	public com.kodgames.corgi.protocol.Task Task { get { return task; } }

	public void SetData(com.kodgames.corgi.protocol.Task task)
	{
		// Set Task Data.
		this.task = task;

		// Task Config.
		var taskCfg = ConfigDatabase.DefaultCfg.TaskConfig.GetTaskById(task.taskId);

		// Set Icon.
		itemIcon.SetData(taskCfg.TaskId);

		// Set Desc.
		itemDesc.Text = ItemInfoUtility.GetAssetDesc(taskCfg.TaskId);
		switch (taskCfg.TaskType)
		{
			case TaskConfig._TaskType.DungeonCanCombatAssistant:
				itemDesc.Text = string.Format(itemDesc.Text,
					ConfigDatabase.DefaultCfg.CampaignConfig.GetZonesIndexById(task.datas[0]) == 0 ?
					GameUtility.GetUIString("UIPnlCampaign_Zone_Zeo") : ItemInfoUtility.GetLevelCN(ConfigDatabase.DefaultCfg.CampaignConfig.GetZonesIndexById(task.datas[0])),
											  ItemInfoUtility.GetAssetName(task.datas[0]),
											  _DungeonDifficulity.GetDisplayNameByType(ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(task.datas[1]).DungeonDifficulty, ConfigDatabase.DefaultCfg),
											  ItemInfoUtility.GetAssetName(task.datas[1]));
				break;
			case TaskConfig._TaskType.SecretDungeonAssistant:
				itemDesc.Text = string.Format(itemDesc.Text,
											  ItemInfoUtility.GetAssetName(task.datas[0]),
											  _DungeonDifficulity.GetDisplayNameByType(task.datas[1], ConfigDatabase.DefaultCfg));
				break;

			case TaskConfig._TaskType.VipLevelGoodsAssistant:
			case TaskConfig._TaskType.ShopGoodsAssistant:
				itemDesc.Text = string.Format(itemDesc.Text, ItemInfoUtility.GetAssetName(task.datas[0]));
				break;
		}

		// Set Data.
		operatorBtn.Data = task;

		// Use this data for get UIElemAssistantItem, can't be deleted.
		operatorBtn.IndexData = task.taskId;
	}
}