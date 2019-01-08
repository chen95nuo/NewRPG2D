#pragma strict

public class JoyClass extends MonoBehaviour {

	@HideInInspector
	var pressDownPosition : Vector3;
	
	@HideInInspector
	var pressUpPosition : Vector3;
	
	@HideInInspector
	var pressTime : float = 0;

	@HideInInspector
	var pressHolding : boolean = false;

	public function PressedLeftUp() : boolean
	{
		if(Input.GetMouseButtonUp(0))
		{
			pressUpPosition = Input.mousePosition;
			return true;
		}
		return false;
	}

	public function PressedLeftDown() : boolean
	{
		if(Input.GetMouseButtonDown(0))
		{
			pressDownPosition = Input.mousePosition;
			return true;
		}
		return false;
	}

	public function DistancePressOn() : float{
		return Vector3.Distance(pressDownPosition , pressUpPosition);
	}

	public function DistanceAsMousePosition(pos : Vector3) : float
	{
		return Vector3.Distance(Input.mousePosition , pos);
	}
	
	public function AngleDownToUp() : Vector3
	{
		return pressUpPosition - pressDownPosition;
	}
	
	public function AngelMoveTo() : Vector3
	{
		return Input.mousePosition - pressDownPosition;
	}
	
}