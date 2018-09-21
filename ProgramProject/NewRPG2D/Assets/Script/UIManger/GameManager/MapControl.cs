using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public GameObject MainMap;
    public GameObject EditMap;
    public CameraMgr mainCamera;
    public CameraMgr editCamera;
    public CastleMgr mainCastle;
    public EditMgr editCastle;

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.InEditMode, ShowEditMap);
        HallEventManager.instance.AddListener(HallEventDefineEnum.EditMode, ShowMainMap);
        ShowMainMap();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.InEditMode, ShowEditMap);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.EditMode, ShowMainMap);

    }
    public void ShowMainMap()
    {
        mainCastle.editMode = true;
        editCastle.editMode = false;
        MainMap.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        EditMap.SetActive(false);
        editCamera.gameObject.SetActive(false);
        mainCamera.transform.localPosition = editCamera.transform.localPosition;
    }
    public void ShowEditMap()
    {
        mainCastle.editMode = false;
        editCastle.editMode = true;
        EditMap.SetActive(true);
        editCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        editCamera.transform.localPosition = mainCamera.transform.localPosition;
        editCastle.UpdateEditCastle(mainCastle);
    }
}
