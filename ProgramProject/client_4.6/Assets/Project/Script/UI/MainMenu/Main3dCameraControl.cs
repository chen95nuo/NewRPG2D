using UnityEngine;
using System.Collections;

public class Main3dCameraControl : BWUIPanel {
	
	public static  Main3dCameraControl mInstance;
	public float xSpeed = 1;
	public float xBegin = -28, xEnd = 28;
	public float yBegin = -15, yEnd = 20;
	public float x = 0;
	public float y = 0;
	public float distance = 21.84f;
	public bool isCanMoveCamera;
	public GameObject CameraObj;		//3d摄像机//
	public GameObject cameraLookAtObj;
	private bool isOpenOtherUI = false;		//是否有其他ui界面在主场景上层//
	private Vector3 initEulerAngles;
	private Vector3 initPosition;
	private Quaternion initRotation;
	MainMenuManager uiMainMenu;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
        isOpenOtherUI = false;
//		hide();
	}
	
	void Start()
	{
		uiMainMenu = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
		StartCameraData(true);
	}
	
	
	public void StartCameraData(bool moveCamera){
		isCanMoveCamera = moveCamera;
		initCameraData();
		//float des = Vector3.Distance(CameraObj.transform.position, cameraLookAtObj.transform.position);
//		Debug.Log("des : " + des);
	}
	
	public void initCameraData(){
		xBegin = -28;
		xEnd = 28;
		yBegin = -5;
		yEnd = 20;
		initPosition=CameraObj.transform.position;
		initRotation=CameraObj.transform.rotation;
		
		initEulerAngles = CameraObj.transform.eulerAngles;
		x = initEulerAngles.y;
//		y = initEulerAngles.x;;
		y = 0;
		//cameraMove(x,0);
	}
	public void SetBool(bool isOpen)
    {
        isOpenOtherUI = isOpen;
    }
	void Update ()
	{

        if (isOpenOtherUI)
        {
            uiMainMenu.SetEf(false);
            return;
        }
        else
        {
            uiMainMenu.SetEf(true);
        }
		if(GuideManager.getInstance().isGuideRunning())
		{
			return;
		}
			
//		if(AchievementPanel.mInstance != null && AchievementPanel.mInstance.isVisible())
//			return;
//		if(ActivityPanel.mInstance != null && ActivityPanel.mInstance.isVisible())
//			return;
//		if(ChargePanel.mInstance != null && ChargePanel.mInstance.isVisible())
//			return;
//		if(SignUI.mInstance != null && SignUI.mInstance.isVisible())
//			return;
//		if(MailUI.mInstance != null && MailUI.mInstance.isVisible())
//			return;
	
		if(uiMainMenu)
		{
			if(UISceneStateControl.mInstace == null)
			{
				return;
			}
			else
			{
				uiMainMenu = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
				if(uiMainMenu == null)
					return;
			}
			
		}

       
		//float des = Vector3.Distance(CameraObj.transform.position, cameraLookAtObj.transform.position);
//		Debug.Log("des : " + des);
//		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		if(isCanMoveCamera && (uiMainMenu!= null && uiMainMenu.isCanClick))
		{
			mouseControl();
			
		}
	}
	
	
//	public void MoveCamera()
//	{
//		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
//		main.isCanClick = false;
//		CameraData camera = CameraData.getData(13);
//		ViewPathData vpd = ViewPathData.getData(camera.path);
//		iTweenPath iPath = CameraObj.GetComponent<iTweenPath>();
//		iPath.nodes = vpd.nodes;
//		CameraObj.transform.position = vpd.nodes[0];
//		GameObject mainObj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
//		iTween.MoveTo(CameraObj,iTween.Hash("path",iTweenPath.GetPath(iPath.pathName),"time",2f,"oncomplete", "MoveCameraCallBack", 
//				"oncompletetarget", mainObj, "easetype",iTween.EaseType.linear));
//			
//	}
	
	
	public void mouseControl()
	{
		
		if(Input.GetMouseButtonDown(0))
		{
			
		}
		else if(Input.GetMouseButton(0))
		{
			float xDelta = Input.GetAxis("Mouse X");
			float yDelta = Input.GetAxis("Mouse Y");
			yDelta = -yDelta;
            x += xDelta * xSpeed ;
			y += yDelta * xSpeed ;
			x = Mathf.Clamp(x, xBegin,xEnd);
			y = (Mathf.Clamp(y, yBegin,yEnd));
//			Debug.Log("y ======= " + y);
            cameraMove(x,y);
		}
	}
	
	public void cameraMove(float x,float y)
	{
		Quaternion rotation = Quaternion.Euler(y, x,initEulerAngles.y);

        Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * disVector + cameraLookAtObj.transform.position;
        position = transform.InverseTransformPoint(position);
		position = position.normalized * distance;
		position = transform.TransformPoint(position);

        CameraObj.transform.position = position;
        CameraObj.transform.LookAt(cameraLookAtObj.transform.position);
	}
	
	public void reset()
	{
		if(CameraObj != null)
		{
			x = initEulerAngles.y;
			y = 0;
			CameraObj.transform.position=initPosition;
			CameraObj.transform.rotation=initRotation;
		}
	}
	
}
