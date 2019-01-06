using UnityEngine;
using System.Collections;

public class MainMenuItem
{
	public delegate bool OnCallback(object data);
	private OnCallback callback;
	public OnCallback Callback
	{
		get { return this.callback; }
		set { this.callback = value; }
	} 
	
	public object CallbackData;
	
	private string methodToInvoke;
	public string MethodToInvoke
	{
		get { return this.methodToInvoke; }
		set { this.methodToInvoke = value; }
	}
	
	private MonoBehaviour scriptMethodToInvoke;
	public MonoBehaviour ScriptMethodToInvoke
	{
		get { return this.scriptMethodToInvoke; }
		set { this.scriptMethodToInvoke = value; }
	}
	
	private string controlText;
	public string ControlText
	{
		get { return this.controlText; }
		set { this.controlText = value; }
	}
}
