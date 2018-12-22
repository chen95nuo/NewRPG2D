using UnityEngine;
using System.Collections;

public class ShopDetail : MonoBehaviour {

    public GameObject objContent1;  //goods
    public GameObject objContent2;  //add black market pos

    public GameObject goodsType1to5;//parent GameObject of item,equip,card,skill,passive
    public UISprite goodsType6to8;  //UISprite of gold,crystal,exp

    public UISprite texPay;         //gold crystal texture
    public UILabel labelPay;        //gold crystal num

    public GameObject objVip;       //vip obj
    public UILabel labelVip;        //vip num
    public UISprite texFrame;       //icon frame
    public UISprite texCard;        //card icon
    public UISprite texOther;       //other icon
    public GameObject objPiece;     //items fragment
    public UILabel labelDesc;       //description

    public UILabel nameLabel;            //product name
    public GameObject objLv;        //level GameObject
    public GameObject objNum;       //num GameObject
    public UILabel lvOrNum;         //items gold crystal show num, other lv

    public GameObject butShow;      //button show box goods
    public GameObject butBuy;       //button buy

    public GameObject objBox;       //shop box detail

    //int mode;
    //int id;

    int itemId;                     //box item id
	
	private UIAtlas honorAtlas;
	private UIAtlas goldAtlas;

	void Awake () {
		honorAtlas = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon");
		goldAtlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas01");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(int mode, int id)
    {
        //this.mode = mode;
        //this.id = id;

        if (mode == 1)
        {
            ShopData data = ShopData.getData(id);

            if (data.viplevel > 0)
            {
                objVip.SetActive(true);
                labelVip.text = data.viplevel.ToString();
            }
            else
            {
                objVip.SetActive(false);
            }

            if (data.costtype1 == 1)
            {
				texPay.atlas = goldAtlas;
                texPay.spriteName = "crystal2";
            }
            else if (data.costtype1 == 2)
            {
				texPay.atlas = goldAtlas;
                texPay.spriteName = "gold";
            }
            labelPay.text = data.cost.ToString();

            if (data.goodstype == 1 || data.goodstype >= 6)
            {
                objLv.SetActive(false);
                objNum.SetActive(true);
            }
            else
            {
                objLv.SetActive(true);
                objNum.SetActive(false);
            }
            lvOrNum.text = "1";

            initShowBut(data.goodstype, data.itemId);

            ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, nameLabel, labelDesc);
        }
        else if (mode == 2)
        {
            BlackMarketData data = BlackMarketData.getData(id);

            objVip.SetActive(false);

            if (data.costtype == 1)
            {
				texPay.atlas = goldAtlas;
                texPay.spriteName = "crystal2";
            }
            else if (data.costtype == 2)
            {
				texPay.atlas = goldAtlas;
                texPay.spriteName = "gold";
            }
            labelPay.text = data.cost.ToString();

            if (data.goodstype == 1 || data.goodstype >= 6)
            {
                objLv.SetActive(false);
                objNum.SetActive(true);
            }
            else
            {
                objLv.SetActive(true);
                objNum.SetActive(false);
            }
            lvOrNum.text = data.number.ToString();

            initShowBut(data.goodstype, data.itemId);

            ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, nameLabel, labelDesc);

            if (data.goodstype > 5)
                nameLabel.text = data.name;
        }
		else if(mode == 4)
		{
			ShopPvpData data = ShopPvpData.getData(id);

            objVip.SetActive(false);

            if (data.costtype == 1)
            {
				texPay.atlas = goldAtlas;
                texPay.spriteName = "crystal2";
            }
            else if (data.costtype == 2)
            {
				texPay.atlas = goldAtlas;
                texPay.spriteName = "gold";
            }
			else if (data.costtype == 3)
			{
				texPay.atlas = honorAtlas;
				texPay.spriteName = "homC2";
			}
            labelPay.text = data.cost.ToString();

            if (data.goodstype == 1 || data.goodstype >= 6)
            {
                objLv.SetActive(false);
                objNum.SetActive(true);
            }
            else
            {
                objLv.SetActive(true);
                objNum.SetActive(false);
            }
            lvOrNum.text = data.number.ToString();

            initShowBut(data.goodstype, data.itemId);

            ShopUI.updateIconPic(data.goodstype, data.itemId, texFrame, texCard, texOther, objPiece, goodsType1to5, goodsType6to8, nameLabel, labelDesc);

            if (data.goodstype > 5)
                nameLabel.text = data.name;
		}

        gameObject.SetActive(true);
    }

    void initShowBut(int goodsType, int itemId)
    {
        if (goodsType == 1 && ItemsData.getData(itemId).type == 5)
        {
            butShow.SetActive(true);
            butBuy.transform.localPosition = new Vector3(100, butBuy.transform.localPosition.y, butBuy.transform.localPosition.z);

            this.itemId = itemId;
        }
        else
        {
            butShow.SetActive(false);
            butBuy.transform.localPosition = new Vector3(0, butBuy.transform.localPosition.y, butBuy.transform.localPosition.z);
        }
    }

    void OnBtnShowBox()
    {
        objBox.GetComponent<ShopBoxShow>().show(itemId);
    }
}
