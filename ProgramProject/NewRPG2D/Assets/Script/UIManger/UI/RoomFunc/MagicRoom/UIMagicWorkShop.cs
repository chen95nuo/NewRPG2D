using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMagicWorkShop : TTUIPage
{
    public static UIMagicWorkShop instance;

    public Text txt_tip_1;
    public Text txt_tip_2;
    public Text txt_tip_3;
    public Text txt_tip_4;
    public Text txt_tip_5;
    public Text txt_tip_6;
    public Text txt_tip_7;
    public Text txt_tip_8;
    public Text txt_FightTip;
    public Text txt_ReadTip;
    public Text txt_WhitTip;
    public Text txt_diaNum_1;
    public Text txt_diaNum_2;
    public Text txt_ReadNum;

    public Button btn_Message;
    public Button btn_Add;
    public Button btn_Remove;
    public Button btn_Change;
    public Button btn_Cancel;
    public Button btn_SpeedUp;

    public GameObject grid;
    public UIMagicGrid[] useGrids;
    public Transform readGridPoint;
    public List<UIMagicGrid> readGrids = new List<UIMagicGrid>();
    public List<UIMagicGrid> workGrids = new List<UIMagicGrid>();

    public UIMagicTip tip;
    private int empty = 0;

    private MagicWorkShopHelper allMagic { get { return MagicDataMgr.instance.AllMagicData; } }
    private LocalBuildingData currentRoom;

    private void Awake()
    {
        txt_tip_1.text = "讯息";
        txt_tip_2.text = "添加";
        txt_tip_3.text = "移除";
        txt_tip_4.text = "替换";
        txt_tip_5.text = "取消";
        txt_tip_6.text = "加速";
        txt_tip_7.text = "制作";
        txt_tip_8.text = "全部加速";
        txt_FightTip.text = "战斗法术";
        txt_ReadTip.text = "就绪的法术";
        txt_WhitTip.text = "制作中的法术";
    }

    public override void Show(object mData)
    {
        tip.RemovePage();
        base.Show(mData);
        RoomMgr data = mData as RoomMgr;
        currentRoom = data.currentBuildData;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        //for (int i = 0; i < allMagic.useMagic.Length; i++)
        //{
        //    if (allMagic.useMagic[i] != null)
        //    {
        //        useGrid[i].UpdateInfo(allMagic.useMagic[i]);
        //    }
        //    else
        //    {
        //        empty++;
        //        useGrid[i].UpdateInfo();
        //    }
        //}
        //int type = 0;
        //if (empty == 0)
        //{
        //    type = 2;
        //}
        //for (int i = 0; i < allMagic.readyMagic.Count; i++)
        //{
        //    if (readGrids.Count == i)
        //    {
        //        InstanceGrid(readGrids);
        //    }
        //    readGrids[i].gameObject.SetActive(true);
        //    //readGrids[i].UpdateInfo(allMagic.useMagic[i], type);
        //}
        //for (int i = allMagic.readyMagic.Count; i < readGrids.Count; i++)
        //{
        //    readGrids[i].gameObject.SetActive(false);
        //}
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
    public void UpdateReadMagic() { }
    public void UpdateWorkMagic() { }

    public void ChickUseBtn(RealMagic grid)
    {
        tip.ShowPage(1, grid);
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