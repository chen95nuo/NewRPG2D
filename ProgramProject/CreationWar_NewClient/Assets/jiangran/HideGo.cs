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
			Go.SetActiveRecursively(false);
			TalkAbout.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
			HangUp.SetActiveRecursively(false);
			BtnNextMap.SetActiveRecursively(false);
		}
		if(Go&&Application.loadedLevelName == "Map311"){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
			ShowName.SetActiveRecursively(true);
			ObjText.text = StaticLoc.Loc.Get("buttons040");
		}else
		if(Go&&Application.loadedLevelName == "Map321"){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
			ShowName.SetActiveRecursively(true);
			ObjText.text = StaticLoc.Loc.Get("info1038");
		}else{
			ShowName.SetActiveRecursively(false);
		}
		if(Go&&Application.loadedLevelName == "Map331"){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
		}
		if(Go&&Application.loadedLevelName == "Map411"){
			if(KaSi){
				KaSi.SetActiveRecursively(false);
			}
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
			HaloHp1.SetActiveRecursively(true);
		}else{
			HaloHp1.SetActiveRecursively(false);
		}
		if(Go&&Application.loadedLevelName == "Map421"){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
		}
		if(Go&&Application.loadedLevelName == "Map431"){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
		}
		if(Go&&Application.loadedLevelName == "Map441"){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
		}

//		if(Go&&Application.loadedLevelName == "Map718"){
//			Go.SetActiveRecursively(false);
//			HaloHp.SetActiveRecursively(false);
//		}

		if(Go&&Application.loadedLevelName == "Map711"&&HaloHp){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(false);
			BossRanking.SetActive(false);
		}else
		if(Go&&Application.loadedLevelName == "Map712"&&HaloHp){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(true);
			BossRanking.SetActive(false);
		}else
		if(Go&&Application.loadedLevelName == "Map713"&&HaloHp){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(true);
			BossRanking.SetActive(false);
		}else
		if(Go&&Application.loadedLevelName == "Map911"&&HaloHp){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(true);
			BossRanking.SetActive(true);
			MonSterName.text = StaticLoc.Loc.Get("info1026");
		}
		else
		if(Go&&Application.loadedLevelName == "Map912"&&HaloHp){
			Go.SetActiveRecursively(false);
			HaloHp.SetActiveRecursively(true);
			BossRanking.SetActive(true);
			MonSterName.text = StaticLoc.Loc.Get("info1026");
		}else{
			HaloHp.SetActiveRecursively(false);
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
