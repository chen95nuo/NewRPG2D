using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MatchBtnState
{
    NoState,
    TeamMatch,
    SingleMatch,
    CantMatch,
    Matching,
    CancelMatch,
}

public class PanelPVPBattlefield : MonoBehaviour 
{
    public static PanelPVPBattlefield pvpBattlefield;

    public UILabel lblBattleInfo;
    public UILabel lblCondition;
    public UIGrid battlefieldGrid;
    public GameObject battleItemPrefab;

    public UIButton teamMatchBtn;
    public UIButton singleMatchBtn;
    public UILabel teamMatchLbl;
    public UILabel singleMatchLbl;
    public UILabel queueState;
    private MatchBtnState teamBtnState = MatchBtnState.NoState;
    private MatchBtnState singleBtnState = MatchBtnState.NoState;

    private int selectedID = -1;// 被选中的战场的ID

    [HideInInspector]
    public bool isTeamMatching = false; // 组队匹配正在排队中
    [HideInInspector]
    public bool isSingleMatching = false; // 单人匹配正在进行中
    [HideInInspector]
    public bool isReady = false; // 是否准备好，弹出匹配板是为true，匹配板关闭为false


    private int matchingID = -1;// 正在排队中的战场ID

    void Awake()
    {
        pvpBattlefield = this;
    }

	// Use this for initialization
	void Start () 
	{
        GetBattlefieldInfo();
        InstantiateItem();
	}

    public void OnEnable()
    {
        //InRoom.GetInRoomInstantiate().GetMyTeams(teamyt, "DarkSword2", "PlayerInfo");
        EnableItem();

        if (isReady)
        {
            teamMatchBtn.isEnabled = false;
            singleMatchBtn.isEnabled = false;
            return;
        }

        if ((isTeamMatching || isSingleMatching) && selectedID != matchingID) // 正在排队中且排队战场不是当前选中战场时，不允许点击按钮
        {
            SetTeamBtnState(MatchBtnState.CantMatch);
            SetSingleBtnState(MatchBtnState.CantMatch);
            return;
        }

        if (teamBtnState == MatchBtnState.NoState)
        {
            if (!isSingleMatching && BtnGameManagerBack.my.teamMembersID.Count > 1)
            {
                if (BtnGameManagerBack.my.captainID.Equals(BtnGameManager.yt[0]["PlayerID"].YuanColumnText))
                {
                    if (isTeamMatching)
                    {
                        SetTeamBtnState(MatchBtnState.CancelMatch);// 队长显示取消匹配
                    }
                    else
                    {
                        SetTeamBtnState(MatchBtnState.TeamMatch);// 队长显示组队匹配
                    }
                }
                else
                {
                    if (isTeamMatching)
                    {
                        SetTeamBtnState(MatchBtnState.Matching);// 队员显示匹配中
                    }
                    else
                    {
                        SetTeamBtnState(MatchBtnState.CantMatch);// 队员不能点击按钮
                    }
                }
            }
            else
            {
                SetTeamBtnState(MatchBtnState.CantMatch);
            }
        }

        if (singleBtnState == MatchBtnState.NoState)
        {
            if (!isSingleMatching && BtnGameManagerBack.my.teamMembersID.Count > 1)
            {
                SetSingleBtnState(MatchBtnState.CantMatch);
            }
            else
            {
                if (isSingleMatching)
                {
                    SetSingleBtnState(MatchBtnState.CancelMatch);
                }
                else
                {
                    SetSingleBtnState(MatchBtnState.SingleMatch);
                }
            }
        }
    }

    public void OnDisable()
    {
        singleBtnState = MatchBtnState.NoState;
        teamBtnState = MatchBtnState.NoState;
    }
     

    public void SetBattleInfo(string info)
    {
        if (null == lblBattleInfo)
        {
            Debug.LogError("PanelPVPBattlefield::SetBattleInfo--------------lblBattleInfo is null!");
            return;
        }

        lblBattleInfo.text = info;
    }

    public void SetConditionInfo(string info)
    {
        if (null == lblCondition)
        {
            Debug.LogError("PanelPVPBattlefield::SetConditionInfo--------------lblCondition is null!");
            return;
        }

        lblCondition.text = info;
    }

