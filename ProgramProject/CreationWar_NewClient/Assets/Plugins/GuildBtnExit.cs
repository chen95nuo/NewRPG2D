using UnityEngine;
using System.Collections;

public enum BtnEsc{
	Dissolve,
	Esc
}
public class GuildBtnExit : MonoBehaviour {
	
	public static BtnEsc  btnEsc = BtnEsc.Esc;
	public static GuildBtnExit guildBtnExit;
	public UILabel LabText;

	private string text;
	
	void Awake () {
		guildBtnExit = this ;
	}
	// Use this for initialization
	void Start () {

	}
	void OnEnable(){
		ShowText();
	}
	void ShowText(){
		if(int.Parse(BtnGameManager.yt.Rows[0]["GuildPosition"].YuanColumnText)==1){
			ShowGuildExit(BtnEsc.Dissolve);
		}else{
			ShowGuildExit(BtnEsc.Esc);
		}
	}

	public void ShowGuildExit(BtnEsc mBtnEsc){
		switch(mBtnEsc){
		case BtnEsc.Dissolve:
			text = StaticLoc.Loc.Get("info994");
			btnEsc = BtnEsc.Dissolve;
			break;
		case BtnEsc.Esc:
			text = StaticLoc.Loc.Get("info995");
			btnEsc = BtnEsc.Esc;
			break;
		}
		LabText.text = text;
	}

	public void GuildEscClick(){
		switch(btnEsc){
		case BtnEsc.Dissolve:
			//会长解散工会的方法调用
			PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1076"));
			PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
			PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.btnEnter.functionName = "GuildEscbtn";
			break;
		case BtnEsc.Esc:
			//会员的退出工会的方法调用
			PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1077"));
			PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
			PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.btnEnter.functionName = "GuildProEscbtn";
			break;
		}

	}

	void GuildEscbtn(){
		InRoom.GetInRoomInstantiate().GuildDismiss(BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
		PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.Close();
	}

	void GuildProEscbtn(){
		InRoom.GetInRoomInstantiate().GuildDismiss(BtnGameManager.yt.Rows[0]["GuildID"].YuanColumnText);
		PanelStatic.StaticBtnGameManager.warnings.warningAllEnterClose.Close();
	}

}
