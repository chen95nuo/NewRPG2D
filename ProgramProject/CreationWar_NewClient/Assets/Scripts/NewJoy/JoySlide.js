#pragma strict
public class JoySlide extends JoyClass{

	function Update () {
		if(PressedLeftDown()){
			pressHolding = true;
			pressTime = Time.time + CommonDefine.JoyDodgeroll_HoldTime;
		}
		
		if(pressHolding && PressedLeftUp() && Time.time < pressTime && DistanceAsMousePosition(pressDownPosition) > CommonDefine.JoySlide_Movingdistance){
//			UIControl.UICL.DoSlide( AngleDownToUp() );
		}
	}

}
