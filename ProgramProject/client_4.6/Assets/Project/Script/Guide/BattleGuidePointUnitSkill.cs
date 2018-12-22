using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// in battle point unit skill //
public class BattleGuidePointUnitSkill: BWUIPanel
{
	public static BattleGuidePointUnitSkill mInstance = null;
	public int step = 0;
	
	public List<GameObject> stepObjList = new List<GameObject>();
	
	public UILabel label0;
	public UILabel label1;
	
	public List<GameObject> cardPosList = new List<GameObject>();
	
	public List<bool> btnClickList = new List<bool>();
	
	private bool isPointed = false;
	
	public GameObject boxColliderObj;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = gameObject;
		init();
		isPointed = false;
	}
	
	// Use this for initialization
	void Start ()
	{
		close();
	}
	
	// Update is called once per frame

	
	public override void init()
	{
		base.init();
		label0.text = TextsData.getData(182).chinese;
		label1.text = TextsData.getData(249).chinese;

		for(int i = 0 ; i < 2; ++i)
		{
			btnClickList.Add(false);
		}
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			boxColliderObj.SetActive(true);
		}
		else
		{
			boxColliderObj.SetActive(false);
		}
	}
	
	public override void show()
	{
		base.show();
		hideAllStep();
	}
	
	public override void hide()
	{
		base.hide();
		hideAllStep();
		//BattleGameHelperCard.mInstance.hideCard();
	}
	
	public void showStep(int step)
	{
		
		show();
		stepObjList[step].SetActive(true);
		//BattleGameHelperCard.mInstance.showCard();
		//GameObjectUtil.copyTarnsformValue(cardPosList[step],BattleGameHelperCard.mInstance.gameObject);
		isPointed = true;
	}
	
	public void hideAllStep()
	{
		for(int i = 0; i < stepObjList.Count;++i)
		{
			stepObjList[i].SetActive(false);
		}
		//BattleGameHelperCard.mInstance.hideCard();
	}
	
	public void onClickUnitSkill()
	{
		PVESceneControl.mInstance.uniteManager.UniteSkillBtn01();
		PVESceneControl.mInstance.guidePause = false;
		isPointed = true;
		hide();
		ViewControl.mInstacne.startWaitNormalCamareMove();
	}
	
	public void onClickBounesPanel()
	{
		hide();
		BattleBounesPanel.mInstance.runBonuce();
		BattleBounesPanel.mInstance.showClickBounceEffect(Screen.width/2,Screen.height/2);
	}
	
	public void gc()
	{
		GameObject.Destroy(_MyObj);
		mInstance = null;
		_MyObj = null;
	}
	
	public bool getIsPointed()
	{
		return isPointed;
	}
}
