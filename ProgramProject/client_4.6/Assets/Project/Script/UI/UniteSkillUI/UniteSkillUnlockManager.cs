using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UniteSkillUnlockManager : MonoBehaviour
{
	public UILabel labelName;
	public UILabel disLabel;
	public UIButtonMessage3 showUniteSkillBtn;
	public static UniteSkillUnlockManager mInstance = null;
	GameObject target;


    public GameObject[] uniteSkillCards;

    public UISprite uniteSkillIcon;
	void Awake()
	{
		mInstance = this;
		Hide();
	}
	
	public void Show()
	{
		Main3dCameraControl.mInstance.SetBool(true);
		gameObject.SetActive(true);
	}	
	
	public void Hide()
	{
		gameObject.SetActive(false);
	}
	
	void Update ()
	{
		
	}
	
	public void OnClickShowUniteSkill(string param)
	{	
		//返回主城界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
		if(main!= null)
		{
			main.setNeedShowUnitSkill(true);
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		target.SetActive(false);
		Hide();
	}
	
	public void OnClickBack(int param)
	{
		Hide();
	}

    public void SetDataAndShow(string ids, GameObject target)
    {
        if (ids.Equals(""))
        {
            return;
        }
        string[] ss = ids.Split('&');
        UnitSkillData usd = UnitSkillData.getData(StringUtil.getInt(ss[ss.Length - 1]));
        //disLabel.text = usd.name+TextsData.getData(623).chinese;
		if(usd.number>100)
		{
			return;
		}
		if(PlayerInfo.getInstance().player.level<=3)
		{
			return;
		}
        uniteSkillIcon.spriteName = usd.icon;
        List<int> cardIdList = UnitSkillData.getUniteSkillAllNeedCardId(StringUtil.getInt(ss[ss.Length - 1]));
        disLabel.text = usd.description;
        labelName.text = usd.name;
        print(cardIdList.Count);
        print(uniteSkillCards.Length);
        for (int i = 0; i < cardIdList.Count; i++)
        {
            int cardId = cardIdList[i];
            try
            {

                if (cardId == 0)
                {
                    uniteSkillCards[i].SetActive(false);
                }
                else
                {
                    uniteSkillCards[i].SetActive(true);
                    uniteSkillCards[i].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(cardId, GameHelper.E_CardType.E_Hero);
                }
            }
            catch (KeyNotFoundException)
            {
                ;
            }
            catch (IndexOutOfRangeException)
            {
                ;
            }
        }
        showUniteSkillBtn.stringParam = ids;
        showUniteSkillBtn.target = gameObject;
        showUniteSkillBtn.functionName = "OnClickShowUniteSkill";
        if (target == null)
        {
            this.target = gameObject;
        }
        else
        {
            this.target = target;
        }
        Show();
    }
}
