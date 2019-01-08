using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class AllLeveReward : MonoBehaviour {
	public static AllLeveReward All;
	
	void Awake()
	{
		All = this;
	}

	void OnEnable(){
		InRoom.GetInRoomInstantiate().GetLevelPackInfo();
		BtnShowState();
	}

	public AllLeveItem[] LeveMe;

	public UIButton[] AllBtn;

	private Dictionary<int, AllLeveItem> itemDic = new Dictionary<int, AllLeveItem>();
	void Start()
	{
		for(int i=0;i<LeveMe.Length;i++)
		{
			itemDic.Add(LeveMe[i].MyBtnLeve, LeveMe[i]);
		}
	}

	public void ShowLeveItem(Dictionary<int, object> info){
		foreach (KeyValuePair<int, object> kvp in info){
			int key = kvp.Key;
			Dictionary<string,string> value = ((Dictionary<object, object>)kvp.Value).DicObjTo<string, string>();
			AllLeveItem item = null;
			if(itemDic.TryGetValue(key, out item))
			{
//				for(int i=0;i<item.LeveMe.Length;i++)
//				{
				item.LeveMe[0].picEnabled.spriteName = "Gold";
				if(!string.IsNullOrEmpty(value["gold"]) && int.Parse(value["gold"])>0)
				{
					item.LeveMe[0].lblNum.text = value["gold"];
					item.LeveMe[0].gameObject.SetActive(true);
				}
				else
				{
					item.LeveMe[0].gameObject.SetActive(false);
				}

				item.LeveMe[1].picEnabled.spriteName = "Bloodstone";
				if(!string.IsNullOrEmpty(value["blood"]) && int.Parse(value["blood"])>0)
				{
					item.LeveMe[1].lblNum.text = value["blood"];
					item.LeveMe[1].gameObject.SetActive(true);
				}
				else
				{
					item.LeveMe[1].gameObject.SetActive(false);
				}
				item.LeveMe[2].picEnabled.spriteName = "UIM-Crystals";
				if(!string.IsNullOrEmpty(value["MarrowIron"]) && int.Parse(value["MarrowIron"])>0)
				{
					item.LeveMe[2].lblNum.text = value["MarrowIron"];
					item.LeveMe[2].gameObject.SetActive(true);
				}
				else
				{
					item.LeveMe[2].gameObject.SetActive(false);
				}

				item.LeveMe[3].picEnabled.spriteName = "UIM-Sulfur";
				if(!string.IsNullOrEmpty(value["MarrowGold"]) && int.Parse(value["MarrowGold"])>0)
				{
					item.LeveMe[3].lblNum.text = value["MarrowGold"];
					item.LeveMe[3].gameObject.SetActive(true);
				}
				else
				{
					item.LeveMe[3].gameObject.SetActive(false);
				}
				item.LeveMe[4].picEnabled.spriteName = "624";
				if(!string.IsNullOrEmpty(value["soul"]) && int.Parse(value["soul"])>0)
				{
					item.LeveMe[4].lblNum.text = value["soul"];
					item.LeveMe[4].gameObject.SetActive(true);
				}
				else
				{
					item.LeveMe[4].gameObject.SetActive(false);
				}

//				if(string.IsNullOrEmpty(item.LeveMe[5].ItemID))
//				{
					if(!string.IsNullOrEmpty(value["Item"]))
					{
						string rewardsStr = value["Item"] ;
						string itemCount = null;
						string[] rewards =  rewardsStr.Split(';');
						for(int j=0;j<rewards.Length;j++){
						if(!string.IsNullOrEmpty(rewards[0])){
						string[] reward = rewards[0].Split(',');
						string itemID = reward[0];
						if(reward.Length>1&&!string.IsNullOrEmpty(reward[1])){
							itemCount = reward[1];
						}else
						{
							itemCount = "01";
						}
						string str = string.Format("{0},{1}",itemID,itemCount);
						object[] parms = new object[4];
						parms[0] = str;
						parms[1] = item.LeveMe[5].picEnabled;
						parms[2] = item.LeveMe[5].picQualit;
						parms[3] = "";
						PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms, SendMessageOptions.DontRequireReceiver);
						item.LeveMe[5].lblNum.text = itemCount;
						item.LeveMe[5].ItemID = itemID;	
						item.LeveMe[5].gameObject.SetActive(true);
						item.LeveMe[6].gameObject.SetActive(false);
						}

									if(!string.IsNullOrEmpty(rewards[1])){
										string[] reward1 = rewards[1].Split(',');
										string itemID1 = reward1[0];
										if(reward1.Length>1&&!string.IsNullOrEmpty(reward1[1])){
											itemCount = reward1[1];
										}else
										{
											itemCount = "01";
										}
										string str1 = string.Format("{0},{1}",itemID1,itemCount);
										object[] parms1 = new object[4];
										parms1[0] = str1; 
										parms1[1] = item.LeveMe[6].picEnabled;
										parms1[2] = item.LeveMe[6].picQualit;
										parms1[3] = "";
										PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms1, SendMessageOptions.DontRequireReceiver);
										item.LeveMe[6].lblNum.text = itemCount;
										item.LeveMe[6].ItemID = itemID1;
										item.LeveMe[6].gameObject.SetActive(true);
								}
							
							else
							{
								item.LeveMe[6].gameObject.SetActive(false);
							}
						}
					}
					else
					{
						item.LeveMe[5].gameObject.SetActive(false);
						item.LeveMe[6].gameObject.SetActive(false);
					}

				item.uiGrid.repositionNow = true;
			}
		}


		return;

	}


	public void BtnShowState(){

		int Leve = int.Parse(BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText);
//		Debug.Log("ran========================================"+Leve.ToString());
 
		string[] HasLeve =  BtnGameManager.yt.Rows[0]["hasLevePacks"].YuanColumnText.Split (';');
//		string[] HasLeve = new string[4];
//		HasLeve[0] = "5";
//		HasLeve[1] = "10";
//		HasLeve[2] = "15";
//		HasLeve[3] = "20";

		bool isOne = true;
		bool isTwo = true;
		bool isThree = true;
		bool isFour = true;
		bool isFive = true;
		bool isSix = true;
		bool isSeven = true;
		bool isEight = true;
		bool isNine = true;

		for(int i=0;i<HasLeve.Length;i++)
		{
			for(int j = 0 ; j<AllBtn.Length ; j++)
			{
				AllBtn[j].isEnabled = true;
			}

			if(Leve>=5&&isOne){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==5)
			{
				AllBtn[0].isEnabled = false;
				isOne = false;
				for(int k = 0 ; k<LeveMe[0].LeveMe.Length ; k++){
				LeveMe[0].LeveMe[k].Disable = true;
					}
				
			}else{
				AllBtn[0].isEnabled = true;	
				}
			}else{
				AllBtn[0].isEnabled = false;
			}

			if(Leve>=10&&isTwo){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==10)
			{
				AllBtn[1].isEnabled = false;
				isTwo =  false;
				for(int k = 0 ; k<LeveMe[1].LeveMe.Length ; k++){
						LeveMe[1].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[1].isEnabled = true;	
				}
			}else{
				AllBtn[1].isEnabled = false;
			}

			if(Leve>=15&&isThree){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==15)
			{
				AllBtn[2].isEnabled = false;
					isThree = false;
				for(int k = 0 ; k<LeveMe[2].LeveMe.Length ; k++){
						LeveMe[2].LeveMe[k].Disable = true;
					}
			}else{

				AllBtn[2].isEnabled = true;
				
				}
			}else{
				AllBtn[2].isEnabled = false;
			}
		
			if(Leve>=20&&isFour){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==20)
			{
				AllBtn[3].isEnabled = false;
				isFour = false;
			for(int k = 0 ; k<LeveMe[3].LeveMe.Length ; k++){
						LeveMe[3].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[3].isEnabled = true;
				
				}
			}else{
				AllBtn[3].isEnabled = false;
			}

			if(Leve>=25&&isFive){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==25)
			{
				AllBtn[4].isEnabled = false;
					isFive = false;
			for(int k = 0 ; k<LeveMe[4].LeveMe.Length ; k++){
						LeveMe[4].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[4].isEnabled = true;
				
				}
			}else{
				AllBtn[4].isEnabled = false;
			}

			if(Leve>=30&&isSix){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==30)
			{
				AllBtn[5].isEnabled = false;
					isSix = false;
			for(int k = 0 ; k<LeveMe[5].LeveMe.Length ; k++){
						LeveMe[5].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[5].isEnabled = true;
				
				}
			}else{
				AllBtn[5].isEnabled = false;
			}

			if(Leve>=40&&isSeven){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==40)
			{
				AllBtn[6].isEnabled = false;
					isSeven = false;
			for(int k = 0 ; k<LeveMe[6].LeveMe.Length ; k++){
						LeveMe[6].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[6].isEnabled = true;
				
				}
			}else{
				AllBtn[6].isEnabled = false;
			}

			if(Leve>=50&&isEight){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==50)
			{
				AllBtn[7].isEnabled = false;
				isEight = false;
			for(int k = 0 ; k<LeveMe[7].LeveMe.Length ; k++){
						LeveMe[7].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[7].isEnabled = true;
				
				}
			}else{
				AllBtn[7].isEnabled = false;
			}

			if(Leve>=60&&isNine){
			if(HasLeve[i]!=""&&int.Parse(HasLeve[i])==60)
			{
				AllBtn[8].isEnabled = false;
					isNine = false;
			for(int k = 0 ; k<LeveMe[8].LeveMe.Length ; k++){
					LeveMe[8].LeveMe[k].Disable = true;
					}
			}else{

					AllBtn[8].isEnabled = true;
				}
			}else{
				AllBtn[8].isEnabled = false;
			}



		}
	}

}
