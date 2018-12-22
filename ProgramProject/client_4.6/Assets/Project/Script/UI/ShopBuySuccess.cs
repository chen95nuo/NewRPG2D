using UnityEngine;
using System.Collections;

public class ShopBuySuccess : MonoBehaviour {

    public UILabel labTitle;
    public UILabel labName;
    public UILabel labNum;

    public GameObject goodsType1to5;//parent GameObject of item,equip,card,skill,passive
    public UISprite goodsType6to8;  //UISprite of gold,crystal,exp
    public UISprite texFrame;       //icon frame
    public UISprite texCard;        //card icon
    public UISprite texOther;       //other icon
    public GameObject objPiece;     //items fragment

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void show(int mode, int detailId, int buyNum)
    {
        if (mode != 1 && mode != 2) return;

        string sName = "";
        if (mode == 1)
        {
            ShopData data = ShopData.getData(detailId);
            ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, labName);
        }
        else if (mode == 2)
        {
            BlackMarketData data = BlackMarketData.getData(detailId);
            ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, labName);
            
            if (data.goodstype > 5) sName = data.name;
        }
        labTitle.text = TextsData.getData(599).chinese;
        labName.text = TextsData.getData(253).chinese + (labName.text.Equals("") ? sName : labName.text);//为""表示为黑市6,7,8类型物品,从黑市表中取名字,否则updateIconPic中赋值//
        labNum.text = "x" + (mode == 2 ? 1 : buyNum);
        
        gameObject.SetActive(true);
    }

    void OnBtnCloseSuccess()
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        gameObject.SetActive(false);
    }
}
