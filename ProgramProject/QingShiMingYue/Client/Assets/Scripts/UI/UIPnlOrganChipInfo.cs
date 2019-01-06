using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIPnlOrganChipInfo : UIPnlItemInfoBase
{	
	public SpriteText chipName;
	public UIElemAssetIcon chipIcon;	
	public SpriteText chipExtraDesc;		

	public GameObjectPool chipItemPool;
	public UIScrollList chipsList;
	private int chipId;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		chipId = (int)userDatas[0];
		
		// Set chip info.
		chipIcon.SetData(chipId);
		chipName.Text = ItemInfoUtility.GetAssetName(chipId);
		chipExtraDesc.Text = ItemInfoUtility.GetAssetDesc(chipId);

		FillData();

		return true;
	}

	public void FillData()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	public void ClearData()
	{					
		StopCoroutine("FillList");
		chipsList.ClearList(false);
		chipsList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		BeastConfig.BeastPart beastChip = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartByBeastPartId(chipId);

		foreach (var getway in beastChip.GetWays)
		{
			UIElemBeastGotoItem item = chipItemPool.AllocateItem().GetComponent<UIElemBeastGotoItem>();
			item.SetData(getway);
			chipsList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		ClientServerCommon.GetWay getway = (ClientServerCommon.GetWay)btn.data;
		if (getway.type != _UIType.UI_ActivityDungeon && getway.type != _UIType.UI_Dungeon)
		{
			if (!SysUIEnv.Instance.IsUIModuleShown(getway.type))
				GameUtility.JumpUIPanel(getway.type);
			else HideSelf();
		}			
		else
		{
			GameUtility.JumpUIPanel(getway.type, getway.data);
		}		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
		Debug.Log("OnClickMenuBot  " + "UIPnlchipInfo");
	}
}