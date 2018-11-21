using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class UIBoxOpen : TTUIPage
{
    public static UIBoxOpen instance;

    public SkeletonGraphic boxAnim;
    public GameObject BoxGrid;
    public Transform boxTrans;
    public List<UIItemGrid> boxGrids = new List<UIItemGrid>();

    public Button btn_Close;

    private float showSpeed = 0.1f;

    private void Awake()
    {
        instance = this;
        btn_Close.onClick.AddListener(ChickClose);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        boxAnim.AnimationState.SetAnimation(0, "open", false);
    }

    public void ShowAnim(List<RealPropData> propData, List<EquipmentRealProperty> equipData)
    {
        int index = 0;
        for (int i = 0; i < propData.Count; i++)
        {
            if (boxGrids.Count <= index)
            {
                GameObject go = Instantiate(BoxGrid, boxTrans) as GameObject;
                UIItemGrid grid = go.GetComponent<UIItemGrid>();
                boxGrids.Add(grid);
            }
            boxGrids[index].UpdateInfo(propData[i].propData.ItemId, propData[i].number);
            index++;
        }
        for (int i = 0; i < equipData.Count; i++)
        {
            if (boxGrids.Count <= index)
            {
                GameObject go = Instantiate(BoxGrid, boxTrans) as GameObject;
                UIItemGrid grid = go.GetComponent<UIItemGrid>();
                boxGrids.Add(grid);
            }
            ItemGridHelp helpData = new ItemGridHelp(equipData[i].EquipId, ItemType.Equip);
            boxGrids[index].UpdateInfo(helpData);
            index++;
        }
    }

    private void ChickClose()
    {
        if (showSpeed == 0)
        {
            ClosePage();
        }
        else
        {
            showSpeed = 0;
        }
    }
}
