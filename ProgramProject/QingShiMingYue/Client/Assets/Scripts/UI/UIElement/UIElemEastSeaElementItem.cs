using UnityEngine;
using System.Collections;
using ClientServerCommon;
using KodGames;
using System.Collections.Generic;

public class UIElemEastSeaElementItem :MonoBehaviour
{
	public SpriteText nameLable;
	public SpriteText contextLable;
	public SpriteText eastSeaNumLable;
	public UIButton exchangeBtn;
	public UIElemAssetIcon icon;

	public void SetData(KodGames.ClientClass.ZentiaGood zentiaGood)
	{
		nameLable.Text = ItemInfoUtility.GetAssetName(zentiaGood.Cost.Id);
		eastSeaNumLable.Text = zentiaGood.Cost.Count.ToString();
		List<KodGames.Pair<int, int>> rewardPars = SysLocalDataBase.ConvertIdCountList(zentiaGood.Reward);
		if (rewardPars != null && rewardPars.Count > 0)
		{
			icon.SetData(rewardPars[0].first);
			icon.GetComponent<UIButton>().Data = ToAssetTypeRewardComm(zentiaGood.Reward)[0];
			contextLable.Text = string.Format("{0} × {1}", ItemInfoUtility.GetAssetName(rewardPars[0].first), rewardPars[0].second);
		}
		
		exchangeBtn.Data = zentiaGood;
	}

	private List<ClientServerCommon.Reward> ToAssetTypeRewardComm(KodGames.ClientClass.Reward item)
	{
		List<ClientServerCommon.Reward> rewardCommList = new List<ClientServerCommon.Reward>();
		List<KodGames.Pair<int, int>> rewardPars = SysLocalDataBase.ConvertIdCountList(item);
		for (int i = 0; i < rewardPars.Count; i++)
		{
			var reward = rewardPars[i];
			var rewardComm = new ClientServerCommon.Reward();
			rewardComm.id = reward.first;
			rewardComm.count = reward.second;
			switch (IDSeg.ToAssetType(reward.first))
			{
				case IDSeg._AssetType.Avatar:
					rewardComm.breakthoughtLevel = item.Avatar[i].BreakthoughtLevel;
					rewardComm.level = item.Avatar[i].LevelAttrib.Level;
					break;
				case IDSeg._AssetType.Equipment:
					rewardComm.breakthoughtLevel = item.Equip[i].BreakthoughtLevel;
					rewardComm.level = item.Equip[i].LevelAttrib.Level;
					break;
				case IDSeg._AssetType.CombatTurn:
					if (item.Skill != null && item.Skill.Count > 0)
						rewardComm.level = item.Skill[i].LevelAttrib.Level;
					break;
			}
			rewardCommList.Add(rewardComm);
		}

		return rewardCommList;
	}
}
