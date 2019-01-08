using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContinuTopUp : MonoBehaviour {
    public UILabel oneTitle;
    public UISprite oneIcon;
    public UILabel oneInfo;

    public UILabel twoTitle;
    public UISprite twoIcon;
    public UILabel twoInfo;

    public UILabel threeTitle;
    public UISprite threeIcon;
    public UILabel threeInfo;

	public UIButton GetBtn3 ;
	public UIButton GetBtn5 ;
	public UIButton GetBtn7 ;
	public UILabel LabText;
	private Dictionary<string,string> rechargeDic = new Dictionary<string, string>();
	private string RechargeDay ;

	private string IsNumber;

	public static ContinuTopUp continuTopUp;

	// Use this for initialization
	void Awake () {
		continuTopUp = this;
	}

	void PutToDic()
	{
		RechargeDay  = BtnGameManager.yt[0]["NumberRechargeDay"].YuanColumnText.Trim();
		IsNumber  = BtnGameManager.yt[0]["IsNumberRecharge"].YuanColumnText.Trim();
		string[] StrIsNumber =  IsNumber.Split(';');

		rechargeDic.Clear();
		for(int i = 0; i<StrIsNumber.Length ; i++){
			string[] StrIs = StrIsNumber[i].Trim().Split(',');
			if(StrIs!=null)
				rechargeDic[StrIs[0]] = StrIs[1].Trim();
		}
	}

	public void OnEnable(){
		PutToDic();
		
		ShowTopUp(int.Parse(RechargeDay));

        //SetInfo();
	}

    //void SetInfo()
    //{
    //    Dictionary<string, Dictionary<string, object>> info = DynamicActivity.instance.GetNoticeInfo();
    //    Dictionary<string, object> dic = null;
    //    if (info.TryGetValue("Continuous", out dic))
    //    {
    //        Dictionary<string, string> one = ((Dictionary<object, object>)dic["one"]).DicObjTo<string, string>();
    //        Dictionary<string, string> two = ((Dictionary<object, object>)dic["two"]).DicObjTo<string, string>();
    //        Dictionary<string, string> three = ((Dictionary<object, object>)dic["three"]).DicObjTo<string, string>();

    //        oneTitle.text = one["title"];
    //        //oneIcon.spriteName = one["icon"];
    //        oneInfo.text = one["info"];

    //        twoTitle.text = two["title"];
    //        //twoIcon.spriteName = two["icon"];
    //        twoInfo.text = two["info"];

    //        threeTitle.text = three["title"];
    //        //threeIcon.spriteName = three["icon"];
    //        threeInfo.text = three["info"];
    //    }
    //}

    private bool isRecharge;
	void ShowTopUp(int number){
		LabText.text = RechargeDay;
        if (number < 3)
        {
            GetBtn3.isEnabled = false;
            GetBtn5.isEnabled = false;
            GetBtn7.isEnabled = false;
        }
        else if (number >= 3 && number < 5)
        {
            GetBtn3.isEnabled = !IsRecharge("3");
            GetBtn5.isEnabled = false;
            GetBtn7.isEnabled = false;
        }
        else if (number >= 5 && number < 7)
        {
            GetBtn3.isEnabled = !IsRecharge("3");
            GetBtn5.isEnabled = !IsRecharge("5");
            GetBtn7.isEnabled = false;
        }
        else
        {
            GetBtn3.isEnabled = !IsRecharge("3");
            GetBtn5.isEnabled = !IsRecharge("5");
            GetBtn7.isEnabled = !IsRecharge("7");
        }
	}

    bool IsRecharge(string day)
    {
        if (rechargeDic.ContainsKey(day))
        {
            return rechargeDic[day].Equals("0") ? false : true;
        }

        return false;
    }


	public  void GetReward3(){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().NumberRechargeDay(3));
	}

	public void GetReward5(){
			PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().NumberRechargeDay(5));
   }
	public void GetReward7(){
				PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().NumberRechargeDay(7));
 }
    public void BuyBloodStore()
    {
        PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);
    }
}
