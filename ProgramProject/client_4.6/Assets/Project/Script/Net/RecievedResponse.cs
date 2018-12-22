using UnityEngine;
using System.Collections;

public class RecievedResponse : MonoBehaviour {
	
	public GameObject downloadUI;			//open this object and show the text
	private static int _curTaskId;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	/**
	 * show the UI for connect end  
	 */
	void ShowResponeSuccesUI(string str)
	{
		//parse the string to get the message , the frist is get the taskID
		//_curTaskId = 
		
		
		//switch (_curTaskId){
		//case DEF_TOOLS.TASK_LOGIN:
			// show the login ui
			
		//	break;
			
		//case DEF_TOOLS.TASK_REGISTER:
			// show the register ui
			
		//	break;
			
		//case DEF_TOOLS.TASK_UPDATE:
			// show the update ui
			
		//	break;
		//}
		
	}
	
	void ShowResponeFaildUI(string str){
		
		//show faild UI 
		
	}
}
