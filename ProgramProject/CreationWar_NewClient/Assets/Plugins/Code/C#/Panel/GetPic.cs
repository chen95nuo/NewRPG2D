using UnityEngine;
using System.Collections;

public class GetPic : MonoBehaviour {

    public YuanPicManager yuanPicManager;
    public UISprite picBackground;
    public UISprite picMark;
    public UISprite picMask;
    private string picID = "0,0,0";
    /// <summary>
    /// ͼƬID
    /// </summary>
    public string PicID
    {
        get { return picID; }
        set
        { 
            picID = value;
            SetPic();
        }
    }

    void Start()
    {
        //PicID = "0,0,0";
        yuanPicManager = PanelStatic.StaticYuanPicManger;
    }

    private void SetPic()
    {

        string[] strPic = picID.Split(',');
        int num;
            num = int.Parse(strPic[0]);

            SetPicInfo(picBackground, yuanPicManager.picSelectBackground[num]);

            num = int.Parse(strPic[1]);
            SetPicInfo(picMark, yuanPicManager.picSelectMark[num]);

            num = int.Parse(strPic[2]);
            SetPicInfo(picMask, yuanPicManager.picSelectMask[num]);
    }

    private void SetPicInfo(UISprite mSprite,YuanPic yuanPic)
    {
        mSprite.atlas = yuanPic.atlas;
        mSprite.spriteName = yuanPic.spriteName;
    }
}
