using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemEmailItem : MonoBehaviour
{
	public SpriteText emailTimeLabel;
	public SpriteText emailMessageLabel;
	public SpriteText statusText;
	public UIButton emailOperate;
	public GameObjectPool attachmentsPool;
	public AutoSpriteControlBase root;
	public UIListItemContainer container;
	public UIBox newMessageBox;
	public UIButton goShoppingBtn;

	private const int Max_Colunm_Attachment_Count = 4;
	public KodGames.ClientClass.EmailPlayer email;

	private float originalRootHeight;
	private List<GameObject> attachmentItems;

	public void Awake()
	{
		root.height = 38;
		originalRootHeight = root.height;
		attachmentItems = new List<GameObject>();
	}

	public void SetData(KodGames.ClientClass.EmailPlayer email, long lastTime)
	{
		container.Data = this;
		this.email = email;
	
		// Receive date
		System.DateTime receiveTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(email.SendTime);
		System.DateTime nowTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		System.TimeSpan timeSpane = nowTime.Subtract(receiveTime);

		//计算邮件时间与上次退出时间的差，然后控制是否显示“新”【错误】
		if (lastTime < 0)
			lastTime = email.SendTime;

		SetNewMessageBox(lastTime - email.SendTime >= 0 ? true : false);

		if (nowTime.Day != receiveTime.Day || timeSpane.Days > 0)
		{
			int dayBefore = timeSpane.Days <= 0 ? 1 : timeSpane.Days;
			emailTimeLabel.Text = string.Format(GameUtility.GetUIString("UIPnlEmail_Receive_TimeShortDesc"), dayBefore);
		}
		else
			emailTimeLabel.Text = string.Format(GameUtility.GetUIString("UIPnlEmail_Receive_TimeLongDesc"), receiveTime.Hour.ToString("D2"), receiveTime.Minute.ToString("D2"));

		//对面板的大小控制
		int row = 0;
		int startIndex = 0;
		SpriteText tempText = emailMessageLabel;
		tempText.Text = "";

		for (int index = 0; index < email.EmailBody.Length; index++)
		{
			if (email.EmailBody[index].Equals('\n'))
			{
				row++;
				string strTemp = email.EmailBody.Substring(startIndex, index - startIndex);
				tempText.Text = strTemp;
				row += (int)(tempText.DisplayString.Length / 20.0f);
				startIndex = index + 1;
				tempText.Text = "";
			}
		}

		if (startIndex < email.EmailBody.Length)
		{
			string strTemp1 = email.EmailBody.Substring(startIndex, email.EmailBody.Length - startIndex);
			tempText.Text = strTemp1;
			row += (int)(tempText.DisplayString.Length % 20.0f == 0 ? tempText.DisplayString.Length / 20.0f : tempText.DisplayString.Length / 20.0f + 1);
		}

		//显示邮件文字内容
		emailMessageLabel.Text = email.EmailBody;

		root.height = root.height + (row + 1) * 14.0f;
		//root.height = root.height + 14.0f;
		goShoppingBtn.Hide(email.EmailType != _MailType.GuildBossKilledWithShop);
		if (email.EmailType == _MailType.GuildBossKilledWithShop)
			root.height = root.height >= 100 ? root.height : 100;
		
		// Operation button
		//根据不同的情况标签进行邮件内容渲染
		if (email.AttachmentRewards != null && email.GetAttachmentRewardsCount() != 0)//has attachment 
		{
			SetAttachmentAchievedState(email.StatusDidPick != 0);
			//Show the attachment
			ShowEmailAttachment();

		}
		else if (email.EmailType == _MailType.AddFriendRequrst)
		{
			SetEmailFriendRequestState();
		}
		else if (email.EmailType == _MailType.CombatRob)
		{
			SetEmailCombatRobRequestState();
		}
		else if (email.EmailType == _MailType.CombatArena)
		{
			SetEmailAreanRequestState();
		}
		else if (email.EmailType == _MailType.Guild || email.EmailType == _MailType.GuildBossKilledNoShop || email.EmailType == _MailType.GuildBossKilledWithShop)
		{
			SetEmailGuildRequestState();
			if (email.EmailType == _MailType.Guild)
				emailOperate.Hide(true);
			else
				emailOperate.Hide(false);
			
		}

		else
		{
			emailOperate.Hide(true);
			statusText.Text = "";
		}

		emailOperate.data = this;
	}


	//设置附件是否已经领取掉
	public void SetAttachmentAchievedState(bool hasAchieved)
	{
		emailOperate.Hide(hasAchieved);
		if (hasAchieved)
		{
			//设置标签颜色和文字
			statusText.Text = GameDefines.textColorGreen + GameUtility.GetUIString("UIPnlEmail_Achieved_Attachment");
			//如果附件已经领取掉，那么强制清除“新”标签
			SetNewMessageBox(true);
		}
		else
		{
			statusText.Text = "";
			emailOperate.Text = GameUtility.GetUIString("UIPnlEmail_Open_Attachment");
		}
	}

	public void SetEmailCombatRobRequestState()
	{
		emailOperate.Hide(false);
		statusText.Text = "";
		emailOperate.Text = GameUtility.GetUIString("UIPnlEmail_CombatRob");
	}

	public void SetEmailGuildRequestState()
	{
		statusText.Text = "";
		emailOperate.Text = GameUtility.GetUIString("UIPnlEmail_GetReward");
	}

	public void SetEmailAreanRequestState()
	{
		emailOperate.Hide(false);
		statusText.Text = "";
		emailOperate.Text = GameUtility.GetUIString("UIPnlEmail_CombatArean");
	}

	public void SetEmailFriendRequestState()
	{
		// If has deal this email , hide the operation button.
		emailOperate.Hide(email.StatusDidPick > 0);

		if (email.StatusDidPick > 0)
			SetNewMessageBox(true);

		switch (email.StatusDidPick)
		{
			case 0:  // Not Deal with this Email.
				emailOperate.Text = GameUtility.GetUIString("UIPnlEmail_Handle_Message");
				statusText.Text = "";
				break;
			case 1:  // Agree this friend request.
				statusText.Text = GameDefines.textColorGreen + GameUtility.GetUIString("UIPnlEmail_FriendReq_Agree");
				break;
			case 2:  // Refuse this friend request.
				statusText.Text = GameDefines.txColorRed + GameUtility.GetUIString("UIPnlEmail_FriendReq_Refuse");
				break;
		}
	}

	//对邮件附件进行渲染
	private void ShowEmailAttachment()
	{
		if (email.AttachmentRewards == null || email.GetAttachmentRewardsCount() == 0)
		{
			return;
		}

		float lineSpacing = -4.0f;
		UIElemEmailGiftItem item = null;
		int columnIndex = 0;

		//角色
		foreach (var avatar in email.AttachmentRewards.Avatar)
		{
			if (item == null)
			{
				GameObject attachement = attachmentsPool.AllocateItem();
				item = attachement.GetComponent<UIElemEmailGiftItem>();
				item.InitButtonState();
				attachmentItems.Add(attachement);
			}

			Reward reward = new Reward();
			reward.id = avatar.ResourceId;
			reward.breakthoughtLevel = avatar.BreakthoughtLevel;
			reward.level = avatar.LevelAttrib.Level;

			item.attachmentBtns[columnIndex].Data = reward;
			item.attachmentBtns[columnIndex++].SetData(avatar);

			if (columnIndex == Max_Colunm_Attachment_Count)
			{
				float rootHeight = root.height;
				root.height = rootHeight + (item.attachmentBtns[0].border).height;
				item.transform.parent = root.transform;
				item.transform.localPosition = new Vector3(0, (-1) * (rootHeight - lineSpacing), 0);
				item = null;
				columnIndex = 0;
			}
		}

		//装备
		foreach (KodGames.ClientClass.Equipment equipment in email.AttachmentRewards.Equip)
		{
			if (item == null)
			{
				GameObject attachement = attachmentsPool.AllocateItem();
				item = attachement.GetComponent<UIElemEmailGiftItem>();
				item.InitButtonState();
				attachmentItems.Add(attachement);
			}

			Reward reward = new Reward();
			reward.id = equipment.ResourceId;
			reward.breakthoughtLevel = equipment.BreakthoughtLevel;
			reward.level = equipment.LevelAttrib.Level;

			item.attachmentBtns[columnIndex].Data = reward;
			item.attachmentBtns[columnIndex++].SetData(equipment);

			if (columnIndex == Max_Colunm_Attachment_Count)
			{
				float rootHeight = root.height;
				root.height = rootHeight + (item.attachmentBtns[0].border).height;
				item.transform.parent = root.transform;
				item.transform.localPosition = new Vector3(0, (-1) * (rootHeight - lineSpacing), -0.001f);
				item = null;
				columnIndex = 0;
			}
		}

		//技能
		foreach (KodGames.ClientClass.Skill skill in email.AttachmentRewards.Skill)
		{
			SkillConfig.Skill skillConfig = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
			if (skillConfig == null)
			{
				Debug.LogError("Skill " + skill.ResourceId.ToString("X") + " in SkillConfig is not found.");
				continue;
			}
			else if (skillConfig.type != CombatTurn._Type.PassiveSkill)
				continue;

			if (item == null)
			{
				GameObject attachement = attachmentsPool.AllocateItem();
				item = attachement.GetComponent<UIElemEmailGiftItem>();
				item.InitButtonState();
				attachmentItems.Add(attachement);
			}

			Reward reward = new Reward();
			reward.id = skill.ResourceId;
			reward.level = skill.LevelAttrib.Level;

			item.attachmentBtns[columnIndex].Data = reward;
			item.attachmentBtns[columnIndex++].SetData(skill);

			if (columnIndex == Max_Colunm_Attachment_Count)
			{
				float rootHeight = root.height;
				root.height = rootHeight + (item.attachmentBtns[0].border).height;
				item.transform.parent = root.transform;
				item.transform.localPosition = new Vector3(0, (-1) * (rootHeight - lineSpacing), -0.001f);
				item = null;
				columnIndex = 0;
			}
		}

		//消耗品
		foreach (KodGames.ClientClass.Consumable consumable in email.AttachmentRewards.Consumable)
		{
			if (item == null)
			{
				GameObject attachement = attachmentsPool.AllocateItem();
				item = attachement.GetComponent<UIElemEmailGiftItem>();
				item.InitButtonState();
				attachmentItems.Add(attachement);
			}

			Reward reward = new Reward();
			reward.id = consumable.Id;

			item.attachmentBtns[columnIndex].Data = reward;
			item.attachmentBtns[columnIndex++].SetData(consumable);

			if (columnIndex == Max_Colunm_Attachment_Count)
			{
				float rootHeight = root.height;
				root.height = rootHeight + (item.attachmentBtns[0].border).height;
				item.transform.parent = root.transform;
				item.transform.localPosition = new Vector3(0, (-1) * (rootHeight - lineSpacing), -0.001f);
				item = null;
				columnIndex = 0;
			}
		}

		//对附件进行处理
		if (item != null && columnIndex < item.attachmentBtns.Count)
		{
			for (; columnIndex < item.attachmentBtns.Count; columnIndex++)
				item.attachmentBtns[columnIndex].Hide(true);

			//设置第二排附件的高度
			float rootHeight = root.height;
			root.height = rootHeight + (item.attachmentBtns[0].border).height;
			item.transform.parent = root.transform;
			item.transform.localPosition = new Vector3(0, (-1) * (rootHeight - lineSpacing), -0.001f);
			item = null;
			columnIndex = 0;
		}
	}

	public void ReleaseAttachmentItems()
	{
		foreach (var item in attachmentItems)
		{
			attachmentsPool.ReleaseItem(item);
		}

		attachmentItems.Clear();

		root.SetSize(root.width, originalRootHeight);
	}

	//设置新旧标签显示状况
	public void SetNewMessageBox(bool pRet)
	{
		newMessageBox.Hide(pRet);
	}
}
