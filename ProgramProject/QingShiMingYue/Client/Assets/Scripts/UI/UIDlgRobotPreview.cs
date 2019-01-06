using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgRobotPreview : UIModule
{
	public List<UIElemAssetIcon> foundIcon;
	public List<UIElemAssetIcon> laftIcon;
	public List<SpriteText> foundIconName;
	public List<SpriteText> laftIconName;
	public UIChildLayoutControl foundLayout;
	public UIChildLayoutControl laftLayout;

	private const int MAXINDEX = 3;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		SetData(userDatas[0] as List<KodGames.ClientClass.Avatar>);
		return true;
	}

	public  void SetData(List<KodGames.ClientClass.Avatar> avatars)
	{
		for (int index = 0; index < MAXINDEX; index++)
		{
			if (index < avatars.Count)
			{
				foundIcon[index].Hide(false);
				foundIcon[index].SetData(avatars[index].ResourceId);
				foundIconName[index].Hide(false);
				foundIconName[index].Text = ItemInfoUtility.GetAssetName(avatars[index].ResourceId);
			}
			else
			{
				foundIcon[index].Hide(true);
				foundIcon[index].SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconBgBtn, null);
				foundIconName[index].Hide(true);
			}
		}
		for (int index = MAXINDEX; index < laftIcon.Count;index++ )
		{
			if (index < avatars.Count)
			{
				laftIcon[index].Hide(false);
				laftIcon[index].SetData(avatars[index].ResourceId);
				laftIconName[index].Hide(false);
				laftIconName[index].Text = ItemInfoUtility.GetAssetName(avatars[index].ResourceId);
			}
			else
			{
				laftIcon[index].Hide(true);
				laftIcon[index].SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconBgBtn, null);
				laftIconName[index].Hide(true);
			}
		}
	}


}
