using UnityEngine;
using System.Collections;

public class SectionLabel : MonoBehaviour {
	public static SectionLabel sectionLabel;
	public UILabel LabText;
	public UILabel Finish0;
	public UILabel Finish1;
	public UILabel Finish2;
	public UILabel Finish3;
	public UILabel Finish4;
	public UILabel Finish5;
	public UILabel FinishEnd;

	public UISprite ObjPart0;
	public UISprite ObjPart1;
	public UISprite ObjPart2;
	public UISprite ObjPart3;


	void Awake(){
		sectionLabel = this ;
	}


}