    public void SetTeamBtnState(MatchBtnState state)
    {
        SetQueueState(false);

        teamBtnState = state;
        if (state == MatchBtnState.TeamMatch) // 队长显示组队匹配
        {
            isReady = false;
            isTeamMatching = false;
            teamMatchBtn.isEnabled = true;
            teamMatchLbl.text = StaticLoc.Loc.Get("battle002");
        }
        else if (state == MatchBtnState.CantMatch)// 不是队长或者不在队伍中，不允许点击
        {
            isTeamMatching = false;
            teamMatchBtn.isEnabled = false;
        }
        else if (state == MatchBtnState.Matching)// 队员显示正在配对中
        {
            isTeamMatching = true;
            teamMatchBtn.isEnabled = false;
            teamMatchLbl.text = StaticLoc.Loc.Get("battle002");

            SetQueueState(true);
        }
        else if (state == MatchBtnState.CancelMatch)// 队长显示取消匹配
        {
            isTeamMatching = true;
            teamMatchLbl.text = StaticLoc.Loc.Get("battle008");

            SetQueueState(true);
        }
    }

    public void SetSingleBtnState(MatchBtnState state)
    {
        SetQueueState(false);

        // 组队状态时不允许点击单人匹配
        singleBtnState = state;
        if (state == MatchBtnState.SingleMatch)// 不在队伍中，显示单人匹配
        {
            isReady = false;
            isSingleMatching = false;
            singleMatchBtn.isEnabled = true;
            singleMatchLbl.text = StaticLoc.Loc.Get("battle003");
        }
        else if (state == MatchBtnState.CantMatch)// 正在组队匹配中，不允许点击
        {
            isSingleMatching = false;
            singleMatchBtn.isEnabled = false;
            singleMatchLbl.text = StaticLoc.Loc.Get("battle003");
        }
        else if (state == MatchBtnState.CancelMatch)// 匹配中，显示取消匹配
        {
            isSingleMatching = true;
            singleMatchBtn.isEnabled = true;
            singleMatchLbl.text = StaticLoc.Loc.Get("battle008");

            SetQueueState(true);
        }
    }

    /// <summary>
    /// 设置排队状态
    /// </summary>
    /// <param name="num"></param>
    public void SetQueueState(bool isShow)
    {
        if (isShow)
        {
            queueState.text = StaticLoc.Loc.Get("meg0175");
        }
        else
        {
            queueState.text = "";
        }
    }

    /*
    /// <summary>
    /// 当打开PVP战场板时，接收到组队成功消息
    /// </summary>
    public void TeamCreate()
    {

    }

    /// <summary>
    /// 队伍被解散时应该调用此方法
    /// </summary>
    public void TeamDissolved()
    {
        // TODO：当队伍被取消时，组队匹配不允许点击，单人组队按钮允许点击
        SetTeamBtnState(MatchBtnState.CantMatch);
        SetSingleBtnState(MatchBtnState.SingleMatch);
    }
    */

    /// <summary>
    /// 组队匹配按钮回调
    /// </summary>
    public void TeamMatchClick()
    {
        if (-1 == selectedID)
        {
            // 提示请选择一个战场！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("battle006"));
            return;
        }

        if (teamBtnState == MatchBtnState.TeamMatch)// 队长点击组队匹配
        {
            matchingID = selectedID;
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().PVPTeamCreate(selectedID.ToString()));

            SetTeamBtnState(MatchBtnState.CancelMatch);
        }
        else if (teamBtnState == MatchBtnState.CancelMatch)// 队长点击取消匹配
        {
            matchingID = -1;
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().PVPCancel(selectedID.ToString()));

