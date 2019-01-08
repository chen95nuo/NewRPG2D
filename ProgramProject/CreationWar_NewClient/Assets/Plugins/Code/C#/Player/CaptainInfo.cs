using UnityEngine;
using System.Collections;

public class CaptainInfo : MonoBehaviour {

    public string teamID;
    public UISprite picHead;
    public UILabel lblName;
    public UILabel lblPro;
    public UILabel lblLevel;
    public UILabel lblRanking;
    public UILabel lblNotice;
    public yuan.YuanMemoryDB.YuanRow yr = null;
    private YuanPicManager yuanPicManager;

    void Start()
    {
        //yuanPicManager = GameObject.Find("YuanPicManager").GetComponent<YuanPicManager>();
        yuanPicManager = PanelStatic.StaticYuanPicManger;
    }

    public void RefreshPlayerInfo()
    {
        if (yr != null)
        {
            lblName.text = yr["PlayerName"].YuanColumnText.Trim();
            lblLevel.text = yr["PlayerLevel"].YuanColumnText.Trim();
            lblPro.text = RefreshList.GetPro(yr["ProID"].YuanColumnText.Trim());
            lblRanking.text = yr["VSRanking"].YuanColumnText.Trim();
            int numPro = int.Parse(yr["ProID"].YuanColumnText.Trim());
            picHead.atlas = yuanPicManager.picPlayer[numPro-1].atlas;
            picHead.spriteName = yuanPicManager.picPlayer[numPro-1].spriteName;
        }
    }
}
