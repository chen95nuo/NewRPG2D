using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerBattleDataLayer : MonoBehaviour
{
	public UIBox successBg;
	public UIBox failedBg;
	public SpriteText layerLabel;
	public SpriteText failedLabel;
	public List<UIElemTowerBattleDataInfo> avatarInfos;
	

	public void SetData(int layerNumber, BattleDataAnalyser analyser, bool isWin)
	{
		List<int> avatarIds = analyser.GetAvatarIdxes(0, layerNumber);

		bool isAvatarLife = false;

		//战斗起始层数为玩家所站楼层+1
		layerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerNpcLineUp_TitleLabel_Label"), SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer+layerNumber+1);

		for (int i = 0; i < avatarInfos.Count; i++)
		{
			avatarInfos[i].Hide(true);
		}

		for(int index = 0; index < avatarIds.Count && index < avatarInfos.Count; index++)		
		{
			avatarInfos[index].Hide(false);

			int avatarId = analyser.GetAvatarResourceIDByIndex(avatarIds[index], layerNumber);
			float leftHp = analyser.GetAvatarLeftHP(avatarIds[index], layerNumber);
			double maxHp = analyser.GetAvatarLeftMaxHP(avatarIds[index], layerNumber);

			if (leftHp > 0)
				isAvatarLife = true;

			avatarInfos[index].SetData(avatarId, leftHp, maxHp);			
		}

		if (isWin)
		{
			failedBg.Hide(true);
			successBg.Hide(false);			

			failedLabel.Text = string.Empty;
		}
		else
		{
			failedBg.Hide(false);
			successBg.Hide(true);

			if (isAvatarLife)
				failedLabel.Text = GameUtility.GetUIString("UIPnlTowerBattleResult_FailedResult2_Label");
			else failedLabel.Text = GameUtility.GetUIString("UIPnlTowerBattleResult_FailedResult1_Label");
		}
	}
}
