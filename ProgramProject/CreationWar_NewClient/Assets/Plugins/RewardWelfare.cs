using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardWelfare : MonoBehaviour {
    public UILabel activityContents;
    public UILabel oneTitle;
    public UILabel oneBlood;
    public UILabel twoTitle;
    public UILabel twoBlood;
    public UILabel threeTitle;
    public UILabel threeBlood;

	public UIButton GetBtn1;
	public UIButton GetBtn2;
	public UIButton GetBtn3;
	public UILabel LabBlood;

	private string WelfareNumber ;
	
	private string StrNumber;

	private int LeveMe;

	public static RewardWelfare  rewardWelfare;
	// Use this for initialization

	void Awake(){
		rewardWelfare = this;
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		WelfareNumber = BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText.Trim();
		LeveMe = int.Parse(WelfareNumber);
		PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GetGrowthWelfareInfo());

        //SetInfo();
    }

    //void SetInfo()
    //{
    //    Dictionary<string, Dictionary<string, object>> info = DynamicActivity.instance.GetNoticeInfo();
    //    Dictionary<string, object> dic = null;
    //    if (info.TryGetValue("Growup", out dic))
    //    {
    //        Dictionary<string, string> one = ((Dictionary<object, object>)dic["one"]).DicObjTo<string, string>();
    //        Dictionary<string, string> two = ((Dictionary<object, object>)dic["two"]).DicObjTo<string, string>();
    //        Dictionary<string, string> three = ((Dictionary<object, object>)dic["three"]).DicObjTo<string, string>();

    //        oneTitle.text = one["title"];
    //        oneBlood.text = one["blood"];

    //        twoTitle.text = two["title"];
    //        twoBlood.text = two["blood"];

    //        threeTitle.text = three["title"];
    //        threeBlood.text = three["blood"];

    //        int a = 0;
    //        int b = 0;
    //        int c = 0;
    //        if (int.TryParse(one["blood"], out a) && int.TryParse(two["blood"], out b) && int.TryParse(three["blood"], out c))
    //        {
    //            activityContents.text = string.Format("{0}{1}{2}", StaticLoc.Loc.Get("meg0197"), (a + b + c), StaticLoc.Loc.Get("meg0198"));
    //        }
    //    }
    //}

	public void ShowWelfare(int nowBlood,int WelfareLevel,int WelfareBlood){
		LabBlood.text = nowBlood.ToString();
		if(LeveMe>15&&WelfareBlood>300){
//		if(LeveMe<20){
//			GetBtn2.isEnabled = false;
//			GetBtn1.isEnabled = false;
//			GetBtn3.isEnabled = false;
//		}else if(LeveMe>=20&&LeveMe<30){
//				GetBtn1.isEnabled = true;
//				GetBtn2.isEnabled = false;
//				GetBtn3.isEnabled = false;
//		}else if(LeveMe>=30&&LeveMe<40){
//				GetBtn2.isEnabled = true;
//				GetBtn1.isEnabled = true;
//				GetBtn3.isEnabled = false;
//		}else if(LeveMe>=40){
				GetBtn1.isEnabled = true;
				GetBtn2.isEnabled = true;
				GetBtn3.isEnabled = true;
			}else{
			GetBtn1.isEnabled = false;
			GetBtn2.isEnabled = false;
			GetBtn3.isEnabled = false;
		}
	}

	public  void GetReward20(){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GetGrowthWelfare(20));
	}
	
	public void GetReward30(){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GetGrowthWelfare(30));
	}
	public void GetReward40(){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GetGrowthWelfare(40));
	}

	public void BuyBlood(){
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);
	}
}
