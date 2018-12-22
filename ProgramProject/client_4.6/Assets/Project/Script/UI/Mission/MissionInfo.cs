using UnityEngine;
using System.Collections.Generic;

public class MissionInfo : MonoBehaviour {
	
	public UILabel level;
	public GameObject[] drops;
	public GameObject dropNo;
	public GameObject dropReward;
	public GameObject swap;
	public GameObject enter;
	public GameObject[] stars;
	public UILabel timesLabel;
	public GameObject koReward;
	public GameObject koNo;
	public UILabel costPower;
	public GameObject cost;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setData(int mdId,int starNum,int bonus,int times,GameObject target,string enterFunctionName,string swapFunctionName,string dropFunctionName,int canEnter)
	{
		MissionData md=MissionData.getData(mdId);
		costPower.text=md.cost+"";
		
		if(PlayerInfo.getInstance().player.level<md.unlocklevel)
		{
			level.text=TextsData.getData(472).chinese.Replace("num",md.unlocklevel+"");
			//level.color=Color.red;
			cost.SetActive(false);
		}
		else
		{
			level.text="";
			//level.color=Color.white;
			cost.SetActive(true);
		}
		foreach(GameObject drop in drops)
		{
			drop.SetActive(false);
		}
		List<string> dropInfos=new List<string>();
		//==只显示equip、item==//
		for(int i=0;i<md.drops.Count;i++)
		{
			string[] ss=md.drops[i].Split('-');
			int droptype=StringUtil.getInt(ss[0]);
			if(droptype==1 || droptype==2)
			{
				dropInfos.Add(md.drops[i]);
			}
		}
		if(dropInfos.Count>0)
		{
			dropReward.SetActive(true);
			dropNo.SetActive(false);
		}
		else
		{
			dropReward.SetActive(false);
			dropNo.SetActive(true);
			dropNo.GetComponent<UILabel>().text=TextsData.getData(585).chinese;
		}
		for(int i=0;i<dropInfos.Count;i++)
		{
			if(i>=drops.Length)
			{
				continue;
			}
			drops[i].SetActive(true);
			
			SimpleCardInfo2 sci2 = drops[i].GetComponent<SimpleCardInfo2>();
			
			string[] ss=dropInfos[i].Split('-');
			int droptype=StringUtil.getInt(ss[0]);
			string dropitem=StringUtil.getString(ss[1]);
			//int pro=StringUtil.getInt(ss[2]);
			
			string[] dropSs=dropitem.Split(',');
			int dropId = StringUtil.getInt(dropSs[0]);;
			GameHelper.E_CardType type = GameHelper.E_CardType.E_Equip;
			switch(droptype)
			{
			case 1://==items==//
				type = GameHelper.E_CardType.E_Item;
				break;
			case 2://==equip==//
				type = GameHelper.E_CardType.E_Equip;
				break;
			}
			sci2.clear();
			sci2.setSimpleCardInfo(dropId, type);
			//==点击事件==//
			drops[i].GetComponent<RewardsItem>().rewData=droptype+"-"+dropId+",1";
		}
		
		if(md.addtasktype>0)
		{
			koReward.SetActive(true);
			koReward.GetComponent<KORewardsItem>().setData(mdId,bonus);
			koNo.SetActive(false);
		}
		else
		{
			koReward.SetActive(false);
			koNo.SetActive(true);
		}
		
		UIButtonMessage message=enter.GetComponent<UIButtonMessage>();
		UIButtonMessage swapUBM = swap.GetComponent<UIButtonMessage>();
		if(canEnter==1)
		{
			message.GetComponent<UISprite>().color=Color.white;
			message.target=target;
			message.functionName=enterFunctionName;
			message.param=md.id;
			
			if(starNum<3)
			{
				swap.GetComponent<UISprite>().color=Color.gray;
				swapUBM.target=target;
				swapUBM.functionName = swapFunctionName;
				swapUBM.param = 0;
			}
			else
			{
				swap.GetComponent<UISprite>().color=Color.white;
				swapUBM.target = target;
				swapUBM.functionName = swapFunctionName;
				swapUBM.param = md.id;
			}
		}
		else
		{
			message.GetComponent<UISprite>().color=Color.gray;
			message.target=null;
			swap.GetComponent<UISprite>().color=Color.gray;
            swapUBM.target = target;
            swapUBM.functionName = swapFunctionName;
		}
		
		int lastTimes = md.times - times;
		if(md.type == 2)
		{
			timesLabel.gameObject.SetActive(true);
			timesLabel.text=TextsData.getData(357).chinese+" "+lastTimes;
		}
		else if(md.type == 1)
		{
			timesLabel.gameObject.SetActive(false);
			timesLabel.text="";
		}
		
		switch(starNum)
		{
		case 0:
			stars[0].GetComponent<UISprite>().spriteName="map_star_2";
			stars[1].GetComponent<UISprite>().spriteName="map_star_2";
			stars[2].GetComponent<UISprite>().spriteName="map_star_2";
			break;
		case 1:
			stars[0].GetComponent<UISprite>().spriteName="map_star";
			stars[1].GetComponent<UISprite>().spriteName="map_star_2";
			stars[2].GetComponent<UISprite>().spriteName="map_star_2";
			break;
		case 2:
			stars[0].GetComponent<UISprite>().spriteName="map_star";
			stars[1].GetComponent<UISprite>().spriteName="map_star";
			stars[2].GetComponent<UISprite>().spriteName="map_star_2";
			break;
		case 3:
			stars[0].GetComponent<UISprite>().spriteName="map_star";
			stars[1].GetComponent<UISprite>().spriteName="map_star";
			stars[2].GetComponent<UISprite>().spriteName="map_star";
			break;
		}
	}
}
