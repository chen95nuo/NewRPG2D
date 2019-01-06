using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;

public class UIPnlAssistantTask : UIModule
{
	public UIScrollList taskList;
	public GameObjectPool taskItemPool;
	public SpriteText emptyLabel;

	private bool needRefresh= false;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		emptyLabel.Text = string.Empty;

		SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().ChangeTab(_UIType.UIPnlAssistantTask);

		RequestMgr.Inst.Request(new QueryTaskListReq());

		return true;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		if (needRefresh)
		{
			StartCoroutine("FillList");
			needRefresh = false;
		}
	}

	public override void OnHide()
	{		
		ClearData();
		base.OnHide();
	}

	public void QueryTaskListReqSuccess()
	{
		if (IsOverlayed)
			needRefresh = true;
		else
			StartCoroutine("FillList");
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		taskList.ClearList(false);
		taskList.ScrollPosition = 0;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		if (SysLocalDataBase.Inst.LocalPlayer.TaskData.Tasks.Count <= 0)
		{
			// Set Empty Label.
			emptyLabel.Text = GameUtility.GetUIString("UIPnlAssistant_EmptyLabel");
			// Clear List.
			ClearData();

			yield break;
		}
		else
			emptyLabel.Text = string.Empty;

		// Delete Task Not exists.
		for (int index = 0; index < taskList.Count; index++)
		{
			var contianer = taskList.GetItem(index) as UIListItemContainer;
			var item = contianer.Data as UIElemAssistantItem;

			if (SysLocalDataBase.Inst.LocalPlayer.TaskData.GetTaskById(item.operatorBtn.IndexData) == null)
				taskList.RemoveItem(contianer, false);
		}

		// Add New Task and Update Old Task.
		foreach (var task in SysLocalDataBase.Inst.LocalPlayer.TaskData.Tasks)
		{
			var oldTaskItem = GetAssistantItemByID(task.taskId);

			if (oldTaskItem == null)
			{
				var container = taskItemPool.AllocateItem().GetComponent<UIListItemContainer>();
				var item = container.GetComponent<UIElemAssistantItem>();

				container.Data = item;
				item.SetData(task);

				taskList.AddItem(container);
			}
			else if (IsAssistantDataChanged(task, oldTaskItem.Task))
				oldTaskItem.SetData(task);
		}
	}

	private bool IsAssistantDataChanged(com.kodgames.corgi.protocol.Task taskOne, com.kodgames.corgi.protocol.Task taskTwo)
	{
		if (taskOne == null || taskTwo == null)
			return true;

		if (taskOne.datas.Count != taskTwo.datas.Count)
			return true;

		for (int index = 0; index < taskOne.datas.Count; index++)
		{
			if (taskOne.datas[index] != taskTwo.datas[index])
				return true;
		}

		return false;
	}

	private UIElemAssistantItem GetAssistantItemByID(int assistantID)
	{
		for (int i = 0; i < taskList.Count; i++)
		{
			var container = taskList.GetItem(i) as UIListItemContainer;
			var item = container.Data as UIElemAssistantItem;

			if (item != null &&
				item.Task != null &&
				item.Task.taskId == assistantID)
				return item;
		}

		return null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemOperator(UIButton btn)
	{
		var task = btn.Data as com.kodgames.corgi.protocol.Task;

		SysAssistant.Instance.SetTask(task);
	}

	public void OnTaskChanged()
	{
		RequestMgr.Inst.Request(new QueryTaskListReq());
	}
}
