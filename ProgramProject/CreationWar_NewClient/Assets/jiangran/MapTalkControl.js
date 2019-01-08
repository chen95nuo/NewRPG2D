#pragma strict


var TalkObj : GameObject;
var TalkObj1 : GameObject;
var TalkObj5 : GameObject;
var TalkObj6 : GameObject;
var TalkObj7 : GameObject;
var TalkObj8 : GameObject;
var TalkObj9 : GameObject;
var TalkObjBox : BoxCollider;
function Start () {
//		TalkObj1.SetActive(false);
		if(Application.loadedLevelName == "Map200"){
			TalkObj8.SetActive(false);
			TalkObj9.SetActive(false);
		}
		if(Application.loadedLevelName == "Map212"){
			TalkObj7.SetActive(false);
			TalkObjBox.enabled = true;
		}
}

function ShowObjAction(){
		TalkObj.SetActive(false);
		TalkObj1.SetActive(false);
		TalkObj5.SetActive(false);
		TalkObj6.SetActive(false);
		TalkObj7.SetActive(false);
		TalkObj8.SetActive(false);
		TalkObj9.SetActive(false);
		TalkObjBox.enabled = false;
}

function NextOneByOne()
{
		AllManage.camStoryStatic.MonsterOnebyOne();
}

