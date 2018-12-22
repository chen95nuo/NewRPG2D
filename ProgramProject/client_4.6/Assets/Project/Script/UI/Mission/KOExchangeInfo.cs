using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KOExchangeInfo : MonoBehaviour{
	
	//public UILabel name;
	//public UIButtonMessage toExchange;
	public Transform icon;
	public UISprite turn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setData(int numName,GameObject target,List<KOawardElement> koInfo,int koNum,int index)
	{
		int id = 0;
		List<KOAwardData> kadList = new List<KOAwardData>();
		kadList = KOAwardData.getList();
		try
		{
			id = kadList[index].id;
		}
		catch(ArgumentOutOfRangeException)
		{
			icon.gameObject.SetActive(false);
			return;
		}
		icon.gameObject.SetActive(true);
		KOAwardData kad = KOAwardData.getData(id);
		string[] ss = kad.reward1.Split(',');
		int cardId = StringUtil.getInt(ss[0]);
		icon.GetComponent<SimpleCardInfo2>().clear();
		icon.GetComponent<SimpleCardInfo2>().setSimpleCardInfo(cardId,GameHelper.E_CardType.E_Hero,null);
		UILabel name = icon.FindChild("Child/Name").GetComponent<UILabel>();
		CardData cd = CardData.getData(cardId);
		name.text = cd.name;
		KOawardElement kae = koInfo[index];
		Transform exchangeBtn = icon.FindChild("Child/ExchangeBtn");
		Transform koNumTransform = icon.FindChild("Child/KoNum");
		if(kae == null)
		{
			exchangeBtn.gameObject.SetActive(false);
			koNumTransform.gameObject.SetActive(false);
		}
		else
		{
			if(kae.state == 1)
			{
				koNumTransform.gameObject.SetActive(false);
				exchangeBtn.gameObject.SetActive(true);
				turn.gameObject.SetActive(false);
				exchangeBtn.FindChild("Background").GetComponent<UISprite>().color = Color.grey;
				exchangeBtn.FindChild("Label").GetComponent<UILabel>().text = TextsData.getData(415).chinese;
				exchangeBtn.FindChild("Label").GetComponent<UILabel>().color = Color.grey;
				exchangeBtn.GetComponent<UIButtonMessage>().param = -1;
				icon.GetComponent<BoxCollider>().size = new Vector3(160,190,0);
			}
			else if(kae.state == 0)
			{
				if(koNum>=kad.number)
				{
					koNumTransform.gameObject.SetActive(false);
					exchangeBtn.gameObject.SetActive(true);
					turn.gameObject.SetActive(true);
					exchangeBtn.FindChild("Background").GetComponent<UISprite>().color = Color.white;
					exchangeBtn.FindChild("Label").GetComponent<UILabel>().text = TextsData.getData(414).chinese;
					exchangeBtn.FindChild("Label").GetComponent<UILabel>().color = Color.white;
					exchangeBtn.GetComponent<UIButtonMessage>().param = kad.id;
					icon.GetComponent<BoxCollider>().size = new Vector3(160,190,0);
				}
				else
				{
					koNumTransform.gameObject.SetActive(true);
					exchangeBtn.gameObject.SetActive(false);
					turn.gameObject.SetActive(false);
					koNumTransform.FindChild("Num").GetComponent<UILabel>().text = koNum+" / "+kad.number;
					koNumTransform.FindChild("Value").GetComponent<UISprite>().fillAmount = ((float)koNum)/kad.number;
					icon.GetComponent<BoxCollider>().size = new Vector3(160,250,0);
				}
			}
			exchangeBtn.GetComponent<UIButtonMessage>().functionName = "OnClickKoExchange";
			exchangeBtn.GetComponent<UIButtonMessage>().target = target;
			icon.GetComponent<UIButtonMessage>().param = kad.id;
			icon.GetComponent<UIButtonMessage>().target = target;
			icon.GetComponent<UIButtonMessage>().functionName = "onClickCardShowDetail";
		}
            //icon[i].FindChild("Child/Name").GetComponent<UILabel>().text = "[7D4900]"+CardData.getData(cardId).name+"[-]";
		
		//toExchange.target = target;
		//toExchange.param = numName;
		//toExchange.functionName = "onClickBtn";
	}
}
