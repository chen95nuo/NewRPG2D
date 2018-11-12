using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;

public class UIMagicWorkShop : TTUIPage
{
    public static UIMagicWorkShop instance;

    public Text txt_tip_1;
    public Text txt_tip_2;
    public Text txt_FightTip;
    public Text txt_ReadTip;
    public Text txt_WhitTip;
    public Text txt_Diamonds;
    public Text txt_MagicNum;
    public Text txt_ChangeMagic;
    public Button btn_ChangeType;
    public Button btn_AllSpeedUp;

    public GameObject grid;
    public UIMagicGrid[] useGrids;
    public UIMagicGrid[] cryGrids;
    public UIMagicInfoTip magicInfoTip;
    public Transform readGridPoint;
    public Transform WorkGridPoint;
    public RectTransform mainStor;
    public List<UIMagicGrid> readGrids = new List<UIMagicGrid>();
    public List<UIMagicGrid> workGrids = new List<UIMagicGrid>();

    public UIMagicInfoTip tip;
    public GameObject WorkGrid;
    public GameObject UseMagic;
    public GameObject CryMagic;
    public UIMagicGrid changeGrid;
    public GameObject ChangeMagicPage;
    public int maxMagic = 18;
    private int empty = 0;
    private bool isShowUse = true;

    private MagicWorkShopHelper allMagic;
    private MagicWorkShopHelper AllMagic
    {
        get
        {
            if (allMagic == null)
            {
                allMagic = MagicDataMgr.instance.AllMagicData;
            }
            return allMagic;
        }
    }
    private LocalBuildingData currentRoom;

    private void Awake()
    {
        instance = this;

        txt_tip_1.text = "制作";
        txt_tip_2.text = "全部加速";
        txt_FightTip.text = "战斗法术";
        txt_ReadTip.text = "就绪的法术";
        txt_WhitTip.text = "制作中的法术";
        btn_ChangeType.onClick.AddListener(ChangeUseType);
        btn_AllSpeedUp.onClick.AddListener(ChickAllSpeed);
    }

    private void ChickAllSpeed()
    {
        Debug.Log("全部加速");
        for (int i = 0; i < allMagic.workQueue.Count; i++)
        {
            MagicDataMgr.instance.SpeedUpNewMagic(allMagic.workQueue[i].magicID);
        }
    }
    public void ChangeUseType(bool isShow = true)
    {
        isShowUse = isShow;
        ChangeUseType();
    }
    private void ChangeUseType()
    {
        string st_1 = "制作";
        string st_2 = "返回";
        txt_tip_1.text = isShowUse ? st_1 : st_2;
        UseMagic.SetActive(isShowUse);
        CryMagic.SetActive(!isShowUse);
        isShowUse = !isShowUse;
    }

    public override void Show(object mData)
    {
        tip.CloseAllUI();
        base.Show(mData);
        //RoomMgr data = mData as RoomMgr;
        //currentRoom = data.currentBuildData;
        if (currentRoom != null)
        {
            return;
        }
        LocalBuildingData data = mData as LocalBuildingData;
        currentRoom = data;
        UpdateInfo();
    }

    public void ChangeMagic(RealMagic currentMagic, bool isTrue = true)
    {
        ChangeMagicPage.SetActive(isTrue);
        if (isTrue)
        {
            changeGrid.UpdateInfo(currentMagic, true);
            GridAnim(true);
        }
    }

