using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnBenefitsEnter : MonoBehaviour {

    //[HideInInspector]
    public DailyBenefitsType dailyBenefitsType;
    public BtnDisable btnDisable;
    public Warnings warnings;
	// Use this for initialization
	void Start () {
	warnings=PanelStatic.StaticWarnings;
	}


    void OnClick()
    {

		if(!InRoom.GetInRoomInstantiate().ServerConnected||Application.internetReachability==NetworkReachability.NotReachable)
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info637"));
			return;
		}		
        if (!btnDisable.Disable)
        {
            switch (dailyBenefitsType)
            {
                case DailyBenefitsType.First:
				
				if(BtnGameManager.yt.Rows[0]["IsGetFirstVIP"].YuanColumnText == "0")
				{
					if(BtnGameManager.yt.Rows[0]["IsFirstVIP"].YuanColumnText == "1")
					{
						PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("StoreMoveOn",SendMessageOptions.DontRequireReceiver);
//						int numGold = 0;
//						int numBlood=0;
//						List<string> listItem=new List<string>();
//						foreach (SpriteForBenefits item in BtnSelect.listSprite)
//                        {
//                            if (item.gameObject.active == true)
//                            {
//                                
//                                switch (item.benefitsType)
//                                {
//                                    case BenefitsType.Gold:
//                                        numGold += int.Parse(item.benefitsValue);
//
//                                        break;
//                                    case BenefitsType.BloodStone:
//                                        numBlood += int.Parse(item.benefitsValue);
//                                        break;
//                                    case BenefitsType.Item:
//                                        listItem.Add (item.benefitsValue);
//                                        break;									
//                                }
//                            }
//                        }
//						GetItem (numGold.ToString (),numBlood.ToString (),listItem);
//                        BtnGameManager.yt.Rows[0]["IsGetFirstVIP"].YuanColumnText = "1";
//						btnDisable.Disable=true;
//						btnDisable.lblText.text=StaticLoc.Loc.Get("info323");						
					}
					else
					{
						PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("StoreMoveOn",SendMessageOptions.DontRequireReceiver);
					}
				}

                    break;
                case DailyBenefitsType.Salaries:
                    if (BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText == "1")
                    {
					PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.BenefitsSalaries,0,0,null));
//					btnDisable. 
//					foreach (SpriteForBenefits item in BtnSelect.listSprite)
//                        {
//                            if (item.gameObject.active == true)
//                            {
//								
//                                int num = 0;
//                                switch (item.benefitsType)
//                                {
//                                    case BenefitsType.Gold:
//                                        num = int.Parse(item.benefitsValue);
//										
//                                       
//										YuanBackInfo backGold=new YuanBackInfo(System.DateTime.Now.ToString ());
//										StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().ClientMoney (num.To16String (),"0",backGold)));
//                                        warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}",StaticLoc.Loc.Get("info413"),  num , StaticLoc.Loc.Get("info335")));
//                                        break;
//                                    	case BenefitsType.BloodStone:
//                                        num = int.Parse(item.benefitsValue);
//                                      
//                                	
//										YuanBackInfo backBlood=new YuanBackInfo(System.DateTime.Now.ToString ());
//										StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().ClientMoney (num.To16String (),"0",backBlood)));
//										warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info413"), num , StaticLoc.Loc.Get("info297")));
//                                        break;
//                                }
//                            }
//                        }
                        BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText = "0";
						btnDisable.Disable=true;
						btnDisable.lblText.text=StaticLoc.Loc.Get("info323");							
                    }
                    else
                    {
                        warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info414") );
                    }
                    break;
                case DailyBenefitsType.Rank:
                    if (BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText == "1")
                    {
                        if (BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText == "" || BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText == "0")
                        {
						warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info415"));

//                            foreach (SpriteForBenefits item in BtnSelect.listSprite)
//                            {
//                                if (item.gameObject.active)
//                                {
//                                    int num = 0;
//                                    switch (item.benefitsType)
//                                    {
//                                        case BenefitsType.Gold:
//                                            num = int.Parse(item.benefitsValue);
//									if(num!=0)
//									{
//                                            //BtnGameManager.yt.Rows[0]["Money"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["Money"].YuanColumnText) + num).ToString();
//                                    
//										YuanBackInfo backGold=new YuanBackInfo(System.DateTime.Now.ToString ());
//										    StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().ClientMoney (num.To16String (),"0",backGold)));
//										warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info413") , num , StaticLoc.Loc.Get("info335")));
//									}
//                                            break;
//                                        case BenefitsType.BloodStone:
//                                            num = int.Parse(item.benefitsValue);
//									if(num!=0)
//									{
//                                            //BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) + num).ToString();
//                                        
//										YuanBackInfo backBlood=new YuanBackInfo(System.DateTime.Now.ToString ());
//										StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().ClientMoney (num.To16String (),"0",backBlood)));
//										warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info413") , num , StaticLoc.Loc.Get("info297")));
//									}
//                                            break;
//                                    }
//                                }
//                            }
//                            BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText = "0";
//							btnDisable.Disable=true;
//							btnDisable.lblText.text=StaticLoc.Loc.Get("info323");								
//                        }
//                        else
//                        {
//						PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.BenefitsRank,0,0,null));
                        }
                    }
                    else
                    {
                        warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info416"));
                    }
                    break;
                case DailyBenefitsType.Guild:
                    if (BtnGameManager.yt.Rows[0]["CanGuildBenefits"].YuanColumnText == "1")
                    {
                        if (BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText == "" || BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText == "0")
                        {
						warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info417"));

//                            foreach (SpriteForBenefits item in BtnSelect.listSprite)
//                            {
//                                if (item.gameObject.active)
//                                {
//                                    int num = 0;
//                                    switch (item.benefitsType)
//                                    {
//                                        case BenefitsType.Gold:
//                                            num = int.Parse(item.benefitsValue);
//											if(num!=0)
//											{
//                                            //BtnGameManager.yt.Rows[0]["Money"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["Money"].YuanColumnText) + num).ToString();
//                                        YuanBackInfo backGold=new YuanBackInfo(System.DateTime.Now.ToString ());
//										InRoom.GetInRoomInstantiate ().ClientMoney (num.To16String (),"0",backGold);    
//										warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}",StaticLoc.Loc.Get("info413") ,  num , StaticLoc.Loc.Get("info335")));
//											}
//                                            break;
//                                        case BenefitsType.BloodStone:
//                                            num = int.Parse(item.benefitsValue);
//											if(num!=0)
//									{
//                                            //BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) + num).ToString();
//                                        YuanBackInfo backBlood=new YuanBackInfo(System.DateTime.Now.ToString ());
//										InRoom.GetInRoomInstantiate ().ClientMoney (num.To16String (),"0",backBlood);        
//										warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}",StaticLoc.Loc.Get("info413") , num , StaticLoc.Loc.Get("info297") ));
//									}
//                                            break;
//                                    }
//                                }
//                            }
//                            BtnGameManager.yt.Rows[0]["CanGuildBenefits"].YuanColumnText = "0";
//							btnDisable.Disable=true;
//							btnDisable.lblText.text=StaticLoc.Loc.Get("info323");								
                        }
                        else
                        {
						PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.BenefitsGuild,0,0,null));
                        }
                    }
                    else
                    {
                        warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info418"));
                    }
                    break;
            }
        }
        
    }
	
	public void GetItem(string mGold,string mBlood,List<string> listItem)
	{
		foreach(string item in listItem)
		{
			if(item.Substring (0,2)=="88")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewDaojuItemAsID", item, SendMessageOptions.DontRequireReceiver);
			}
			else if(item.Substring (0,2)=="72")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddNewRideItemAsID", item, SendMessageOptions.DontRequireReceiver);
			}
			else if(item.Substring (0,2)=="70")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagDigestItemAsID",  item, SendMessageOptions.DontRequireReceiver);
			}
			else if(item.Substring (0,2)=="71")
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagSoulItemAsID",  item, SendMessageOptions.DontRequireReceiver);
			}				
			else
			{
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("AddBagItemAsID", item, SendMessageOptions.DontRequireReceiver);
			}
		}
			PanelStatic.StaticWarnings.OpenBoxBar("0","0",listItem.ToArray ());
	}

   

}
