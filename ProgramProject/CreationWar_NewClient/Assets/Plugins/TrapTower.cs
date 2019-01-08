using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TrapTower : MonoBehaviour 
{
    public static TrapTower instance;

    public UIGrid grid;
    public UILabel description;// 简介
    public UILabel lastTime;// 剩余挑战次数
    public UILabel currentLayer;// 当前所在层数
    public UILabel skipChallengeTxt;
    public UILabel startChallengeTxt;
    public UIButton skipChallengeBtn;
    public UIButton startChallengeBtn;
    public UIButton rewardLeaveBtn;
    public UIButton tempLeaveBtn;
    public UILabel lblGold;
    public UILabel lblExp;
    public GameObject closeBtn;
    public GameObject flashSprite;

    public TrapTowerItem firstRewardItem;

    //private bool isFirst = false;
    void Awake()
    {
        instance = this;
        //isFirst = true;
    }

    private int maxTowerLevel = 30;// 困魔塔最大层数
	// Use this for initialization
	void Start () 
	{
        yuan.YuanMemoryDB.YuanTable bossTower = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBosstower;
        List<string> ids = bossTower.GetColumnText("id");
        maxTowerLevel = ids.Count;
        int layer = 0;
        yuan.YuanMemoryDB.YuanRow yRow = null;
        string item = "";
        
        for(int i=0;i<ids.Count;i++)
        {
            layer = int.Parse(ids[i]);

            yRow = bossTower.SelectRowEqual("id", ids[i]);
            bossTowerInfoDic.Add(layer, yRow);
            item = yRow["itemlist"].YuanColumnText.Trim();
            if(string.IsNullOrEmpty(item))
            {
                continue;
            }
            string[] items = item.Split(';');
            itemList.Add(layer, items);

            if (i == maxTowerLevel - 1)
            {
                //显示首次通关奖励
                string rewardItem = yRow["rewarditem"].YuanColumnText.Trim();
                if (!string.IsNullOrEmpty(rewardItem))
                {
                    firstRewardItem.SetItemInfo(rewardItem);
                    firstRewardItem.DisableItem();
                }
            }
        }


        //if (isFirst)
        //{
        //    //yield return new WaitForSeconds(1.0f);
        //    InitItemGrid();
        //    InitGoldAndExp(currLayer);
        //    isFirst = false;
        //}

        if (!BtnGameManagerBack.isTown())
        {
            closeBtn.SetActive(false);
        }
	}

    void OnEnable()
    {
        //DisableItems();

        SetDescriptionTxt();

        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => ServerRequest.requestTowerOpen());

        //if (!isFirst)
        //{
        //    InitItemGrid();
        //    InitGoldAndExp(currLayer);
        //}

        if (!BtnGameManagerBack.isTown())
        {
            closeBtn.SetActive(false);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
    }

    public void InitBtnState()
    {
        if (!isRequest) return;

        if(BtnGameManagerBack.isTown())
        {
            tempLeaveBtn.isEnabled = false;
        }

        if(!BtnGameManagerBack.isTown() || skipLayers <= 0)
        {
            skipChallengeBtn.isEnabled = false;
        }

        if (gold <= 0)// gold小于0表示无奖励可领取
        {
            rewardLeaveBtn.isEnabled = false;
        }
        else
        {
            rewardLeaveBtn.isEnabled = true;
        }

        if(currLayer >= maxTowerLevel)
        {
            startChallengeBtn.isEnabled = false;
            flashSprite.GetComponent<UISprite>().enabled = false;
        }
    }

    void DisableItems()
    {
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            grid.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private Dictionary<int, yuan.YuanMemoryDB.YuanRow> bossTowerInfoDic = new Dictionary<int, yuan.YuanMemoryDB.YuanRow>();
    private Dictionary<int, string[]> itemList = new Dictionary<int, string[]>();
    private int currLayer = 0;
    private LoopScrollView m_scrollView;
    public void InitItemGrid()
    {
        m_scrollView = grid.transform.GetComponent<LoopScrollView>();
        m_scrollView.Init(true);

        int count = 0;
        string[] tempItems = null;
        //if (itemList.TryGetValue(currLayer, out tempItems))
        //{
        //    count = tempItems.Length;
        //}
        if (itemList.TryGetValue(maxTowerLevel, out tempItems))
        {
            count = tempItems.Length;
        }
        
        m_scrollView.UpdateListItem(count);
        m_scrollView.SetDelegate(UpdateItemInfo, null);

        //yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            UpdateItemInfo(grid.transform.GetChild(i).gameObject);
        }
    }

    void UpdateItemInfo(GameObject go)
    {
        TrapTowerItem item = go.GetComponent<TrapTowerItem>();
        if(item.gameObject.activeSelf)
        {
            int index = int.Parse(item.gameObject.name);

            string[] tempItems = null;
            //if (itemList.TryGetValue(currLayer, out tempItems))
            //{
            //    item.SetItemInfo(tempItems[index]);
            //}
            if (itemList.TryGetValue(maxTowerLevel, out tempItems))
            {
                item.SetItemInfo(tempItems[index]);

                if (index >= currLayer)
                {
                    item.DisableItem();
                }
                else
                {
                    item.EnableItem();
                }
            }

            if (currLayer >= maxTowerLevel)
            {
                firstRewardItem.EnableItem();
            }
        }
    }

    private int gold = 0;
    void InitGoldAndExp(int layer)
    {
        yuan.YuanMemoryDB.YuanRow yr = null;
        if (bossTowerInfoDic.TryGetValue(layer, out yr))
        {
            gold = int.Parse(yr["money"].YuanColumnText.Trim());
            SetGoldNum(yr["money"].YuanColumnText);
            SetExpNum(yr["exp"].YuanColumnText);
        }
        else
        {
            SetGoldNum("0");
            SetExpNum("0");
        }
    }

    private int towerLevel = 0;
    private int towerState = 0;
    public void SetTowerLvlAndState(int level, int state)
    {
        towerLevel = level;
        towerState = state;
    }

    public void SetTowerState(int state)
    {
        towerState = state;
    }

    public void SetDescriptionTxt()
    {
        description.text = StaticLoc.Loc.Get("tower001");
    }

    private int remianChallengeTime = 0;// 剩余挑战次数
    public void SetLastTimeTxt(int times)
    {
        remianChallengeTime = times < 0 ? 0 : times;
        lastTime.text = StaticLoc.Loc.Get("tower002") + remianChallengeTime;
    }

    public void SetCurrLayerTxt(int layers)
    {
        if (BtnGameManagerBack.isTown())
        {
            currentLayer.text = StaticLoc.Loc.Get("tower003") + (layers + 1);
        }
        else
        {
            currentLayer.text = StaticLoc.Loc.Get("tower003") + (layers + 1);
        }

        if (layers >= maxTowerLevel)
        {
            currentLayer.text = StaticLoc.Loc.Get("tower003") + StaticLoc.Loc.Get("info689");
            startChallengeBtn.isEnabled = false;
            flashSprite.GetComponent<UISprite>().enabled = false;
            layers = maxTowerLevel;
        }

        currLayer = layers;

        InitItemGrid();
        InitGoldAndExp(currLayer);
    }

    public void SetGoldNum(string gold)
    {
        if (string.IsNullOrEmpty(gold) || gold.Equals("0"))
        {
            this.gold = 0;
            rewardLeaveBtn.isEnabled = false;
        }
        else
        {
            rewardLeaveBtn.isEnabled = true;
        }

        lblGold.text = gold;
    }

    public void SetExpNum(string exp)
    {
        lblExp.text = exp;
    }

    private int skipLayers = 0;
    public void SetSkipChallengeTxt(int layer, int hasReward)
    {
        //skipLayers = layer - layer % 5;
        skipLayers = layer > 1 ? layer : 0;
        skipChallengeTxt.text = StaticLoc.Loc.Get("tower005") + skipLayers + StaticLoc.Loc.Get("tower009");

        if (!BtnGameManagerBack.isTown() || skipLayers < 1)
        {
            skipChallengeBtn.isEnabled = false;
        }
        else
        {
            if (remianChallengeTime <= 0 || 1 == hasReward)// 没有挑战次数或者有奖励可领取时，不允许扫荡
            {
                skipChallengeBtn.isEnabled = false;
            }
            else
            {
                skipChallengeBtn.isEnabled = true;
            }
        }
    }

    public void SetSkipChallengeTxt(int hasReward)
    {
        this.SetSkipChallengeTxt(skipLayers, hasReward);
    }

    public void SetChallengeBtnTxt()
    {
        if (BtnGameManagerBack.isTown())
        {
            //if (currLayer < 1)
            //{
            //    startChallengeTxt.text = StaticLoc.Loc.Get("info1149");// 开始挑战
            //}
            //else
            //{
            //    startChallengeTxt.text = StaticLoc.Loc.Get("info1150"); // 继续挑战
            //}

            if(towerState == CommonDefine.TOWER_STATE_NOT_START)
            {
                startChallengeTxt.text = StaticLoc.Loc.Get("info1149");// 开始挑战
            }
            else
            {
                startChallengeTxt.text = StaticLoc.Loc.Get("info1150"); // 继续挑战
            }
        }
        //else
        //{
        //    startChallengeTxt.text = StaticLoc.Loc.Get("info1150"); // 继续挑战
        //    startChallengeBtn.isEnabled = false;
        //}

        if (remianChallengeTime <= 0 && towerState == CommonDefine.TOWER_STATE_NOT_START)
        {
            if (!isRequest) return;

            startChallengeBtn.isEnabled = false;
            flashSprite.GetComponent<UISprite>().enabled = false;
            flashSprite.SetActive(false);
        }
        else if (towerState == CommonDefine.TOWER_STATE_NOT_START && remianChallengeTime > 0 &&  gold > 0)// 前一天没有领奖，当天继续挑战后死亡，只能领奖，不能继续挑战，领奖后才可继续挑战
        {
            startChallengeBtn.isEnabled = false;
            flashSprite.GetComponent<UISprite>().enabled = false;
            flashSprite.SetActive(false);
        }
        else
        {
            if (!isRequest) return;

            startChallengeBtn.isEnabled = true;
            flashSprite.GetComponent<UISprite>().enabled = true;
            flashSprite.SetActive(true);
        }
    }

    //public void EnableStartChallengeBtn()
    //{
    //    startChallengeTxt.text = StaticLoc.Loc.Get("info1150"); // 继续挑战
    //    startChallengeBtn.isEnabled = true;
    //}

    private int needBloodstone = 0;
    public void SkipChallengeBtnClick()
    {
        yuan.YuanMemoryDB.YuanRow yr = null;
        if (bossTowerInfoDic.TryGetValue(skipLayers, out yr))
        {
            if(int.TryParse(yr["needbloodstone"].YuanColumnText.Trim(), out needBloodstone))
            {
                PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target = this.gameObject;
                PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "RequestWipeOut";
                string tipInfo = "";
                if(needBloodstone > 0)
                {
                    tipInfo = string.Format("{0}{1}{2}{3}{4}", StaticLoc.Loc.Get("tower006"), needBloodstone, StaticLoc.Loc.Get("tower007"), skipLayers, StaticLoc.Loc.Get("tower008"));//花费XX血石直接获得XX层的奖励
                }
                else
                {
                    tipInfo = "";
                }
                PanelStatic.StaticWarnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("meg0154"), tipInfo);
            }
            else
            {
                needBloodstone = -1;
            }
        }
        else
        {
            needBloodstone = -1;
        } 
    }

    /// <summary>
    /// 向服务器请求扫荡
    /// </summary>
    public void RequestWipeOut()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "";
        PanelStatic.StaticWarnings.warningAllEnterClose.Close();

        if (needBloodstone > int.Parse(BtnGameManager.yt[0]["Bloodstone"].YuanColumnText))// 血石不够
        {
            BtnGameManagerBack.my.SwitchToStore();
            return;
        }

        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => ServerRequest.requestTowerChallenge());
    }

    /// <summary>
    /// 服务器返回扫荡信息时调用
    /// </summary>
    public void RefreshPanelInfo(int hasReward)
    {
        SetSkipChallengeTxt(skipLayers, hasReward);
        SetChallengeBtnTxt();
        InitItemGrid();
        InitGoldAndExp(skipLayers);
        InitBtnState();
    }

    public void ChallengeBtnClick()
    {
        int[] towerInfo = new int[2];
        //towerInfo[0] = currLayer == 0 ? 1 : currLayer;// 困魔塔层数
        towerInfo[0] = currLayer + 1;// 困魔塔层数
        towerInfo[1] = towerLevel; // 困魔塔难度
        if (BtnGameManagerBack.isTown())
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("TrappedtowerLoadLevel", towerInfo, SendMessageOptions.RequireReceiver);// 如果在主城Loading场景
        }
        else
        {
            Debug.Log("ChallengeBtnClick===================towernumber" + towerInfo[0]);
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("TrappedtowerCallMonsterAsNum", towerInfo[0], SendMessageOptions.RequireReceiver);// 如果在副本直接调用刷怪
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show0", SendMessageOptions.RequireReceiver);
        }
    }

    public void RewardLeaveBtnClick()
    {
        Debug.Log("RewardLeaveBtnClick---------------------------");
        //PanelStatic.StaticBtnGameManager.RunOpenLoading(() => ServerRequest.requestTowerReward());
        //if (!BtnGameManagerBack.isTown())
        //{
        //    PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("nowYesReturn", SendMessageOptions.RequireReceiver);
        //}

        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.target = this.gameObject;
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "GetReward";
        PanelStatic.StaticWarnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("meg0154"), StaticLoc.Loc.Get("meg0155"));
    }

    public void GetReward()
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => ServerRequest.requestTowerReward());
    }

    public void CloseAndLeave()
    {
        PanelStatic.StaticWarnings.warningAllEnterClose.btnEnter.functionName = "";
        PanelStatic.StaticWarnings.warningAllEnterClose.Close();

        if (!BtnGameManagerBack.isTown())
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("nowYesReturn", SendMessageOptions.RequireReceiver);
        }
    }

    public void EnableRewardLeaveBtn(bool isEnabled)
    {
        rewardLeaveBtn.isEnabled = isEnabled;
    }

    public void TempLeaveBtn()
    {
        if (BtnGameManagerBack.isTown())
        {
            return;
        }
        PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("nowYesReturn", SendMessageOptions.RequireReceiver);  
    }

    private bool isRequest = true;
    public void OpenTowerPanel(bool isSuccess)
    {
        isRequest = isSuccess;
        if (this.gameObject.activeSelf)
        {
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => ServerRequest.requestTowerOpen());
        }
        else
        {
            PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("ShowTowerPanel", SendMessageOptions.RequireReceiver);
        }

        if (!isSuccess)
        {
            skipChallengeBtn.isEnabled = false;
            startChallengeBtn.isEnabled = false;
            flashSprite.GetComponent<UISprite>().enabled = false;
            flashSprite.SetActive(false);
            rewardLeaveBtn.isEnabled = true;
            tempLeaveBtn.isEnabled = true;
        }
        else
        {
            startChallengeTxt.text = StaticLoc.Loc.Get("info1150"); // 继续挑战
            startChallengeBtn.isEnabled = true;
            flashSprite.GetComponent<UISprite>().enabled = true;
            flashSprite.SetActive(true);
        }
    }
}
