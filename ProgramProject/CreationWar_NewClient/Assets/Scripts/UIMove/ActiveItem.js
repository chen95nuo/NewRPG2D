#pragma strict

var conClose : boolean = false;
function Start(){
//	yield;
	if(! conClose){
		gameObject.SetActiveRecursively(false);	
	}
	transform.localScale = oldScale;
}

var oldScale : Vector3;
function CloseNow(){
	oldScale = transform.localScale;
	transform.localScale = Vector3.zero;
}
var isfrist : int=0;
var mapTalk : MapTalkControl;
function OnEnable(){
	mapTalk	=	FindObjectOfType(	MapTalkControl	);
	
		if(mapTalk){
			mapTalk.TalkObjBox.enabled = true;
//		if(Application.loadedLevelName == "Map200"&&isfrist==0){
//			mapTalk.TalkObj.SetActive(true);
//			mapTalk.TalkObj1.SetActive(true);
//			mapTalk.TalkObj5.SetActive(true);
//			mapTalk.TalkObj6.SetActive(true);
//			mapTalk.TalkObj7.SetActive(true);
//			isfrist = 1;
//		}
//		if(Application.loadedLevelName == "Map212"&&isfrist==0){
//			mapTalk.TalkObj.SetActive(true);
//			mapTalk.TalkObj1.SetActive(true);
//			mapTalk.TalkObj5.SetActive(true);
//			mapTalk.TalkObj6.SetActive(true);
//			mapTalk.TalkObj7.SetActive(true);
//			mapTalk.TalkObj8.SetActive(true);
//			mapTalk.TalkObj9.SetActive(true);
//			isfrist = 1;
//		}
			
		}
}
