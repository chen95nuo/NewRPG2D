using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfGain : UIModule
{
	public List<UIButton> btns;
	public List<SpriteText> descs;

	public UIChildLayoutControl layoutCoutrol;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		foreach (var desc in descs)
		{
			desc.Text = string.Empty;
		}

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		Show((int)userDatas[0]);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	private void Show(int levelNumber)
	{
		for (int index = 0; index < layoutCoutrol.childLayoutControls.Length; index++)
		{
			layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, true);
		}

		var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageById(levelNumber);
		if (config != null)
		{
			for (int index = 0; index < config.AdditionIds.Count; index++)
			{
				btns[index].Data = config.AdditionIds[index];
				btns[index].Text = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById((int)(btns[index].Data)).EmBattleAttribute.name;

				descs[index].Text = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById((int)(btns[index].Data)).EmBattleAttribute.desc;

				layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, false);
			}
		}
	}


	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickButton(UIButton btn)
	{
		int GainId = (int)btn.Data;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfInfo)))
			SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().JumpToMyBattle(GainId);


		HideSelf();
	}

	#endregion
}
