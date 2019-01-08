using UnityEngine;
using System.Collections;

public class GuildBuild : MonoBehaviour {
	public UILabel NowTime;
	public UILabel NowNamber;
	public UISprite BackGround;
	public BoxCollider Box;
	public Color b;
	public Color c;
	// Use this for initialization
	void Start () {
		InvokeRepeating("ShowBuildText", 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		ShowBuildText();
	}
	private bool isClick;
	void ShowBuildText(){
		int Number = int.Parse(BtnGameManager.yt.Rows[0]["GBuildGoldNum"].YuanColumnText);

		if(Number<=10){
			NowNamber.text = Number.ToString()+"/10";
			isClick = true;
			if ((InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["GBGColdDown"].YuanColumnText)).TotalMinutes >= 5)
			{
				if(isClick){
					NowTime.text = StaticLoc.Loc.Get("info953");
					Box.enabled = true;
					BackGround.color = b;
				}
			}
			else
			{
				if(isClick){
					int remainingTime = 5 * 60 - (int)(InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["GBGColdDown"].YuanColumnText)).TotalSeconds;
					NowTime.text = string.Format("{0}:{1}",StaticLoc.Loc.Get("info951"), remainingTime);
					BackGround.color = c;
					Box.enabled = false;
				}
			}
		}else{
			BackGround.color = c;
			Box.enabled = false;
			isClick = false;
		}

//		Debug.Log("================================================---------------------"+BtnGameManager.yt[0]["GBGColdDown"].YuanColumnText);
//		Debug.Log("================================================---------------------++++++++"+System.DateTime.Parse(BtnGameManager.yt[0]["GBGColdDown"].YuanColumnText));

	}
}
