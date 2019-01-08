//----------------------------------------------
//           ZealmTools
// Copyright © 2010-2014 Zealm
// Copyright © 2010-2014 FernYuan
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// //
/// </summary>
public class SDKOpenPackEditor : YuanEditorWindow {

	/// <summary>
	/// 打包唯一标示
	/// </summary>
	private string bundleIdentifier=string.Empty;

	/// <summary>
	/// 打包唯一标示
	/// </summary>
	/// <value>The bundle identifier.</value>
	public string BundleIdentifier
	{
		get{return bundleIdentifier;}
		set
		{
			bundleIdentifier=value;
		}
	}

	/// <summary>
	/// 包名
	/// </summary>
	private string productName=string.Empty;
	/// <summary>
	/// 包名
	/// </summary>
	/// <value>The name of the product.</value>
	public string ProductName {
		get {
			return productName;
		}
		set {
			productName = value;
		}
	}

	/// <summary>
	/// 图标
	/// </summary>
	private Texture2D icon;
	/// <summary>
	/// 图标
	/// </summary>
	/// <value>The icon.</value>
	public Texture2D Icon {
		get {
			return icon;
		}
		set
		{
			icon=value;
		}
	}

	/// <summary>
	/// 图标组
	/// </summary>
	public Texture2D[] icons;

	/// <summary>
	/// 所有的场景名
	/// </summary>
	private string[] listScenes;


	/// <summary>
	/// 生成平台
	/// </summary>
	public BuildTarget buildTarget=BuildTarget.Android;

	/// <summary>
	///  脚本平台
	/// </summary>
	public BuildTargetGroup buildTargetGroup=BuildTargetGroup.Android;

	/// <summary>
	/// 版本
	/// </summary>
	public string bundleVersion=string.Empty;

	/// <summary>
	/// SDK平台
	/// </summary>
	public int sdkTarget;

	/// <summary>
	/// SDK平台字段
	/// </summary>
	public string strSdkTarget;

	/// <summary>
	/// 出包品质
	/// </summary>
	public int qualitySettings;


	/// <summary>
	/// 获取打包相关信息
	/// </summary>
	public void GetInfo()
	{
		this.bundleIdentifier=EditorPrefs.GetString ("SDK_bundleIdentifier","com.company.product");
		this.ProductName=EditorPrefs.GetString ("SDK_productName","ProductName");
		this.icon=AssetDatabase.LoadMainAssetAtPath(EditorPrefs.GetString ("SDK_icon","")) as Texture2D;
		this.icons=new Texture2D[7];
		for(int i=0;i<icons.Length;i++)
		{
			icons[i]=icon;
		}
		//bundleVersion=EditorPrefs.GetString ("SDK_bundleVersion","v1.0");
		bundleVersion=YuanUnityPhoton.GameVersion.ToString ();
		buildTarget=(BuildTarget)EditorPrefs.GetInt ("SDK_buildTarget",0);
		buildTargetGroup=(BuildTargetGroup)EditorPrefs.GetInt ("SDK_buildTargetGroup",0);
		sdkTarget=EditorPrefs.GetInt ("SDK_sdkTarget",0);
		strSdkTarget=EditorPrefs.GetString ("SDK_strSdkTarget","");
		PlayerSettings.SetScriptingDefineSymbolsForGroup (buildTargetGroup,strScriptDefine);
	
		qualitySettings=EditorPrefs.GetInt("SDK_qualitySettings",0);



	}

	/// <summary>
	/// 保存打包信息
	/// </summary>
	public void SaveInfo()
	{
		EditorPrefs.SetString ("SDK_bundleIdentifier",bundleIdentifier);
		EditorPrefs.SetString ("SDK_productName",productName);
		EditorPrefs.SetString ("SDK_icon",AssetDatabase.GetAssetPath (icon) );
		EditorPrefs.SetString ("SDK_bundleVersion",bundleVersion);
		EditorPrefs.SetInt ("SDK_buildTarget",(int)buildTarget);
		EditorPrefs.SetInt ("SDK_buildTargetGroup",(int)buildTargetGroup);
		EditorPrefs.SetInt ("SDK_sdkTarget",sdkTarget);
		EditorPrefs.SetString ("SDK_strSdkTarget",strSdkTarget);
		EditorPrefs.SetInt("SDK_qualitySettings",qualitySettings);
	}

