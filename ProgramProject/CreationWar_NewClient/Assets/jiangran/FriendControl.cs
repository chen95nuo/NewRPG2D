using UnityEngine;
using System.Collections;

public class FriendControl : MonoBehaviour {
	public UILabel myFriendNameo;
	public UILabel myFriendLevelo;
	public UILabel myFriendproO;
	public UISprite picPlayero;
	
	public BtnPlayerForTeam btnFriendo;
	public BtnPlayerForTeam btnFirendt;
	
	public UILabel myFriendNamet;
	public UILabel myFriendLevelt;
	public UILabel myFriendproT;
	public UISprite picPlayert;
	
	private int porfes;
	private string myLevel;

	
	
	private YuanPicManager yuanPicManager;
	
	void Awake()
	{
		yuanPicManager = PanelStatic.StaticYuanPicManger;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnEnable(){
		porfes = int.Parse(BtnGameManager.yt[0]["ProID"].YuanColumnText);
		myLevel = BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText;
		int ThisMyLevel = int.Parse(BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText);
		int numPro = int.Parse(BtnGameManager.yt[0]["ProID"].YuanColumnText.Trim());
		if(porfes == 2){
			
			
			
			
			myFriendNameo.text = StaticLoc.Loc.Get("buttons576");
			myFriendproO.text = StaticLoc.Loc.Get("buttons576");
			if(ThisMyLevel-2<=0){
				myFriendLevelo.text = (1).ToString();
			}else{
			myFriendLevelo.text = (ThisMyLevel-2).ToString();
			}
			picPlayero.atlas = yuanPicManager.picPlayer[numPro-2].atlas;
	        picPlayero.spriteName = yuanPicManager.picPlayer[numPro-2].spriteName;
			btnFriendo.strPro="1";
			
			
			myFriendNamet.text = StaticLoc.Loc.Get("buttons582");
			myFriendproT.text = StaticLoc.Loc.Get("buttons582");
			myFriendLevelt.text = (ThisMyLevel+2).ToString();
			
			picPlayert.atlas = yuanPicManager.picPlayer[numPro].atlas;
            picPlayert.spriteName = yuanPicManager.picPlayer[numPro].spriteName;
			btnFirendt.strPro="3";
			
		}
		
		if(porfes == 1){
			myFriendNameo.text = StaticLoc.Loc.Get("buttons581");
			myFriendproO.text = StaticLoc.Loc.Get("buttons581");
			if(ThisMyLevel-2<=0){
				myFriendLevelo.text = (1).ToString();
			}else{
				myFriendLevelo.text = (ThisMyLevel-2).ToString();
			}
			btnFriendo.strPro="2";
			
			picPlayero.atlas = yuanPicManager.picPlayer[numPro].atlas;
	        picPlayero.spriteName = yuanPicManager.picPlayer[numPro].spriteName;
			
			myFriendNamet.text = StaticLoc.Loc.Get("buttons582");
			myFriendproT.text = StaticLoc.Loc.Get("buttons582");
			myFriendLevelt.text = (ThisMyLevel+2).ToString();
			picPlayert.atlas = yuanPicManager.picPlayer[numPro+1].atlas;
            picPlayert.spriteName = yuanPicManager.picPlayer[numPro+1].spriteName;
			btnFirendt.strPro="3";
		}
		
		
		if(porfes == 3){
			myFriendNameo.text = StaticLoc.Loc.Get("buttons576");
			myFriendproO.text = StaticLoc.Loc.Get("buttons576");
			if(ThisMyLevel-2<=0){
				myFriendLevelo.text = (1).ToString();
			}else{
				myFriendLevelo.text = (ThisMyLevel-2).ToString();
			}
			btnFriendo.strPro="1";
			
			picPlayero.atlas = yuanPicManager.picPlayer[numPro-3].atlas;
	        picPlayero.spriteName = yuanPicManager.picPlayer[numPro-3].spriteName;
			
			myFriendNamet.text = StaticLoc.Loc.Get("buttons581");
			myFriendproT.text = StaticLoc.Loc.Get("buttons581");
			myFriendLevelt.text = (ThisMyLevel+2).ToString();
			btnFirendt.strPro="2";
			
			picPlayert.atlas = yuanPicManager.picPlayer[numPro-2].atlas;
            picPlayert.spriteName = yuanPicManager.picPlayer[numPro-2].spriteName;
			
		}
		
	}
	
}
