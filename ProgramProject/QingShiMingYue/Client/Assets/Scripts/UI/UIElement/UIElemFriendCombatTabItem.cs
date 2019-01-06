using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendCombatTabItem : MonoBehaviour
{
	public UIElemAssetIcon avatarIcon;//角色
	public SpriteText avatarName;//名字
	public UIBox avatarType;	//类型
	public UIBox avatarRecruite;//门客
	public SpriteText avatarRecruiteLabel;//门客名字
	public UIProgressBar hpProgress;//显示血量
	public UIBox dieBox;	//死亡
	public UIBox alpBox;	//死亡阴影

	private int position;
	public int Position
	{
		get { return this.position; }
	}

	public void InitData(int position)
	{
		this.position = position;
		dieBox.Hide(true);
		alpBox.Hide(true);
		hpProgress.Value = 0;
		hpProgress.Text = string.Empty;
		avatarRecruite.Hide(true);
		avatarRecruiteLabel.Text = string.Empty;
		avatarName.Text = string.Empty;
		avatarType.Hide(true);
		avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar, bool isRecruiteAvatar, double hp)
	{
		if (avatar != null)
		{
			avatarIcon.Hide(false);
			avatarIcon.SetData(avatar);
			avatarType.Hide(false);

			if (avatar.TraitType == AvatarConfig._AvatarTraitType.UnKnow)
				UIElemTemplate.Inst.SetAvatarTraitIcon(avatarType, ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).traitType);
			else
				UIElemTemplate.Inst.SetAvatarTraitIcon(avatarType, avatar.TraitType);

			if (isRecruiteAvatar)
			{
				avatarRecruite.Hide(false);
				avatarRecruiteLabel.Text = GameUtility.GetUIString("UIPnlFriendCombatTab_Recruite");
			}


			avatarName.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);

			hp = (hp < 0 ? 1 : hp) > 1 ? 1 : (hp < 0 ? 1 : hp);
			hpProgress.Hide(false);
			hpProgress.Value = (float)hp;
			hpProgress.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_HP", (int)(hp * 100));
			dieBox.Hide(hp > 0);
			alpBox.Hide(hp > 0);
		}
	}

	public void SetData(com.kodgames.corgi.protocol.RobotNpc robotNpc, bool isRecruteAvatar, double hp)
	{
		if (robotNpc != null)
		{
			avatarIcon.Hide(false);
			avatarIcon.SetData(robotNpc.recourseId, robotNpc.breakthoughtLevel, robotNpc.level);

			avatarType.Hide(false);
			UIElemTemplate.Inst.SetAvatarTraitIcon(avatarType, robotNpc.traitType);

			if (isRecruteAvatar)
			{
				avatarRecruite.Hide(false);
				avatarRecruiteLabel.Text = GameUtility.GetUIString("UIPnlFriendCombatTab_Recruite");
			}

			avatarName.Text = robotNpc.name;

			hp = (hp < 0 ? 1 : hp) > 1 ? 1 : (hp < 0 ? 1 : hp);
			hpProgress.Hide(false);
			hpProgress.Value = (float)hp;
			hpProgress.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_HP", (int)(hp * 100));
			dieBox.Hide(hp > 0);
			alpBox.Hide(hp > 0);
		}
	}

	public void ClearData()
	{
		dieBox.Hide(true);
		alpBox.Hide(true);
		hpProgress.Value = 0;
		hpProgress.Text = string.Empty;
		hpProgress.Hide(true);
		avatarRecruite.Hide(true);
		avatarRecruiteLabel.Text = string.Empty;
		avatarName.Text = string.Empty;
		avatarType.Hide(true);
		avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		avatarIcon.Hide(true);
	}
}
