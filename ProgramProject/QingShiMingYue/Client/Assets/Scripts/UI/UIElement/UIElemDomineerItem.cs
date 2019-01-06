using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDomineerItem : MonoBehaviour
{
	public UIElemAssetIcon icon;
	public UIBox domineerTag;
	public SpriteText domineerName;
	public SpriteText LevelText;
	public SpriteText LevelTextChangColor;
	public SpriteText desc;

	public void SetData(int id,int level)
	{
		DomineerConfig.Domineer domineerConfig = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById (id);

		if(domineerTag != null)
		{
			int type = domineerConfig.Type;
			if(type == DomineerConfig._DomineerType.HeZong)
			{
				domineerTag.SetToggleState("Hezong");
			}
			else if(type == DomineerConfig._DomineerType.LianHeng)
			{
				domineerTag.SetToggleState("Lianheng");
			}
		}

		icon.SetData (id,0,level);

		LevelText.Text = GameUtility.FormatUIString("UILevelPrefix", level);
		
		switch (level)
		{

			case 0:
			case 1:
				LevelTextChangColor.Text = GameUtility.FormatUIString("UIDomineerLevel", GameDefines.textColorWhite, level);
				break;
			case 2:
				LevelTextChangColor.Text = GameUtility.FormatUIString("UIDomineerLevel", GameDefines.textColorGreen, level);
				break;
			case 3:
				LevelTextChangColor.Text = GameUtility.FormatUIString("UIDomineerLevel", GameDefines.textColorBlue, level);
				break;
			case 4:
				LevelTextChangColor.Text = GameUtility.FormatUIString("UIDomineerLevel", GameDefines.textColorZiSe, level);
				break;
			case 5:
				LevelTextChangColor.Text = GameUtility.FormatUIString("UIDomineerLevel", GameDefines.textColorOrange, level);
				break;
			default:
				LevelTextChangColor.Text = GameUtility.FormatUIString("UIDomineerLevel", GameDefines.textColorWhite, level);
				break;
		}

		domineerName.Text = ItemInfoUtility.GetAssetName (id);

		desc.Text = domineerConfig.GetDomineerLevelByLevel (level).Desc;
	}
}
