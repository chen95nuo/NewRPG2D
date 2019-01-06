using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemBattleRoleInfo : MonoBehaviour
{
	public AutoSpriteControlBase border;
	public float borderEdgeSize;
	public SpriteText roleName;
	public SpriteText roleHp;
	public SpriteText roleSp;
	public SpriteText roleSpeed;
	public SpriteText roleNormalSkill;
	public SpriteText roleActiveSkill;

	public void SetData(BattleRole roleData)
	{
		// Set Pos
		SysUIEnv uiEvn = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		this.gameObject.transform.localPosition = Vector3.zero;
		this.gameObject.transform.localRotation = Quaternion.identity;
		this.gameObject.transform.localScale = Vector3.one;
		Vector3 tmpPos = uiEvn.UICam.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(roleData.FxPosition));

		// must in screen
		UIPnlBattleBar battlebar = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlBattleBar>();
		tmpPos.x = Mathf.Min(90 * battlebar.gameObject.transform.localScale.x, Mathf.Max(-90 * battlebar.gameObject.transform.localScale.x, tmpPos.x));
		this.gameObject.transform.position = tmpPos;

		float borderWith = 0;
		// Set Contents
		roleName.Text = GameUtility.FormatUIString("UIDlgBattleRoleInfo_Name", roleData.AvatarData.DisplayName, roleData.AvatarData.LevelAttrib.Level);
		
		roleHp.Text = string.Format(GameUtility.GetUIString("UIDlgBattleRoleInfo_HP"), 
		                            roleData.AvatarHP,
		                            roleData.AvatarData.GetAttributeByType(_AvatarAttributeType.MaxHP).Value);

		int speed = (int)roleData.AvatarData.GetAttributeByType(_AvatarAttributeType.Speed).Value;
		roleSpeed.Text = GameUtility.FormatUIString("UIDlgBattleRoleInfo_Speed", speed);

		int normalSkillId = 0;
		int activeSKillId = 0;
		int maxSp = 0;
		foreach (KodGames.ClientClass.SkillData skillData in roleData.AvatarData.Skills)
		{
			CombatTurn turnSkill = ConfigDatabase.DefaultCfg.ActionConfig.GetCombatTurnByID(skillData.Id);

			if (turnSkill == null)
			{
				Debug.Log("CombatTurn == null skillID " + skillData.Id.ToString("X"));
				continue;
			}

			if (turnSkill.type == CombatTurn._Type.NormalSkill)
			{
				normalSkillId = skillData.Id;
			}
			else if(turnSkill.type == CombatTurn._Type.ActiveSkill)
			{
				activeSKillId = skillData.Id;
				maxSp = turnSkill.costSkillPower;
			}
		}

		int sp = (int)(roleData.BattleBar.hpBar.SPValue);
		roleSp.Text = GameUtility.FormatUIString("UIDlgBattleRoleInfo_SP", sp, maxSp);

		if (normalSkillId == 0)
		{
			roleNormalSkill.Text = "";
		}
		else
		{
			roleNormalSkill.Text = GameUtility.FormatUIString("UIDlgBattleRoleInfo_NormalSkill", ItemInfoUtility.GetAssetName(normalSkillId));
		}

		if (activeSKillId == 0)
		{
			roleActiveSkill.Text = "";
		}
		else
		{
			roleActiveSkill.Text = GameUtility.FormatUIString("UIDlgBattleRoleInfo_ActiveSkill", ItemInfoUtility.GetAssetName(activeSKillId));
		}

		// Resize boder size
		borderWith = Mathf.Max(roleName.BottomRight.x - roleName.TopLeft.x, borderWith);
		borderWith = Mathf.Max(roleHp.BottomRight.x - roleHp.TopLeft.x, borderWith);
		borderWith = Mathf.Max(roleSp.BottomRight.x - roleSp.TopLeft.x, borderWith);
		borderWith = Mathf.Max(roleSpeed.BottomRight.x - roleSpeed.TopLeft.x, borderWith);
		borderWith = Mathf.Max(roleNormalSkill.BottomRight.x - roleNormalSkill.TopLeft.x, borderWith);
		borderWith = Mathf.Max(roleActiveSkill.BottomRight.x - roleActiveSkill.TopLeft.x, borderWith);
		border.SetSize(borderWith + borderEdgeSize, border.height);

		// Set content position
		float x = 7 - border.width / 2;
		roleName.CachedTransform.localPosition = new Vector3(x, roleName.CachedTransform.localPosition.y, roleName.CachedTransform.localPosition.z);
		roleHp.CachedTransform.localPosition = new Vector3(x, roleHp.CachedTransform.localPosition.y, roleHp.CachedTransform.localPosition.z);
		roleSp.CachedTransform.localPosition = new Vector3(x, roleSp.CachedTransform.localPosition.y, roleSp.CachedTransform.localPosition.z);
		roleSpeed.CachedTransform.localPosition = new Vector3(x, roleSpeed.CachedTransform.localPosition.y, roleSpeed.CachedTransform.localPosition.z);
		roleNormalSkill.CachedTransform.localPosition = new Vector3(x, roleNormalSkill.CachedTransform.localPosition.y, roleNormalSkill.CachedTransform.localPosition.z);
		roleActiveSkill.CachedTransform.localPosition = new Vector3(x, roleActiveSkill.CachedTransform.localPosition.y, roleActiveSkill.CachedTransform.localPosition.z);
	}
}
