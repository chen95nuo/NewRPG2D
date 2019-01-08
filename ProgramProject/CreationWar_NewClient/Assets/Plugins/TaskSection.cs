using UnityEngine;
using System.Collections;

public enum Section{
	Going,
	End,
	NotOption
}
public class TaskSection : MonoBehaviour {
	public UILabel NowState		;
	public UISprite ShowState 	; 
	public UISprite MySpriteState 	; 
	private string showText		;

	public Color IsNoOpen       ;
	public static TaskSection taskSection;
	public static Section section = Section.NotOption;
	// Use this for initialization
	void Start () {
		taskSection = this;
	}
	
	public void ShowTaskSection(Section mSection){
		switch(mSection){
		case Section.Going:
			showText = StaticLoc.Loc.Get("info1225");
			if(ShowState){
				ShowState.spriteName = "yuanY5";
			}
			break;
		case Section.End:
			showText = StaticLoc.Loc.Get("info331");
			if(ShowState){
				ShowState.spriteName = "";
			}
			break;
		case Section.NotOption:
			showText = StaticLoc.Loc.Get("info985");
			if(ShowState){
				ShowState.spriteName = "";
			}
			if(MySpriteState){
				MySpriteState.color = IsNoOpen;
			}
			break;
		}
		NowState.text = showText;

	}
}