    private void UpdateInfo()
    {
        int num = MagicDataMgr.instance.AllMagicData.readyMagic.Count + MagicDataMgr.instance.AllMagicData.workQueue.Count;
        txt_MagicNum.text = num + "/" + maxMagic;
        txt_FightTip.text = "战斗法术";
        txt_tip_1.text = "制作";
        ChangeMagic(null, false);
        UseMagic.SetActive(true);
        CryMagic.SetActive(false);
        magicInfoTip.gameObject.SetActive(false);

        ChangeUseType();
        UpdateUseMagic();
        UpdateReadMagic();
        UpdateWorkMagic();
        UpdateCryMagic();
    }
    /// <summary>
    /// 刷新使用技能
    /// </summary>
    public void UpdateUseMagic()
    {
        for (int i = 0; i < AllMagic.useMagic.Length; i++)
        {
            if (AllMagic.useMagic[i] != null)
            {
                useGrids[i].gameObject.SetActive(true);
                useGrids[i].UpdateInfo(AllMagic.useMagic[i]);
            }
            else if (AllMagic.useMagic[i] == null && i < currentRoom.buildingData.Param2)
            {
                useGrids[i].gameObject.SetActive(true);
                useGrids[i].UpdateInfo();
                empty++;
            }
            else
            {
                useGrids[i].gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 刷新准备技能
    /// </summary>
    public void UpdateReadMagic()
    {
        MainStorMove();
        AllMagic.readyMagic.Sort((RealMagic x, RealMagic y) => ((int)x.magic.magicName.CompareTo((int)y.magic.magicName)));
        for (int i = 0; i < AllMagic.readyMagic.Count; i++)
        {
            if (readGrids.Count == i)
            {
                InstanceGrid(readGrids, readGridPoint);
            }
            readGrids[i].gameObject.SetActive(true);
            readGrids[i].UpdateInfo(AllMagic.readyMagic[i], MagicGridType.Read);
        }
        for (int i = AllMagic.readyMagic.Count; i < readGrids.Count; i++)
        {
            readGrids[i].gameObject.SetActive(false);
        }
        readGridPoint.GetComponent<ContentSizeFitter>().SetLayoutVertical();
    }
    /// <summary>
    /// 刷新制造中的技能
    /// </summary>
    public void UpdateWorkMagic()
    {
        MainStorMove();
        for (int i = 0; i < AllMagic.workQueue.Count; i++)
        {
            if (workGrids.Count == i)
            {
                InstanceGrid(workGrids, WorkGridPoint);
            }
            workGrids[i].gameObject.SetActive(true);
            workGrids[i].UpdateInfo(AllMagic.workQueue[i], MagicGridType.Work);
        }
        for (int i = AllMagic.workQueue.Count; i < workGrids.Count; i++)
        {
            workGrids[i].gameObject.SetActive(false);
        }

        WorkSetActive(AllMagic.workQueue.Count <= 0 ? false : true);
    }

    private void WorkSetActive(bool isShow)
    {
        WorkGrid.SetActive(isShow);
        WorkGridPoint.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        //if (isShow == true)
        //{
        //    Invoke("WorkBack", 0.1f);
        //}
    }
    private void WorkBack()
    {
        WorkGrid.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 刷新可制造的技能
    /// </summary>
    public void UpdateCryMagic()
    {
        for (int i = 0; i < (int)MagicName.Max; i++)
        {
            MagicData data = MagicDataMgr.instance.GetMagicLevel((MagicName)i);
            if (i > currentRoom.buildingData.Param1)
            {
                cryGrids[i].UpdateLock(data);
            }
            else
            {
                cryGrids[i].UpdateInfo(data, MagicGridType.Cry);
            }
        }
    }

    public void GridAnim(bool isUseAnim = false, bool isRun = true)
    {
        if (isUseAnim)
        {
            for (int i = 0; i < useGrids.Length; i++)
            {
                useGrids[i].GridAnim(isRun);
            }
        }
        else
        {
            for (int i = 0; i < readGrids.Count; i++)
            {
                readGrids[i].GridAnim(isRun);
            }
        }
    }

    public void InstanceGrid(List<UIMagicGrid> grids, Transform point)
    {
        GameObject go = Instantiate(grid, point) as GameObject;
        UIMagicGrid data = go.GetComponent<UIMagicGrid>();
        grids.Add(data);
    }

    public void ChickGridEvent(Vector2 point, MagicGridType type, RealMagic data = null)
    {
        magicInfoTip.gameObject.SetActive(true);
        switch (type)
        {
            case MagicGridType.Empty:
                break;
            case MagicGridType.Use:
                int count = allMagic.readyMagic.Count + allMagic.workQueue.Count;
                magicInfoTip.ShowUse(data, count == maxMagic);
                break;
            case MagicGridType.Read:
                magicInfoTip.ShowRead(data, empty == 0);
                break;
            case MagicGridType.Work:
                magicInfoTip.ShowWork(data);
                break;
            default:
                break;
        }
        magicInfoTip.ChickPoint(point);
    }

    private void MainStorMove()
    {
        mainStor.anchoredPosition = Vector2.up * 3000;
        Invoke("MainStorMoveBack", 0.05f);
    }
    private void MainStorMoveBack()
    {
        mainStor.anchoredPosition = Vector2.zero;
    }
}