	void Awake()
	{

		GetInfo ();
	}

	/// <summary>
	/// 绘制打包相关信息
	/// </summary>
	protected override void DarwInfo()
	{
		BundleIdentifier=EditorGUILayout.TextField("BundleIdentifier(唯一标示):",bundleIdentifier);
		ProductName=EditorGUILayout.TextField("ProductName(包名):",ProductName);
		bundleVersion=EditorGUILayout.TextField("BundleVersion(版本):",bundleVersion);
		buildTarget=(BuildTarget)EditorGUILayout.EnumPopup("BuildTarget(生成平台):",buildTarget);
		buildTargetGroup=(BuildTargetGroup)EditorGUILayout.EnumPopup("BuildTargetGroup(脚本平台):",buildTargetGroup);
		EditorUserBuildSettings.symlinkLibraries=EditorGUILayout.Toggle ("SymlinkLibraries:",EditorUserBuildSettings.symlinkLibraries);
		EditorUserBuildSettings.development=EditorGUILayout.Toggle ("DevelopmentBuild:",EditorUserBuildSettings.development);
		if(EditorUserBuildSettings.development)
		{
			EditorUserBuildSettings.connectProfiler=EditorGUILayout.Toggle ("ConnectProfiler:",EditorUserBuildSettings.connectProfiler);
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("QualitySettings(出包品质):");
		qualitySettings=EditorGUILayout.Popup(qualitySettings,QualitySettings.names);
		EditorGUILayout.EndHorizontal();

		GetSDKList ();
		Icon=EditorGUILayout.ObjectField ("Icon(图标):",Icon,typeof(Texture2D),true) as Texture2D;
		{
			for(int i=0;i<icons.Length;i++)
			{
				icons[i]=icon;
			}
		}




		if(GUILayout.Button ("Output(爆发吧,小宇宙,出包!!!)"))
		{
			BuildSDK ();
		}
	}


	string strScriptDefine="SDK_DOWN;SDK_UC;SDK_QH;SDK_MI;SDK_OPPO;SDK_CMGE;SDK_PEASECOD;SDK_DUOKU;SDK_LENOVO;SDK_HUAWEI;SDK_AZ;SDK_MUZI;SDK_VIVO;SDK_JYIOS;SDK_KUAIYONG;SDK_PP;SDK_ITOOLS;SDK_TONGBU;SDK_JY;SDK_XY;SDK_I4;SDK_ZSY;SDK_ZSYIOS;SDK_HM";
	/// <summary>
	/// 设置SDK平台
	/// </summary>
	void GetSDKList()
	{
		if(buildTargetGroup!=null)
		{

			//string[] strSDK =PlayerSettings.GetScriptingDefineSymbolsForGroup (buildTargetGroup).Split (';');
			string[] strSDK =strScriptDefine.Split (';');
			if(strSDK.Length>sdkTarget)
			{
				sdkTarget=EditorGUILayout.Popup ("SDKTarget(SDK平台):",sdkTarget,strSDK);
				strSdkTarget=strSDK[sdkTarget];
			}
			else if(strSDK.Length>0)
			{
				sdkTarget=EditorGUILayout.Popup ("SDKTarget(SDK平台):",0,strSDK);
				strSdkTarget=strSDK[sdkTarget];
			}
		}
	}

	void OnInspectorUpdate()
	{
		this.Repaint ();
	}

	void OnFocus()
	{
		listScenes=FindEnabledEditorScenes ();
	}

	/// <summary>
	/// 查找已勾选的场景
	/// </summary>
	/// <returns>The enabled editor scenes.</returns>
	private static string[] FindEnabledEditorScenes() {
	
			List<string> EditorScenes = new List<string>();

	
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) 
		{

				if (!scene.enabled) continue;

				EditorScenes.Add(scene.path);
		
		}
			return EditorScenes.ToArray();

	}

	
	void OnLostFocus()
	{
		SaveInfo ();
	}
	
