/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: JR
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectChannel : MonoBehaviour {
	public static SelectChannel SC;
	public yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("MapLevel","id");
	void Start () 
	{
		yt.Add(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID" , Application.loadedLevelName.Substring(3,3)));
		SC = this;
//		Dictionary<int, bool> selectchannel = new Dictionary<int, bool>();
//
//		for(int i=0;i<30;i++)
//		{
//			selectchannel.Add(i, i%2==0);
//		}
//		ShowSelectChannel(selectchannel);
	}
	
	void Update () 
	{
	}
	public UIGrid gridChanel;
	public UILabel LblMyChannel;
	public UILabel LblMyChannelOther;
	public GameObject btnChannel;
	private int selectedID = -1;

	private Dictionary<int, List<string>> channelDic = new Dictionary<int, List<string>>();
	private int index = 0;
	private int size = 1000;




    void OnEnable()
    {
		ServerRequest.requestDuelMapList();
//        if (channelDic.Count>0)
//        {
//
//            foreach(int key in channelDic.Keys)
//            {
//                GameObject objBtn = (GameObject)Instantiate(btnChannel);
//                objBtn.transform.parent = gridChanel.transform;
//                objBtn.transform.localScale = new Vector3(1, 1, 1);
//                UIButtonMessage btnMessage = objBtn.GetComponent<UIButtonMessage>();
//                btnMessage.target = this.gameObject;
//                btnMessage.functionName = "SelectClick";
//                PVPBattleItem tempBtn = objBtn.GetComponent<PVPBattleItem>();
//                tempBtn.ItemID = key;
//                //				index = tempBtn.ItemID;
//                //				childItems.Add(tempBtn);
//
//            }
//        }
    }
	public void ShowSelectChannel(Dictionary<int, bool> selectchannel)
	{
		index = 0;
		int count = 0;
        List<string> channelInfoList = new List<string>();
		foreach(KeyValuePair<int, bool> kvp in selectchannel)
		{
			int val = kvp.Value ? 1 : 0;
			string tempStr = string.Format("{0},{1}", kvp.Key, val);
			channelInfoList.Add(tempStr);
			count ++;

			if(count >= size)
			{
				channelDic.Add(index, channelInfoList);
				index ++	;
				count = 0	;
				channelInfoList = new List<string>();
			}
		}

		if(selectchannel.Count < size)
		{
			channelDic.Add(0, channelInfoList);
		}

        if (channelDic.Count >= 0)
        {

            foreach (int key in channelDic.Keys)
            {
                GameObject objBtn = (GameObject)Instantiate(btnChannel);
                objBtn.transform.parent = gridChanel.transform;
                objBtn.transform.localScale = new Vector3(1, 1, 1);
                UIButtonMessage btnMessage = objBtn.GetComponent<UIButtonMessage>();
                btnMessage.target = this.gameObject;
                btnMessage.functionName = "SelectClick";
                PVPBattleItem tempBtn = objBtn.GetComponent<PVPBattleItem>();
				tempBtn.SetBattleName(string.Format("{0}{1}{2}",yt.Rows[0]["MapName"].YuanColumnText,(key+1).ToString(),StaticLoc.Loc.Get("info1154")));
                tempBtn.ItemID = key;

				Transform firstTran = this.gridChanel.GetChild(0);
				firstTran.GetComponent<UIToggle>().value = true;
				SelectClick(firstTran.gameObject);
            }
        }
		gridChanel.repositionNow = true;

	}

    public UIGrid gridChanel1;
    public GameObject btnChannelMap;

	private int MyInID;
	List<PVPBattleItem> childItems = new List<PVPBattleItem>();
	public void SelectClick(GameObject obj)
	{

		foreach (PVPBattleItem CI in childItems)
		{
			CI.gameObject.SetActive(false);
		}

        PVPBattleItem item = obj.GetComponent<PVPBattleItem>();

        if(channelDic.ContainsKey(item.ItemID))
        {
            List<string> channels = channelDic[item.ItemID];

            for (int i = 0; i < channels.Count;i++)
            {
                string[] channelInfo = channels[i].Split(',');
				if(i >= gridChanel1.transform.childCount){
					GameObject objBtn = (GameObject)Instantiate(btnChannelMap);
	                objBtn.transform.parent = gridChanel1.transform;
	                objBtn.transform.localScale = new Vector3(1, 1, 1);
					objBtn.SetActive(false);
					UIButtonMessage btnMessage = objBtn.GetComponent<UIButtonMessage>();
					btnMessage.target = this.gameObject;
					btnMessage.functionName = "BtnToMap";
					PVPBattleItem tempBtn = objBtn.GetComponent<PVPBattleItem>();
					tempBtn.SetBattleName(string.Format("{0}{1}{2}",yt.Rows[0]["MapName"].YuanColumnText,(i+1).ToString(),StaticLoc.Loc.Get("info1153")));
//					Debug.Log("ran=============================="+channelInfo[1].ToString());
					if(int.Parse(channelInfo[1])==1){
						tempBtn.SetBattleLblbusy(StaticLoc.Loc.Get("info1160"));

					}
				
					if(int.Parse(channelInfo[1])==0){
						tempBtn.SetBattleLblbusy(StaticLoc.Loc.Get("info1161"));
					}

					tempBtn.ItemID = int.Parse(channelInfo[0]);
					childItems.Add(tempBtn);

				}
                
				childItems[i].ItemID = int.Parse(channelInfo[0]);
				childItems[i].gameObject.SetActive(true);
				if(PlayerUtil.mapInstanceID==childItems[i].ItemID){
					LblMyChannel.text = string.Format("{0}{1}{2}{3}",StaticLoc.Loc.Get("info1162"),yt.Rows[0]["MapName"].YuanColumnText,(i+1).ToString(),StaticLoc.Loc.Get("info1153"));
				}

				if(LblMyChannelOther){
					LblMyChannelOther.text = LblMyChannel.text;
				}
            }
			gridChanel1.repositionNow = true;
            
        }
	}

	public void BtnToMap(GameObject obj){
		PVPBattleItem item = obj.GetComponent<PVPBattleItem>();
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SelectChannel", item.ItemID , SendMessageOptions.DontRequireReceiver);
		//		item.ItemID		//item.ItemID	就是地图实例ID
//		ServerRequest.requestAddToMap(MapId,point);
	}

}
