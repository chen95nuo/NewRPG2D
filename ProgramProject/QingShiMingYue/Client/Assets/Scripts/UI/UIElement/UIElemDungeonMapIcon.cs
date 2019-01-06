using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIElemDungeonMapIcon : MonoBehaviour
{
	public string mapTag;
	public UIElemAssetIcon mapIcon;
	public UIElemQualityBtn mapAppraiseIcon;
	public AutoSpriteControlBase mapDiffcult;
	public GameObject mapAnimation;
	public CampaignConfig.Dungeon dungeon;

	private int indexInMap;
	public int IndexInMap { get { return indexInMap; } }

	public void Init(int indexInMap)
	{
		mapIcon.Data = this;
		this.indexInMap = indexInMap;
	}
}