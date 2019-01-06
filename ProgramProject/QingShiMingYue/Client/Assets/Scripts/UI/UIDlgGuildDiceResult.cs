using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDlgGuildDiceResult : UIModule
{
	public List<UIBox> dices1;
	public List<UIBox> dices2;
	public List<UIBox> dices3;
	public SpriteText resultLabel;
	public SpriteText processState;

	private List<List<UIBox>> dices=new List<List<UIBox>>();

	public override bool Initialize()
	{
		if(base.Initialize()==false)
			return false;

		dices.Add(dices1);

		dices.Add(dices2);

		dices.Add(dices3);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		List<int> results = userDatas[0] as List<int>;

		resultLabel.Text = GameUtility.FormatUIString("UIPnlGuildTask_Result", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, results[0], results[1], results[2]);

		if ((bool)userDatas[1])
			processState.Text = GameUtility.FormatUIString("UIPnlGuildTask_ProcessChange", (string)userDatas[1],GameDefines.textColorBtnYellow);
		else
			processState.Text = GameUtility.FormatUIString("UIPnlGuildTask_ProcessNoChange", (string)userDatas[1], GameDefines.textColorBtnYellow); ;

		SetData(results);

		return true;
	}

	private void SetData(List<int> results)
	{
		for(int index=0;index<results.Count&&index<dices.Count;index++)
		{
			int result = results[index];

			if (result <= 0 || result > 6)
			{
				Debug.LogError("The Dices Result Error!");
				return;
			}

			for (int i = 0; i < dices[index].Count;i++)
				dices[index][i].Hide(!(i==result-1));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}
