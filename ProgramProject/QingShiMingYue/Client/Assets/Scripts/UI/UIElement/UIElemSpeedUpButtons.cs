using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemSpeedUpButtons : MonoBehaviour
{
	public List<UIButton> speedButtons;
	private GameState_Battle battleState = null;
	public List<float> speeds;

    private int index = 0;

	public void Awake()
	{
		index = 0;
		ShowSpeedUpButton();
	}
	
	public void Start()
	{
		battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		// Boss battle can not set speed
		if (battleState.BattleResultData != null && battleState.BattleResultData.BattleResultUIType == _UIType.UIPnlWorldBossBattleChallengeResult)
		{
			speedButtons.Clear();
			speeds.Clear();
			index = 0;
		}
		else
		{
			if (SysLocalDataBase.Inst != null)
			{
				if (battleState != null)
				{
					float speed = SysLocalDataBase.Inst.GetBattleSpeed();

					for (int i = 0; i < speeds.Count; i++)
					{
						if (speeds[i].CompareTo(speed) == 0)
						{
							index = i;
						}
					}
				}

				if (!GameUtility.CheckFuncOpenedByPlayerLevel(_OpenFunctionType.BattleSpeedUpX1, false, true) && 
				    !GameUtility.CheckFuncOpenedByVIPLevel(_OpenFunctionType.BattleSpeedUpX1, false, true))
				{
					speedButtons.RemoveRange(1, speedButtons.Count-1);
					speeds.RemoveRange(1, speeds.Count-1);
					index = 0;
				}
				else if (!GameUtility.CheckFuncOpenedByPlayerLevel(_OpenFunctionType.BattleSpeedUpX2, false, true) && 
				         !GameUtility.CheckFuncOpenedByVIPLevel(_OpenFunctionType.BattleSpeedUpX2, false, true))
				{
					speedButtons.RemoveAt(speedButtons.Count - 1);
					speeds.RemoveAt(speeds.Count - 1);
					index = Mathf.Min(1, index);
				}
			}
		}
		//UpdateUI
		ShowSpeedUpButton();
	}

    public void Show()
    {
		ShowSpeedUpButton();
    }

	private void ShowSpeedUpButton()
	{
		for (int i = 0; i < speedButtons.Count; i++ )
		{
			if (i == index)
			{
				speedButtons[i].Hide(false);
			}
			else
			{
				speedButtons[i].Hide(true);
			}
		}
	}

	private void SetSpeed()
	{
		SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
		if (sysFx != null)
		{
			sysFx.ScaleTime(speeds[index], float.MaxValue);
			if (SysLocalDataBase.Inst != null && battleState != null)
			{
				SysLocalDataBase.Inst.SetBattleSpeed(speeds[index]);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
    private void OnClickSpeedUpButton(UIButton btn)
    {
		index++;

		if (SysLocalDataBase.Inst != null && index == speeds.Count)
		{
			if (!GameUtility.CheckFuncOpenedByPlayerLevel(_OpenFunctionType.BattleSpeedUpX1, false, true) && 
			    !GameUtility.CheckFuncOpenedByVIPLevel(_OpenFunctionType.BattleSpeedUpX1, false, true))
			{
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIBattleSpeedUpButtonTip1"));
			}
			else if (!GameUtility.CheckFuncOpenedByPlayerLevel(_OpenFunctionType.BattleSpeedUpX2, false, true) && 
			         !GameUtility.CheckFuncOpenedByVIPLevel(_OpenFunctionType.BattleSpeedUpX2, false, true))
			{
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIBattleSpeedUpButtonTip2"));
			}
		}

		index = index % speeds.Count;
        ShowSpeedUpButton();
		SetSpeed();
	}

    public void Hide()
    {
		foreach (UIButton button in speedButtons)
		{
			button.Hide(true);
		}
    }
}
