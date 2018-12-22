using UnityEngine;
using System.Collections;

public class RotateWheelControl : MonoBehaviour,ProcessResponse {
	
	public int cellID;
	public GameObject activityPanel;
	public GameObject wheelPanel;
	public GameObject noWheelPanel;
	public GameObject staticCirCleGroup;
	public Transform targetObJ;
	
	//当前激活的活动类型//
	public int curActiveType;
	//当前档位的index//
	public int curIndex;
	
	public float speed = 20f;
	bool isPressed = false;
	float xDeg;
	float yDeg;
	float deg;
	bool isUseNormal;
//	Quaternion fromRot;
//	Quaternion toRot;
	
	//Transform mTrans;
	//Vector3 startPos;
	
	bool isAutoRot = false;
	bool isReduceSpeed = false;
	bool isRotating = false;
	float needReduceAngle = 0.0f;
	float reduceAngleStartAngle = 0;
	float curRTime = 0.0f;
	float startTime;
	float endTime;
	
	float rotZ;
	
	bool receiveData = false;
    int errorCode = -1;
	int requestType = 0;
	
	bool isRequestData = true;
	
	RunnerResultJson runnerResultJson;
	
	float deltaAngle ;
	
	public float ttttt = 1.3f;
	public float sensitiveNum = 10f;
	
	Camera nguiC;
	
	Vector3 lastMousePos;
	//鼠标当前的x轴坐标//
	float x;
	//鼠标当前的y轴坐标//
	float y;
	//转盘的屏幕坐标//
	Vector2 screenPos;
	
	int wheelPlatSpeed;
	
	// Use this for initialization
	void Start () {
		
		//mTrans = transform;
		nguiC = GameObject.Find("UI Root (2D)/UICamera").camera;
		
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			ttttt = 1f;
			wheelPlatSpeed = 360;
		}
		else
		{
			wheelPlatSpeed = 720;
		}
		
