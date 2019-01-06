using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIElemDungeonStarRewardItem : MonoBehaviour
{
	public AutoSpriteControlBase starRewardIcon;
	public AutoSpriteControlBase starIcon;
	public AutoSpriteControlBase complementIcon;
	public GameObject particleRoot;
	public Vector3 particlePos;

	private int zoneId;
	public int ZoneId { get { return zoneId; } }
	private int starRewardIndex;
	public int StarRewardIndex { get { return starRewardIndex; } }

	public void Init(int zoneId, int starRewardIndex)
	{
		this.zoneId = zoneId;
		this.starRewardIndex = starRewardIndex;
		this.starRewardIcon.Data = this;
		this.gameObject.SetActive(false);

		// Assistant Data.
		this.GetComponent<UIElemAssistantBase>().assistantData = starRewardIndex;
	}

	public void SetData(int difficultType, int sumStar, List<int> alreadGetRewardIndexs)
	{
		var diffConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId).GetDungeonDifficultyByDifficulty(difficultType);

		if (starRewardIndex >= diffConfig.starRewardConditions.Count || diffConfig.starRewardConditions[starRewardIndex].starRewardId == IDSeg.InvalidId)
			this.gameObject.SetActive(false);
		else
		{
			this.gameObject.SetActive(true);

			int requireCount = diffConfig.starRewardConditions[starRewardIndex].requireStarCount;
			int currentCount = sumStar >= requireCount ? requireCount : sumStar;
			string color = sumStar >= requireCount ? GameDefines.txColorGreen.ToString() :GameDefines.txColorGray.ToString();

			// Set Particle State.
			particleRoot.SetActive(sumStar >= requireCount && !alreadGetRewardIndexs.Contains(starRewardIndex));

			// Set Label.
			this.starRewardIcon.Text = string.Format("{0}{1}/{2}", color, currentCount, requireCount);

			// Set Complement Icon.
			this.complementIcon.Hide(!alreadGetRewardIndexs.Contains(starRewardIndex));
		}
	}
}