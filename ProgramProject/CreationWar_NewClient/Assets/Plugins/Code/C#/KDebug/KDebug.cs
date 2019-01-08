
//#define	KDebug
using	UnityEngine;
using	System.Collections;

//#if !UNITY_ANDROID


using	System.IO;
using	System;
//#endif
/// <summary>
/// 在某个角色头上打Log工具 -
/// </summary>
public	class	KDebug	:	MonoBehaviour 
{
	private	static	GameObject	Res	=	null;
	/// <summary>
	/// 在transform位置出现的Log -
	/// </summary>
	/// <param name="Data">Log数据</param>
	/// <param name="Tras">相对位置</param>
	public	static	void	Log(	string Data, Transform Tras )
	{
#if UNITY_EDITOR
	#if KDebug
		if(	Res	==	null	)
		{
			Res	=	Resources.Load( "KLog" ) as GameObject;
		}
		GameObject	newlog = GameObject.Instantiate( Res )as GameObject;
		newlog.transform.parent	=	Tras;
		newlog.transform.localPosition	=	 new	Vector3(0,2,0 );
		TextMesh	TM	=	newlog.GetComponentInChildren<TextMesh>();
		TM.text	=	Data;
		GameObject.Destroy( newlog, 3f );
	#endif
#endif
	}
	/// <summary>
	/// 在transform位置出现的Log -
	/// </summary>
	/// <param name="Data">Log数据</param>
	/// <param name="Tras">相对位置</param>
	/// <param name="color">文字颜色</param>
	public	static	void	Log(	string Data, Transform Tras, Color	color )
	{
#if UNITY_EDITOR
	#if KDebug
		if(	Res	==	null	)
		{
			Res	=	Resources.Load( "KLog" ) as GameObject;
		}
		GameObject	newlog = GameObject.Instantiate( Res )as GameObject;
		newlog.transform.parent	=	Tras;
		newlog.transform.localPosition	=	 new	Vector3(0,2,0 );
		TextMesh	TM	=	newlog.GetComponentInChildren<TextMesh>();
		TM.text		=	Data;
		TM.color	=	color;
		GameObject.Destroy( newlog, 3f );
	#endif
#endif
	}
	public	static	void	Log(	string Data, Vector3 Pos )
	{
#if UNITY_EDITOR
	#if KDebug
		if(	Res	==	null	)
		{
			Res	=	Resources.Load( "KLog" ) as GameObject;
		}
		GameObject	newlog = GameObject.Instantiate( Res )as GameObject;
		newlog.transform.position	=	Pos	+	(	new	Vector3(0,2,0 )	);
		TextMesh	TM	=	newlog.GetComponentInChildren<TextMesh>();
		TM.text	=	Data;
		GameObject.Destroy( newlog, 3f );
	#endif
#endif
	}
	public	static	void	Log(	string Data, Vector3 Pos, Color	color  )
	{
#if UNITY_EDITOR
	#if KDebug
		if(	Res	==	null	)
		{
			Res	=	Resources.Load( "KLog" ) as GameObject;
		}
		GameObject	newlog = GameObject.Instantiate( Res )as GameObject;
		newlog.transform.position	=	Pos	+	(	new	Vector3(0,2,0 )	);
		TextMesh	TM	=	newlog.GetComponentInChildren<TextMesh>();
		TM.text		=	Data;
		TM.color	=	color;
		GameObject.Destroy( newlog, 3f );
	#endif
#endif
	}
	/// <summary>
	/// 只在编辑器内进行的NGUILog -
	/// </summary>
	/// <param name="Data">Log数据</param>
	public	static	void	Log(	string Data	)
	{
#if UNITY_EDITOR
	#if KDebug
//		NGUIDebug.Log(	Data	);
	#endif
#endif
	}
	Vector3	Pos	=	new	Vector3(0,0,0 );
	void	LateUpdate () 
	{
		Pos		=	transform.localPosition;
		Pos.y	+=	Time.deltaTime	*	0.8f;
		transform.localPosition	=	Pos;
		transform.LookAt(	Camera.main.transform.position	);
	}

	/// <summary>
	/// 写出Log -
	/// </summary>
	/// <param name="LogStr">Log string.</param>
	public	static	void	WriteLog(string LogStr)
	{
		StreamWriter sw = null;
		try
		{
			LogStr = DateTime.Now.ToLocalTime().ToString()  +"\n" + LogStr;
			sw = new StreamWriter("KDebug.log", true);
			sw.WriteLine(LogStr);
		}
		catch
		{
		}
		finally
		{
			if (sw != null)
			{
				sw.Close();
			}
		}
	}
}