		screenPos = nguiC.WorldToScreenPoint(transform.position);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (receiveData)
        {
            receiveData = false;
			if(errorCode == -3)
				return;
			
			if (errorCode != 0)
            {
				//活动已结束//
				if (errorCode == 113)
                {
					isRequestData = true;
                    string errorMsg = TextsData.getData(628).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
				//活动未开始//
				if (errorCode == 114)
                {
					isRequestData = true;
                    string errorMsg = TextsData.getData(629).chinese;
                    ToastWindow.mInstance.showText(errorMsg);
                }
				//没有抽奖机会//
				if (errorCode == 127)
                {
					isRequestData = true;
					noWheelPanel.SetActive(true);
					noWheelPanel.GetComponent<NoWheelPanel>().InitNoWheelPanel(activityPanel,cellID,curActiveType,curIndex);
                }
				return;
			}
			
			switch (requestType)
            {
                case 1:
				{
					Debug.Log("Get runnerresult json success!");
					isAutoRot = true;
					isRotating = true;
					startTime = Time.time;
					endTime = Time.time;
					wheelPanel.GetComponent<WheelPanel>().rotNum--;
					//设置目标点//
					targetObJ = staticCirCleGroup.transform.FindChild("Pos"+runnerResultJson.index.ToString());
					deltaAngle = transform.localEulerAngles.z;
					//targetObJ = staticCirCleGroup.transform.FindChild("Pos2");
				
				}
                break;
			}
		}
		
		//手动旋转风车，不发请求//
		if(Input.GetMouseButton(0) && isPressed && !isRotating)
		{
			xDeg = Input.GetAxis("Mouse X") * 10 ;
			yDeg = Input.GetAxis("Mouse Y")  * 10;
			
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			if(lastMousePos == Input.mousePosition)
			{
				return;
			}
			/*
			if(isUseNormal)
			{
				if(x > screenPos.x)
				{
					transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z +deg);
				}
				else
				{
					transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z -deg);
				}
			}
			else
			{
				if(y > screenPos.y)
				{
					transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z -deg);
				}
				else
				{
					transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z +deg);
				}
			}
			*/
			
			lastMousePos = Input.mousePosition;
//			toRot = Quaternion.Euler(0,0, yDeg);
//			transform.rotation = Quaternion.Lerp(fromRot,toRot,Time.deltaTime * 500);
		}
		
		if(isReduceSpeed )
		{
			if( curRTime < 2)
			{
				doReduceSpeed();
			}
			else
			{
				isReduceSpeed = false;
				curRTime = 0.0f;
				isRotating = false;
				
			}
			
		}

		//开始自动旋转风车//
		if(isAutoRot)
		{
			transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z - wheelPlatSpeed * Time.deltaTime) ;
			endTime = Time.time;
			if(endTime - startTime > (deltaAngle +3 * 360)/720)
			{
				isReduceSpeed = true;
				reduceAngleStartAngle = transform.localEulerAngles.z;


					needReduceAngle =  (360 -targetObJ.transform.localEulerAngles.z) +  reduceAngleStartAngle;

				
				if(needReduceAngle < 180.0f)
				{
					needReduceAngle += 360;
				}
				curRTime = 0;
				

				isAutoRot = false;
				isRequestData = true;
				Invoke("UpdateWheelData",2);

			}
		
		}
	
	}
	
	void doReduceSpeed()
	{
		curRTime += Time.deltaTime;
		float needAddAngle = needReduceAngle * Mathf.Sin(Mathf.Deg2Rad*(curRTime / 2 * 90));
		transform.localEulerAngles = new Vector3(0,0,reduceAngleStartAngle-needAddAngle);

	}
	
	void OnPress(bool isPressed)
	{
		this.isPressed = isPressed;
		//xDeg = 0;
		//yDeg = 0;

	}
	
	
	void OnDrag(Vector2 delta)
	{
		if(isRotating)
			return;
		if(isPressed)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			if( Mathf.Abs( yDeg) > Mathf.Abs( xDeg))
			{
				isUseNormal = true;
				deg = yDeg;
			}
			else
			{
				isUseNormal = false;
				deg = xDeg;
			}
			//Debug.Log("xDeg yDeg : " + xDeg.ToString() + "  " + xDeg.ToString());
			
			if(isUseNormal)
			{
				//第一象限//
				if(x > screenPos.x && y > screenPos.y)
				{
					if(yDeg >0)
						return;
				}
				//第二象限//
				else if(x < screenPos.x && y > screenPos.y)
				{
					if(yDeg< 0)
						return;
				}
				//第三象限//
				else if(x < screenPos.x && y < screenPos.y)
				{
					if(yDeg < 0)
						return;
				}
				//第四象限//
				else if(x > screenPos.x && y < screenPos.y)
				{
					if(yDeg > 0)
						return;
				}
				else
				{
					return;
				}
			}
			else
			{
				//第一象限//
				if(x > screenPos.x && y > screenPos.y)
				{
					if(xDeg <0)
						return;
				}
				//第二象限//
				else if(x < screenPos.x && y > screenPos.y)
				{
					if(xDeg< 0)
						return;
				}
				//第三象限//
				else if(x < screenPos.x && y < screenPos.y)
				{
					if(xDeg > 0)
						return;
				}
				//第四象限//
				else if(x > screenPos.x && y < screenPos.y)
				{
					if(xDeg > 0)
						return;
				}
				else
				{
					return;
				}
			}
			
			//第一象限//
			if(x > screenPos.x && y > screenPos.y)
			{
				if(yDeg >0)
				{
					return;
				}
			}
			//第二象限//
			else if(x < screenPos.x && y > screenPos.y)
			{
				if(yDeg< 0)
				{
					return;
				}
			}
			//第三象限//
			else if(x < screenPos.x && y < screenPos.y)
			{
				if(yDeg < 0)
				{
					return;
				}
			}
			//第四象限//
			else if(x > screenPos.x && y < screenPos.y)
			{
				if(yDeg > 0)
				{
					return;
				}
			}
			//Debug.Log("deg:"+deg);
			if(Mathf.Abs(deg) > sensitiveNum)
			{
				if(isRequestData)
				{
					isRequestData = false;
					requestType = 1;
					//请求转动转盘//
					UIJson uijson = new UIJson();
					uijson.UIJsonForActWheel(STATE.UI_ACT_GEAR_WHEEL,cellID,2);
					PlayerInfo.getInstance().sendRequest(uijson, this);
				}
				
			}

		}
	}
	
	void UpdateWheelData()
	{
		wheelPanel.GetComponent<WheelPanel>().UpdateWheelPanelData(runnerResultJson);
	}
	
	public void receiveResponse(string json)
    {
        if (json != null)
        {
            //关闭连接界面的动画//
            PlayerInfo.getInstance().isShowConnectObj = false;
            switch (requestType)
            {
                case 1:
                    {
						RunnerResultJson rJson = JsonMapper.ToObject<RunnerResultJson>(json);
				Debug.Log("RunnerResultJson:"+json);
                        errorCode = rJson.errorCode;
                        if (errorCode == 0)
                        {
                            runnerResultJson = rJson;
                        }
                        receiveData = true;
                    }
                    break;
			}
		}
	}
}
