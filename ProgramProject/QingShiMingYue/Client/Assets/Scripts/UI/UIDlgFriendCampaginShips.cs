using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgFriendCampaginShips : UIModule
{
	public SpriteText topLabel;
	public SpriteText shipsCount;
	public SpriteText friendMaxCount;

	public UIScrollList shipsList;
	public GameObjectPool shipsRootPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		friendMaxCount.Text = GameUtility.FormatUIString("UIDlgFriendCampaginShips_FriendMaxNumber",
								GameDefines.textColorBtnYellow.ToString(),
								GameDefines.textColorWhite.ToString(),
								(int)userDatas[0]);
		shipsCount.Text = GameUtility.FormatUIString("UIDlgFriendCampaginShips_ShipsNumber",
								GameDefines.textColorBtnYellow.ToString(),
								GameDefines.textColorWhite.ToString(),
								(int)userDatas[1]);
		topLabel.Text = userDatas[2] as string;

		StartCoroutine("FillShipsList", userDatas[3] as List<com.kodgames.corgi.protocol.FCPointInfo>);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillShipsList");
		shipsList.ClearList(false);
		shipsList.ScrollListTo(0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillShipsList(List<com.kodgames.corgi.protocol.FCPointInfo> pointInfos)
	{
		yield return null;

		pointInfos.Sort((p1, p2) =>
		{
			int id1 = p1.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId ? 0 : 1;
			int id2 = p2.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId ? 0 : 1;
			if (id1 != id2)
				return id1 - id2;

			if (p2.point != p1.point)
				return p2.point - p1.point;

			return (int)(p1.time - p2.time);
		});

		for (int index = 0; index < pointInfos.Count; index++)
		{
			UIElemFriendCampaginShipsRootItem item = shipsRootPool.AllocateItem(false).GetComponent<UIElemFriendCampaginShipsRootItem>();
			item.SetData(pointInfos[index]);
			shipsList.AddItem(item);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickClose(UIButton btn)
	{
		this.HideSelf();
	}
}
