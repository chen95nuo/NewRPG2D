using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemFirstRechargeItem : MonoBehaviour
{
	public UIElemAssetIcon rewardIcon;
	public SpriteText rewardNameLabel;

	public void SetData(ClientServerCommon.Reward reward)
	{
		switch (IDSeg.ToAssetType(reward.id))
		{
			case IDSeg._AssetType.Avatar:

				KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
				avatar.IsAvatar = true;
				avatar.ResourceId = reward.id;
				avatar.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
				avatar.LevelAttrib.Level = reward.level;
				avatar.BreakthoughtLevel = reward.breakthoughtLevel;

				rewardIcon.Data = reward.id;
				rewardIcon.SetData(avatar);
				break;

			case IDSeg._AssetType.Equipment:
				KodGames.ClientClass.Equipment equip = new KodGames.ClientClass.Equipment();
				equip.ResourceId = reward.id;
				equip.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
				equip.LevelAttrib.Level = reward.level;
				equip.BreakthoughtLevel = reward.breakthoughtLevel;

				rewardIcon.Data = reward.id;
				rewardIcon.SetData(equip);
				break;

			case IDSeg._AssetType.CombatTurn:
				KodGames.ClientClass.Skill skill = new KodGames.ClientClass.Skill();
				skill.ResourceId = reward.id;
				skill.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
				skill.LevelAttrib.Level = reward.level;

				rewardIcon.Data = reward.id;
				rewardIcon.SetData(skill);
				break;

			default:

				rewardIcon.Data = reward.id;
				rewardIcon.SetData(reward.id, reward.count);
				break;
		}

		rewardNameLabel.Text = ItemInfoUtility.GetAssetName(reward.id);
	}
}
