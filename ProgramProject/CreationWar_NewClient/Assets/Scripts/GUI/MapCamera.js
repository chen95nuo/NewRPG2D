
var help : boolean = false;
private var camerat:Camera;
static var  mapsize =120.0;
function Awake(){
	var preSkill = Resources.Load("Song", GameObject);
//	print(preSkill + " == preSkill");
	GameObject.Instantiate(preSkill);
	
	
}

function Start(){
//	AllResources.arObj.SendMessage("LoadUI" , SendMessageOptions.DontRequireReceiver);
//	if(AllResources.SongGUI == null){
//		AllResources.SongGUI = Resources.Load("Song", GameObject);
//	}
//	GameObject.Instantiate(AllResources.SongGUI);
Screen.sleepTimeout = SleepTimeout.NeverSleep;
yield;
//Application.LoadLevelAdditiveAsync ("Map0UI");
camerat = GetComponent(Camera);
mapsize = camerat.orthographicSize;
if(help){
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.StartMap111).ToString());
	var preShengchan = Resources.Load("Anchor - helproot", GameObject);
	GameObject.Instantiate( preShengchan );
//	Application.LoadLevelAdditiveAsync ("Map0UIHELP");
}
	#if UNITY_ANDROID
		var pars : ParticleEmitter[];
		pars = FindObjectsOfType(ParticleEmitter);
		for(var i=0; i<pars.length; i++){
			if(pars[i] && pars[i].gameObject.name == "shine"){
				pars[i].active = false;
			}
		}
	#endif
}

