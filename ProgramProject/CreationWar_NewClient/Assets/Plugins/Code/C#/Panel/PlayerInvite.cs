using UnityEngine;
using System.Collections;

public class PlayerInvite : MonoBehaviour {

    public UILabel lblNum;
    public Warnings warnings;
    public GameObject invCL;
    yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("UserInfo", "userEmail");
	// Use this for initialization
	void Start () {
		warnings=PanelStatic.StaticWarnings;
	}

    void OnEnable()
    {
        //StartCoroutine(ReadTable());
    }

    IEnumerator ReadTable()
    {
        lblNum.text = "0";
        InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select * from UserInfo where userEmail='{0}'", BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText),"ZealmPass",yt,PlayerPrefs.GetString ("ConnectionIP").Split (':')[0]);
        while (yt.IsUpdate)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (yt != null && yt.Rows.Count > 0)
        {
            int inviter = yt.Rows[0]["Inviter"].YuanColumnText == "" ? 0 : int.Parse(yt.Rows[0]["Inviter"].YuanColumnText);
            int invitees = yt.Rows[0]["Invitees"].YuanColumnText == "" ? 0 : int.Parse(yt.Rows[0]["Invitees"].YuanColumnText);
            lblNum.text = inviter.ToString() ;
        }
    }

    string txtInfo = string.Empty;
    void OnInviterClick(GameObject mObj)
    {
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.PlayerInvite,0,0,null));
//		if(!InRoom.GetInRoomInstantiate().ServerConnected||Application.internetReachability==NetworkReachability.NotReachable)
//		{
//			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info637"));
//			return;
//		}		
//        if (!yt.IsUpdate&&yt.Rows.Count>0&&( yt.Rows[0]["Inviter"].YuanColumnText == "" ? 0 : int.Parse(yt.Rows[0]["Inviter"].YuanColumnText))>0)
//        {
//            txtInfo="恭喜您获得";
//            //string tempGold=(int.Parse(BtnGameManager.yt.Rows[0]["Money"].YuanColumnText)+(int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterGold]).ToString();
//             //string tempBlood=(int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText)+(int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterBlood]).ToString();
//           //BtnGameManager.yt.Rows[0]["Money"].YuanColumnText=tempGold;
//            //BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText=tempBlood;
//            
//			YuanBackInfo backGold=new YuanBackInfo(System.DateTime.Now.ToString ());
//			
//			StartCoroutine( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().ClientMoney (((int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterGold]).To16String () ,((int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterBlood]).To16String (),backGold)));
//			
//            yt.Rows[0]["Inviter"].YuanColumnText = (int.Parse(yt.Rows[0]["Inviter"].YuanColumnText) - 1).ToString();
//            InRoom.GetInRoomInstantiate().UpdateYuanTable("ZealmPass", yt,PlayerPrefs.GetString ("ConnectionIP").Split (':')[0]);
//            txtInfo += int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) + (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterBlood] + "血石和" + (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterGold] + "金币";
//            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), txtInfo);
//            lblNum.text = yt.Rows[0]["Inviter"].YuanColumnText;
//        }
//        else
//        {
//            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),StaticLoc.Loc.Get("info404"));
//        }
    }

    void OnInvitees(GameObject mObj)
    {
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().UseMoney (yuan.YuanPhoton.UseMoneyType.PlayerInvitees,0,0,null));
//		if(!InRoom.GetInRoomInstantiate().ServerConnected||Application.internetReachability==NetworkReachability.NotReachable)
//		{
//			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info637"));
//			return;
//		}				
//        if (!yt.IsUpdate&&yt.Rows.Count>0 && (yt.Rows[0]["Invitees"].YuanColumnText == "" ? 0 : int.Parse(yt.Rows[0]["Invitees"].YuanColumnText)) > 0)
//        {
//            txtInfo = "恭喜您获得";
//            string tempGold = (int.Parse(BtnGameManager.yt.Rows[0]["Money"].YuanColumnText) + (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviteesGold]).ToString();
//            string tempBlood = (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) + (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviteesBlood]).ToString();
//            BtnGameManager.yt.Rows[0]["Money"].YuanColumnText = tempGold;
//            BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText = tempBlood;
//            yt.Rows[0]["Invitees"].YuanColumnText = (int.Parse(yt.Rows[0]["Invitees"].YuanColumnText) - 1).ToString();
//            InRoom.GetInRoomInstantiate().UpdateYuanTable("ZealmPass", yt,PlayerPrefs.GetString ("ConnectionIP").Split (':')[0]);
//            txtInfo += int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) + (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterBlood] + "血石和" + (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.InviterGold] + "金币";
//            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), txtInfo);
//            lblNum.text = yt.Rows[0]["Inviter"].YuanColumnText;
//        }
//        else
//        {
//            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info405"));
//        }
    }
}
