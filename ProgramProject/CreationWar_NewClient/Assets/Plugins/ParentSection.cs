using UnityEngine;
using System.Collections;

public class ParentSection : MonoBehaviour {
	public TaskSection taskSection0;
	public TaskSection taskSection1;
	public TaskSection taskSection2;
	public TaskSection taskSection3;
	public TaskSection taskSection4;
	public TaskSection taskSection5;


	public static ParentSection parentSection;

	[HideInInspector]
	public yuan.YuanMemoryDB.YuanTable yt;

	void Awake(){
		yt = new yuan.YuanMemoryDB.YuanTable("ParentSection" + this.name, "");
	}
	void Start(){
		parentSection = this ;
	}

	void OnEnable(){
		ShowTaskSection();
	}
	void ShowTaskSection(){
		string[] task = BtnGameManager.yt.Rows[0]["CompletTask"].YuanColumnText.Split(';');
		for(int i = 0;i<task.Length; i++){
			string a = task[i];
			if(a!="39"){
				taskSection1.ShowTaskSection(Section.Going);
				taskSection2.ShowTaskSection(Section.NotOption);
				taskSection3.ShowTaskSection(Section.NotOption);
			}
			if(a=="39"){
				taskSection1.ShowTaskSection(Section.End);
				taskSection2.ShowTaskSection(Section.Going);
				taskSection3.ShowTaskSection(Section.NotOption);
			}
			if(a=="80"){
				taskSection1.ShowTaskSection(Section.End);
				taskSection2.ShowTaskSection(Section.End);
				taskSection3.ShowTaskSection(Section.Going);
			}
			if(a=="364"){
				taskSection1.ShowTaskSection(Section.End);
				taskSection2.ShowTaskSection(Section.End);
				taskSection3.ShowTaskSection(Section.End);
			}
		}
		taskSection0.ShowTaskSection(Section.End);

	}
}
