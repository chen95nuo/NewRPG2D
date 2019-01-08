#pragma strict

function Start () {
	dungcl = AllManage.dungclStatic;
	npcCL = FindObjectOfType(NPCControl);
}
function OnEnable(){
	if(myTask != null){
		if(myTask.jindu == 3){
			myTask.jindu = 2;
		}
	}
}

var LabelTask : UILabel;
var npcid : String;
var myTask : MainTask;
var dungcl : DungeonControl;
function SetNPCTask(task : MainTask , id : String){

	var str : String;
	npcid = id;
	myTask = task;
	var use : String;
	if(task.readyDone){
		if(task.taskType == MainTaskType.duihua){ 
			use =  dungcl.GetNPCIDAsID(parseInt(task.doneType.Substring(0,4)).ToString());
//			//print(use);
			if(npcid == use){
				str = "[ffff00]";
				CheckGuideTask(task);	
			}else{
				str = "[ffffff]";	
			}
		}else{
			str = "[ffff00]";
			CheckGuideTask(task);		
		}
	}else 
	if(task.jindu == 1 && task.taskType == MainTaskType.duihua){
			use =  dungcl.GetNPCIDAsID(parseInt(task.doneType.Substring(0,4)).ToString());
		//	//print(use);
			if(npcid == use){
				str = "[ffff00]";
				//CheckGuideTask(task);	
			}else{
				str = "[ffffff]";	
			}
					
	}else
	if(task.jindu == 0){
	    str = "[00ff00]";	
	    CheckGuideTask(task);
	}else
	if(task.jindu == 0){
	    str = "[000000]";
	    //CheckGuideTask(task);
	}
//	//print(task.leixing);
	LabelTask.text =  "("+UIControl.taskLeixingStrs[task.leixing] + ")" + str + task.taskName;
}

var npcCL : NPCControl;
var isPointtoTalk : boolean = false;
function selectme(){

	npcCL.SelectOneTaskAsNPC(myTask , npcid);
	
	var use : String;
	if(myTask.id == "4"){
	    if(myTask.jindu == 0)
	    {
	//            	Debug.Log("1==================================================" + myTask.jindu);
	        AllManage.UICLStatic.MainTW.guidPanelPos1 = guidPanelPos1;
	        if(null != BeginnersGuide.beginnersGuide)
	        {
	            BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",myTask.taskName,",点击继续按钮"));
	            BeginnersGuide.beginnersGuide.SetSpriteAlpha(0.1f); 
	            //BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidPanelPos1, StaticLoc.Loc.Get("info825"), button_GO ,"OnClick", yuan.YuanPhoton.GameScheduleType.ContinueBtn);
	            BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidPanelPos1, StaticLoc.Loc.Get("info832"), button_GO ,"OnClick", yuan.YuanPhoton.GameScheduleType.ContinueBtn);
	        }
	    }
	    else if(myTask.jindu == 1 && myTask.taskType == MainTaskType.duihua)
	    {
			use =  dungcl.GetNPCIDAsID(parseInt(myTask.doneType.Substring(0,4)).ToString());
			if(npcid == use && null != BeginnersGuide.beginnersGuide)
			{
		        BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",myTask.taskName,",点击完成按钮"));
		        BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidPanelPos1, StaticLoc.Loc.Get("info833"), button_GO ,"OnClick", yuan.YuanPhoton.GameScheduleType.CompleteBtn);
	        }
	    }
	}
    //else if(myTask.id == "10" || myTask.id == "111" || myTask.id == "499" || myTask.id == "11" || myTask.id == "294"){
	else if(myTask.id == "11" || myTask.id == "294"){
	    if(myTask.jindu == 0 && null != BeginnersGuide.beginnersGuide)
		{
		    BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",myTask.taskName,",点击接受按钮"));
		    BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidPanelPos1, StaticLoc.Loc.Get("info832"), button_GO ,"OnClick", yuan.YuanPhoton.GameScheduleType.AcceptBtn);
	    }
	    else if(myTask.jindu == 1 && myTask.taskType == MainTaskType.duihua || (myTask.id == "294" && myTask.jindu == 2 && myTask.taskType == MainTaskType.Robot))
	    { 
			
			use =  dungcl.GetNPCIDAsID(parseInt(myTask.doneType.Substring(0,4)).ToString());
			if(npcid == use && null != BeginnersGuide.beginnersGuide)
			{
                BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",myTask.taskName,",点击完成按钮"));
                BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidPanelPos1, StaticLoc.Loc.Get("info833"), button_GO ,"OnClick", yuan.YuanPhoton.GameScheduleType.CompleteBtn);
        	}
	    }
	}
}

public var guidePanelParentObj : Transform;
private var beginnersGuide : GameObject;
public var guidPanelPos : Transform;
public var guidPanelPos1 : Transform;
public var button_GO : GameObject;
private var showTxt : String = "";
private var gameScheduleType : yuan.YuanPhoton.GameScheduleType;

function CheckGuideTask(task : MainTask)
{
	if(null == AllManage.UICLStatic.MainTW.beginnersGuide)
	{
		AllManage.UICLStatic.MainTW.CreateBeginnerGuid();
	}
        
	if(null == beginnersGuide) 
	{
		beginnersGuide = AllManage.UICLStatic.MainTW.beginnersGuide;
	}
	
    //if(task.id == "4" || task.id == "10" || task.id == "11" || task.id == "111" || task.id == "294" || task.id == "499")
	if(task.id == "4" || task.id == "11" || task.id == "294")
    {
        if(task.jindu == 0)
        {
            showTxt = StaticLoc.Loc.Get("info823");
            BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",task.taskName, ",点击文字接受"));
            gameScheduleType = yuan.YuanPhoton.GameScheduleType.AcceptTask;
        }
        if(task.jindu == 1 && task.taskType == MainTaskType.duihua)
        {
            showTxt = StaticLoc.Loc.Get("info824");
            
            BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",task.taskName, ",点击文字完成"));
            gameScheduleType = yuan.YuanPhoton.GameScheduleType.CompleteTask;
        }

        if(task.jindu == 2 && task.id == "294" && task.taskType == MainTaskType.Robot)
        {
            showTxt = StaticLoc.Loc.Get("info824");
            
            BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",task.taskName, ",点击文字完成"));
            gameScheduleType = yuan.YuanPhoton.GameScheduleType.CompleteTask;
        }
 
        BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.LeftTop,guidPanelPos, showTxt, this.gameObject ,"selectme", gameScheduleType);
    }
    else
    {
    	Destroy (beginnersGuide);
    }
}
