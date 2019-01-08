using UnityEngine;
using System.Collections;

public class BtnPlayer : MonoBehaviour {

    public BtnManager btnManager;
    public UILabel lblNew;
    public GameObject slotNum;
    public UILabel lblSlotNum;
    public UILabel lblPlayerName;
    public UILabel lblLevel;
    public UILabel lblLevelNum;
    public UILabel lblPro;
    public UILabel lblAear;
    public UISprite background;
    public yuan.YuanMemoryDB.YuanRow yuanRow;
    public enum BtnType
    {
        New=0,
        Read,
		Buy,
    }

    public BtnType btnType = BtnType.New;
	
	public GameObject PlayerSelect;
	public UIToggle checkbox;
	void Awake()
	{
		if(null == btnManager)
		{
			btnManager = GameObject.Find("ButtonManager").GetComponent<BtnManager>();
		}
		
		if(null == PlayerSelect)
		{
			PlayerSelect = GameObject.Find("SongPlayerSelect");
		}
		
		checkbox = GetComponent<UIToggle>();
	}
	
    public void OnClick()
    {
        switch (btnType)
        {
            case BtnType.New:
                {
                   // background.gameObject.active = false;
					//checkbox.value = false;
                    EnableToggle(false);
                    btnManager.CameraToNew();
//					btnManager.SelectDefaultRole();
                }
                break;
            case BtnType.Read:
                {
                    EnableToggle(true);
                    btnManager.CameraToSelectPlayer();
                    //InRoom.GetInRoomInstantiate().SendID(yuanRow["PlayerID"].YuanColumnText.Trim(), yuanRow["ProID"].YuanColumnText.Trim(), yuanRow["PlayerName"].YuanColumnText.Trim());
                    PlayerSelect.SendMessage("SelectOnePlayer", yuanRow, SendMessageOptions.DontRequireReceiver);
                }
                break;
			case BtnType.Buy:
                {
					//checkbox.value = false;
                    EnableToggle(false);
                    btnManager.BuyPlayerSlot(checkbox);
	//				Debug.Log("----------------购买栏位---------------");
                }
                break;
        }
		if(ShowPart.showPart)
		{
			ShowPart.showPart.ClickBtn();
		}
    }

    /// <summary>
    /// 设置角色栏位数量显示
    /// </summary>
    /// <param name="currNum"></param>
    /// <param name="maxNum"></param>
    public void SetSlotNum(int currNum, int maxNum)
    {
        slotNum.SetActive(true);
        lblSlotNum.text = string.Format("{0}/{1}", currNum, maxNum);
    }

    /// <summary>
    /// 隐藏栏位显示
    /// </summary>
    public void HideSlotNumObj()
    {
        slotNum.SetActive(false);
    }

    /// <summary>
    /// 启用用Toggle
    /// </summary>
    public void EnableToggle(bool isEnabled)
    {
        if (null == checkbox)
        {
            return;
        } 
        //checkbox.value = false;
        //checkbox.enabled = false;
        if (checkbox.enabled != isEnabled)
        {
            checkbox.value = isEnabled;
            checkbox.enabled = isEnabled;
        }  
    }
}
