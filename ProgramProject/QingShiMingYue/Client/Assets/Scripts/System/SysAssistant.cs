//#define ENABLE_ASSISTANT_LOG
using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class SysAssistant : SysModule
{
	public class AssistantData
	{
		public int taskId;
		public List<int> datas = new List<int>();
		public TaskConfig.TaskGuid taskGuidCfg;
		public TaskConfig.Task taskCfg;
	}

	public static SysAssistant Instance { get { return SysModuleManager.Instance.GetSysModule<SysAssistant>(); } }

	private bool pause;
	public bool Pause
	{
		get { return pause; }
		set { pause = value; }
	}

	private Dictionary<string, GameObject> cachedTagObjects = new Dictionary<string, GameObject>();

	private AssistantData taskData;
	public AssistantData CurrentTaskData { get { return taskData; } }

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		ClearData();

		return true;
	}

	public override void OnUpdate()
	{
		if (pause || taskData == null)
			return;

		if (CheckGuidCondition())
			ShowGuid();
	}

	public override void Dispose()
	{
		base.Dispose();

		ClearData();
	}

	public void SetTask(com.kodgames.corgi.protocol.Task taskData)
	{
		if (taskData == null)
		{
#if ENABLE_ASSISTANT_LOG
			Debug.Log("SysAssistant SetTask : TaskData is null");
#endif
			return;
		}

		var taskCfg = ConfigDatabase.DefaultCfg.TaskConfig.GetTaskById(taskData.taskId);
		if (taskCfg == null)
		{
#if ENABLE_ASSISTANT_LOG
		Debug.Log("TaskConfig Error TaskId : " + taskData.taskId.ToString("X"));	
#endif
			return;
		}

		var guidCfg = ConfigDatabase.DefaultCfg.TaskConfig.GetTaskGuidByType(taskCfg.TaskType);
		if (guidCfg == null)
		{
#if ENABLE_ASSISTANT_LOG
		Debug.Log("TaskConfig Not Container GuidType : " + TaskConfig._TaskType.GetNameByType(taskCfg.TaskType));	
#endif
			return;
		}

		// Set Data.
		this.taskData = new AssistantData();
		this.taskData.taskId = taskData.taskId;
		this.taskData.taskGuidCfg = guidCfg;
		this.taskData.taskCfg = taskCfg;
		this.taskData.datas.Clear();

		for (int index = 0; index < taskData.datas.Count; index++)
			this.taskData.datas.Add(taskData.datas[index]);

		// Show Goto UI.
		if (taskCfg.GotoUI != _UIType.UnKonw)
		{
			switch (taskCfg.GotoUI)
			{
				case _UIType.UIPnlHandBook:
					GameUtility.JumpUIPanel(taskCfg.GotoUI, taskData.datas[0]);
					break;
				default:
					GameUtility.JumpUIPanel(taskCfg.GotoUI);
					break;
			}
		}
	}

	private bool CheckGuidCondition()
	{
		var go = GetObjectWithTag(taskData.taskGuidCfg.AttachConParent);

		switch (taskData.taskCfg.TaskType)
		{
			case TaskConfig._TaskType.TavernAssistant:
				if (go == null)
					return false;

				var tavernList = go.GetComponent<UIScrollList>();
				return SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos.Count > 0 && tavernList.Count == SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos.Count;

			case TaskConfig._TaskType.LevelRewardAssistant:
				if (go == null)
					return false;

				var levelList = go.GetComponent<UIScrollList>();
				return ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards != null &&
						ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards.Count > 0 &&
						ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards.Count == levelList.Count;

			case TaskConfig._TaskType.GachaAssistant:
				if (go == null)
					return false;

				var itemList = go.GetComponent<UIScrollList>();
				return itemList.Count == GameUtility.GetConsumableCounts();

			case TaskConfig._TaskType.BuyStaminaAssistant:
				return true;

			case TaskConfig._TaskType.MonthCardAssistant:
				if (go == null)
					return false;

				var cardList = go.GetComponent<UIScrollList>();
				var cardInfo = ActivityManager.Instance.GetActivity<ActivityMonthCard>();

				return cardInfo != null && cardInfo.MonthCardInfos != null && cardInfo.MonthCardInfos.Count > 0 && cardList.Count == cardInfo.MonthCardInfos.Count;

			case TaskConfig._TaskType.VipLevelGoodsAssistant:
			case TaskConfig._TaskType.ShopGoodsAssistant:

				if (go == null || SysLocalDataBase.Inst.LocalPlayer == null)
					return false;

				int goodCount = 0;
				var goods = SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods;
				for (int i = 0; i < goods.Count; i++)
				{
					if (goods[i].Status == _GoodsStatusType.Closed)
						continue;

					var goodConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goods[i].GoodsID);
					if (goodConfig.goodsType != _Goods.NormalGoods ||
					   goods[i].ShowVipLevel > SysLocalDataBase.Inst.LocalPlayer.VipLevel ||
					   goods[i].ShowPlayerLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
						continue;

					if (taskData.taskCfg.TaskType == TaskConfig._TaskType.VipLevelGoodsAssistant &&
					   goodConfig.GetGoodItemType() != ItemConfig._Type.Package)
						continue;

					if (taskData.taskCfg.TaskType == TaskConfig._TaskType.ShopGoodsAssistant &&
					   goodConfig.GetGoodItemType() == ItemConfig._Type.Package)
						continue;

					goodCount++;
				}

				var goodList = go.GetComponent<UIScrollList>();
				if (goodList == null)
					return false;

				return goodList.Count >= goodCount;

			default:
				if (go == null)
					return false;

				var comBases = go.GetComponentsInChildren<UIElemAssistantBase>();
				if (comBases == null || comBases.Length <= 0)
					return false;
				else
				{
					for (int index = 0; index < comBases.Length; index++)
						if (comBases[index].gameObject.activeInHierarchy)
							return true;
				}
				break;
		}

		return false;
	}

	public void ClearData()
	{
		taskData = null;
	}

	// 是否有ui引导的小助手任务
	public bool HasUIGuid()
	{
		if (taskData == null)
			return false;

		switch (taskData.taskCfg.TaskType)
		{
			case TaskConfig._TaskType.TavernAssistant:
			case TaskConfig._TaskType.LevelRewardAssistant:
			case TaskConfig._TaskType.GachaAssistant:
			case TaskConfig._TaskType.AvatarIllustrationAssistant:
			case TaskConfig._TaskType.EquipIllustrationAssistant:
			case TaskConfig._TaskType.SkillIllustrationAssistant:
			case TaskConfig._TaskType.DungeonStarRewardAssistant:
			case TaskConfig._TaskType.MonthCardAssistant:
			case TaskConfig._TaskType.ShopGoodsAssistant:
			case TaskConfig._TaskType.VipLevelGoodsAssistant:
				return true;
			default:
				return false;
		}
	}

	private void ShowGuid()
	{
		switch (taskData.taskCfg.TaskType)
		{
			case TaskConfig._TaskType.BuyStaminaAssistant:
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAffordCost), IDSeg._SpecialId.Stamina);
				break;

			default:
				if (string.IsNullOrEmpty(taskData.taskGuidCfg.AttachConParent))
					return;

				var go = GetObjectWithTag(taskData.taskGuidCfg.AttachConParent);
				if (go == null)
				{
#if ENABLE_ASSISTANT_LOG
		Debug.Log("Not Found GameObject :" + guidCfg.AttachConParent));	
