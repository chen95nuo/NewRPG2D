using UnityEngine;
using System.Collections;

public class PanelStatic : MonoBehaviour {
	
	public static PanelStatic panelStatic;
	
    public BtnGameManager btnGameManager;
    public BtnGameManagerBack btnGameManagerBack;
    public YuanPicManager yuanPicManger;
    public Warnings warnings;
    public TVMessage tvMessage;
    public SendManager sendManager;
    public GameObject itemInfo;

    public static BtnGameManager StaticBtnGameManager;
    public static BtnGameManagerBack StaticBtnGameManagerBack;
    public static YuanPicManager StaticYuanPicManger;
    public static Warnings StaticWarnings;
    public static TVMessage StaticTVMessage;
    public static SendManager StaticSendManager;
    public static GameObject StaticIteminfo;

    void Awake()
    {
		panelStatic=this;
        StaticBtnGameManager = this.btnGameManager;
        StaticBtnGameManagerBack = this.btnGameManagerBack;
        StaticSendManager = this.sendManager;
        StaticTVMessage = this.tvMessage;
        StaticWarnings = warnings;
        StaticYuanPicManger = yuanPicManger;
        StaticIteminfo = itemInfo;
    }


}
