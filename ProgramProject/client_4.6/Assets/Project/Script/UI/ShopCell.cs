using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopCell : MonoBehaviour {

    public GameObject objContent1;  //goods
    public GameObject objContent2;  //add black market pos

    public GameObject goodsType1to5;//parent GameObject of item,equip,card,skill,passive
    public UISprite goodsType6to8;  //UISprite of gold,crystal,exp,honor
    public UILabel refreshNum;      //black market refresh num

    public UILabel labelName;       //label name

    public UISprite texPay;         //gold crystal texture
    public UILabel labelPay;        //gold crystal num

    public GameObject objVip;       //vip obj
    public UILabel labelVip;        //vip num
    public GameObject objSellOut;   //sell out texture
    public UISprite texFrame;       //icon frame
    public UISprite texCard;        //card icon
    public UISprite texOther;       //other icon
    public GameObject objPiece;     //items fragment

    //int mode;                       //1-shop 2-black market 3-black market add pos 4-shoppvp
    //ShopData shopData;              //shop data
    //BlackMarketData blackData;      //black market data
    BlackShopboxData blackAdd;      //black add data
	//ShopPvpData shopPvpData;
	
	private UIAtlas honorAtlas;
	private UIAtlas goldAtlas;

	// Use this for initialization
	void Awake () {
		honorAtlas = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon");
		goldAtlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas01");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(ShopData data)
    {
        //mode = 1;
        //this.shopData = data;

        if (data.viplevel > 0)
        {
            objVip.SetActive(true);
            labelVip.text = data.viplevel.ToString();
        }

        if (data.costtype1 == 2)
        {
			texPay.atlas = goldAtlas;
            texPay.spriteName = "gold";
        }
        labelPay.text = data.cost.ToString();

        ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, labelName);
    }

    public void Init(BlackMarketData data)
    {
        //mode = 2;
        //this.blackData = data;

        if (data.number > 1)
            refreshNum.text = "x" + data.number;

        if (data.costtype == 2)
        {
			texPay.atlas = goldAtlas;
            texPay.spriteName = "gold";
        }
        labelPay.text = data.cost.ToString();

        ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, labelName);

        if (data.goodstype > 5)
            labelName.text = data.name;//黑市中goodsType6-8的在updateIconPic中name将会赋"",所以在此重新赋值//
    }
	
	public void Init(ShopPvpData data)
	{
		//mode = 4;
		//this.shopPvpData = data;
	    if (data.number > 1)
        	refreshNum.text = "x" + data.number;
		if(data.costtype == 3)
		{
			texPay.atlas = honorAtlas;
			texPay.spriteName = "homC2";
		}
		labelPay.text = data.cost.ToString();
		
		ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, labelName);
		
		if (data.goodstype > 5)
            labelName.text = data.name;//黑市中goodsType6-8的在updateIconPic中name将会赋"",所以在此重新赋值//
	}
	
	//public void Init()
    public void Init(int num)
    {
        //mode = 3;
        this.blackAdd = BlackShopboxData.getData(num);

        if (blackAdd.costtype == 1)
        {
			texPay.atlas = goldAtlas;
            texPay.spriteName = "crystal2";
        }
        else if (blackAdd.costtype == 2)
        {
			texPay.atlas = goldAtlas;
            texPay.spriteName = "gold";
        }
        labelPay.text = blackAdd.cost.ToString();

        objContent1.SetActive(false);
        objContent2.SetActive(true);
    }

    public void SellOut(bool flag)
    {
        objSellOut.SetActive(flag);
        gameObject.GetComponent<BoxCollider>().enabled = !flag;
    }
}
