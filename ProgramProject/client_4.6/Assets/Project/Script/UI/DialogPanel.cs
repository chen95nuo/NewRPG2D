using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogPanel : BWUIPanel
{
	public static DialogPanel mInstance;
	
	public enum DIALOGTYPE : int
	{
		E_Null = -1,
		E_Begin = 0,
		E_Win = 1,
		E_Lose = 2,
		E_UnitSkill = 3,
	}
	
	public GameObject bubble = null;

	// left ctrl
	public GameObject leftCardCtrl = null;
	public GameObject leftCardPos = null;
	public UILabel leftCardDialog = null;

	// right ctrl
	public GameObject rightCardCtrl = null;
	public GameObject rightCardPos = null;
	public UILabel rightCardDialog = null;
	
	public Camera NGUICamera;
	
	
	List<int> dialogIDList = new List<int>();
	int dialogIndex = -1;
	GameObject bubbleTargetObj = null;
	
	bool isGuideDialog = false;	
	int dialogID = 0; // use for guide	
	public DIALOGTYPE dialogType = DIALOGTYPE.E_Null;  // 0 - begin, 1 - win, 2 - lose
	
	bool isBattleWinEndDialog = false;
	
	public List<GameObject> bubblePointList;
	
	public int showPointIndex = -1;
	public int showPointCount = 0;
	bool runBubbleAnim = false;
	float waitPointTime = 0.25f;
	float nowTime = 0;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		close();
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(runBubbleAnim)
		{
			if(bubbleTargetObj != null)
			{
				Vector3 worldPos = bubbleTargetObj.transform.position;
				Vector2 screenPos =  Camera.main.WorldToScreenPoint(worldPos);
				Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
				bubble.transform.position = curPos;
			}
			
			if(nowTime >= waitPointTime)
			{
				nowTime = 0 ;
				showPointIndex++;
				if(showPointIndex == bubblePointList.Count)
				{
					showPointIndex = -1;
					hideAllPoint();
				}
				else
				{
					bubblePointList[showPointIndex].SetActive(true);
				}
			}
			else
			{
				nowTime += Time.deltaTime;
			}
		}

		
		
	}
	
	public override void init()
	{
		base.init();
		clear();
	}
	
	public override void show()
	{
		if(dialogIndex == -1)
			return;
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			BattleGuidePointUnitSkill.mInstance.boxColliderObj.SetActive(false);
		}
		
		base.show();
		if(!isGuideDialog)
		{
			PVESceneControl.mInstance.hitNumParent.SetActive(false);
			PVESceneControl.mInstance.UIInterface.SetActive(false);
			PVESceneControl.mInstance.BloodParent.SetActive(false);
				
		}
		showDialog(dialogIndex);
	}
	
	public override void hide()
	{
		base.hide();
		clear();
		gc();
	}
	
	public void clear()
	{
		dialogType = DIALOGTYPE.E_Null;
		bubbleTargetObj = null;
		dialogIndex = -1;
		dialogIDList.Clear();
		bubble.SetActive(false);
		clearLeftCtrl();
		clearRightCtrl();
		isGuideDialog = false;
		dialogID = 0;
		isBattleWinEndDialog = false;
		runBubbleAnim = false;
		showPointIndex = -1;
		nowTime = 0;
	}

	public void clearLeftCtrl()
	{
		leftCardCtrl.SetActive(false);
		GameObjectUtil.destroyGameObjectAllChildrens(leftCardPos);
		leftCardDialog.text = string.Empty;
	}
	
	public void clearRightCtrl()
	{
		rightCardCtrl.SetActive(false);
		GameObjectUtil.destroyGameObjectAllChildrens(rightCardPos);
		rightCardDialog.text = string.Empty;
	}
	
	public void showleftCtrl()
	{
		clearRightCtrl();
		clearLeftCtrl();
		leftCardCtrl.SetActive(true);
		
	}
	
	public void showRightCtrl()
	{
		clearRightCtrl();
		clearLeftCtrl();
		rightCardCtrl.SetActive(true);
		
	}
	
	public void onClickDialogPanel()
	{
		if(!isGuideDialog)
		{
			if(dialogIndex >= (dialogIDList.Count-1))
			{
				// notify show ui , go on battle logic
				switch(dialogType)
				{
				case DIALOGTYPE.E_Begin:
				{
					PVESceneControl.mInstance.finishShowBeginDialog = true;	
					PVESceneControl.mInstance.hitNumParent.SetActive(true);
					PVESceneControl.mInstance.UIInterface.SetActive(true);
					PVESceneControl.mInstance.BloodParent.SetActive(true);
				}break;
				case DIALOGTYPE.E_Win:
				{
					PVESceneControl.mInstance.finishShowWinDialog = true;
					PVESceneControl.mInstance.finishShowLoseDialog = true;
					PVESceneControl.mInstance.hitNumParent.SetActive(true);
					PVESceneControl.mInstance.UIInterface.SetActive(true);
					PVESceneControl.mInstance.BloodParent.SetActive(true);
				}break;
				case DIALOGTYPE.E_Lose:
				{
					PVESceneControl.mInstance.finishShowWinDialog = true;
					PVESceneControl.mInstance.finishShowLoseDialog = true;
					PVESceneControl.mInstance.hitNumParent.SetActive(true);
					PVESceneControl.mInstance.UIInterface.SetActive(true);
					PVESceneControl.mInstance.BloodParent.SetActive(true);
				}break;
				case DIALOGTYPE.E_UnitSkill:
				{
					PVESceneControl.mInstance.finishShowUnitSkillDialog = true;
				}break;
				}
				hide();
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
				{
					BattleGuidePointUnitSkill.mInstance.boxColliderObj.SetActive(true);
				}
			}
			else
			{
				dialogIndex++;
				showDialog(dialogIndex);
			}
		}
		else
		{
			if(dialogIndex >= (dialogIDList.Count-1))
			{
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes) 
					&& dialogID == 5)
				{
					BattleGuidePointUnitSkill.mInstance.showStep(1);
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes) 
					&& dialogID == 6)
				{
					BattleBounesPanel.mInstance.finishBounes();
				}
				hide();
			}
			else
			{
				dialogIndex++;
				showDialog(dialogIndex);
			}
		}
		
	}
	
	public void setDialogIDList(List<int> idList,DIALOGTYPE t,bool b = false)
	{
		dialogIDList = idList;
		if(dialogIDList.Count == 0)
			return;
		dialogIndex = 0;
		dialogType = t;
		isBattleWinEndDialog = b;
	}
	
	public void showDialog(int index)
	{
		TaskData td = TaskData.getData(dialogIDList[index]);
		if(td == null)
		{
			hide();
			return;
		}
		string dialogText = td.acceptWord;
		GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(td.ci_model,0)) as GameObject;
		if(cardModel == null)
		{
			return;
		}
		GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
		Vector3 scale = cardModel.transform.localScale;
		CardEffectControl cec = cardModel.GetComponent<CardEffectControl>();
		if(cec != null)
		{
			cec.hideEffect();	
		}
		bubble.SetActive(false);
		if(isGuideDialog)
		{
			showleftCtrl();
			GameObjectUtil.gameObjectAttachToParent(cardModel,leftCardPos);
			leftCardDialog.text = dialogText;
		}
		else
		{
			if(td.ci_Id == 0)
			{
				showleftCtrl();
				GameObjectUtil.gameObjectAttachToParent(cardModel,leftCardPos);
				leftCardDialog.text = dialogText;
			}
			else
			{
				showRightCtrl();
				GameObjectUtil.gameObjectAttachToParent(cardModel,rightCardPos);
				rightCardDialog.text = dialogText;
				Card targetCard = PVESceneControl.mInstance.players[1].getCard(td.ci_Id -1);
				if(targetCard == null)
				{
					return;
				}
				if(!isBattleWinEndDialog)
				{
					bubble.SetActive(true);
					
					playBubble();
				}
				
				bubbleTargetObj = targetCard.head;
			}
		}
		cardModel.transform.localScale = scale;
	}
	
	public void showGuideDialogID(int id)
	{
		List<int> idList = TaskData.getGuideTaskIDArray(id);
		this.dialogID = id;
		dialogIDList = idList;
		if(dialogIDList.Count == 0)
			return;
		dialogIndex = 0;
		show();
		isGuideDialog = true;
	}
	
	public void playBubble()
	{
		runBubbleAnim = true;
		hideAllPoint();
		showPointIndex = -1;
	}
	
	public void hideAllPoint()
	{
		for(int i = 0;i < bubblePointList.Count;++i)
		{
			bubblePointList[i].SetActive(false);
		}
	}
	
	
	public void gc()
	{
		Resources.UnloadUnusedAssets();
	}
}
