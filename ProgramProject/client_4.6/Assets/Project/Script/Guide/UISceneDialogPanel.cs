using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISceneDialogPanel : BWUIPanel
{
	public static UISceneDialogPanel mInstance;
	
	// left ctrl
	public GameObject leftCardCtrl = null;
	public GameObject leftCardPos = null;
	public UILabel leftCardDialog = null;

	// right ctrl
	public GameObject rightCardCtrl = null;
	public GameObject rightCardPos = null;
	public UILabel rightCardDialog = null;
	

	List<int> dialogIDList = new List<int>();
	int dialogID = 0;
	int dialogIndex = -1;
	
	int needShowDialogID = -1;

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
		base.show();
		showDialog(dialogIndex);
	}
	
	public override void hide()
	{
		base.hide();
		clear();
		gc();
	}
	
	private void gc()
	{
		Resources.UnloadUnusedAssets();
	}
	
	public void clear()
	{
		dialogID = -1;
		dialogIndex = -1;
		dialogIDList.Clear();
		clearLeftCtrl();
		clearRightCtrl();
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
		if(dialogIndex >= (dialogIDList.Count-1))
		{
			
	
			if(GuideManager.getInstance().isGuideRunning())
			{
				int curGuideID = GuideManager.getInstance().getCurrentGuideID();
				switch(curGuideID)
				{
				case (int)GuideManager.GuideType.E_GetCard:
				{
					if(dialogID == 2)
					{
						GuideUI_GetCard.mInstance.showStep(0);
					}
					else if(dialogID == 3)
					{
						GuideUI_GetCard.mInstance.showStep(2);
					}
				}break;
				case (int)GuideManager.GuideType.E_CardInTeam:
				{
					if(dialogID == 28)
					{
						GuideUI_CardInTeam.mInstance.showStep(0);
					}

				}break;
				case (int)GuideManager.GuideType.E_UseCombo3:
				{
					if(dialogID == 37)
					{
						GuideFlashWnd.mInstance.showFlashWnd((int)GuideManager.GuideType.E_UseCombo3,-1,53,GuideFlashWnd.FlashWndType.E_UseCombo3);
					}
					else if(dialogID == 53)
					{
						GuideUI_UseCombo3.mInstance.showStep(1);
					}
				}break;
				case (int)GuideManager.GuideType.E_Battle1_UnitSkill:
				{
				
				}break;
				case (int)GuideManager.GuideType.E_IntensifyCard:
				{
					if(dialogID == 4)
					{
						GuideUI_Intesnify.mInstance.showStep(1);
					}
					else if(dialogID == 10)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_IntensifyCard);
						//GuideUI_Bounes.mInstance.showStep(0);
					}
				}break;
				case (int)GuideManager.GuideType.E_Battle2_Bounes:
				{
					if(dialogID == 38)
					{
						GuideFlashWnd.mInstance.showFlashWnd((int)GuideManager.GuideType.E_Battle2_Bounes,-1,39,GuideFlashWnd.FlashWndType.E_ShowKOTips);
					}
					else if(dialogID == 39)
					{
						GuideUI_Bounes.mInstance.showStep(2);
					}
				}break;
				case (int)GuideManager.GuideType.E_KO_Exchange:
				{
					if(dialogID == 40)
					{
						GuideUI7_KOExchange.mInstance.showStep(2);
					}
					else if(dialogID == 41)
					{
						GuideUI7_KOExchange.mInstance.showStep(3);
					}
					else if(dialogID == 42)
					{
						GuideUI7_KOExchange.mInstance.showStep(4);
					}

				}break;
				case (int)GuideManager.GuideType.E_CardInTeam2:
				{
					if(dialogID == 43)
					{
						GuideUI_CardInTeam2.mInstance.showStep(7);
					}
					else if(dialogID == 51)
					{
						GuideUI_CardInTeam2.mInstance.showStep(6);
					}
					else if(dialogID == 52)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_CardInTeam2);
					}
				}break;
				case (int)GuideManager.GuideType.E_ChangePlayerName:
				{
					if(dialogID == 46)
					{
						GuideUI_ChangePlayerName.mInstance.showStep(0);
					}
					else if(dialogID == 48)
					{
						GuideUI_ChangePlayerName.mInstance.hide();
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_ChangePlayerName);
						//GuideUI_ChangePlayerName.mInstance.showStep(3);
					}
					
				}break;
				case (int)GuideManager.GuideType.E_Battle3_Friend:
				{
					if(dialogID == 8)
					{
						GuideUI_Friend.mInstance.showStep(0);
					}
				}break;
				case (int)GuideManager.GuideType.E_Achievement:
				{
					if(dialogID == 25)
					{
						GuideUI8_Achievement.mInstance.showStep(0);
					}
					else if(dialogID == 26)
					{
						GuideUI8_Achievement.mInstance.showStep(1);
					}
				}break;
				case (int)GuideManager.GuideType.E_GetCard2:
				{
					if(dialogID == 31)
					{
						GuideUI9_GetCard2.mInstance.showStep(0);
					}
				}break;
				case (int)GuideManager.GuideType.E_Equip:
				{
					if(dialogID == 27)
					{
						GuideUI12_Equip.mInstance.showStep(GuideUI12_Equip.mInstance.needRunStep);
					}
				}break;
				case (int)GuideManager.GuideType.E_IntensifyEquip:
				{
					if(dialogID == 44)
					{
						if(GuideUI_IntensifyEquip.mInstance.normalType)
						{
							if(PlayerInfo.getInstance().mainIconShow)
							{
								GuideUI_IntensifyEquip.mInstance.showStep(2);
							}
							else
							{
								GuideNeedOpenBtnList.mInstance.showPanel(curGuideID,2);
							}
						}
						else
						{
							GuideUI_IntensifyEquip.mInstance.showStep(0);
						}
					}
				}break;
				case (int)GuideManager.GuideType.E_Compose:
				{
					if(dialogID == 14)
					{
						GuideUI14_Compose.mInstance.showStep(0);
					}
					else if(dialogID == 30)
					{
						if(PlayerInfo.getInstance().mainIconShow)
						{
							GuideUI14_Compose.mInstance.showStep(1);
						}
						else
						{
							GuideNeedOpenBtnList.mInstance.showPanel(curGuideID,1);
						}
					}
				}break;
				case (int)GuideManager.GuideType.E_ActiveCopy:
				{
					if(dialogID == 22)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_ActiveCopy);
					}
				}break;
				case (int)GuideManager.GuideType.E_WarpSpace:
				{
					if(dialogID == 19)
					{
						GuideUI17_WarpSpace.mInstance.showStep(0);
					}
					else if(dialogID == 20)
					{
						GuideUI17_WarpSpace.mInstance.showStep(3);
					}
					else if(dialogID == 21)
					{
						NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager")as NewMazeUIManager;
						if(maze.scheduleNum == 0 || maze.scheduleNum == 1)
						{
							if(maze.finishOnceMove)
							{
								maze.waitMoveForShowGuidePoint = false;
								if(maze.isCanUseMedicine())
								{
									GuideUI17_WarpSpace.mInstance.showStep(4);
								}
							}
							else
							{
								maze.waitMoveForShowGuidePoint = true;
							}
							
						}
						else
						{
							GuideUI17_WarpSpace.mInstance.hideAllStep();
						}
					}
				}break;
				case (int)GuideManager.GuideType.E_Skill:
				{
					if(dialogID == 33)
					{
						GuideUI18_Skill.mInstance.showStep(0);
					}
					else if(dialogID == 34)
					{
						if(GuideUI18_Skill.mInstance.isOffLine)
						{
							GuideUI18_Skill.mInstance.showStep(5);
						}
						else
						{
							if(GuideManager.getInstance().isMazeFail)
							{
								GuideUI18_Skill.mInstance.showStep(2);
							}
							else
							{
								GuideUI18_Skill.mInstance.showStep(1);
							}
						}
						
						
					}
				}break;
				case (int)GuideManager.GuideType.E_PVP:
				{
					if(dialogID == 15)
					{
						GuideUI19_PVP.mInstance.showStep(0);
					}
					else if(dialogID == 16)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_PVP);
					}
				}break;
				case (int)GuideManager.GuideType.E_Rune:
				{
					if(dialogID == 23)
					{
						if(PlayerInfo.getInstance().mainIconShow)
						{
							GuideUI20_Rune.mInstance.showStep(0);
						}
						else
						{
							GuideNeedOpenBtnList.mInstance.showPanel(curGuideID,0);
						}
					}
					else if(dialogID == 24)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Rune);
					}
				}break;
				case (int)GuideManager.GuideType.E_Break:
				{
					if(dialogID == 11)
					{
						if(PlayerInfo.getInstance().mainIconShow)
						{
							GuideUI22_Break.mInstance.showStep(0);
						}
						else
						{
							GuideNeedOpenBtnList.mInstance.showPanel(curGuideID,0);
						}
					}
					else if(dialogID == 12)
					{
						GuideUI22_Break.mInstance.showStep(1);
                        GuideUI22_Break.mInstance.hideAllStep();
					}
					else if(dialogID == 13)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Break);
					}
				}break;
				case (int)GuideManager.GuideType.E_Spirit:
				{
					if(dialogID == 35)
					{
						if(PlayerInfo.getInstance().mainIconShow)
						{
							GuideUI23_Spirit.mInstance.showStep(0);
						}
						else
						{
							GuideNeedOpenBtnList.mInstance.showPanel(curGuideID,0);
						}
						
					}
					else if(dialogID == 17)
					{
						GuideUI23_Spirit.mInstance.showStep(1);
					}
					else if(dialogID == 18)
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Spirit);
					}
				}break;
				}

			}
			
			hide();
			if(needShowDialogID != -1)
			{
				waitShowDialogID();
			}
			
		}
		else
		{
			dialogIndex++;
			showDialog(dialogIndex);
		}
	}
	
	public void waitShowDialogID()
	{
		showDialogID(needShowDialogID);
		needShowDialogID = -1;
	}
	
	public void showDialogID(int dialogID)
	{
		List<int> idList = TaskData.getGuideTaskIDArray(dialogID);
		this.dialogID = dialogID;
		dialogIDList = idList;
		if(dialogIDList.Count == 0)
			return;
		dialogIndex = 0;
		show();
	}
	
	void showDialog(int index)
	{
		TaskData td = TaskData.getData(dialogIDList[index]);
		if(td == null)
		{
			hide();
			return;
		}
		string dialogText = td.acceptWord;
		
		dialogText = dialogText.Replace("num",PlayerInfo.getInstance().player.name);
		
		GameObject prefab = Resources.Load("Prefabs/Cards/" + td.ci_model) as GameObject;
		GameObject cardModel = GameObject.Instantiate(prefab) as GameObject;
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
		}
        if (td.ci_model.Equals("card045"))
        {
            cardModel.transform.localPosition = new Vector3(cardModel.transform.localPosition.x, -0.5f, cardModel.transform.localPosition.z);
        }
		cardModel.transform.localScale = scale;
	}
	

}
