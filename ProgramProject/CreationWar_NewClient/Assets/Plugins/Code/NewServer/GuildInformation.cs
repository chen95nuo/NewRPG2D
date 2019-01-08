using UnityEngine;
using System.Collections;

public enum LeveBuilds{


}
public class GuildInformation : MonoBehaviour {
	public GetPic getPic;
	public UILabel LabName;
	public UILabel lblHeaderName;
	public UILabel LabSecond;
	public UILabel LabNumber;
	public UILabel LabMoney;
	public UILabel LabNumberMax;
	public UILabel LabBulid;
	public UISprite SpriteBuild;
	public UILabel LabLevel;
	//公告
	public UILabel LabCement;

	public UILabel MyPosition;
	public UILabel MyContribution;
	public UILabel MyRanking;

	public UIInput SetNotice;
	public UIInput SetDeclaration;


	public UILabel LblSetNotice;
	public UILabel LblSetDeclaration;

	public GameObject onjSetNotice;
	public GameObject objSetDeclaration;

//	public GameObject btnSetNotice;
//	public GameObject btnSetDeclaration;

	public UIPanel BtnObjPanel;

	public UIPanel TextObjPanel;

    [HideInInspector]
    public yuan.YuanMemoryDB.YuanTable yt;
	// Use this for initialization

	public static GuildInformation guildInfoMation;
    void Awake()
    {
        yt = new yuan.YuanMemoryDB.YuanTable("GuidInformation" + this.name, "");
    }
	void Start () {
		guildInfoMation = this;		
		StartCoroutine(ShowText());
	}

	void OnEnable(){
	StartCoroutine(ShowText());
	StartCoroutine(ShowLity());
		InvokeRepeating("ShowLity",0,1f);
	}

