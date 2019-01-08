using UnityEngine;
using System.Collections;

public class GuildSkill : MonoBehaviour {
	public UILabel labelNowName;
	public UILabel labelNow;
	public UILabel labelNowLV;
	public UILabel labelAddMoney;
	
	public UILabel labelNextName;
	public UILabel labelNext;
	public UILabel labelNextLV;
	public UILabel labelStones;

	public UILabel labelGold;
	public UILabel labelStone;

	[HideInInspector]
	public yuan.YuanMemoryDB.YuanTable yt;
	// Use this for initialization

    void Awake()
    {
        yt = new yuan.YuanMemoryDB.YuanTable("GuildSkil" + this.name, "");
    }

	void Start () {
	
	}

	void OnEnable(){
		StartCoroutine(ShowGuildSkill());
	}
	IEnumerator ShowGuildSkill(){
//		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>
		InRoom.GetInRoomInstantiate().GetTableForID(BtnGameManager.yt[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.TableType.GuildInfo, yt);
		while (yt.IsUpdate)
		{
			yield return new WaitForSeconds(0.1f);
		}
		labelGold.text = yt[0]["GuildLevel"].YuanColumnText.Trim();
		labelStone.text = yt[0]["GuildLevel"].YuanColumnText.Trim();

		labelNowLV.text = yt[0]["GuildLevel"].YuanColumnText.Trim();
		labelNextLV.text = (int.Parse((yt[0]["GuildLevel"].YuanColumnText.Trim())) + 1).ToString();
		labelAddMoney.text =yt[0]["GuildLevel"].YuanColumnText.Trim() + "%";
		labelStones.text = (int.Parse((yt[0]["GuildLevel"].YuanColumnText.Trim())) + 1).ToString() + "%";
	}
}
