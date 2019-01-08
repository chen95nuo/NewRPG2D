using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetPic : MonoBehaviour {

    public YuanPicManager yuanPicManager;
    public GameObject ckbSelect;
    public UIGrid gridBackground;
    public UIGrid gridMark;
    public UIGrid gridMask;
    public GetPic getPic;

    [HideInInspector]
    public string picID = "0,0,0";


    
	// Use this for initialization
	void Start () {
        yuanPicManager = PanelStatic.StaticYuanPicManger;
        InsCbk(gridBackground, yuanPicManager.picSelectBackground, PicSelectClick.SelectType.Background);
        InsCbk(gridMark, yuanPicManager.picSelectMark, PicSelectClick.SelectType.Mark);
        InsCbk(gridMask, yuanPicManager.picSelectMask, PicSelectClick.SelectType.Mask);

	}
	


    private void InsCbk(UIGrid mParent, List<YuanPic> yuanPic, PicSelectClick.SelectType selectType)
    {
        int num = 0;
        foreach (YuanPic item in yuanPic)
        {
            UIToggle tempCkbSelect = ((GameObject)Instantiate(ckbSelect)).GetComponent<UIToggle>();
            tempCkbSelect.transform.parent = mParent.transform;
            UIPanel tempPanel =  tempCkbSelect.transform.GetComponent<UIPanel>();
            if (tempPanel != null)
            {
                Destroy(tempPanel);
            }
            PicSelectClick tempPicSelectClick = tempCkbSelect.GetComponent<PicSelectClick>();
            UISlicedSprite tempSprite = tempCkbSelect.transform.FindChild("Background").GetComponent<UISlicedSprite>();

            tempSprite.atlas = item.atlas;
            tempSprite.spriteName = item.spriteName;

            tempPicSelectClick.sender = this;
            tempPicSelectClick.selectType = selectType;
            tempPicSelectClick.num = num.ToString();

            tempCkbSelect.group = 3;
            if (num == 0)
            {
                tempCkbSelect.isChecked = true;
            }
            else
            {
                tempCkbSelect.isChecked = false;
            }
           
            tempCkbSelect.transform.localPosition = Vector3.zero;
            tempCkbSelect.transform.localScale = new Vector3(1, 1, 1);

            num++;
        }
       mParent.repositionNow = true;
    }

    public void OnCbkClick(PicSelectClick.SelectType selectType,string num)
    {
        string[] strPic = picID.Split(',');
        switch (selectType)
        {
            case PicSelectClick.SelectType.Background:
                {
                    strPic[0] = num;
                }
                break;
            case PicSelectClick.SelectType.Mark:
                {
                    strPic[1] = num;
                }
                break;
            case PicSelectClick.SelectType.Mask:
                {
                    strPic[2] = num;
                }
                break;
        }

        picID = string.Format("{0},{1},{2}", strPic[0], strPic[1], strPic[2]);
        getPic.PicID = this.picID;
    }
}