	void OnDestroy()
	{
		SaveInfo ();
	}


	/// <summary>
	/// 出包吧！！！
	/// </summary>
	void BuildSDK()
	{
	
			string mPath=string.Empty;
			switch(buildTargetGroup)
			{
				case BuildTargetGroup.Android:
				{
					mPath=EditorUtility.SaveFilePanel ("Output(爆发吧,小宇宙,出包!!!)","",ProductName+".apk","apk");
				}
				break;
				case BuildTargetGroup.iPhone:
				{
					mPath=EditorUtility.SaveFolderPanel ("Output(爆发吧,小宇宙,出包!!!)","",ProductName);
				}
				break;
			}
			if(!string.IsNullOrEmpty (mPath))
			{
				PlayerSettings.bundleIdentifier=this.bundleIdentifier;
				PlayerSettings.bundleVersion=this.bundleVersion;
				PlayerSettings.SetIconsForTargetGroup (buildTargetGroup,icons);
				PlayerSettings.SetScriptingDefineSymbolsForGroup (buildTargetGroup,strSdkTarget);
				QualitySettings.SetQualityLevel(qualitySettings);
				

				EditorUserBuildSettings.SwitchActiveBuildTarget (buildTarget);
				
				BuildOptions buildOptions=BuildOptions.None;
				SetBuildOption (ref buildOptions);
				
				
				string res=BuildPipeline.BuildPlayer (listScenes,mPath,buildTarget,buildOptions);
				if(res.Length>0)
				{
					Debug.LogError ("BuildFail(残念,生成失败!):"+res);
					EditorUtility.DisplayDialog ("Message(有消息啦)","BuildFail,Please see console info!(残念,生成失败!赶快看看控制台里的错误消息吧,少年!)","OK");
				}
				else
				{
					EditorUtility.DisplayDialog ("Message(有消息啦)","BuildSuccess!Congratulation!(少年!出包成功了,普天同庆啊!)","OK");
				}

			PlayerSettings.SetScriptingDefineSymbolsForGroup (buildTargetGroup,strScriptDefine);
		}

	}

	/// <summary>
	/// Sets the build option.
	/// </summary>
	/// <param name="buildOptions">Build options.</param>
	void SetBuildOption(ref BuildOptions buildOptions)
	{
		if(EditorUserBuildSettings.symlinkLibraries)
		{
			buildOptions=BuildOptions.SymlinkLibraries;
			if(EditorUserBuildSettings.development)
			{
				buildOptions=BuildOptions.SymlinkLibraries&BuildOptions.Development;
				if(EditorUserBuildSettings.connectProfiler)
				{
					buildOptions=BuildOptions.SymlinkLibraries&BuildOptions.Development&BuildOptions.ConnectWithProfiler;
				}
			}
		}
		else if(EditorUserBuildSettings.development)
		{
			buildOptions=BuildOptions.Development;
			if(EditorUserBuildSettings.connectProfiler)
			{
				buildOptions=BuildOptions.Development&BuildOptions.ConnectWithProfiler;
			}
		}

	}

//	Vector2 mPos = Vector2.zero;
//	void OnGUI()
//	{
//		mPos = GUILayout.BeginScrollView (mPos);
//		{
//			DarwInfo ();
//
//		}
//		GUILayout.EndScrollView ();
//
//		GUI.color=Color.green;
//		GUILayout.Label ("Copyright © 2010-2014 Zealm\nCopyright © 2010-2014 FernYuan");
//
//	}










	 
}
