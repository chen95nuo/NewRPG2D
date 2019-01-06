using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemConvertMainRoot : MonoBehaviour
{
	public GameObject numberGroup;
	public SpriteText groupLevel;
	public UIButton groupJihuo;
	public UIBox groupWeijihuo;
	public SpriteText groupShuzi;

	private int showType;
	public int Showtype
	{
		get { return showType; }
	}

	public void SetData(int position, int number)
	{
		//获取开启等级
		int positionX = 0;
		for (int index = 0; index < ConfigDatabase.DefaultCfg.SevenElevenGiftConfig.NumberPositions.Count; index++)
		{
			if (ConfigDatabase.DefaultCfg.SevenElevenGiftConfig.NumberPositions[index].NumberPositionType == position)
				positionX = ConfigDatabase.DefaultCfg.SevenElevenGiftConfig.NumberPositions[index].NeedLevel;
		}

		groupLevel.Text = GameUtility.FormatUIString("UIDlgConvetExplain_Main_Level", positionX);

		//计算显示控制
		showType = number;
		if (number == -1)
		{
			if (positionX <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
				showType = 1;
			else
				showType = 0;
		}
		if (number > -1)
			showType = 2;

		//隐藏界面内容
		HideData();

		switch (showType)
		{
			case 0:
				groupWeijihuo.Hide(false);
				break;
			case 1:
				groupJihuo.Hide(false);
				groupJihuo.Data = position;
				break;
			case 2:
				groupShuzi.Text = number.ToString();
				break;
		}
	}

	public void HideData()
	{

		groupJihuo.Hide(true);
		groupWeijihuo.Hide(true);
		groupShuzi.Text = string.Empty;
	}

	public UIElemConvertMainRoot GetSelf()
	{
		return this;
	}
}
