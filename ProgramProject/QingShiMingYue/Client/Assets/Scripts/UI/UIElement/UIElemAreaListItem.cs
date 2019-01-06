using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemAreaListItem : MonoBehaviour
{
	public SpriteText areaNameLabel;
	public SpriteText areaNumLabel;
	public SpriteText areaStatusLabel;

	public UIButton areaBtn;

	public UIBox rencentSign;
	public UIBox characterSign;

	public static readonly Color NEW_AREA_COLOR = new Color(0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
	public static readonly Color HOT_AREA_COLOR = new Color(255f / 255f, 46f / 255f, 46f / 255f, 255f / 255f);
	public static readonly Color BUSY_AREA_COLOR = new Color(255f / 255f, 209f / 255f, 0f / 255f, 255f / 255f);
	public static readonly Color SMOOTH_AREA_COLOR = new Color(192f / 255f, 255f / 255f, 38f / 255f, 255f / 255f);
	public static readonly Color MAINTAIN_AREA_COLOR = new Color(167f / 255f, 167f / 255f, 167f / 255f, 255f / 255f);
	public static readonly Color UNKNOWN_AREA_COLOR = Color.gray;

	public static Color GetColorByStatus(int areaStatus)
	{
		switch (areaStatus)
		{
			case _ServerAreaStatus.New:
				return NEW_AREA_COLOR;

			case _ServerAreaStatus.Busy:
				return BUSY_AREA_COLOR;

			case _ServerAreaStatus.Hot:
				return HOT_AREA_COLOR;

			case _ServerAreaStatus.Maintain:
				return MAINTAIN_AREA_COLOR;

			case _ServerAreaStatus.Smooth:
				return SMOOTH_AREA_COLOR;

			default:
				return UNKNOWN_AREA_COLOR;
		}
	}

	private string GetState(int state)
	{
		switch (state)
		{
			case _ServerAreaStatus.New:
				return GameUtility.GetUIString("UIDlgSelectArea_AreaState_New");

			case _ServerAreaStatus.Busy:
				return GameUtility.GetUIString("UIDlgSelectArea_AreaState_Busy");

			case _ServerAreaStatus.Hot:
				return GameUtility.GetUIString("UIDlgSelectArea_AreaState_Hot");

			case _ServerAreaStatus.Maintain:
				return GameUtility.GetUIString("UIDlgSelectArea_AreaState_Maintain");

			case _ServerAreaStatus.Smooth:
				return GameUtility.GetUIString("UIDlgSelectArea_AreaState_Smooth");

			default:
				return GameUtility.GetUIString("UIDlgSelectArea_AreaState_Unknown");
		}
	}

	private bool GetButtonState(int areaStatus)
	{
		switch (areaStatus)
		{
			case _ServerAreaStatus.New:
			case _ServerAreaStatus.Busy:
			case _ServerAreaStatus.Hot:
			case _ServerAreaStatus.Smooth:
				return true;

			case _ServerAreaStatus.Maintain:
			default:
				return false;
		}
	}

	public void SetData(KodGames.ClientClass.Area area, MonoBehaviour script, string method)
	{
		areaBtn.controlIsEnabled = GetButtonState(area.AreaStatus);
		if (areaBtn.controlIsEnabled)
		{
			areaBtn.scriptWithMethodToInvoke = script;
			areaBtn.methodToInvoke = method;
			areaBtn.Data = area;
		}

		Color labelColor = GetColorByStatus(area.AreaStatus);

		areaStatusLabel.Text = labelColor.ToString() + GetState(area.AreaStatus);
		areaNameLabel.Text = labelColor.ToString() + area.AreaName;
		areaNumLabel.Text = labelColor.ToString() + GameUtility.FormatUIString("UIDlgAccontBinding_Label_Area", area.ShowAreaId);

		if (area.AreaID == SysLocalDataBase.Inst.LoginInfo.LastAreaId)
			rencentSign.Hide(false);
		else
			rencentSign.Hide(true);

		if (area.Nun > 0)
			characterSign.Hide(false);
		else
			characterSign.Hide(true);
	}
}