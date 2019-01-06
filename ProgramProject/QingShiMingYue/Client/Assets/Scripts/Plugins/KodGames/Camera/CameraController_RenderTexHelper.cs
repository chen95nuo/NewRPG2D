using System;
using System.Collections.Generic;
using System.Text;

public class CameraController_RenderTexHelper : CameraController_Touch
{
	public void OnInput(POINTER_INFO data)
	{
		ProcessDragLogic(data);
	}
}
