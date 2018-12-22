using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollViewItem : MonoBehaviour
{
	public int index = -1; /*合体己能寸的是表ID，其他类型存放各个类型在数据库中的Index */
	public UIAtlas atlas = null;
	public int mark = 0;  /*1使用中,2可使用,3可激活,4不可激活 */
	public int useCardHeroIndex = -1;  /*使用该卡牌的所在当前卡组中的位置Index*/
	public bool useCurPassiveSkill = false;
	public bool isBaseSkill = false;
	public int baseSkillID = -1;
	public bool isCanUnEquiped = false;

	public GameObject obj = null;
	public GameObject warnningText = null;
	public GameObject useBtn = null;
    public GameObject WayBtn = null;
	public GameObject intensifyBtn = null;
	public UIButtonMessage ibm = null;
	public UIButtonMessage bm = null;
	public GameObject selectObj = null;
	public UISprite icon = null;
	public UISprite bg = null;
	public UISprite race = null;
	public UILabel nameText = null;
	public UILabel descText = null; 
	public UILabel usedText = null;
	public UILabel btnText = null;
	public UILabel levelText = null;
	public SimpleCardInfo2 cardInfo = null;
	
	
	
	void OnClick()
	{
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW);
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.clickScrollViewItem(index);
	}
}


