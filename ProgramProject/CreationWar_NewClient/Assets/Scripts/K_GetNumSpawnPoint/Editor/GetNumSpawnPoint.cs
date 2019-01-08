using	UnityEngine;
using	UnityEditor;
using	System.Collections;
using	System.Xml;
using	System.IO;

/// <summary>
/// 读取每个场景的生成点数量 -
/// </summary>
public	class GetNumSpawnPoint	:	Editor
{
	[MenuItem("ZealmTools/记录生成点数量、处理怪物生成点、炸药点、陷阱点、物品生成点数量",false, -100)]
	static	void	WriteNumSpawnPoint()
	{
		//Xml文件路径//
		string	FilePath	=	Application.dataPath	+	"/Scripts/K_GetNumSpawnPoint/Resources/SpawnPointsCount.xml";

		//获取当前SceneName//
		string[]	buff1	=	EditorApplication.currentScene.Split('/');
		string[]	buff2	=	buff1[buff1.Length-1].Split('.');
		string		LoadedSceneName	=	buff2[0];

		MonsterSpawn[]		AllSpawnPoint	=	FindObjectsOfType<MonsterSpawn>();
		BodyguardSpawn[]	BodyGuards		=	FindObjectsOfType<BodyguardSpawn>();
		if(	AllSpawnPoint.Length	>	0	||	BodyGuards.Length	>	0	)
		{
			//------	顺次赋值当前场景的SpawnPointID		------//
			for( int i = 0; i < AllSpawnPoint.Length; i++ )
			{
				//AllSpawnPoint[i].CurrentMonsterSpawnPointID	=	i;
				SpawnPointNetView	SPNV	=	AllSpawnPoint[i].gameObject.GetComponent<SpawnPointNetView>();
				if(	SPNV	==	null	)
					SPNV	=	AllSpawnPoint[i].gameObject.AddComponent<SpawnPointNetView>();
				SPNV.CurrentSpawnPointID	=	i;
				MonsterSpawnPointHandler	MSPHandle	=	AllSpawnPoint[i].gameObject.GetComponent<MonsterSpawnPointHandler>();
				if(	MSPHandle	!=	null	)
					GameObject.DestroyImmediate(MSPHandle);
			}

			for( int i = 0,j = AllSpawnPoint.Length; i < BodyGuards.Length; i++ )
			{
				//AllSpawnPoint[i].CurrentMonsterSpawnPointID	=	i;
				SpawnPointNetView	SPNV	=	BodyGuards[i].gameObject.GetComponent<SpawnPointNetView>();
				if(	SPNV	==	null	)
					SPNV	=	BodyGuards[i].gameObject.AddComponent<SpawnPointNetView>();
				SPNV.CurrentSpawnPointID	=	i + j;
				MonsterSpawnPointHandler	MSPHandle	=	BodyGuards[i].gameObject.GetComponent<MonsterSpawnPointHandler>();
				if(	MSPHandle	!=	null	)
					GameObject.DestroyImmediate(MSPHandle);
			}

			//------	顺次赋值当前场景的SpawnPointID		------//
			XmlDocument	XML	=	new	XmlDocument();
			Object	o	=	Resources.Load("SpawnPointsCount");
			if(	o	!=	null	)
			{	//创建//
				XML.LoadXml( o.ToString()	);
			}
			else
			{
				XmlNode	RootNode	=	XML.CreateElement("SpawnPointsCount");
				XML.AppendChild(	RootNode	);
			}
			XmlNodeList	NList	=	XML.SelectNodes("SpawnPointsCount/PerScene");
			bool	Founded	=	false;
			XmlNode	XNode;
			for( int i = 0; i < NList.Count; i++ )
			{
				XNode	=	NList[i].SelectSingleNode("SceneName");
				if(	XNode.InnerText	==	LoadedSceneName	)
				{
					Founded	=	true;
					XNode	=	NList[i].SelectSingleNode("Counts");
					XNode.InnerText	=	""+( AllSpawnPoint.Length + BodyGuards.Length );
				}
			}
			if(	!Founded	)
			{
				Debug.Log("AddNewScene");
				XmlNode		Root		=	XML.SelectSingleNode("SpawnPointsCount");
				XmlElement	PerScene	=	XML.CreateElement("PerScene");
				XmlElement	SceneName	=	XML.CreateElement("SceneName");
				SceneName.InnerText		=	LoadedSceneName;
				XmlElement	Counts		=	XML.CreateElement("Counts");
				Counts.InnerText		=	""+( AllSpawnPoint.Length + BodyGuards.Length );
				PerScene.AppendChild(SceneName);
				PerScene.AppendChild(Counts);
				Root.AppendChild(PerScene);
			}
			XML.Save(@FilePath);
			AssetDatabase.Refresh();
			Debug.Log("更新完成！  ==>"+ LoadedSceneName	);
		}
		else
		{
			Debug.Log("没有怪物刷新点  ==>"+ LoadedSceneName	);
		}
	}
}
