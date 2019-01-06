using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

abstract class UserDataBase
{
	public abstract void Process();
}

class UserData_ShowUI : UserDataBase
{
	public int uiType;
	public object[] userData;

	public UserData_ShowUI(int uiType, params object[] userData)
	{
		this.uiType = uiType;
		this.userData = userData;
	}

	public override void Process()
	{
		GameUtility.JumpUIPanel(uiType, userData);
	}
}

class UserData_Callback : UserDataBase
{
	public delegate void Callback();
	public Callback callback;

	public UserData_Callback(Callback callback)
	{
		this.callback = callback;
	}

	public override void Process()
	{
		if (callback != null)
			callback();
	}
}