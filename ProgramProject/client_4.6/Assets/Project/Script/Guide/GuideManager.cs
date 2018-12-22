using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuideManager
{
	private static GuideManager instance = null;
	
	public int curRunningID = -1;
	public int lastFinishGuideID = -1;
	bool isRunning = false;
	
	public int finishGiveEquipMissionID = 110203;
	
	public bool isMazeBoss = false;
	
	public bool isMazeFail = false;
	
	public bool onlyShowLotCardPointer = false;
	
	public bool hasUseHP = false;
	
	public enum GuideType : int
	{
		E_BeginNull = -1, 
		E_Demo = 1, 													//演示 //
		E_GetCard = 2,												//抽卡 //
		E_CardInTeam = 3,											//孙悟空上阵//
		E_UseCombo3 = 4,											//使用三连强袭//
		E_Battle1_UnitSkill = 5,									//第1次战斗&合体技 //
		E_Battle2_Bounes = 6,									//第2次战斗&合体技&BONUS //
		E_KO_Exchange = 7,										//KO Exchange//
		E_CardInTeam2 = 8,										//哪吒上阵//
		
		E_IntensifyCard = 9,										//强化卡牌 //

		E_Achievement = 10,										//成就//
		E_GetCard2 = 11,											// get card 2//

		E_ChangePlayerName = 12,							//改名教学 //
		
		E_Battle3_Friend = 13,									//第3次战斗&好友 //
		E_Equip = 14,													//装备教学//
		E_IntensifyEquip = 15,										//装备强化//
		E_UnlockCompose = 16,									 
		E_Compose = 17,											//合成//
		E_WarpSpace = 18,										//扭曲空间//
		E_Skill = 19,														//主动技能教学//
		E_UnlockBreak = 20,										//unlock break//
		E_Break = 21,													//break//
		E_PVP = 22,													//竞技场//
		E_Rune = 23,													//符文//
		E_ActiveCopy = 24,											//活动副本//
		E_Spirit = 25,													//灵界//
		E_EndNull = 99,
	}
	
	public  GuideManager()
	{
		
	}
	
	public static GuideManager getInstance()
	{
		if(instance == null)
		{
			instance = new GuideManager();
		}
		return instance;
	}
	
	public void runGuide()
	{
		isRunning = true;
	}
	
	public void finishGuide(int id)
	{
		isRunning = false;
		curRunningID = ++id;
		if(checkIsCanRunning(curRunningID))
		{
			runGuide();
		}
	}
	
	public bool isRunningGuideID(int id )
	{
		if(isRunning && curRunningID == id )
			return true;
		return false;
	}
	
	// 一些步骤的新手指引要求在当前步骤时需要自动开始强制执行//
	public bool checkIsCanRunning(int id)
	{
		if((id > (int)GuideType.E_BeginNull 
			&& id <= (int)GuideType.E_CardInTeam2) 
			|| (id == (int)GuideType.E_GetCard2)
			//|| (id == (int)GuideType.E_IntensifyCard) 
			|| (id == (int)GuideType.E_IntensifyEquip)
			|| (id == (int)GuideType.E_ChangePlayerName))
		{
			return true;
		}
		else
		{
			if(RequestUnlockManager.mInstance == null)
				return false;
			int sceneID = Application.loadedLevel;
			if(sceneID != 2)
				return false;
			
			switch(curRunningID)
			{
			case (int)GuideManager.GuideType.E_Achievement:
			{
				if(RequestUnlockManager.mInstance.isCanUnlockFunctionByFinishMissionID((int)RequestUnlockManager.MODELID.E_Achievement))
				{
					RequestUnlockManager.mInstance.showUnlockPanel((int)RequestUnlockManager.MODELID.E_Achievement);
					GuideManager.getInstance().runGuide();
				}
			}break;
			case (int)GuideManager.GuideType.E_Equip:
			{
				if(PlayerInfo.getInstance().player.missionId >= GuideManager.getInstance().finishGiveEquipMissionID)
				{
					GuideManager.getInstance().runGuide();
					GuideUI12_Equip.mInstance.needRunStep = 0;
					UISceneDialogPanel.mInstance.showDialogID(27);
				}
			}break;
			}
		}
		return false;
	}
	
	public bool isGuideRunning()
	{
		return isRunning;
	}
	
	public int getCurrentGuideID()
	{
		return curRunningID;
	}
	
	
	
}
