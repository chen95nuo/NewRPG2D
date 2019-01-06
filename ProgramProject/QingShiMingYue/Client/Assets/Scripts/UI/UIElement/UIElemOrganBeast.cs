using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganBeast : MonoBehaviour
{
	public UIElemAssetIcon iconBtn;
	public UIBox beastType;
	public SpriteText beastNameLabel;
	public SpriteText powerLabel;
	public SpriteText beastQualityLabel;
	public SpriteText defineLabel;

	public UIElemAssetBeastEquips beastEquips;
	public UIButton explainBtn;
	public UIButton changeBtn;
	public UIButton ClickBtn;

	private KodGames.ClientClass.Beast beast;
	public KodGames.ClientClass.Beast Beast { get { return beast; } }

	public void SetData(KodGames.ClientClass.Beast beast)
	{
		this.beast = beast;
	    iconBtn.SetData(beast);
		iconBtn.border.Data = beast;
		ClickBtn.Data = beast;

		if(changeBtn != null)
			changeBtn.Data = beast;
		
		//设置机关兽装备栏
		if(beastEquips != null)
			beastEquips.SetData(beast);

		var beastInfoCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beast.ResourceId);
		if (beastInfoCfg == null)
		{
			Debug.LogError("OrganCfg Not Found Id " + beast.ResourceId.ToString("X"));
		}

		UIElemTemplate.Inst.SetBeastTraitIcon(beastType, beastInfoCfg.BeastType);
		beastType.Hide(false);				

		defineLabel.Text = "";
		bool isLineUp = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, beast.Guid, beast.ResourceId);
		if (isLineUp)
			defineLabel.Text = GameUtility.GetUIString("UIPnlOrgansBeastTab_LineUp");
		explainBtn.Hide(!isLineUp);
		explainBtn.Data = beast;

		powerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Quality_BattleNumber"), PlayerDataUtility.CalculateBeastPower(beast).ToString());
		beastNameLabel.Text = beastInfoCfg.BeastName;

		BeastConfig.BeastLevelUp beastLevelCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(beast.LevelAttrib.Level);

		string message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Quality_Label_NoAdd"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevelCfg.Quality));
		
		if (beastLevelCfg.AddLevel > 0)
			message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastLevel_Quality_Show"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevelCfg.Quality), beastLevelCfg.AddLevel);
		
		beastQualityLabel.Text = message;
	}
}

