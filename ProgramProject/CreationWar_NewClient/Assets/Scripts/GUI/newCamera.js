#pragma strict

	var HideAndShowCursor = true;
	var LockRotationWhenRightClick = false;
    var target :Transform;
   
    var targetHeight = 3.0;
    var distance = 5.0;

    var maxDistance = 25;
    var minDistance = 8;

    var xSpeed = 250.0;
    var ySpeed = 120.0;

    var yMinLimit = -80;
    var yMaxLimit = 80;

    var zoomRate = 40;

    var rotationDampening = 3.0;
    var zoomDampening = 10.0;
static var huantime = -2.0;
static var fd = 0.5;
static var cameraposition :Vector3;

private var x = 0.0;
private var y = 0.0;
private var currentDistance : float;
private var desiredDistance: float;
private var correctedDistance: float;
private var spawnpoint:Transform;
//var BMC : AudioClip[];

function Awake(){
    var distances = new float[32];
    var qualityLevel : int = QualitySettings.GetQualityLevel();
    
    distances[8] = 32+ qualityLevel*3; 
    distances[12] = 32 + qualityLevel*6;
    camera.layerCullDistances = distances;
    camera.cullingMask = CullingMask;
    		var str : String = Application.loadedLevelName.Substring(3,3);
		if(str=="111")
			audio.clip = AllResources.staticBMC[4]; //city
    	else
		if(str=="151"||str=="711")
	    	audio.clip = AllResources.staticBMC[0]; //city
		else if(str.Substring(0,1)=="1")
			audio.clip = AllResources.staticBMC[1]; // camp
		else if(str=="200"||str=="271"||str=="272")
	    	audio.clip = AllResources.staticBMC[3]; //drogoen
		else if(str.Substring(0,1)=="2"||str=="511"||str=="521"||str=="551"||str=="552"||str=="561"||str=="562"||str=="713")	
		audio.clip = AllResources.staticBMC[2]; // wild
		else 
		audio.clip = AllResources.staticBMC[3]; //drogoen
		audio.Play();
		audio.volume = 0.5;
}
	
function Start () {
spawnpoint = GameObject.FindWithTag("spawn").transform;
if (PlayerPrefs.GetInt("LockFov",1) == 0)
GodFOV();
else
FreeFOV();
        correctedDistance = distance - 0.2;
if(spawnpoint)
  Movespawn();
}

function Movespawn(){
transform.position = spawnpoint.transform.position - spawnpoint.transform.forward*12 + spawnpoint.transform.up*8;
}

private var LockFOV = false;
function FreeFOV(){
LockFOV = false;
        x = 0;
        y = 20;
//		Mx = 0;
		My = 20;
desiredDistance = 14;		
}

function GodFOV(){
LockFOV = true;
        x = 45;
        y = 35;
		Mx = 45;
		My = 35;
// desiredDistance = maxDistance-4;	
		desiredDistance = PlayerPrefs.GetInt("CameraHeight", 25);
}

static function huandong(lv:int,position:Vector3){
var range = Vector3.Distance(position, cameraposition);
if(range<=32.0){
  if(range>12.0)
    fd = lv*3/range;
  else{
    fd = lv*0.25;
    if(PlayerPrefs.GetInt("SwichShock" , 0) == 1){
    	#if UNITY_IPHONE || UNITY_ANDROID
			Handheld.Vibrate ();   
	    #endif
    }
    }
huantime=Time.time;
}
}

var CullingMask : LayerMask = 0;
var lineOfSightMask : LayerMask = 0;
private var oldPosition1 : Vector2;
private var oldPosition2 : Vector2;
 
private var Mx : float;
private var My : float;

private var lastFingerID : int = -1;

