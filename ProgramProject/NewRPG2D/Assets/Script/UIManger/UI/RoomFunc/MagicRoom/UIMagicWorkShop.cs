using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMagicWorkShop : TTUIPage
{
    public static UIMagicWorkShop instance;

    public Text txt_tip_1;
    public Text txt_tip_8;
    public Text txt_FightTip;
    public Text txt_ReadTip;
    public Text txt_WhitTip;
    public Text txt_Diamonds;
    public Text txt_MagicNum;

    public GameObject grid;
    public UIMagicGrid[] useGrids;
    public UIMagicGrid[] cryGrids;
    public Transform readGridPoint;
    public List<UIMagicGrid> readGrids = new List<UIMagicGrid>();
    public List<UIMagicGrid> workGrids = new List<UIMagicGrid>();

    public UIMagicInfoTip tip;
    public GameObject page_2;
    private int empty = 0;

    private MagicWorkShopHelper allMagic { get { return MagicDataMgr.instance.AllMagicData; } }
    private LocalBuildingData currentRoom;

    private void Awake()
    {
        txt_tip_1.text = "制作";
        txt_tip_8.text = "全部加速";
        txt_FightTip.text = "战斗法术";
        txt_ReadTip.text = "就绪的法术";
        txt_WhitTip.text = "制作中的法术";
    }

    public override void Show(object mData)
    {
        tip.CloseAllUI();
        base.Show(mData);
        RoomMgr data = mData as RoomMgr;
        currentRoom = data.currentBuildData;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        int num = MagicDataMgr.instance.AllMagicData.readyMagic.Count + MagicDataMgr.instance.AllMagicData.workQueue.Count;
        txt_MagicNum.text = num + "/18";
    }

    public void UpdateUseMagic()
    {
        for (int i = 0; i < allMagic.useMagic.Length; i++)
        {
            if (allMagic.useMagic[i] != null)
            {
                useGrids[i].gameObject.SetActive(true);
                useGrids[i].UpdateInfo(allMagic.useMagic[i]);
            }
            else if (allMagic.useMagic[i] == null && i < currentRoom.buildingData.Param2)
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
    public void UpdateReadMagic()
    {
        for (int i = 0; i < allMagic.readyMagic.Count; i++)
        {
            if (readGrids.Count == i)
            {
                InstanceGrid(readGrids);
            }
            readGrids[i].gameObject.SetActive(true);
            //readGrids[i].UpdateInfo(allMagic.useMagic[i], type);
        }
        for (int i = allMagic.readyMagic.Count; i < readGrids.Count; i++)
        {
            readGrids[i].gameObject.SetActive(false);
        }
    }
    public void UpdateWorkMagic()
    {
        //for (int i = 0; i < allMagic.workQueue.Length; i++)
        //{
        //    if (workGrids.Count == i)
        //    {
        //        InstanceGrid(workGrids);
        //    }
        //    workGrids[i].gameObject.SetActive(true);
        //    //workGrids[i].UpdateInfo(allMagic.workQueue[i], type);
        //}
        //for (int i = allMagic.readyMagic.Count; i < workGrids.Count; i++)
        //{
        //    workGrids[i].gameObject.SetActive(false);
        //}
    }

    public void ChickUseBtn(RealMagic grid)
    {
        //tip.ShowPage(1, grid);
    }

    public void ShowMagicTip(RealMagic magic)
    {
        int type = readGrids.Count < 18 ? 0 : 2;
        tip.UpdateInfo(magic, type);
    }

    public void ShowInfo()
    {

    }

    public void GridAnim(bool isUseAnim = false, bool isRun = false)
    {
        if (isUseAnim)
        {
            for (int i = 0; i < useGrids.Length; i++)
            {
                useGrids[i].GridShake(isRun);
            }
        }
        else
        {
            for (int i = 0; i < readGrids.Count; i++)
            {
                readGrids[i].GridShake(isRun);
            }
        }
    }

    public void InstanceGrid(List<UIMagicGrid> grids)
    {
        GameObject go = Instantiate(grid, readGridPoint) as GameObject;
        UIMagicGrid data = go.GetComponent<UIMagicGrid>();
        grids.Add(data);
    }
}