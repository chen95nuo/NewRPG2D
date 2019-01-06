using UnityEngine;
using System.Collections;

public abstract class SysModule : MonoBehaviour
{
	public virtual bool Initialize() { return true; }
	public virtual void Dispose() { }
	public virtual void Run(object userData) { }
	public virtual void OnUpdate() { }
	public virtual void OnGUIUpdate() { } // To show engine gui.
}