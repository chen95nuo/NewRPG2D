using UnityEngine;
using System.Collections;

public class FristRewardControl : MonoBehaviour {
	public static FristRewardControl fristReward;
	public bool isFrist = false;
	private UIButtonMessage MyMessage;
	public UISprite Frist;
	public UISprite VIP;
	public UISprite Null;
	public UILabel LabText;
	public GameObject ObjLucky;
    public GameObject SprShan;
	// Use this for initialization
	void Awake () {
		fristReward = this;
		MyMessage = this.gameObject.transform.GetComponent<UIButtonMessage>();

	}

	void LateUpdate(){
		ShowVIP();
	}
	
	void OnClick(){
		isFrist = true;
	}

   public void ShowVIP(){
		int isChange = int.Parse(BtnGameManager.yt[0]["IsChargeReward"].YuanColumnText);
        int showVIP = PlayerPrefs.GetInt("ShowVIP", 1); // showVIP变量为0时表示审核中要隐藏VIP按钮，1表示不再审核中
		if(isChange==0){
			Frist.gameObject.SetActive(true);
			VIP.gameObject.SetActive(false);
			Null.gameObject.SetActive(false);
			MyMessage.enabled = true;
			LabText.enabled = true;
			MyMessage.functionName = "show39";
			LabText.text = StaticLoc.Loc.Get("info912");
			if(showVIP==1){
				ObjLucky.SetActive(true);
			}else{
				ObjLucky.SetActive(false);
			}
		}else{
			if(showVIP==1){
			Frist.gameObject.SetActive(false);
			VIP.gameObject.SetActive(true);
			Null.gameObject.SetActive(false);
			MyMessage.enabled = true;
			LabText.enabled = true;
			MyMessage.functionName = "show16";
			LabText.text = BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText;
			ObjLucky.SetActive(true);
            SprShan.SetActive(false);
			if(null!=DailyBenefitsPanelSelect.My){
			DailyBenefitsPanelSelect.My.btnCode.gameObject.SetActive(true);
				}
			}else{
			Frist.gameObject.SetActive(false);
			VIP.gameObject.SetActive(false);
			Null.gameObject.SetActive(true);
			MyMessage.enabled = false;
			LabText.enabled = false;
			ObjLucky.SetActive(false);
            SprShan.SetActive(false);
			if(null!=DailyBenefitsPanelSelect.My){
			DailyBenefitsPanelSelect.My.btnCode.gameObject.SetActive(false);
			}
			}
		}
	}
}
