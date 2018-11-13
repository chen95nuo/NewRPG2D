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
    private int empty = 0;
    private bool isShowUse = true;
    private bool animIsRun = false;
    private bool animIsUse = false;

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

    public int MaxMagic
    {
        get
        {
            return MagicDataMgr.instance.allMagicSpace;
        }
    }
    public int nowMagic
    {
        get { return AllMagic.readyMagic.Count + AllMagic.workQueue.Count; }
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
    private void Update()
    {
        if (animIsRun)
        {
            Debug.Log("运行了222");

            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                Debug.Log("运行了");
                GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                if (go == null || go.GetComponent<UIMagicGrid>() == null)
                {
                    animIsRun = false;
                    GridAnim(animIsUse, animIsRun);
                }
            }
        }
    }
    private void ChickAllSpeed()
    {
        Debug.Log("全部加速");
        int count = allMagic.workQueue.Count;
        for (int i = 0; i < count; i++)
        {
            MagicDataMgr.instance.SpeedUpNewMagic(allMagic.workQueue[0].magicID);
        }
        UpdateReadMagic();
        UpdateWorkMagic();
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
        if (currentRoom != null)
        {
            return;
        }
        RoomMgr data = mData as RoomMgr;
        currentRoom = data.currentBuildData;
        UpdateInfo();
    }

    /// <summary>
    /// 准备好的添加替换
    /// </summary>
    /// <param name="currentMagic"></param>
    /// <param name="isTrue"></param>
    public void ChangeMagic(RealMagic currentMagic, bool isTrue = true)
    {
        ChangeMagicPage.SetActive(isTrue);
        mainStor.gameObject.SetActive(!isTrue);
        if (isTrue)
        {
            changeGrid.UpdateInfo(currentMagic, true);
            GridAnim(true);
        }
    }

    private void UpdateInfo()
    {
        int num = nowMagic;
        txt_MagicNum.text = num + "/" + MaxMagic;
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
        empty = 0;
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
        AllMagic.readyMagic.Sort((RealMagic x, RealMagic y) => (x.magic.magicName.CompareTo(y.magic.magicName)));
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
        int num = nowMagic;
        txt_MagicNum.text = num + "/" + MaxMagic;
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
        int num = nowMagic;
        txt_MagicNum.text = num + "/" + MaxMagic;
    }
    private void WorkSetActive(bool isShow)
    {
        WorkGrid.SetActive(isShow);
        WorkGridPoint.GetComponent<ContentSizeFitter>().SetLayoutVertical();
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
        animIsRun = isRun;
        Debug.Log(animIsRun);
        animIsUse = isUseAnim;
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

    public void ChangeMagicData(UIMagicGrid gridData)
    {
        GridAnim(gridData.ClickType == MagicGridType.Use, false);
        MagicDataMgr.instance.ChangeMagic(gridData.currentRealMagic, changeGrid.currentRealMagic);
        ChangeMagic(null, false);
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
                magicInfoTip.ShowUse(data, count == MaxMagic);
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

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim);
    }
}