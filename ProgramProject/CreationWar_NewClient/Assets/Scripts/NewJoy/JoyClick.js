#pragma strict

public class JoyClick extends JoyClass{

	function Update () {
		if(PressedLeftDown()){
			pressTime = Time.time + CommonDefine.JoyClick_pressTime;
		}
		if(PressedLeftUp() && pressTime > Time.time && DistancePressOn() < CommonDefine.JoyClick_mouseMove){
//			UIControl.UICL.DoClick();
		}
	}
}
