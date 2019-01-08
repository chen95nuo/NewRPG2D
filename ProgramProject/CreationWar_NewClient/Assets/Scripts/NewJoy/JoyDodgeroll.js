#pragma strict
public class JoyDodgeroll extends JoyClass{

	function Update () {
		if(PressedLeftDown()){
			pressHolding = true;
			pressTime = Time.time + CommonDefine.JoySlide_HoldTime;
		}
		
		if(PressedLeftUp()){
			pressHolding = false;
		}
		
		if(pressHolding && Time.time > pressTime && DistanceAsMousePosition(pressDownPosition) < CommonDefine.JoyDodgeroll_Movingdistance){
			pressHolding = false;
//			UIControl.UICL.DoDodgeroll();
		}
	}
	
}
