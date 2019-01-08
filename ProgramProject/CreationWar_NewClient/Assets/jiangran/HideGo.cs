using UnityEngine;
using System.Collections;

public class HideGo : MonoBehaviour {
	private GameObject Go;
	public GameObject HaloHp;
	public UILabel MonSterName;
	public GameObject HaloHp1;
	public GameObject KaSi;
	public GameObject HangUp;

	public GameObject ShowName;
	public UILabel ObjText;

	public GameObject TalkAbout;

	public GameObject BtnTiaoguo;
	public GameObject BossRanking;
	public GameObject ObjTakPanel;
	public GameObject ObjHosting;
	public GameObject BtnNextMap;
	// Use this for initialization
	void Start () {
	Go = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		ShowGO();
	}

	void OnEnable(){

	}
	void ShowGO(){
		if(Go&&Application.loadedLevelName == "Map200"){
			Go.SetActive(false);
			TalkAbout.SetActive(false);
			HaloHp.SetActive(false);
			HangUp.SetActive(false);
			BtnNextMap.SetActive(false);
		}
		if(Go&&Application.loadedLevelName == "Map311"){
			Go.SetActive(false);
			HaloHp.SetActive(false);
			ShowName.SetActive(true);
			ObjText.text = StaticLoc.Loc.Get("buttons040");
		}else
		if(Go&&Application.loadedLevelName == "Map321"){
			Go.SetActive(false);
			HaloHp.SetActive(false);
			ShowName.SetActive(true);
			ObjText.text = StaticLoc.Loc.Get("info1038");
		}else{
			ShowName.SetActive(false);
		}
		if(Go&&Application.loadedLevelName == "Map331"){
			Go.SetActive(false);
			HaloHp.SetActive(false);
		}
		if(Go&&Application.loadedLevelName == "Map411"){
			if(KaSi){
				KaSi.SetActive(false);
			}
			Go.SetActive(false);
			HaloHp.SetActive(false);
			HaloHp1.SetActive(true);
		}else{
			HaloHp1.SetActive(false);
		}
		if(Go&&Application.loadedLevelName == "Map421"){
			Go.SetActive(false);
			HaloHp.SetActive(false);
		}
		if(Go&&Application.loadedLevelName == "Map431"){
			Go.SetActive(false);
			HaloHp.SetActive(false);
		}
		if(Go&&Application.loadedLevelName == "Map441"){
			Go.SetActive(false);
			HaloHp.SetActive(false);
		}

//		if(Go&&Application.loadedLevelName == "Map718"){
//			Go.SetActive(false);
//			HaloHp.SetActive(false);
//		}

		if(Go&&Application.loadedLevelName == "Map711"&&HaloHp){
			Go.SetActive(false);
			HaloHp.SetActive(false);
			BossRanking.SetActive(false);
		}else
		if(Go&&Application.loadedLevelName == "Map712"&&HaloHp){
			Go.SetActive(false);
			HaloHp.SetActive(true);
			BossRanking.SetActive(false);
		}else
		if(Go&&Application.loadedLevelName == "Map713"&&HaloHp){
			Go.SetActive(false);
			HaloHp.SetActive(true);
			BossRanking.SetActive(false);
		}else
		if(Go&&Application.loadedLevelName == "Map911"&&HaloHp){
			Go.SetActive(false);
			HaloHp.SetActive(true);
			BossRanking.SetActive(true);
			MonSterName.text = StaticLoc.Loc.Get("info1026");
		}
		else
		if(Go&&Application.loadedLevelName == "Map912"&&HaloHp){
			Go.SetActive(false);
			HaloHp.SetActive(true);
			BossRanking.SetActive(true);
			MonSterName.text = StaticLoc.Loc.Get("info1026");
		}else{
			HaloHp.SetActive(false);
			BossRanking.SetActive(false);
		}

		if(Application.loadedLevelName == "Map200"){
			BtnTiaoguo.transform.localPosition = new Vector3(BtnTiaoguo.transform.localPosition.x,320,BtnTiaoguo.transform.localPosition.z);
			return;
			}else{
			BtnTiaoguo.transform.localPosition = new Vector3(BtnTiaoguo.transform.localPosition.x,3000f,BtnTiaoguo.transform.localPosition.z);
			BtnTiaoguo.gameObject.SetActive(false);
		}
		if(Application.loadedLevelName == "Map721"){
			ObjHosting.SetActive(false);
			ObjTakPanel.SetActive(false);
		}
		if(Application.loadedLevelName == "Map812"){
			ObjTakPanel.SetActive(false);
		}

	}
}
