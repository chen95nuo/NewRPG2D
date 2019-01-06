using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIPnlGuildMenuBot : UIModule
{
	public UIButton homeBtn;
	public UIScrollList menuList;

	private int[] MenuItem = new int[]
	{
		_UIType.UIPnlGuildIntroMember,
		_UIType.UIPnlGuildMessage,
		_UIType.UIPnlChatTab,
		_UIType.UIPnlGuildGuide,
	};

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (menuList.sceneItems.Length <= 0)
			return true;

		for (int i = 0; i < menuList.sceneItems.Length && i < MenuItem.Length; i++)
			menuList.sceneItems[i].GetComponent<UIElemMainMenuBotItem>().LnkUI = MenuItem[i];

		return true;
	}

	public void ShowMainScene()
	{
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.mainSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
		else
			SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlMainScene));	
	}

	public void SetLight(int uitype)
	{
		if (menuList.sceneItems.Length <= 0)
			return;

		for (int i = 0; i < menuList.sceneItems.Length; i++)
		{
			UIElemMainMenuBotItem botbtn = menuList.sceneItems[i].GetComponent<UIElemMainMenuBotItem>();
			if (botbtn == null)
				continue;

			botbtn.SetSelectedStat(botbtn.LnkUI == uitype);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMenuButtonClick(UIButton btn)
	{
		if ((int)btn.data == _UIType.UIPnlChatTab)
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlChatTab, null, true,true);
		else
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab))
				SysUIEnv.Instance.HideUIModule(_UIType.UIPnlChatTab);

			GameUtility.JumpUIPanel((int)btn.data);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnIndexMenuClick(UIButton btn)
	{
		ShowMainScene();
	}
}
