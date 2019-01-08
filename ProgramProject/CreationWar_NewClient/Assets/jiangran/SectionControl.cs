using UnityEngine;
using System.Collections;

public class SectionControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		StartCoroutine(ShowMap());
		ShowMap();
	}
	
	// Update is called once per frame
	void Update () {

	
	}
	IEnumerator ShowSection(){
		yield return new WaitForSeconds(1f);
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show41",SendMessageOptions.DontRequireReceiver);
	}
	void ShowMap(){
		if(Application.loadedLevelName == "Map200"){
//			yield return StartCoroutine(ShowSection());
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("show41",SendMessageOptions.DontRequireReceiver);
			SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info977");
			SectionLabel.sectionLabel.ObjPart0.enabled = true;
		}
	}
	void ShowSectionMap(string MapID){
		StartCoroutine(SectionMap(MapID));
	}
	IEnumerator SectionMap(string MapID ){
//		if(MapID=="111"||MapID=="121"||MapID=="151"){
//
//		}
		switch(MapID){
		case "111":
            //yield return StartCoroutine(ShowSection());
            //SectionLabel.sectionLabel.Finish0.enabled = true;
            //SectionLabel.sectionLabel.ObjPart1.enabled = true;
            //SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info973");
			break;
		case "121":
			yield return StartCoroutine(ShowSection());
			SectionLabel.sectionLabel.Finish0.enabled = true;
			SectionLabel.sectionLabel.Finish1.enabled = true;
			SectionLabel.sectionLabel.ObjPart2.enabled = true;

//			ParentSection.parentSection.taskSection0.ShowTaskSection(Section.End);
//			ParentSection.parentSection.taskSection1.ShowTaskSection(Section.End);
//			ParentSection.parentSection.taskSection2.ShowTaskSection(Section.Going);
			SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info974");
			break;
		case "151":
			yield return StartCoroutine(ShowSection());
			SectionLabel.sectionLabel.Finish0.enabled = true;
			SectionLabel.sectionLabel.Finish1.enabled = true;
			SectionLabel.sectionLabel.Finish2.enabled = true;
			SectionLabel.sectionLabel.ObjPart3.enabled = true;

//			ParentSection.parentSection.taskSection0.ShowTaskSection(Section.End);
//			ParentSection.parentSection.taskSection1.ShowTaskSection(Section.End);
//			ParentSection.parentSection.taskSection2.ShowTaskSection(Section.End);
//			ParentSection.parentSection.taskSection3.ShowTaskSection(Section.Going);
			SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info975");
			break;
		}
	}

	IEnumerator SectionTask(string TaskID){
//		if(TaskID=="39"||TaskID=="80"||TaskID=="364"){
//			StartCoroutine(ShowSection());
//		}
		switch(TaskID){
		case "39":
			yield return StartCoroutine(ShowSection());
			SectionLabel.sectionLabel.Finish0.enabled = true;
			SectionLabel.sectionLabel.Finish1.enabled = true;
			SectionLabel.sectionLabel.FinishEnd.enabled = true;
			SectionLabel.sectionLabel.ObjPart1.enabled = true;

			SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info973");
			break;
		case "80":
			yield return StartCoroutine(ShowSection());
			SectionLabel.sectionLabel.Finish0.enabled = true;
			SectionLabel.sectionLabel.Finish1.enabled = true;
			SectionLabel.sectionLabel.Finish2.enabled = true;
			SectionLabel.sectionLabel.FinishEnd.enabled = true;
			SectionLabel.sectionLabel.ObjPart2.enabled = true;


			SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info974");
			break;
		case "364":
			yield return StartCoroutine(ShowSection());
			SectionLabel.sectionLabel.Finish0.enabled = true;
			SectionLabel.sectionLabel.Finish1.enabled = true;
			SectionLabel.sectionLabel.Finish2.enabled = true;
			SectionLabel.sectionLabel.Finish3.enabled = true;
			SectionLabel.sectionLabel.FinishEnd.enabled = true;
			SectionLabel.sectionLabel.ObjPart3.enabled = true;

			SectionLabel.sectionLabel.LabText.text = StaticLoc.Loc.Get("info975");
			break;
			
		}
	}

}
