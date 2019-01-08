using UnityEngine;
using System.Collections;

public class PlayerInformation : MonoBehaviour {

	public UILabel labelNowName;
	public UILabel labelPvp;
	public UILabel labelChengHao;
	
	public UILabel labelTitle;
	public UILabel labelGameTime;
	
	[HideInInspector]
	public yuan.YuanMemoryDB.YuanTable yt;
	// Use this for initialization
	
	void Awake()
	{
		yt = new yuan.YuanMemoryDB.YuanTable("PlayerInformation" + this.name, "");
	}
	
	void Start () {
		StartCoroutine(ShowPlayerInforMation());
	}

	void Update(){
		labelGameTime.text = ((int)(Time.time)).ToString()+"s";
	}
	
	void OnEnable(){
		StartCoroutine(ShowPlayerInforMation());
	}
	IEnumerator ShowPlayerInforMation(){
		//		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>
		InRoom.GetInRoomInstantiate().GetTableForID(BtnGameManager.yt[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.TableType.GuildInfo, yt);
		while (yt.IsUpdate)
		{
			yield return new WaitForSeconds(0.1f);
		}
//		if(yt.Count>0){
//		labelNowName.text = yt[0]["GuildName"].YuanColumnText.Trim();
		labelNowName.text = BtnGameManager.yt[0]["GuildName"].YuanColumnText;
		labelPvp.text = BtnGameManager.yt[0]["Rank"].YuanColumnText;
		
		labelChengHao.text = BtnGameManager.yt[0]["SelectTitle"].YuanColumnText;
		labelTitle.text = BtnGameManager.yt[0]["SelectTitle"].YuanColumnText;
//		}

	}
}