private var lastHeight : float;
private var currHeight : float;
function Update (){
    if(!target || !UIAllPanelControl.isGround){
	    return;
    }
cameraposition = target.position;
if(Input.touchCount >1 && (Input.GetTouch(0).phase==TouchPhase.Moved&& Input.GetTouch(0).position.x>Screen.width*0.35 && Input.GetTouch(0).position.x<Screen.width*0.75 && Input.GetTouch(0).position.y>Screen.height*0.2 && Input.GetTouch(0).position.y<Screen.height*0.8)&&(Input.GetTouch(1).phase==TouchPhase.Moved&& Input.GetTouch(1).position.x>Screen.width*0.35 && Input.GetTouch(1).position.x<Screen.width*0.75 && Input.GetTouch(1).position.y>Screen.height*0.2 && Input.GetTouch(1).position.y<Screen.height*0.8))
	{
                var tempPosition1 = Input.GetTouch(0).position;
	            var tempPosition2 = Input.GetTouch(1).position;

	                if(isEnlarge(oldPosition1,oldPosition2,tempPosition1,tempPosition2))
	                {
	                    if(desiredDistance > minDistance)
                    	{
	                        desiredDistance -= Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance)*0.1;
	                    }
					}else
	                {
	                    if(desiredDistance < maxDistance)
	                    {
                        desiredDistance += Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance)*0.1;
	                    }
                	}
	            //备份上一次触摸点的位置，用于对比
                oldPosition1=tempPosition1;
	            oldPosition2=tempPosition2;
}
else if(Input.touchCount >=1)
{ 	
	for (var i = 0; i < Input.touchCount; ++i) {
	    if (Input.GetTouch(i).phase==TouchPhase.Moved && Input.GetTouch(i).position.x>Screen.width*0.35 && Input.GetTouch(i).position.x<Screen.width*0.75 && Input.GetTouch(i).position.y>Screen.height*0.2 && Input.GetTouch(i).position.y<Screen.height*0.8) {
	        Mx += Input.GetAxis("Mouse X") * xSpeed * 0.05;
	        if(!LockFOV)
	            My -= Input.GetAxis("Mouse Y") * ySpeed * 0.05;      
	    }

	    //if (Input.GetTouch(i).position.x>Screen.width*0.35 && Input.GetTouch(i).position.x<Screen.width*0.75 && Input.GetTouch(i).position.y>Screen.height*0.2 && Input.GetTouch(i).position.y<Screen.height*0.8) {
	    //    if(Input.GetTouch(i).phase==TouchPhase.Began  && (lastFingerID == -1 || lastFingerID != Input.GetTouch(i).fingerId))// 增加lastFingerID变量是为了解决转动视角的同时推摇杆摄像机乱转的问题
	    //    {
	    //        lastFingerID = Input.GetTouch(i).fingerId;
	    //    }
	    //    else if(Input.GetTouch(i).phase==TouchPhase.Moved && lastFingerID == Input.GetTouch(i).fingerId)
	    //    {
	    //        Mx += Input.GetAxis("Mouse X") * xSpeed * 0.05;
	    //        if(!LockFOV)
	    //            My -= Input.GetAxis("Mouse Y") * ySpeed * 0.05; 
	    //    }
	    //    else if(Input.GetTouch(i).phase==TouchPhase.Ended || Input.GetTouch(i).phase==TouchPhase.Canceled)
	    //    {
	    //        lastFingerID = -1;
	    //    }
	    //}
    }
}
else if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
			if(LockRotationWhenRightClick== false){
            Mx += Input.GetAxis("Mouse X") * xSpeed * 0.05;
            if(!LockFOV)
            My -= Input.GetAxis("Mouse Y") * ySpeed * 0.05;
			}
        }
   if (!LockFOV && alljoy.h != 0 )
        {   
         Mx += alljoy.h * xSpeed * 0.005;
        }

   if(LockFOV)
   {
       // My = PlayerPrefs.GetInt("CameraHeight", 40);
       currHeight = PlayerPrefs.GetInt("CameraHeight", 25);
       if(currHeight != lastHeight)
       {
           desiredDistance = currHeight;
           lastHeight = currHeight;
       }
   }
       
        My = ClampAngle(My, yMinLimit, yMaxLimit);
        
		x = Mathf.Lerp(x , Mx, Time.deltaTime * 8);  
		y = Mathf.Lerp(y , My, Time.deltaTime * 8); 

	if(HideAndShowCursor){
			 if(Input.GetMouseButton(1) || Input.GetMouseButton(2)) 
				Screen.lockCursor = true; 
			else 
				Screen.lockCursor = false; 
	}
		
}

function isEnlarge(oP1 : Vector2,oP2 : Vector2,nP1 : Vector2,nP2 : Vector2) : boolean
	{
	var leng1 =Mathf.Sqrt((oP1.x-oP2.x)*(oP1.x-oP2.x)+(oP1.y-oP2.y)*(oP1.y-oP2.y));
    var leng2 =Mathf.Sqrt((nP1.x-nP2.x)*(nP1.x-nP2.x)+(nP1.y-nP2.y)*(nP1.y-nP2.y));
	 if(leng1<leng2)
         return true;
     else
	     return false;
}

function LateUpdate ()
 {
        if (!target)
           return;

        var rotation = Quaternion.Euler(y, x, 0);
        if(LockFOV) 
        {
            rotation = Quaternion.Euler(45, x, 0);// 上帝视角角度是固定的
        }

        // calculate the desired distance
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);       
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;
        //if(LockFOV)
        //{
        //    PlayerPrefs.SetInt("CameraHeight", Mathf.Clamp(correctedDistance, minDistance, maxDistance - 4));
        //}
        // calculate desired camera position
        var position = target.position - (rotation * Vector3.forward * desiredDistance + Vector3(0, -targetHeight, 0));

        // check for collision using the true target's desired registration point as set by user using height
        var collisionHit:RaycastHit;
        var trueTargetPosition = Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        // if there was a collision, correct the camera position and calculate the corrected distance
        var isCorrected = false;
       if (Physics.Linecast(trueTargetPosition, position, collisionHit,lineOfSightMask.value))
       {
            position = collisionHit.point;
            correctedDistance = Vector3.Distance(trueTargetPosition, position);
            if(correctedDistance <= minDistance)
            correctedDistance = minDistance;
            isCorrected = true;
        }
        
        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // recalculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -targetHeight - 0.05, 0));

        transform.rotation = rotation;
        transform.position = position;
// 摄像机抖动
	if(huantime+0.3>Time.time  )
		transform.position.y += Random.Range(-fd,fd);	 
		
	}

private static function ClampAngle(angle:float , min:float , max:float ):float
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    
 function SetTarget (t : Transform)
{
	target = t;
	Mx = t.eulerAngles.y;
}

 function SetTargetfast (t : Transform)
{
	target = t;
	Mx = t.eulerAngles.y;
	x = t.eulerAngles.y;
}

function OnDisable()
{
    if(LockFOV)
    {
        PlayerPrefs.SetInt("CameraHeight", Mathf.Clamp(desiredDistance, minDistance, maxDistance));// 在切换地图时保存一下上帝视角的高度
    }
}