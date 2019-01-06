using UnityEngine;
using System.Collections;

public class MainSceneData : MonoBehaviour
{
	private static MainSceneData instance = null;
	public static MainSceneData Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(MainSceneData)) as MainSceneData;

			return instance;
		}
	}

	private CentralCityCameraController cameraCtrl = null;
	public CentralCityCameraController CameraCtrl
	{
		get
		{
			if (cameraCtrl == null)
				cameraCtrl = mainCamera.GetComponent<CentralCityCameraController>();

			return cameraCtrl;
		}
	}
	public Camera mainCamera;
	public GameObject cameraRoot;
	public GameObject sceneRoot;

	//buttons[0] is true building.+++++++++++++++++++++++++++++++++++++++++++++
	public UIButton3D[] tavernButtons;
	public UIButton3D[] retainerButtons;
	public UIButton3D[] arenaButtons;
	public UIButton3D[] towerButtons;
	public UIButton3D[] beaconfireButtons;
	public UIButton3D[] guildButtons;
	public UIButton3D[] adventureButtons;
	public UIButton3D[] friendCombatSystemButtons;
	public UIButton3D[] illusionSceneButtons;
	public UIButton3D[] coreSceneButtons;
	public UIButton3D[] organSceneButtons;

	// 酒馆
	public GameObject tavernNameButton;

	// 门客馆
	public GameObject retainerNameButton;

	// 比武场
	public GameObject arenaNameButton;

	// 千层塔
	public GameObject towerNameButton;

	// 烽火狼烟
	public GameObject beaconfireNameButton;

	// 家族
	public GameObject guildNameButton;

	//奇遇
	public GameObject adventureButton;

	//好友战斗系统
	public GameObject friendCombatSystemButton;
	
	public GameObject illusionButon;

	//炼丹房
	public GameObject coreButton;

	//机关兽工坊
	public GameObject organButton;
}