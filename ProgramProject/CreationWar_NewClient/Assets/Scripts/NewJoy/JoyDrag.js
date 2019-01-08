#pragma strict
public class JoyDrag extends JoyClass {

	function Update () {
		if(PressedLeftDown()){
			pressHolding = true;
		}
		
		if(PressedLeftUp()){
			pressHolding = false;
		}
		
		if(pressHolding && DistanceAsMousePosition(pressDownPosition) > CommonDefine.JoyDrag_Movingdistance){
//			UIControl.UICL.DoDrag( AngelMoveTo() );
		}
	}
	
}