#endif
					return;
				}

				var comBases = go.GetComponentsInChildren<UIElemAssistantBase>(true);
				if (comBases == null || comBases.Length <= 0)
					return;
				else
				{
					for (int index = 0; index < comBases.Length; index++)
					{
						if (taskData.datas != null && taskData.datas.Count > 0 && comBases[index].assistantData != taskData.datas[taskData.datas.Count - 1])
							continue;

						// Scroll To Item.
						if (comBases[index].container != null)
						{
							var scroll = go.GetComponent<UIScrollList>();
							if (scroll != null)
								scroll.ScrollToItem(comBases[index].container, 0f);
						}

						// Attach Component.
						SysUIEnv.Instance.GetUIModule<UITipAssistant>().ShowAssistant(comBases[index].attachObject);

						break;
					}
				}
				break;
		}

		ClearData();
	}

	private GameObject GetObjectWithTag(string tag)
	{
		GameObject go = GameObject.FindGameObjectWithTag(tag);

		if (go == null)
		{
			if (cachedTagObjects.ContainsKey(tag) == false)
				return null;

			go = cachedTagObjects[tag];

			return go;
		}

		if (cachedTagObjects.ContainsKey(tag) == false)
			cachedTagObjects.Add(tag, go);
		else
			cachedTagObjects[tag] = go;

		return go;
	}
}