            SetTeamBtnState(MatchBtnState.TeamMatch);
        }
    }

    /// <summary>
    /// 单人匹配按钮回调
    /// </summary>
    public void SingleMatchClick()
    {
        if (-1 == selectedID)
        {
            // 提示请选择一个战场！
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("battle006"));
            return;
        }

        if (singleBtnState == MatchBtnState.SingleMatch)// 点击单人匹配
        {
            matchingID = selectedID;
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().PVPTeamCreate(selectedID.ToString()));
            SetSingleBtnState(MatchBtnState.CancelMatch);
        }
        else if (singleBtnState == MatchBtnState.CancelMatch)// 点击取消匹配
        {
            matchingID = -1;
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().PVPCancel(selectedID.ToString()));
            SetSingleBtnState(MatchBtnState.SingleMatch);
        }
    }

    private Dictionary<int, yuan.YuanMemoryDB.YuanRow> battlefieldInfoDic = new Dictionary<int, yuan.YuanMemoryDB.YuanRow>();
    /// <summary>
    /// 获取所有的战场信息
    /// </summary>
    public void GetBattlefieldInfo()
    {
        List<string> ids = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBattlefield.GetColumnText("id");
        
        int key = -1;
        yuan.YuanMemoryDB.YuanRow value = null;

        for(int i=0;i<ids.Count;i++)
        {
            if(!int.TryParse(ids[i].Trim(), out key) || 4 == key)//id为4的表示单人PVP
            {
                continue;
            }

            value = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBattlefield.SelectRowEqual("id", ids[i]);

            if (null != value && -1 != key)
            {
                battlefieldInfoDic.Add(key, value);
            }
        }
    }

    List<PVPBattleItem> childItems = new List<PVPBattleItem>();
    /// <summary>
    /// 实例化战场类型Item
    /// </summary>
    public void InstantiateItem()
    {
        int childCount = battlefieldGrid.transform.childCount;
        if(childCount < battlefieldInfoDic.Keys.Count)
        {
            for(int i=0;i<battlefieldInfoDic.Keys.Count-childCount;i++)
            {
                if(null == battleItemPrefab)
                {
                    Debug.LogError("PanelPVPBattlefield::InstantiateItem--------------battlefieldInfoDic is null!");
                }

                GameObject go = Instantiate(battleItemPrefab) as GameObject;
                go.SetActive(false);
                go.transform.parent = battlefieldGrid.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
            }
        }

        Transform tempChild = null;
        for (int i = 0; i < battlefieldGrid.transform.childCount; i++)
        {
            tempChild = battlefieldGrid.transform.GetChild(i);
            if (tempChild.gameObject.activeSelf)
            {
                tempChild.gameObject.SetActive(false);
            }
            childItems.Add(tempChild.GetComponent<PVPBattleItem>());
        }

        int index = 0;
        foreach (KeyValuePair<int, yuan.YuanMemoryDB.YuanRow> kvp in battlefieldInfoDic)
        {
            childItems[index].ItemID = kvp.Key;
            childItems[index].SetBattleName(kvp.Value["name"].YuanColumnText);
            childItems[index].btnMsg.target = this.gameObject;
            childItems[index].btnMsg.functionName = "BattleItemClick";
            childItems[index].gameObject.SetActive(true);
            index++;
        }

        battlefieldGrid.Reposition();

        Transform firstChild = battlefieldGrid.transform.GetChild(0);
        firstChild.GetComponent<UIToggle>().Set(true);
        firstChild.GetComponent<UIButtonMessage>().OnClick();
    }

    public void EnableItem()
    {
        for (int i = 0; i < childItems.Count; i++)
        {
            if (battlefieldInfoDic.ContainsKey(childItems[i].ItemID))
            {
                childItems[i].gameObject.SetActive(true);
            }
            else
            {
                childItems[i].gameObject.SetActive(false);
            }
        }


    }

    public void BattleItemClick(GameObject sender)
    {
        if (isSingleMatching || isTeamMatching)
        {
            for (int i = 0; i < childItems.Count; i++)
            {
                if (selectedID == childItems[i].ItemID)
                {
                    childItems[i].transform.GetComponent<UIToggle>().value = true;
                }
            }

            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("battle009"));

            return;
        }

        PVPBattleItem item = sender.GetComponent<PVPBattleItem>();
        selectedID = item.ItemID;
        if(battlefieldInfoDic.ContainsKey(selectedID))
        {
            yuan.YuanMemoryDB.YuanRow yrBattle = null;
            if(battlefieldInfoDic.TryGetValue(selectedID, out yrBattle))
            {
                SetBattleInfo(yrBattle["description"].YuanColumnText);
                SetConditionInfo(string.Format("{0}-{1}{2}", yrBattle["minLevel"].YuanColumnText, yrBattle["maxLevel"].YuanColumnText, StaticLoc.Loc.Get("battle005")));
            }
        }
        else
        {
            Debug.LogError("PanelPVPBattlefield::BattleItemClick---------------selectedID is not contained in battlefieldInfoDic!");
        }

        //OnDisable();
        //OnEnable();
    }
}
