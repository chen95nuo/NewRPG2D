using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideTeamPos : MonoBehaviour 
{
	public List<GameObject> posCtrlList; 

	public void showPosCtrl(int pos,string labelStr)
	{
		posCtrlList[pos].SetActive(true);
		UILabel strLabel = posCtrlList[pos].transform.FindChild("Label").gameObject.GetComponent<UILabel>();
		strLabel.text = labelStr;
	}
	
	public void hide()
	{
		for(int i = 0;i < posCtrlList.Count;++i)
		{
			posCtrlList[i].SetActive(false);
		}
	}
	
	public void onClickTeamPos(int param)
	{
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO);
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		cardInfo.curCardBoxId = param;
		cardInfo.show();
		UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
		this.hide();
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
		{
			GuideUI_CardInTeam.mInstance.showStep(2);
			
		}
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
		{
			GuideUI_CardInTeam2.mInstance.showStep(2);

		}
		else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip))
		{
			// todo
			GuideUI12_Equip.mInstance.showStep(4);
		}
		else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
		{
			GuideUI18_Skill.mInstance.showStep(7);
		}
		
	}
	
	
	
}
