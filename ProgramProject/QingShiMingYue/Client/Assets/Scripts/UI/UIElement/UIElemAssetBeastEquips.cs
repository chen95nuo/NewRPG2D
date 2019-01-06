using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAssetBeastEquips : MonoBehaviour
{
	public UIElemAssetIcon[] equipIcons;

	public UIBox[] greenLights;
	public UIBox[] orangeLights;
	public UIBox[] mashBoxs;
	public SpriteText[] stateTextLabels;
	public SpriteText[] countLabels;

	//private KodGames.ClientClass.Beast beast;
	//public KodGames.ClientClass.Beast Beast { get { return beast; } }

	public void SetData(KodGames.ClientClass.Beast beast)
	{
		equipIcons[0].Data = BeastConfig._PartIndex.One;
		equipIcons[1].Data = BeastConfig._PartIndex.Two;
		equipIcons[2].Data = BeastConfig._PartIndex.Three;
		equipIcons[3].Data = BeastConfig._PartIndex.Four;
		equipIcons[4].Data = BeastConfig._PartIndex.Five;

		//Init
		for (int i = 0; i < greenLights.Length; i++)
		{
			greenLights[i].Hide(true);
		}

		for (int i = 0; i < orangeLights.Length; i++)
		{
			orangeLights[i].Hide(true);
		}

		for (int i = 0; i < countLabels.Length; i++)
		{
			countLabels[i].Text = "";
		}

		for (int i = 0; i < stateTextLabels.Length; i++)
		{
			stateTextLabels[i].Text = "";
		}	

		var beastBreakAndLevelCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beast.ResourceId, beast.BreakthoughtLevel, beast.LevelAttrib.Level);
		
		for (int i = 0; i < beastBreakAndLevelCfg.BeastPartActives.Count && i < equipIcons.Length; i++)
		{
			equipIcons[i].SetData(beastBreakAndLevelCfg.BeastPartActives[i].PartCost.id);		
			//equipIcons[i].border.SetColor(GameDefines.cardColorGray);
			mashBoxs[i].Hide(false);

			if(beast.PartIndexs.Count > equipIcons.Length)
				Debug.LogError("The Part Count Or EquipIcons Count Is Error");

			bool flag = false;

			for (int j = 0; j < beast.PartIndexs.Count; j ++)
			{
				if ((int)equipIcons[i].Data == beast.PartIndexs[j])				
				{
					//equipIcons[i].border.SetColor(GameDefines.colorWhite);
					mashBoxs[i].Hide(true);
					flag = true;
					break;
				}								
			}

			//如果零件没有被装备
			if(!flag)
			{
				int playerHaveCount = 0;
				var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(beastBreakAndLevelCfg.BeastPartActives[i].PartCost.id);
				if(consumable != null)
					playerHaveCount = consumable.Amount;
				
				int needCount = beastBreakAndLevelCfg.BeastPartActives[i].PartCost.count;

				if (i < countLabels.Length)
					countLabels[i].Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Beasts_EquipCount"), playerHaveCount, needCount);

				if (playerHaveCount > needCount)
				{
					if (i < stateTextLabels.Length)
						stateTextLabels[i].Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Beasts_EquipState"), GameDefines.txColorGreen);
				
					greenLights[i].Hide(false);
					orangeLights[i].Hide(true);
				}
				else
				{
					List<GetWay> getWays = ConfigDatabase.DefaultCfg.BeastConfig.BeastParts[0].GetWays;
					//List<GetWay> getWays = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartByBeastPartId(beastBreakAndLevelCfg.BeastPartActives[i].PartCost.id).GetWays;

					bool isGetWay = false;

					foreach (var getway in getWays)
					{
						if (getway.type == _UIType.UI_ActivityDungeon || getway.type == _UIType.UI_Dungeon)
						{
							string errorMsg = string.Empty;
							if (getway.data != 0)
								errorMsg = CampaignData.CheckDungeonEnterErrorMsg(getway.data, true);

							if (string.IsNullOrEmpty(errorMsg))
							{
								isGetWay = true;
								break;
							}
						}
					}

					if (isGetWay)
					{
						if (i < stateTextLabels.Length)
							stateTextLabels[i].Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Beasts_EquipGet"), GameDefines.colorGoldYellow);

						greenLights[i].Hide(true);
						orangeLights[i].Hide(false);
					}		
				}						
			}
		}	
	}
}

