using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemEastSeaElementAllServerReward : MonoBehaviour
{
	public SpriteText contextLable;
	public SpriteText eastSeaNumLable;
	public UIButton exchangeBtn;
	public UIBox hasGet;
	public UIBox hasReach;
	public UIBox arrows;
	public UIElemAssetIcon icon1;
	public UIElemAssetIcon icon2;
	public SpriteText totalLable;
	public UIBox notifyIcon;

	private Vector3 arrowLocalPosition = new Vector3(0, 52.93f, 0);
	private Vector2 arrowSize = new Vector2(120,30);
	private Vector3 arrowPoint=Vector3.zero;

	private void SetArrowStatus(int index)
	{
		if(arrows!=null)
			arrows.SetToggleState(index);
	}

	public void SetData(KodGames.ClientClass.ZentiaServerReward zServerReward, int totalZentiaPoint, long serverZentiaPoint, bool isLast, KodGames.ClientClass.ZentiaServerReward zServerRewardLast)
	{
		
		exchangeBtn.Data = zServerReward;
		Color zentiaColor = Color.red;
		if (arrowPoint == Vector3.zero)
			arrowPoint = arrows.transform.localPosition;
		if (zServerReward.IsActived)
		{
			if (serverZentiaPoint >= zServerReward.ServerZentiaPoint)
			{
				zentiaColor = GameDefines.zentiaColorGreen;
				SetArrowStatus(2);
			}
			else
			{
				zentiaColor = GameDefines.zentiaColorBlue;
				SetArrowStatus(1);
			}
			exchangeBtn.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			hasReach.gameObject.SetActive(true);

		}
		else
		{
			if (serverZentiaPoint >= zServerRewardLast.ServerZentiaPoint)
			{
				zentiaColor = GameDefines.zentiaColorBlue;
				SetArrowStatus(1);
			}
			else
			{
				zentiaColor = GameDefines.zentiaColorGrey;
				SetArrowStatus(0);
			}
			exchangeBtn.SetControlState(UIButton.CONTROL_STATE.DISABLED);
			hasReach.gameObject.SetActive(false);
		}

		if (totalZentiaPoint < zServerReward.TotalZentiaPoint)
			totalLable.Text = (GameUtility.FormatUIString("UIPnlEastElementAllServerReward_TotalContext", GameDefines.textColorTipsInBlack, totalZentiaPoint, GameDefines.textColorRed, zServerReward.TotalZentiaPoint));
		else
			totalLable.Text = (GameUtility.FormatUIString("UIPnlEastElementAllServerReward_TotalContext_Success", GameDefines.textColorTipsInBlack, zServerReward.TotalZentiaPoint));

		if (serverZentiaPoint >= zServerReward.ServerZentiaPoint && totalZentiaPoint >= zServerReward.TotalZentiaPoint)
			notifyIcon.gameObject.SetActive(true);
		else
			notifyIcon.gameObject.SetActive(false);

		if (isLast)
		{
			arrows.SetSize(0, 0);
			if (serverZentiaPoint < zServerReward.ServerZentiaPoint)
				zentiaColor = GameDefines.zentiaColorBlue;
			arrows.gameObject.transform.localPosition = arrowLocalPosition;
		}
		else
		{
			arrows.SetSize(arrowSize.x, arrowSize.y);
			arrows.gameObject.transform.localPosition = arrowPoint;
		}
			
		
		if (zServerReward.IsRewardGet)
		{
			hasGet.gameObject.SetActive(true);
			exchangeBtn.gameObject.SetActive(false);
		}
		else
		{
			hasGet.gameObject.SetActive(false);
			exchangeBtn.gameObject.SetActive(true);
		}
		List<KodGames.Pair<int, int>> rewardPars = SysLocalDataBase.ConvertIdCountList(zServerReward.Reward);
		if (rewardPars != null && rewardPars.Count > 0)
		{
			icon1.gameObject.SetActive(true);
			icon1.SetData(rewardPars[0].first, rewardPars[0].second);
			icon1.GetComponent<UIButton>().Data = ToAssetTypeRewardComm(zServerReward.Reward)[0];
			if (rewardPars.Count > 1)
			{
				icon2.gameObject.SetActive(true);
				icon2.SetData(rewardPars[1].first, rewardPars[1].second);
				icon2.GetComponent<UIButton>().Data = ToAssetTypeRewardComm(zServerReward.Reward)[1];
			}
			else
				icon2.gameObject.SetActive(false);
		}
		else
		{
			icon1.gameObject.SetActive(false);
			icon2.gameObject.SetActive(false);
		}

		contextLable.Text = GameUtility.FormatUIString("UIPnlEastElementAllServerReward_EastSeaFont", zentiaColor);
		eastSeaNumLable.Text = string.Format("{0}{1}", zentiaColor, zServerReward.ServerZentiaPoint);
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



	public void HasGetReward()
	{
		exchangeBtn.gameObject.SetActive(false);
		hasGet.gameObject.SetActive(true);
	}
}
