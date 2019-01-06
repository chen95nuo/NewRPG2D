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

	// �ƹ�
	public GameObject tavernNameButton;

	// �ſ͹�
	public GameObject retainerNameButton;

	// ���䳡
	public GameObject arenaNameButton;

	// ǧ����
	public GameObject towerNameButton;

	// �������
	public GameObject beaconfireNameButton;

	// ����
	public GameObject guildNameButton;

	//����
	public GameObject adventureButton;

	//����ս��ϵͳ
	public GameObject friendCombatSystemButton;
	
	public GameObject illusionButon;

	//������
	public GameObject coreButton;

	//�����޹���
	public GameObject organButton;
}