	private bool isStop = true;
	private int IsNumber  = 0;
	IEnumerator ShowLity()
	{
		while(isStop)
		{
			StartCoroutine(ShowText());
			IsNumber++;
			if(IsNumber ==5)
			{
				isStop = false;
				IsNumber = 0;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void ShowBack(){
		StartCoroutine(ShowText());
	}

	private string[] strGuildHead=new string[2];
	IEnumerator ShowText(){
		
            InRoom.GetInRoomInstantiate().GetTableForID(BtnGameManager.yt[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.TableType.GuildInfo, yt);
            while (yt.IsUpdate)
            {
                yield return new WaitForSeconds(0.1f);
            }
        PanelStatic.StaticBtnGameManager.CloseLoading();
		getPic.PicID = yt[0]["PicID"].YuanColumnText.Trim();
        LabName.text = yt[0]["GuildName"].YuanColumnText.Trim();
        strGuildHead = yt[0]["GuildHeadID"].YuanColumnText.Trim().Split(',');
		lblHeaderName.text = strGuildHead[1];

   //     MyRanking.text = yt[0]["GuildRanking"].YuanColumnText.Trim();
        LabSecond.text = yt[0]["GuildDepHeadID"].YuanColumnText.Trim() ;
        LabMoney.text = yt[0]["GuildFunds"].YuanColumnText.Trim();

        if (int.Parse(BtnGameManager.yt.Rows[0]["GuildConbut"].YuanColumnText) == 0)
        {
            MyRanking.text = StaticLoc.Loc.Get("info971");
        }
        else {
            MyRanking.text = BtnGameManager.yt.Rows[0]["GuildConbut"].YuanColumnText;
        }




		LabNumber.text = (yt[0]["MemverID"].YuanColumnText.Trim().Split(';').Length-1).ToString();


		if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==1){
			MyPosition.text = StaticLoc.Loc.Get("info950");
			BtnObjPanel.enabled = true;
			SetNotice.gameObject.SetActive(true);
			SetDeclaration.gameObject.SetActive(true);
			TextObjPanel.enabled = true;
			onjSetNotice.SetActive(false);
			objSetDeclaration.SetActive(false);
		}else if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==2){
			MyPosition.text = StaticLoc.Loc.Get("info947");
			BtnObjPanel.enabled = true;
			TextObjPanel.enabled = true;
			LblSetNotice.gameObject.SetActive(false);
			LblSetDeclaration.gameObject.SetActive(false);
			onjSetNotice.SetActive(false);
			objSetDeclaration.SetActive(false);
		}else if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==3){
			MyPosition.text = StaticLoc.Loc.Get("info948");
			BtnObjPanel.enabled = false;
			TextObjPanel.enabled = false;
			LblSetNotice.gameObject.SetActive(true);
			LblSetDeclaration.gameObject.SetActive(true);
		}else if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==4){
			MyPosition.text = StaticLoc.Loc.Get("buttons705");
			BtnObjPanel.enabled = false;
			TextObjPanel.enabled = false;
			LblSetNotice.gameObject.SetActive(true);
			LblSetDeclaration.gameObject.SetActive(true);
        }else if (int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==5){
            MyPosition.text = StaticLoc.Loc.Get("info964");
			BtnObjPanel.enabled = false;
			TextObjPanel.enabled = false;
			LblSetNotice.gameObject.SetActive(true);
			LblSetDeclaration.gameObject.SetActive(true);
        }else if (int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==0){
			MyPosition.text = StaticLoc.Loc.Get("info949");
			BtnObjPanel.enabled = false;
			TextObjPanel.enabled = false;
			LblSetNotice.gameObject.SetActive(true);
			LblSetDeclaration.gameObject.SetActive(true);
		}

//		MyPosition.text = BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText;
		MyContribution.text = BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText;

		SetNotice.text = yt[0]["GuildNotice"].YuanColumnText.Trim();
		SetDeclaration.text = yt[0]["GuildDeclaration"].YuanColumnText.Trim();
		LblSetNotice.text = yt[0]["GuildNotice"].YuanColumnText.Trim();
		LblSetDeclaration.text = yt[0]["GuildDeclaration"].YuanColumnText.Trim();
		LabNumberMax.text = "/"+yt[0]["PlayerNumber"].YuanColumnText.Trim();
		
		LabLevel.text = yt[0]["GuildLevel"].YuanColumnText.Trim();
		//公会等级
		int leve =  int.Parse(yt[0]["GuildLevel"].YuanColumnText.Trim());
		//建设值
		float BuildNum = float.Parse(yt[0]["GuildBuild"].YuanColumnText.Trim());
		float BuildUP = 0.0f;
		switch(leve){
		case 1 :
		
			BuildUP = BuildNum/6000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/6000";
			break;

		
		case 2 :
		
			BuildUP = BuildNum/8000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/8000";
			break;
		
		case 3 :
		
			BuildUP = BuildNum/20000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/20000";
			break;
		
		case 4 :
		
			BuildUP = BuildNum/24000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/24000";
			break;
		
		case 5 :
		
			BuildUP = BuildNum/42000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/42000";
			break;
		
		case 6 :
		
			BuildUP = BuildNum/64000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/64000";
			break;
		
		case 7 :
		
			BuildUP = BuildNum/90000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/9000";
			break;
		
		case 8 :
		
			BuildUP = BuildNum/120000;
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/120000";
			break;
		
		case 9 :
			LabBulid.text = yt[0]["GuildBuild"].YuanColumnText.Trim()+"/154000";
			BuildUP = BuildNum/154000;
			break;
		

		}
		SpriteBuild.fillAmount = BuildUP ;


	}
	//设置公会
	public void ClickGuildNotice(){
		GuildPacktHandler.SetGuildNotice(SetNotice.text.Trim());
	}
	//设置公告
	public void ClickGuildDeclaration(){
		GuildPacktHandler.SetGuildDeclaration(SetDeclaration.text.Trim());
	}
	//公会升级
	public void ClickGuildLeveUP(){
		InRoom.GetInRoomInstantiate().GuildLevelUp(BtnGameManager.yt[0]["GuildID"].YuanColumnText,false);
	}
}
