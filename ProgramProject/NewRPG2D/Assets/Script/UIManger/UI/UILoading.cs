using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILoading : TTUIPage
{

    public Slider slider;
    public Text txt_LoadNum;


    private void Update()
    {
        if (slider.value < 97)
        {
            slider.value += Time.deltaTime * 80;
        }
        else
        {
            slider.value += Time.deltaTime * 5;
        }
        txt_LoadNum.text = slider.value.ToString("#0") + "%";
        if (slider.value == slider.maxValue)
        {
            ClosePage();
            UIPanelManager.instance.ShowPage<UIMain>();
            UIMain.instance.gold.num = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Gold);
            UIMain.instance.food.num = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
            //UIMain.instance.mana.num = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Mana);
            //UIMain.instance.wood.num = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Wood);
            //UIMain.instance.iron.num = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Iron);
            for (int i = 1; i < 6; i += 2)
            {
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, (BuildRoomName)i);
            }
            HallEventManager.instance.SendEvent(HallEventDefineEnum.diamondsSpace);
        }
    }

    public override void Hide(bool needAnim)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
