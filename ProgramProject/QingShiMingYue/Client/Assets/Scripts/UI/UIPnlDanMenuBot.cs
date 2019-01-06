using UnityEngine;
using ClientServerCommon;

// TODO : 处理不透明底板
public class UIPnlDanMenuBot : UIModule
{
	public UIScrollList menuList;
	private bool isDanGet = false;
	public bool IsDanGet
	{
		get { return isDanGet; }
		set { isDanGet = value; }
	}

	private int[] MenuItem = new int[]
	{
	    _UIType.UIPnlDanFurnace,
		_UIType.UIPnlDanAttic,             //灵丹阁
	    _UIType.UIPnlDanMaterial,       //内丹材料   
	    _UIType.UIPnlDanDecompose,  //内丹分解
	};

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (menuList.sceneItems.Length <= 0)
			return true;

		for (int i = 0; i < menuList.sceneItems.Length && i < MenuItem.Length; i++)
		{
			menuList.sceneItems[i].GetComponent<UIElemMainMenuBotItem>().LnkUI = MenuItem[i];
		}

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnIndexMenuClick(UIButton btn)
	{
		ShowMainScene();
	}

	public void ShowMainScene()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnace)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanFurnace));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnaceActivity)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanFurnaceActivity));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanAttic)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanAttic));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanDecompose)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanDecompose));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanDecomposeActivity)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanDecomposeActivity));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanMaterial)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanMaterial));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanInfo));

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlDanMain);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMenuButtonClick(UIButton btn)
	{
		if (SysUIEnv.Instance.IsUIModuleShown((int)btn.data))
			return;

		if ((int)btn.data == _UIType.UIPnlDanFurnace)
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlDanFurnaceActivity))
				return;

		if ((int)btn.data == _UIType.UIPnlDanDecompose)
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlDanDecomposeActivity))
				return;

		GameUtility.JumpUIPanel((int)btn.data);
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

	public Vector3 GetSceneItemPosition(int uitype)
	{
		if (menuList.sceneItems.Length <= 0)
			return new Vector3(0f, 0f, 0f);

		for (int i = 0; i < menuList.sceneItems.Length; i++)
		{
			UIElemMainMenuBotItem botbtn = menuList.sceneItems[i].GetComponent<UIElemMainMenuBotItem>();
			if (botbtn == null)
				continue;

			if (botbtn.LnkUI == uitype)			
				return new Vector3(menuList.sceneItems[i].gameObject.transform.position.x, menuList.sceneItems[i].gameObject.transform.position.y, -0.01f);							
		}

		return new Vector3(0f,0f,0f);
	}
}
